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
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Configuration;
using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web.Manager;

namespace RmsAuto.Store.Web.Controls
{
    public partial class OrderLineTemplateToLoad : System.Web.UI.UserControl
    {
        public event EventHandler<ChangeReactionEventArgs> ChangeReaction;

        public string ThisPageUrl { get; set; }

        public bool ShowCustOrderNum { get; set; }

        protected void OnClientChangeReaction(object sender, ChangeReactionEventArgs e)
        {
            if (ChangeReaction != null)
                ChangeReaction(sender, e);
        }

        protected string GetSparePartDetailsUrl(OrderLine line)
        {
            if (line != null)
            {
                SparePartPriceKey key = new SparePartPriceKey(
                    line.Manufacturer,
                    line.PartNumber,
                    line.SupplierID);
                return UrlManager.GetSparePartDetailsUrl(key.ToUrlString());
            }
            else
            {
                return null;
            }
        }

        protected string GetOrderDisplayNumber(Order order)
        {
            return OrderTracking.GetOrderDisplayNumber(order);
        }

        protected string GetOrderLineTrackingUrl(int lineID)
        {
            switch (SiteContext.Current.User.Role)
            {
                case SecurityRole.Client:
                    return UrlManager.GetOrderLineTracking(lineID, ThisPageUrl);
                case SecurityRole.Manager:
                    return ClientOrderLineTracking.GetUrl(lineID, ThisPageUrl);
                default:
                    throw new Exception("Unknown user role");
            }
        }

        protected string GetReorderSearchUrl(object dataItem)
        {
            OrderLine line = (OrderLine)dataItem;
            switch (SiteContext.Current.User.Role)
            {
                case SecurityRole.Client:
                    if (line.Processed == (byte)Processed.NotProcessed)
                        return UrlManager.GetOrderListProcessedSearchRedirectURL(line.Manufacturer, line.PartNumber, true,
                            line.SupplierID, /*line.OrderLineID*/line.AcctgOrderLineID.Value);
                    else
                        return "";
                case SecurityRole.Manager:
                    if (line.Processed == (byte)Processed.NotProcessed)
                        return UrlManager.GetOrderListProcessedSearchRedirectURL(line.Manufacturer, line.PartNumber, true,
                            line.SupplierID, /*line.OrderLineID*/line.AcctgOrderLineID.Value);
                    else
                        return "";
                default:
                    throw new Exception("Invalid user role");
            }
        }

        protected string GetStatusName(byte status)
        {
            return OrderLineStatusUtil.DisplayName(status);
        }

        protected string GetStatusDescription(OrderLine order)
        {
            if (order.LastStatusChange != null)
            {
                if (!string.IsNullOrEmpty(order.LastStatusChange.StatusChangeInfo))
                {
                    return order.LastStatusChange.StatusChangeInfo;
                }
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool ShowReorderPlaceHolder(object dataItem)
        {
            OrderLine line = (OrderLine)dataItem;
            return (line.CurrentStatus == OrderLineStatusUtil.StatusByte("RefusedBySupplier") && line.Processed == (byte)Processed.NotProcessed);
        }

        protected bool ShowReorderFinishPlaceHolder(object dataItem)
        {
            OrderLine line = (OrderLine)dataItem;

            return (line.CurrentStatus == OrderLineStatusUtil.StatusByte("RefusedBySupplier") &&
                    (line.Processed == (byte)Processed.Client || line.Processed == (byte)Processed.Manager));
        }

        protected String GetReorder_Label_Text(object dataItem)
        {
            OrderLine line = (OrderLine)dataItem;
            switch (line.Processed)
            {
                case (byte)Processed.Manager:
                    return "обработано менеджером";
                default:
                case (byte)Processed.Client:
                    return "обработано клиентом";
            }
        }
    }
}