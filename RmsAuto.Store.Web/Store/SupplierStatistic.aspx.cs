using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using System.Net;

namespace RmsAuto.Store.Web
{
    public partial class SupplierStatisticPage : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                ChartImage.ImageUrl = UrlManager.GetSupplierStatUrl(Request.QueryString["ID"]);
		}
	}
}
