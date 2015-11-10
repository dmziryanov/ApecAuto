using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
    public partial class Address_Edit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string PostalCode
        {
            get { return _txtPostalCode.Text; }
            set { _txtPostalCode.Text = value; }
        }

        public string City
        {
            get { return _txtCity.Text; }
            set { _txtCity.Text = value; }
        }

        public string Region
        {
            get { return _txtRegion.Text; }
            set { _txtRegion.Text = value; }
        }

        public string Address
        {
            get { return _txtAddress.Text; }
            set { _txtAddress.Text = value; }
        }
    }
}