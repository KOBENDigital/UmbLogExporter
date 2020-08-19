using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Core.Logging.Viewer;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace UmbLogExporter.Core
{
	[PluginController("UmbLogExporter")]
	public class LogExporterController : UmbracoAuthorizedApiController
	{
		private readonly ILogExportBuilder _logExportBuilder;
		private readonly ILogger _logger;
		private readonly ILogViewer _logViewer;

		public LogExporterController(ILogExportBuilder logExportBuilder, ILogger logger, ILogViewer logViewer)
		{
			_logExportBuilder = logExportBuilder ?? throw new ArgumentNullException(nameof(logExportBuilder));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_logViewer = logViewer ?? throw new ArgumentNullException(nameof(logViewer));
		}

		[HttpGet]
		public HttpResponseMessage Export(string orderDirection = "Descending", string filterExpression = null, [FromUri]string[] logLevels = null, [FromUri] DateTime? startDate = null, [FromUri] DateTime? endDate = null)
		{
			var sleeps = new[] { 1000, 2000, 3000, 4000, 5000, 6000 };
			var rand = new Random();
			//System.Threading.Thread.Sleep(sleeps[rand.Next(sleeps.Length)]);

			try
			{
				var logTimePeriod = GetTimePeriod(startDate, endDate);

				if (CanViewLogs(logTimePeriod) == false)
				{
					throw new HttpResponseException(Request.CreateNotificationValidationErrorResponse("Unable to export logs, due to size"));
				}

				var direction = orderDirection == "Descending" ? Direction.Descending : Direction.Ascending;

				var items = new List<LogMessage>();
				var pageNumber = 1;
				const int pageSize = 2000;

				var results = _logViewer.GetLogs(logTimePeriod, pageNumber, pageSize, direction, filterExpression, logLevels);
				items.AddRange(results.Items);

				while (pageNumber < results.TotalPages)
				{
					pageNumber++;
					results = _logViewer.GetLogs(logTimePeriod, pageNumber, pageSize, direction, filterExpression, logLevels);
					items.AddRange(results.Items);
				}

				if (!items.Any())
				{
					throw new HttpResponseException(Request.CreateNotificationValidationErrorResponse("Unable to export logs, no messages were found"));
				}

				var stream = new MemoryStream();

				var filename = $@"log-export-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

				_logExportBuilder.ProcessData(stream, logTimePeriod, items);

				stream.Position = 0;

				var result = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StreamContent(stream)
				};

				result.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
				{
					FileName = filename
				};

				return result;
			}
			catch (Exception ex)
			{
				_logger.Error<LogExporterController>("Failed to export", ex);
				throw new HttpResponseException(Request.CreateNotificationValidationErrorResponse("Unable to export logs, internal server error"));
			}
		}

		private static LogTimePeriod GetTimePeriod(DateTime? startDate, DateTime? endDate)
		{
			if (startDate == null || endDate == null)
			{
				var now = DateTime.Now;
				if (startDate == null)
				{
					startDate = now.AddDays(-1);
				}

				if (endDate == null)
				{
					endDate = now;
				}
			}

			return new LogTimePeriod(startDate.Value, endDate.Value);
		}

		private bool CanViewLogs(LogTimePeriod logTimePeriod)
		{
			return _logViewer.CanHandleLargeLogs || _logViewer.CheckCanOpenLogs(logTimePeriod);
		}
	}
}