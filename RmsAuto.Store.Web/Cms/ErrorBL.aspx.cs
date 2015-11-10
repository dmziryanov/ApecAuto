using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace RmsAuto.Store.Web.Cms
{
    public partial class ErrorBL : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
            Response.StatusCode = 500;
            Response.StatusDescription = "Ошибка сервера.";
            Title = string.Format( "{0} - {1}", Response.StatusCode, Response.StatusDescription );
            if ( Session["ErrorBL"] != null )
            {
                _mvDetailError.ActiveViewIndex = 1;
                _errorLabel.Text = Session["ErrorBL"].ToString();
                Session["ErrorBL"] = null;
            }
		}
	}
}
