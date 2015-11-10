using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Controls
{
    public partial class ShippingOptions : System.Web.UI.UserControl
    {
        protected const string Method = "method";
        protected const string Pickup = "pickup";
        protected const string Address = "addr_";
        protected const string NewAddress = "addr_new";

        protected string _Method
        {
            get { return (string)ViewState["__method"]; }
            set { ViewState["__method"] = value; }
        }

        public string ShippingAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_Method))
                    return null;

                if (_Method == Pickup)
                    return string.Empty;
                else if (_Method == NewAddress && Page.IsValid)
                {
                    var address = _addressEdit.Text;
                    if (string.IsNullOrEmpty(address))
                        throw new Exception("не задан адрес доставки");
                    return address;
                }
                else
                {
                    int index;
                    try
                    {
                        index = int.Parse(_Method.Substring(_Method.IndexOf('_') + 1));
                    }
                    catch
                    {
                        throw new InvalidOperationException(_Method + " request arg is incorrect");
                    }
                    var repeaterItem = _profileAddresses.Items[index];
                    return ((Label)repeaterItem.FindControl("_lblAddress")).Text;
                }
            }
        }

        protected string StoreName
        {
            get { return _pickupStore != null ? _pickupStore.StoreName : string.Empty; }
        }

        protected string StoreAddress
        {
            get { return _pickupStore != null ? _pickupStore.Address : string.Empty; }
        }

        protected string StorePhone
        {
            get { return _pickupStore != null ? _pickupStore.Phone : string.Empty; }
        }

        private StoreInfo _pickupStore = AcctgRefCatalog.RmsStores[SiteContext.Current.CurrentClient.Profile.RmsStoreId];

        protected void Page_Load(object sender, EventArgs e)
        {
            RefreshMethod();
            if (!Page.IsPostBack)
            {
                BindAddressList();
                _Method = Pickup;
            }
        }

        private void BindAddressList()
        {
            if (SiteContext.Current.CurrentClient.Profile.IsLegalWholesale)
            {
                _newAddressRow.Visible = false;
            }
            else
            {
                var addressList = SiteContext.Current.CurrentClient.Profile.ShippingAddressList;

                if (addressList != null && addressList.Length > 0)
                {
                    _newAddressRow.Visible = false;
                    _profileAddresses.DataSource = addressList;
                    _profileAddresses.DataBind();
                }
                else
                    _newAddressRow.Visible = true;
            }
        }

        private void RefreshMethod()
        {
            var method = Request[Method];
            if (!string.IsNullOrEmpty(method))
               _Method = method;
        }

        protected void ValidateNewAddress(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Request[Method] != NewAddress || !string.IsNullOrEmpty(args.Value);
        }
    }
}