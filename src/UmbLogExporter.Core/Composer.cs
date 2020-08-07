using Umbraco.Core;
using Umbraco.Core.Composing;

namespace UmbLogExporter.Core
{
	public class Composer : IUserComposer
	{
		public void Compose(Composition composition)
		{
			composition.Register<ILogExportBuilder, LogExportBuilder>();
		}
	}
}