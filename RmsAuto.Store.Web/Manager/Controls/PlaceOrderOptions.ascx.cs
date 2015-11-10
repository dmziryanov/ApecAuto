using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Entities;
using RmsAuto.Common.Web.UI; 

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class PlaceOrderOptions : System.Web.UI.UserControl
    {
        public string ShippingAddress
        {
            get { return _shippingOptions.ShippingAddress; }
        }

        public PaymentMethod PaymentMethod
        {
            get { return _ddlPaymentOptions.GetSelectedEnumValue<PaymentMethod>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _ddlPaymentOptions.BindEnumeration(typeof(PaymentMethod), PaymentMethod.Cash);
            }
        }
    }
}