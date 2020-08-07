using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Umbraco.Core.Logging.Viewer;

namespace UmbLogExporter.Core
{
	public class LogExportBuilder : ILogExportBuilder
	{
		public void ProcessData(Stream stream, LogTimePeriod timePeriod, List<LogMessage> messages)
		{
			using (var excel = new ExcelPackage())
			{
				var sheet = excel.Workbook.Worksheets.Add($"{timePeriod.StartTime:yyyy-MM-dd} to {timePeriod.EndTime:yyyy-MM-dd}");

				var row = 0;

				SetHeaderCell(sheet, row, 0, "Timestamp");
				SetHeaderCell(sheet, row, 1, "Level");
				SetHeaderCell(sheet, row, 2, "Message");

				row++;

				foreach (var logMessage in messages)
				{
					//SetDateTimeCell(sheet, row, 1, logMessage.Timestamp);
					SetCell(sheet, row, 0, logMessage.Timestamp.ToString());
					SetCell(sheet, row, 1, logMessage.Level);
					SetCell(sheet, row, 2, logMessage.RenderedMessage);

					row++;
				}

				var allCells = sheet.Cells[1, 1, messages.Count, 3];
				allCells.AutoFitColumns();

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