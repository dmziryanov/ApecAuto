using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using System.Net;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class OptPaymentOrderPrint : RMMPage
	{

        protected SellerInfo _seller;
        protected RmsAuto.Store.Acctg.ClientProfile _profile;
        protected string _paymentName;
        protected decimal _sum;
        
        protected class PrintOrderLineInfo
		{
			public int Number { get; set; }
			public string PartNumber { get; set; }
			public string PartName { get; set; }
			public int Qty { get; set; }
			public decimal Price { get; set; }
			public decimal Total { get; set; }
		}

		public decimal SumTotal { get; set; }
		public int QtyTotal { get; set; }
        public string SupplierName;
        public string ClientName;

		protected void Page_PreRender(object sender, EventArgs e)
		{
			Response.Cache.SetCacheability(HttpCacheability.NoCache);

            int orderID = Convert.ToInt32(Request["OrderID"]);

            Order order = OrderBO.LoadOrderData(SiteContext.Current.CurrentClient.Profile.ClientId, orderID);
            if (order == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "Order not found");

            _seller = GetSellerInfo();//OrderService.GetSellerInfo();
            _profile = SiteContext.Current.CurrentClient.Profile;
            _sum = decimal.Round(order.Total * SiteContext.Current.CurrentClient.Profile.PrepaymentPercent / 100, 2);
            _paymentName = string.Format("{0}", OrderTracking.GetOrderDisplayNumber(order));


            SupplierName = _seller.CompanyName + "," + _seller.Inn + "," + _seller.KPP + "," + _seller.Address + "," + _seller.Phone;
            ClientName = string.Join(", ", new string[] { _profile.ClientName, _profile.BankINN, _profile.KPP, _profile.ShippingAddress, _profile.ContactPhone });
			//if (source != null && !string.IsNullOrEmpty(source.ToString()))
			{
			//	string[] str = source.ToString().Split(';');
			//	int[] ids = Array.ConvertAll(str, s => int.Parse(s));

				PrintOrderLineInfo[] printLines = null; 
				using (var dc = new DCFactory<StoreDataContext>())
				{
					printLines = (from ol in dc.DataContext.OrderLines
								  where ol.OrderID == orderID
								  select new PrintOrderLineInfo()
								  {
									  Number = 0,
									  PartNumber = ol.PartNumber,
									  PartName = ol.PartName,
									  Qty = ol.Qty,
									  Price = ol.UnitPrice,
									  Total = ol.Total
								  }).ToArray();

					Array.ForEach(printLines, (x) => x.Number = Array.IndexOf(printLines, x) + 1);
				}
				rptMain.DataSource = printLines;
				rptMain.DataBind();

				SumTotal = printLines.Sum(l => l.Total);
				QtyTotal = printLines.Count();
			}
			
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
            seller.Phone = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone;

            return seller;
        }
	}
}
