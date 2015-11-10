using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Routing;
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace RmsAuto.Store.Web.Cms.Catalog
{
	public partial class BreadCrumbs : System.Web.UI.UserControl
	{
		public bool RenderCurrentNodeAsLink
		{
			get;
			set;
		}


		protected int _itemsCount;
		protected override void Render( HtmlTextWriter writer )
		{
			List<BreadCrumbItem> items = new List<BreadCrumbItem>();

			//добавить ссылку на главную страницу
			items.Add( new BreadCrumbItem( UrlManager.CatalogItems.RootCatalogItem.CatalogItemName, "~" ) );

			//добавить путь до текущего раздела сайта
			foreach( var catalogItem in CmsContext.Current.CatalogPath )
			{
				items.Add( new BreadCrumbItem( catalogItem.CatalogItemName, UrlManager.GetCatalogUrl( catalogItem.CatalogItemID ) ) );
			}

			//добавить суффикс пути до текущей страницы
			items.AddRange( CmsContext.Current.BreadCrumbSuffix );

			_itemsCount = items.Count;

			_repeater.DataSource = items;
			_repeater.DataBind(); 
			
			base.Render( writer );
		}

	}
}