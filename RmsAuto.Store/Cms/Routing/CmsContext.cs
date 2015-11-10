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
using System.Collections.Specialized;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;

namespace RmsAuto.Store.Cms.Routing
{
	public class CmsContext
	{
		public CatalogItem CatalogItem { get; set; }
		public NameValueCollection PageParameters { get; set; }
		public Dictionary<string,object> CustomPageParameters { get; private set; }
		public List<BreadCrumbItem> BreadCrumbSuffix { get; private set; }
		public PageFields PageFields { get; set; }

		public CatalogItem[] CatalogPath
		{
			get
			{
				if( CmsContext.Current.CatalogItem != null )
					return UrlManager.CatalogItems.GetCatalogPathItems( CmsContext.Current.CatalogItem.CatalogItemID );
				else 
					return new CatalogItem[ 0 ];
			}
		}

		#region Constructor

		private CmsContext()
		{
			BreadCrumbSuffix = new List<BreadCrumbItem>();
			CustomPageParameters = new Dictionary<string, object>();
		}

		#endregion

		#region Singleton

		public static CmsContext Current
		{
			get
			{
				var res = (CmsContext)HttpContext.Current.Items[ _currentContextKey ];
				if( res == null )
					HttpContext.Current.Items[ _currentContextKey ] = res = new CmsContext();
				return res;
			}
		}
		private static object _currentContextKey = new object();

		#endregion

	}

}
