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

namespace RmsAuto.Store.Web.Cms.Controls
{
	public partial class PageFooter : System.Web.UI.UserControl
	{
		protected string _footerBody;

		protected override void Render( HtmlTextWriter writer )
		{
			if( CmsContext.Current.PageFields != null )
			{
				_footerBody = CmsContext.Current.PageFields.Footer;
			}
			else if( CmsContext.Current.CatalogItem != null )
			{
				_footerBody = CmsContext.Current.CatalogItem.PageFooter;
			}
			base.Render( writer );
		}
	}
}