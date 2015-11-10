using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
    [ValidationProperty("Value")]
    public partial class DiscountCard_Edit : System.Web.UI.UserControl
    {
        public string Value
        {
            get { return HasValue ? _txtCardNumberPart1.Text.ToUpper() + _txtCardNumberPart2.Text + _txtCardNumberPart3.Text : string.Empty; }
        }

        public string DisplayValue
        {
            get { return HasValue ? string.Format("{0} {1} {2}", _txtCardNumberPart1.Text.ToUpper(), _txtCardNumberPart2.Text, _txtCardNumberPart3.Text) : string.Empty; }
        }

        public bool HasValue
        {
            get
            {
                return
                    !string.IsNullOrEmpty(_txtCardNumberPart1.Text) ||
                    !string.IsNullOrEmpty(_txtCardNumberPart2.Text) ||
                    !string.IsNullOrEmpty(_txtCardNumberPart3.Text);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}