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
using RmsAuto.Store.Cms.BL;
using System.ComponentModel;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms.News
{
	public partial class NewsTopList : System.Web.UI.UserControl
	{
		[Browsable(true)]
		[DefaultValue(10)]
		public int TopCount
		{
			get { return ViewState[ "TopCount" ] != null ? (int)ViewState[ "TopCount" ] : 4; }
			set { ViewState[ "TopCount" ] = value; }
		}

		protected string GetFileUrl( int fileID )
		{
			return UrlManager.GetFileUrl( fileID );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			var nItems = NewsItemsDac.GetTopNews(TopCount);

			_repeaterCol1.DataSource = nItems.Take(2);
			_repeaterCol1.DataBind();

			_repeaterCol2.DataSource = nItems.Skip(2).Take(2);
			_repeaterCol2.DataBind();

			_allNewsLink.NavigateUrl = UrlManager.GetNewsListUrl();
		}
	}
}