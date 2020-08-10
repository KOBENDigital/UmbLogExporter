using System.Collections.Generic;
using System.IO;
using Umbraco.Core.Logging.Viewer;

namespace UmbLogExporter.Core
{
	public interface ILogExportBuilder
	{
		void ProcessData(Stream stream, LogTimePeriod timePeriod, IEnumerable<LogMessage> messages);
	}
}