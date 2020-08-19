using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web.Trees;

namespace UmbLogExporter.Core
{
	public class TreeEvents : IComponent
	{
		public void Initialize()
		{
			TreeControllerBase.RootNodeRendering += TreeControllerBase_RootNodeRendering;
		}

		public void Terminate() { }

		void TreeControllerBase_RootNodeRendering(TreeControllerBase sender, TreeNodeRenderingEventArgs e)
		{
			if (sender.TreeAlias == Constants.Trees.LogViewer)
			{
				e.Node.CssClasses.Add("hide-section-tree");
			}
		}
	}
}