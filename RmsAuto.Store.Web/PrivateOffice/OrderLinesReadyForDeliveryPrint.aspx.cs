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
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class OrderLinesReadyForDeliveryPrint : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected string GetOrderDisplayNumber( Order order )
		{
			return OrderTracking.GetOrderDisplayNumber( order );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			Response.Cache.SetCacheability( HttpCacheability.NoCache );

			var profile = SiteContext.Current.CurrentClient.Profile;

			//var manager = AcctgRefCatalog.RmsEmployees[ profile.ManagerId ];
			//if( manager != null )
			//{
			//    _managerBlock.Visible = true;
			//    _managerNameLabel.Text = Server.HtmlEncode( manager.FullName );
			//    _managerPhoneLable.Text = Server.HtmlEncode( manager.Phone ?? "" );
			//}
			//else
			//{
			//    _managerBlock.Visible = false;
			//}

			try
			{
				lOfficePhoneValue.Text = Acctg.AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone;
			}
			catch { }
			//var shop = ShopsDac.GetShopByStoreId( profile.RmsStoreId );
			//if( shop != null )
			//{
			//    _storeBlock.Visible = true;
			//    _storeNameLabel.Text = Server.HtmlEncode( shop.ShopName );
			//    _storeAddressLabel.Text = shop.ShopAddress;
			//    _storePhonesLabel.Text = shop.ShopPhones;
			//    if( shop.ShopMapFileID.HasValue )
			//    {
			//        _mapPlaceHolder.Visible = true;
			//        _storeMapImage.ImageUrl = UrlManager.GetFileUrl( shop.ShopMapFileID.Value );
			//    }
			//    else
			//    {
			//        _mapPlaceHolder.Visible = false;
			//    }
			//}
			//else
			//{
			//    _mapPlaceHolder.Visible = false;
			//    _storeBlock.Visible = false;
			//}
	
			var filter = new OrderTracking.OrderLineFilter { OrderLineStatus =  OrderLineStatusUtil.StatusByte("ReadyForDelivery")};
			var totals = OrderTracking.GetOrderLinesCount(profile.ClientId,filter);
			_itemsRepeater.DataSource = OrderTracking.GetOrderLines( profile.ClientId, filter, OrderTracking.OrderLineSortFields.OrderIDAsc, 0, totals.TotalCount);
			_itemsRepeater.DataBind();

			_linesCountLabel.Text = totals.TotalCount.ToString();
			_totalSumLabel.Text = string.Format( "{0:### ### ##0.00}", totals.TotalSum );
		}
	}
}
