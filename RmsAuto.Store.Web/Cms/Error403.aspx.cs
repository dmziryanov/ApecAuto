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
    public partial class Error403 : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			Exception exception = Server.GetLastError();

            // deas 05.03.2011 task3105
            // данная страница обязательно должна возвращать 403
            Response.StatusCode = 403;// exception is HttpException ? ( (HttpException)exception ).GetHttpCode() : 500;

			Title = string.Format( "{0} - {1}", Response.StatusCode, Response.StatusDescription );
		}
	}
}
