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
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using System.Net;
using RmsAuto.Store.Configuration;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class PaymentOrderPrint : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected SellerInfo _seller;
		protected ClientProfile _profile;
		protected string _paymentName;
		protected decimal _sum;

		protected void Page_PreRender(object sender, EventArgs e)
		{
			Response.Cache.SetCacheability(HttpCacheability.NoCache);

			int orderID = Convert.ToInt32(Request["OrderID"]);

			Order order = OrderBO.LoadOrderData(SiteContext.Current.CurrentClient.Profile.ClientId, orderID );
			if (order == null)
				throw new HttpException((int)HttpStatusCode.NotFound , "Order not found" );

			_seller = GetSellerInfo();//OrderService.GetSellerInfo();
			_profile = SiteContext.Current.CurrentClient.Profile;
			_sum = decimal.Round( order.Total * SiteContext.Current.CurrentClient.Profile.PrepaymentPercent / 100, 2 );
			_paymentName = string.Format( "{0}", OrderTracking.GetOrderDisplayNumber( order ) );
		}

		private SellerInfo GetSellerInfo()
		{
			SellerInfo seller = new SellerInfo();
            //seller.BankAccount = /*OurDetailsConfiguration.Current.BankAccount;*/ AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].BankAccount;
            //seller.BankCode = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].BankCode;
            //seller.BankName = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].BankName;
            seller.CompanyName = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Name;
            //seller.CorrAccount = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].CorrAccount;
            //seller.Inn = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].INN;
            //seller.KPP = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].KPP;
            
			return seller;
		}
	}
}
