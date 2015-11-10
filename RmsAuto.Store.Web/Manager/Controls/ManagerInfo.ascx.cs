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

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ManagerInfo : System.Web.UI.UserControl
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			try
			{
				_managerNameLabel.Text = SiteContext.Current.UserDisplayName;
			}
			catch
			{
				//Если Ханса недоступна показываем логин менеджера
				_managerNameLabel.Text = SiteContext.Current.User.Identity.Name;
			}
		}

		protected void _logoffButton_Click( object sender, EventArgs e )
		{
			LogonService.Logoff();
		}
	}
}