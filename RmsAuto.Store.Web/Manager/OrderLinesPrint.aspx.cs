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
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class OrderLinesPrint : RMMPage
	{
        SellerInfo _seller;
                
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
		
        protected class PrintOrderLineInfo
		{
			public int Number { get; set; }
			public string PartNumber { get; set; }
			public string PartName { get; set; }
            public string Manufacturer { get; set; }
			public int Qty { get; set; }
			public decimal Price { get; set; }
			public decimal Total { get; set; }
		}

		public decimal SumTotal { get; set; }
		public int QtyTotal { get; set; }
        public string SupplierName;
        public string ClientName;
		/// <summary>
		/// Номер товарной
		/// </summary>
		public string InvoiceNumber { get; set; }

		protected void Page_PreRender(object sender, EventArgs e)
		{
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			//для проверки
			//Session["PrintOrderLineIDs"] = "82;83;72;79;76";
            RmsAuto.Store.Acctg.ClientProfile cl = RmsAuto.Store.Acctg.ClientProfile.Load((string)Session["ClientID"]);
			//номер накладной: ID клиента + / + часы минуты секунды без разделителя
			InvoiceNumber = cl.AcctgId + "/" + DateTime.Now.ToString("hhmmss");//DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            ClientName = string.Join(", ", new string[] { cl.ClientName, "ИНН " + cl.INN, "КПП " + cl.CorrespondentIBAN, cl.ShippingAddress, cl.ContactPhone });
			object source = Session["PrintOrderLineIDs"];
            _seller = GetSellerInfo();
            
            SupplierName = string.Join(", ", new string[] { _seller.CompanyName, "ИНН" + _seller.Inn, "КПП" + _seller.KPP, _seller.Address, _seller.Phone });
			if (source != null && !string.IsNullOrEmpty(source.ToString()))
			{
				string[] str = source.ToString().Split(';');
				int[] ids = Array.ConvertAll(str, s => int.Parse(s));

				PrintOrderLineInfo[] printLines = null; 
				using (var dc = new DCWrappersFactory<StoreDataContext>())
				{
					printLines = (from ol in dc.DataContext.OrderLines
								  where ids.Contains(ol.OrderLineID)
								  select new PrintOrderLineInfo()
								  {
									  Number = 0,
									  PartNumber = ol.PartNumber,
									  PartName = ol.PartName,
                                      Manufacturer = ol.Manufacturer,
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
			else
			{
				throw new ArgumentException("PrintOrderLineIDs");
			}
		}
	}
}
