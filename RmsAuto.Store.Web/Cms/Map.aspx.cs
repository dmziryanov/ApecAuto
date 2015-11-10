using System;
using System.Linq;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms
{
    public partial class Map : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;

			var root = new TreeNode { Text = /*"Главная"*/ Resources.Texts.Main, NavigateUrl = "~" };
			_siteMapTreeView.Nodes.Add( root );

			AddCatalogItems( root );
			AddSeoPartManufacturers( root );
		}

		void AddCatalogItems( TreeNode root )
		{
			AddCatalogItems( root, UrlManager.CatalogItems.RootCatalogItem.CatalogItemID );
		}

		void AddCatalogItems( TreeNode parentNode, int catalogItemId )
		{
			foreach( var item in UrlManager.CatalogItems.GetCatalogItems( catalogItemId ) )
			{
				var node = new TreeNode
				{
					Text = item.CatalogItemName,
					NavigateUrl = UrlManager.GetCatalogUrl( item.CatalogItemID ),
					Target = item.CatalogItemOpenNewWindow ? "_blank" : null
				};
				parentNode.ChildNodes.Add( node );
				AddCatalogItems( node, item.CatalogItemID );
			}
		}

		void AddSeoPartManufacturers( TreeNode siteMapRoot )
		{
			AddSeoPartManufacturerNode(
				siteMapRoot,
				new SeoPartsCatalogItem[ 0 ],
				SeoPartsCatalogDac.GetSeoPartsCatalogTree() );
		}

		void AddSeoPartManufacturerNode( TreeNode parentNode, SeoPartsCatalogItem[] parents, SeoPartsCatalogItem item )
		{
			SeoPartsCatalogItem[] path = parents.Union( new[] { item } ).ToArray();
			var node = new TreeNode
			{
				Text = item.Name,
				NavigateUrl = UrlManager.GetSeoPartsCatalogUrl( path ),
				Expanded = false
			};
			parentNode.ChildNodes.Add( node );

			foreach( var child in item.ChildItems.OrderBy( i => i.Priority ).ThenBy( i => i.Name ) )
			{
				AddSeoPartManufacturerNode( node, path, child );
			}
		}
	}
}
