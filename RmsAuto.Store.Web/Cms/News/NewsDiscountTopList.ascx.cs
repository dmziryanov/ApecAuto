using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms.News
{
	public partial class NewsDiscountTopList : System.Web.UI.UserControl
	{
		[Browsable(true)]
		[DefaultValue(10)]
		public int TopCount
		{
			get { return ViewState["TopCount"] != null ? (int)ViewState["TopCount"] : 5; }
			set { ViewState["TopCount"] = value; }
		}

		protected string GetFileUrl(int fileID)
		{
			return UrlManager.GetFileUrl(fileID);
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			_repeater.DataSource = NewsItemsDac.GetTopDiscountNews(TopCount);
			_repeater.DataBind();
		}
	}
}