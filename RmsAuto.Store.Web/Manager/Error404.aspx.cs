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

using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
	public partial class Error404 : RMMPage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			Exception exception = Server.GetLastError();

            // deas 05.03.2011 task3105
            // данная страница обязательно должна возвращать 404
            Response.StatusCode = 404;// exception is HttpException ? ( (HttpException)exception ).GetHttpCode() : 500;

			Title = string.Format( "{0} - {1}", Response.StatusCode, Response.StatusDescription );
		}
	}
}
