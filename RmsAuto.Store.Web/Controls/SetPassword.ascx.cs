using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace RmsAuto.Store.Web.Controls
{
    public partial class SetPassword : UserControl
    {
        public string PasswordValue
        {
            get { return _txtPassword1.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}