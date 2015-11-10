using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public partial class Logon : LogonBaseControl
	{
		public override void Focus()
		{
			_txtLogin.Focus();
		}
        protected void Page_Load(object sender, EventArgs e)
        {
			_registerLink.NavigateUrl = UrlManager.GetRegistrationUrl();
            _recoverPasswordLink.NavigateUrl = UrlManager.GetPasswordRecoveryUrl();
        }

        protected void _btnLogin_Click(object sender, EventArgs e)
        {
            ProceedLogon(_txtLogin.Text, _txtPassword.Text, _chkPersist.Checked);
           
        }

        public override string ErrorMessage
        {
            get
            {
                return _errorMessage.InnerText;
            }
            set
            {
                _errorMessage.InnerText = value;
            }
        }
    }
}