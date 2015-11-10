using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Controls
{
    public partial class EditUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
        }
                
        public string Username
        {
            get { return _txtUsername.Text; }
        }

        public string Password
        {
            get { return _txtPassword1.Text; }
        }

        protected void ValidateLogin(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !ClientBO.LoginInUse(args.Value);
        }
    }
}