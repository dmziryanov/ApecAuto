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
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Cms.Controls
{
	public partial class MetaTags : System.Web.UI.UserControl
	{
		protected string _description;
		protected string _keywords;

		protected override void Render( HtmlTextWriter writer )
		{
			if( CmsContext.Current.PageFields != null )
			{
				Page.Title = CmsContext.Current.PageFields.Title;
				_description = CmsContext.Current.PageFields.Description;
				_keywords = CmsContext.Current.PageFields.Keywords;
			}
			else if( CmsContext.Current.CatalogItem != null )
			{
				Page.Title = CmsContext.Current.CatalogItem.PageTitle ?? CmsContext.Current.CatalogItem.CatalogItemName;
				_description = CmsContext.Current.CatalogItem.PageDescription;
				_keywords = CmsContext.Current.CatalogItem.PageKeywords;
			}
			else
			{
				Page.Title = "Главная";
			}
			base.Render( writer );
		}
	}
}