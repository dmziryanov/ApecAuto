using System;
using System.Configuration;
using System.Web.UI;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public partial class RightMenu : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //_finmarketPlaceHolder.Visible = (ConfigurationManager.AppSettings["CountersOn"] ?? "").ToLower() == "true";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
			//try
			//{
			//	//BannerControlPos1.CatalogItemID = CmsContext.Current.CatalogItem.CatalogItemID;
			//	//BannerControlPos1.Position = 1;
			//	//BannerControlPos1.ShowImage = true;
			//}
			//catch (Exception)
			//{
			//	// если баннер отобразить не получилось, не значит что надо падать
			//}
			//try
			//{
			//	if( Page.User.Identity.IsAuthenticated && Context.User.IsInRole( "Client" ) )
			//	{
			//		_btnViewCartLink.Visible = true;
			//		_btnViewCartLink.NavigateUrl = UrlManager.GetCartUrl();
			//		_imgViewCartLink.NavigateUrl = UrlManager.GetCartUrl();

			//		if( SiteContext.Current.CurrentClientTotals.PartsCount != 0 )
			//		{
			//			_cartCountLiteral.Text = SiteContext.Current.CurrentClientTotals.PartsCount.ToString();
			//			_cartSumLiteral.Text = string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClientTotals.Total );
			//			_cartInfoLabel.Visible = true;
			//			_cartEmptyLabel.Visible = false;
			//		}
			//		else
			//		{
			//			_cartInfoLabel.Visible = false;
			//			_cartEmptyLabel.Visible = true;
			//		}

			//		_cartPlaceHolder.Visible = true;
			//	}
			//	else
			//	{
			//		_cartPlaceHolder.Visible = false;
			//	}
			//}
			//catch
			//{
			//	_cartPlaceHolder.Visible = false;
			//}
        }
    }
}