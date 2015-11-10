using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class Phone_Edit : System.Web.UI.UserControl
    {
        public string Value
        {
            get { return HasValue ? /*_txtCode.Text +*/ _txtNumber.Text : string.Empty; }
        }

        public string DisplayValue
        {
            get { return HasValue ? string.Format(/*"({0}) {1}", _txtCode.Text,*/"{0}", _txtNumber.Text) : string.Empty; }
        }

        public bool HasValue
        {
            get 
            {
                return
                    //!string.IsNullOrEmpty(_txtCode.Text) && 
                    !string.IsNullOrEmpty(_txtNumber.Text); 
            }
        }

        public bool Required
        {
            get { return _numberReqValidator.Enabled;  }
            set { _numberReqValidator.Visible = value; }
        }

		//public bool CheckSummCount
		//{
		//    get { return _cvPhoneCount.Enabled; }
		//    set { _cvPhoneCount.Visible = value; }
		//}

		//protected void _cvPhoneCount_ServerValidate( object source, ServerValidateEventArgs args )
		//{
		//    args.IsValid = _txtCode.Text.Length + _txtNumber.Text.Length == 12 || _txtCode.Text.Length + _txtNumber.Text.Length == 0;
		//}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

    }
}