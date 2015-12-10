using System.Collections.Generic;
using System.Web.UI;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Cms.Catalog
{
	public partial class BreadCrumbs : UserControl
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