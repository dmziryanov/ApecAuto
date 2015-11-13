using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;
using RmsAuto.Store.Web;
using System.Threading;

namespace RmsAuto.Store.Cms.Dac
{
	public static class SeoPartsCatalogDac
	{
		private static Func<CmsDataContext, Table<SeoPartsCatalogItem>> _getSeoPartsCatalogItems =
			CompiledQuery.Compile(
			( CmsDataContext dc ) =>
				dc.SeoPartsCatalogItems );

		private static Func<CmsDataContext, SeoPartsCatalogItem> _getVisibleRootCatalogItem =
			CompiledQuery.Compile(
			( CmsDataContext dc ) =>
				dc.SeoPartsCatalogItems.SingleOrDefault( c => c.ParentID == null && c.UrlCode == "~" && c.Visible ) );

		private static Func<CmsDataContext, int, string,  SeoPartsCatalogItem> _getVisibleChildCatalogItem =
			CompiledQuery.Compile(
			( CmsDataContext dc, int parentId, string urlCode ) =>
				dc.SeoPartsCatalogItems.SingleOrDefault( c => c.ParentID == parentId && c.UrlCode == urlCode && c.Visible ) );

		private static Func<CmsDataContext, int, SeoPartsCatalogItem> _getCatalogItem =
			CompiledQuery.Compile(
			( CmsDataContext dc, int id ) =>
				dc.SeoPartsCatalogItems.SingleOrDefault( c => c.ID == id ) );

		private static Func<CmsDataContext, int, SeoPartsCatalogItem> _getCatalogItem2 =
			CompiledQuery.Compile(
			(CmsDataContext dc, int id) =>
				dc.SeoPartsCatalogItems.SingleOrDefault(c => c.ID == id));

		//private static DataLoadOptions _getCatalogItem2Dlo = GetCatalogItem2DataLoadOptions();
		
        private static DataLoadOptions GetCatalogItem2DataLoadOptions(string locale)
		{
			var res = new DataLoadOptions();
			res.LoadWith<SeoPartsCatalogItem>( c => c.NameRU );
			res.LoadWith<SeoPartsCatalogItem>( c => c.BodyRU );
			res.LoadWith<SeoPartsCatalogItem>( c => c.PageDescriptionRU );
			res.LoadWith<SeoPartsCatalogItem>( c => c.PageKeywordsRU );
			res.LoadWith<SeoPartsCatalogItem>( c => c.PageFooterRU );
			if (locale != "ru-RU")
			{
				res.LoadWith<SeoPartsCatalogItem>( c => c.SeoPartsCatalogItemsLocs );
				res.AssociateWith<SeoPartsCatalogItem>( c => c.SeoPartsCatalogItemsLocs.Where(i => i.Localization == locale) );
			}
			return res;
		}

		//private static DataLoadOptions GetCatalogItem2DataLoadOptions2(string locale)
		//{
		//    var res = new DataLoadOptions();
		//    res.LoadWith<SeoPartsCatalogItem>(c => c.NameRU);
		//    if (locale != "ru-RU")
		//    {
		//        res.LoadWith<SeoPartsCatalogItem>(c => c.SeoPartsCatalogItemsLocs);
		//        res.AssociateWith<SeoPartsCatalogItem>(c => c.SeoPartsCatalogItemsLocs.Where(i => i.Localization == locale));
		//    }
		//    return res;
		//}

		private static Func<CmsDataContext, int, IOrderedQueryable<SeoPartsCatalogItem>> _getVisibleChildCatalogItems =
			CompiledQuery.Compile(
			( CmsDataContext dc, int id ) =>
				dc.SeoPartsCatalogItems.Where( c => c.ParentID == id && c.Visible && !c.IsServicePage ).OrderBy( c => c.Priority ).ThenBy( c => c.Name ) );

		public static SeoPartsCatalogItem GetSeoPartsCatalogTree()
		{
			using( var dc = new DCFactory<CmsDataContext>() )
			{
				dc.DataContext.LoadOptions = GetCatalogItem2DataLoadOptions(SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/);
				dc.DataContext.DeferredLoadingEnabled = false;
				//var items = _getSeoPartsCatalogItems( dc.DataContext ).ToArray();
				var items = dc.DataContext.SeoPartsCatalogItems.ToArray();

				var root = items.SingleOrDefault( c => c.ParentID == null && c.UrlCode == "~" && c.Visible );

				if( root != null )
				{
					var dict = items.ToDictionary( i => i.ID );

					foreach( var item in items )
					{
						if( item.Visible && !item.IsServicePage && item.ParentID != null )
						{
							dict[ item.ParentID.Value ].ChildItems.Add( item );
						}
					}
				}

				return root;
			}
		}

