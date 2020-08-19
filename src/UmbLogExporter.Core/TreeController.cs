using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace UmbLogExporter.Core
{
	[PluginController("UmbLogExporter")]
	[Tree(Constants.Applications.Settings, "umbLogExporter", TreeTitle = "Log Viewer/Exporter", SortOrder = 10, TreeGroup = Constants.Trees.Groups.Settings)]
	public class TreeControllerController : TreeController
	{
		protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
		{
			return new TreeNodeCollection();
		}

		protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
		{
			return null;
		}

		protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
		{
			var root = base.CreateRootNode(queryStrings);

			root.RoutePath = $"{Constants.Applications.Settings}/umbLogExporter/overview";

			root.Icon = "icon-box-alt";
			root.HasChildren = false;
			root.MenuUrl = null;

			return root;
		}
	}
}