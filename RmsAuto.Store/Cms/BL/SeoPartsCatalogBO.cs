using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Cms.BL
{
	public static class SeoPartsCatalogBO
	{

		public static string[] GetVirtualPathById( int? seoPartsCatalogItemId )
		{
			if( seoPartsCatalogItemId == null )
			{
				return GetVirtualPathByItems( new SeoPartsCatalogItem[ 0 ] );
			}
			else
			{
				return GetVirtualPathByItems( SeoPartsCatalogDac.GetVirtualPathItemsById( seoPartsCatalogItemId ) );
			}
		}

		public static string[] GetVirtualPathByItems( IEnumerable<SeoPartsCatalogItem> items )
		{
			List<string> res = new List<string>();
			foreach( var item in items )
			{
				if( item.ParentID != null ) res.Add( item.UrlCode );
			}
			return res.ToArray();
		}
	}
}
