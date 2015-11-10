using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using RmsAuto.Common.Web.UI;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public partial class WebUserMaintRequest : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                CaptureImage.RefreshCapture();
        }

        protected void _btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                ClientBO.SubmitPasswordRecoveryRequest(
                    _txtEmail.Text,
                    activationCode => UrlManager.GetClientActivationUrl(activationCode, SiteContext.Current.InternalFranchName));
                _emailPanel.Visible = false;
                ConfirmationMessage1.Visible = true;
            }
            catch (BLException ex)
            {
                _errorMsg.InnerText = ex.Message;
            }
        }
    }
}