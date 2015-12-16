using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Messages;

namespace RmsAuto.Store.Web.Manager
{
    public partial class MailBox : System.Web.UI.Page
    {
        public int UnreadMessageCount
        {
            get
            {
                return new MessageRepository().GetUnreadCount(Convert.ToInt32(SiteContext.Current.User.UserId));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            _messageRepeater.DataSource = new MessageRepository().Get(Convert.ToInt32(SiteContext.Current.User.UserId), 0, 10);
            _messageRepeater.DataBind();
        }
    }
}