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
using RmsAuto.Store.Messages;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ManagerInfo : System.Web.UI.UserControl
	{
	    public int UnreadMessageCount {
	        get
	        {
                return new MessageRepository().GetUnreadCount(Convert.ToInt32(SiteContext.Current.User.UserId));
	        }
	    }

	    protected void Page_Load( object sender, EventArgs e )
		{
            _messageRepeater.DataSource = new MessageRepository().Get(Convert.ToInt32(SiteContext.Current.User.UserId), 0, 3);
            _messageRepeater.DataBind();
		}

		protected void _logoffButton_Click( object sender, EventArgs e )
		{
			LogonService.Logoff();
		}
	}
}