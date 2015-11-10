using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.Cms
{
    public partial class ManufacturerDetails : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			_manufacturerDetails.ManufacturerUrlCode = CmsContext.Current.PageParameters[ "UrlCode" ];
		}
	}
}