		public static SeoPartsCatalogItem[] GetCatalogItemsByVirtualPath( string[] path )
		{
			using( var dc = new DCFactory<CmsDataContext>() )
			{
				dc.DataContext.LoadOptions = GetCatalogItem2DataLoadOptions(SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/);
				if (SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/ != "ru-RU")
				{
					dc.DataContext.DeferredLoadingEnabled = false;
				}
				var res = new List<SeoPartsCatalogItem>();
				var item = dc.DataContext.SeoPartsCatalogItems.SingleOrDefault(c => c.ParentID == null && c.UrlCode == "~" && c.Visible);
					//_getVisibleRootCatalogItem( dc.DataContext );
				if( item != null ) res.Add( item );
				for( int i = 0 ; i < path.Length && item != null ; ++i )
				{
					item = dc.DataContext.SeoPartsCatalogItems.SingleOrDefault(c => c.ParentID == item.ID && c.UrlCode == path[i] && c.Visible);
						//_getVisibleChildCatalogItem( dc.DataContext, item.ID, path[ i ] );
					if( item != null ) res.Add( item );
				}
				return item != null ? res.ToArray() : null;
			}
		}

		public static List<SeoPartsCatalogItem> GetVirtualPathItemsById( int? seoPartsCatalogItemId )
		{
			using( var dc = new DCFactory<CmsDataContext>() )
			{
				dc.DataContext.LoadOptions = GetCatalogItem2DataLoadOptions(SiteContext.CurrentCulture);
				if (SiteContext.CurrentCulture != "ru-RU")
				{
					dc.DataContext.DeferredLoadingEnabled = false;
				}
				List<SeoPartsCatalogItem> items = new List<SeoPartsCatalogItem>();
				if( seoPartsCatalogItemId != null )
				{
					List<SeoPartsCatalogItem> all = dc.DataContext.SeoPartsCatalogItems.ToList();
					for( var item = /*dc.DataContext.SeoPartsCatalogItems.*/all.SingleOrDefault( c => c.ID == seoPartsCatalogItemId.Value )/*_getCatalogItem( dc.DataContext, seoPartsCatalogItemId.Value )*/ ;
						item != null ;
						item = item.ParentItem )
					{
						items.Add( item );
					}
				}
				items.Reverse();
				return items;
			}
		}

		public static SeoPartsCatalogItem GetCatalogItem( int id )
		{
			using( var dc = new DCFactory<CmsDataContext>())
			{
				//dc.DataContext.LoadOptions = _getCatalogItem2Dlo;
				//Меняем LoadOptions в зависимости от локали и как следствие не можем пользоваться скомпилированным запросом
				dc.DataContext.LoadOptions = GetCatalogItem2DataLoadOptions(SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/);
				if (SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/ != "ru-RU")
				{
					dc.DataContext.DeferredLoadingEnabled = false;
				}
				return dc.DataContext.SeoPartsCatalogItems.SingleOrDefault(c => c.ID == id); //_getCatalogItem2(dc.DataContext, id);
			}
		}

		public static SeoPartsCatalogItem[] GetVisibleChildItems(int id)
		{
			using(var dc = new DCFactory<CmsDataContext>())
			{
				dc.DataContext.LoadOptions = GetCatalogItem2DataLoadOptions(SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/);
				if (SiteContext.CurrentCulture/*Thread.CurrentThread.CurrentCulture.Name*/ != "ru-RU")
				{
					dc.DataContext.DeferredLoadingEnabled = false;
				}
				return dc.DataContext.SeoPartsCatalogItems.Where(c => c.ParentID == id && c.Visible && !c.IsServicePage).OrderBy(c => c.Priority).ThenBy(c => c.NameRU).ToArray();
				//return _getVisibleChildCatalogItems( dc.DataContext, id).ToArray();
			}
		}
	}
}
