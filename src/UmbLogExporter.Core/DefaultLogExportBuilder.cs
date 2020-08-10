using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Umbraco.Core.Logging.Viewer;

namespace UmbLogExporter.Core
{
	public class DefaultLogExportBuilder : ILogExportBuilder
	{
		public void ProcessData(Stream stream, LogTimePeriod timePeriod, IEnumerable<LogMessage> messages)
		{
			var logMessages = messages.ToList();
			if (logMessages.Any())
			{
				return;
			}

			using (var excel = new ExcelPackage())
			{
				var sheet = excel.Workbook.Worksheets.Add($"{timePeriod.StartTime:yyyy-MM-dd} to {timePeriod.EndTime:yyyy-MM-dd}");

				var row = 0;

				SetHeaderCell(sheet, row, 0, "Timestamp");
				SetHeaderCell(sheet, row, 1, "Level");
				SetHeaderCell(sheet, row, 2, "Message");
				SetHeaderCell(sheet, row, 3, "Exception");
				SetHeaderCell(sheet, row, 4, "Properties");

				row++;

				var hasExceptions = false;

				foreach (var logMessage in logMessages)
				{
					//SetDateTimeCell(sheet, row, 1, logMessage.Timestamp);
					SetCell(sheet, row, 0, logMessage.Timestamp.ToString());
					SetCell(sheet, row, 1, logMessage.Level);
					SetGeneralCellWithWrappedText(sheet, row, 2, logMessage.RenderedMessage);

					if (!string.IsNullOrEmpty(logMessage.Exception))
					{
						hasExceptions = true;
					}
					SetGeneralCellWithWrappedText(sheet, row, 3, logMessage.Exception);

					var properties = new StringBuilder();

					foreach (var property in logMessage.Properties)
					{
						properties.AppendLine($"{property.Key}: {property.Value.ToString()}");
					}

					SetGeneralCellWithWrappedText(sheet, row, 4, properties.ToString());

					row++;
				}

				var allCells = sheet.Cells[1, 1, logMessages.Count(), 5];
				allCells.AutoFitColumns();
				allCells.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

				sheet.Column(3).Width = 100;
				sheet.Column(4).Width = 100;
				sheet.Column(5).Width = 100;

				if (!hasExceptions)
				{
					sheet.Column(4).Hidden = true;
				}

				excel.SaveAs(stream);
			}
		}

		public static void SetHeaderCell(ExcelWorksheet sheet, int row, int col, object value)
		{
			sheet.Cells[row + 1, col + 1].Value = value;
			sheet.Cells[row + 1, col + 1].Style.Numberformat.Format = "@";
			sheet.Cells[row + 1, col + 1].Style.Font.Bold = true;
		}

		public static void SetCell(ExcelWorksheet sheet, int row, int col, object value)
		{
			sheet.Cells[row + 1, col + 1].Value = value;
			sheet.Cells[row + 1, col + 1].Style.Numberformat.Format = "@";
		}

		public static void SetGeneralCellWithWrappedText(ExcelWorksheet sheet, int row, int col, object value)
		{
			sheet.Cells[row + 1, col + 1].Value = value;
			sheet.Cells[row + 1, col + 1].Style.Numberformat.Format = "General";
			sheet.Cells[row + 1, col + 1].Style.WrapText = true;
		}

		public static void SetDoubleCell(ExcelWorksheet sheet, int row, int col, double value)
		{
			sheet.Cells[row + 1, col + 1].Value = value;
		}

		public static void SetDateCell(ExcelWorksheet sheet, int row, int col, DateTime value)
		{
			sheet.Cells[row + 1, col + 1].Value = value;
			sheet.Cells[row + 1, col + 1].Style.Numberformat.Format = "dd/mm/yyyy";
		}

		public static void SetDateTimeCell(ExcelWorksheet sheet, int row, int col, DateTime value)
		{
			sheet.Cells[row + 1, col + 1].Value = value;
			sheet.Cells[row + 1, col + 1].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
		}
	}
}