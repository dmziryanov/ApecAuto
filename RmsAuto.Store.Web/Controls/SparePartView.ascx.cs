using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class SparePartView : System.Web.UI.UserControl
    {
        public string Manufacturer
        {
            get { return ((string)ViewState["__Manufacturer"]) ?? string.Empty; }
            set { ViewState["__Manufacturer"] = value; }
        }

        public string PartNumber
        {
            get { return ((string)ViewState["__PartNumber"]) ?? string.Empty; }
            set { ViewState["__PartNumber"] = value; }
        }

        public string PartName
        {
            get { return ((string)ViewState["__PartName"]) ?? string.Empty; }
            set { ViewState["__PartName"] = value; }
        }

        public string PartDescription
        {
            get { return ((string)ViewState["__PartDescription"]) ?? string.Empty; }
            set { ViewState["__PartDescription"] = value; }
        }

        public int SupplierId
        {
            get { return Convert.ToInt32(ViewState["__SupplierId"]); }
            set { ViewState["__SupplierId"] = value; }
        }

        public int DisplayDeliveryDaysMin
        {
            get { return Convert.ToInt32(ViewState["__DeliveryDaysMin"]); }
            set { ViewState["__DeliveryDaysMin"] = value; }
        }

        public int DisplayDeliveryDaysMax
        {
            get { return Convert.ToInt32(ViewState["__DeliveryDaysMax"]); }
            set { ViewState["__DeliveryDaysMax"] = value; }
        }

        public decimal? WeightPhysical
        {
            get 
            {
                object o = ViewState["__WeightPhysical"];
                return o != null ? (decimal)o : new decimal?();
            }
            set { ViewState["__WeightPhysical"] = value; }
        }

        public decimal? WeightVolume
        {
            get
            {
                object o = ViewState["__WeightVolume"];
                return o != null ? (decimal)o : new decimal?();
            }
            set { ViewState["__WeightVolume"] = value; }
        }

        public string QtyInStock
        {
            get
            {
                object o = ViewState["__QtyInStock"];
                return o != null ? (string)o : "NA";
            }
            set { ViewState["__QtyInStock"] = value; }
        }

        public int? MinOrderQty
        {
            get
            {
                object o = ViewState["__MinOrderQty"];
                return o != null ? (int)o : new int?();
            }
            set { ViewState["__MinOrderQty"] = value; }

        }

        public decimal? ActualPrice
        {
            get
            {
                object o = ViewState["__ActualPrice"];
                return o != null ? (decimal)o : new decimal?();
            }
            set { ViewState["__ActualPrice"] = value; }
        }
                
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}