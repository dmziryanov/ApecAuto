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
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Routing;
using System.Net;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms.News
{
	public partial class NewsDetails : System.Web.UI.UserControl
	{

		public int ItemId
		{
			get { return ViewState[ "ItemId" ] != null ? (int)ViewState[ "ItemId" ] : 0; }
			set { ViewState[ "ItemId" ] = value; }
		}

		protected NewsItem _item;

		protected void Page_PreRender( object sender, EventArgs e )
		{
			_item = NewsItemsDac.GetNewsItem( ItemId );
			if( _item == null )
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );


			_allNewsLink.NavigateUrl = UrlManager.GetNewsListUrl();
		}
        protected string GetFileUrl(int fileID)
        {
            return UrlManager.GetFileUrl(fileID);
        }
	}
}