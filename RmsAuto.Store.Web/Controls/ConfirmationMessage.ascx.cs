using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace RmsAuto.Store.Web.Controls
{
    public partial class ConfirmationMessage : System.Web.UI.UserControl
    {
        public const string PasswordSuccessfullyCreated = "Пароль успешно создан";
        public const string UserAccountSuccessfullyActivated = "Учетная запись пользователя интернет-магазина активирована";
        public const string ClientProfileSuccessfullyCreated = "Профиль клиента успешно создан";

        public string MessageText
        {
            get { return (string)ViewState["_msgText"]; }
            set { ViewState["_msgText"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}