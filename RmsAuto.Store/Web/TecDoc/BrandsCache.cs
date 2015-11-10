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
using RmsAuto.Common.Caching;
using System.Collections.Generic;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.TecDoc
{
	class BrandsCache
	{
		public static BrandsCollection Default
		{
			get
			{
				var res = (BrandsCollection)HttpContext.Current.Items[ _key ];
				if( res == null )
				{
					res = _brandsCache.CachedObject._brandsCollection;
					HttpContext.Current.Items[ _key ] = res;
				}
				return res;
			}
		}

		BrandsCollection _brandsCollection = new BrandsCollection();


		#region Static

		static object _key = new object();
		static SingleObjectCache<BrandsCache> _brandsCache;

		static BrandsCache()
		{
			//TODO: управление настройкой срока кеширования
			_brandsCache = new SingleObjectCache<BrandsCache>(
				null,
				10 );
		}

		#endregion

	}


}
