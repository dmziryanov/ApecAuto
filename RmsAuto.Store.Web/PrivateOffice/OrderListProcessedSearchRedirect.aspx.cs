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
using RmsAuto.Common.Web;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class OrderListProcessedSearchRedirect : PageWithUrlState
	{
        protected void Page_PreRender(object sender, EventArgs e)
        {
            String url;
            String mfr = Request[UrlKeys.StoreAndTecdoc.ManufacturerName] ?? "";
            String pn = Request[UrlKeys.StoreAndTecdoc.EnteredPartNumber] ?? "";
            Boolean scp = Request[UrlKeys.StoreAndTecdoc.SearchCounterparts] == "1";
            Int32? exSID = !string.IsNullOrEmpty(Request[UrlKeys.StoreAndTecdoc.ExcludeSupplierID]) ? (int?)Convert.ToInt32(Request[UrlKeys.StoreAndTecdoc.ExcludeSupplierID]) : null;
            /*Int32? olid = !string.IsNullOrEmpty(Request[UrlKeys.OrderLineRequests.OrderLineId]) ? (int?)Convert.ToInt32(Request[UrlKeys.OrderLineRequests.OrderLineId]) : null;*/
			Int32? acolid = !string.IsNullOrEmpty( Request[UrlKeys.OrderLineRequests.AcctgOrderLineId] ) ? (int?)Convert.ToInt32( Request[UrlKeys.OrderLineRequests.AcctgOrderLineId] ) : null;

            switch (SiteContext.Current.User.Role)
            {
                case SecurityRole.Client:
                    if (/*olid*/acolid != null)
                        OrderBO.UpdateOrderLineProcessed((int)/*olid*/acolid, (byte) Processed.Client);
                    else
                        throw new ApplicationException("There is no OrderLineID in Request!");
                    if (exSID != null)
                        url = UrlManager.GetSearchSparePartsUrl(mfr, pn, scp, (int)exSID);
                    else
                        url = UrlManager.GetSearchSparePartsUrl(mfr, pn, scp);
                    Response.Redirect(url);
                    break;
                case SecurityRole.Manager:
                    if (/*olid*/acolid != null)
                    {
                        Order o = OrderTracking.GetOrderByAcctgOrderlineId(acolid);

                        var context = (ManagerSiteContext)SiteContext.Current;
                        if (!context.ClientSet.Contains(o.ClientID))
                            { context.ClientSet.AddClient(o.ClientID); }

                        context.ClientSet.SetDefaultClient(o.ClientID);

                        OrderBO.UpdateOrderLineProcessed((int)/*olid*/acolid, (byte)Processed.Manager);
                    }
                    else
                        throw new ApplicationException("There is no OrderLineID in Request!");
                    if (exSID != null)
                        url = ResolveUrl(Manager.Store.SearchSpareParts.GetUrl(mfr, pn, scp, (int)exSID));
                    else
                        url = ResolveUrl(Manager.Store.SearchSpareParts.GetUrl(mfr, pn, scp));
                    Response.Redirect(url);
                    break;
                default:
                    throw new Exception("Invalid user role");
            }
        }
	}
}
