using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Common.Caching;
using System.Collections.Generic;
using System.Web.Caching;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Cms.Routing
{
	class CatalogItemsCache
	{
		public static CatalogItemsDictionary Default
		{
			get
			{
				var res = (CatalogItemsDictionary)HttpContext.Current.Items[ _key];
				if( res == null )
				{
					res = _catalogItemsCache.CachedObject;
                    HttpContext.Current.Items[_key] = res;
				}
				return res;
			}
		}

		public static string CacheKey
		{
			get { return _catalogItemsCache.CacheKey; }
		}

		//CatalogItemsDictionary _catalogItemsDictionary = new CatalogItemsDictionary(/* _catalogItemsTableCache.Values*/ );


		#region Static

		static object _key = new object();
		//static KeyedEntityCache<int, CatalogItem> _catalogItemsTableCache;
		static SingleObjectCache<CatalogItemsDictionary> _catalogItemsCache;

        static CatalogItemsCache()
        {
            //_catalogItemsTableCache = new KeyedEntityCache<int, CatalogItem>(
            //    global::RmsAuto.Store.Cms.Properties.Settings.Default.ex_rmsauto_storeConnectionString,
            //    c => c.CatalogItemID );

            _catalogItemsCache = new SingleObjectCache<CatalogItemsDictionary>(
                null/*new string[] { _catalogItemsTableCache.CacheKey }*/,
                0);
        }

		#endregion

	}

}
