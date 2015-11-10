using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
	public partial class ClientShortInfo : System.Web.UI.UserControl
	{
		private string CurrentClientName
		{
			get
			{
				try { return SiteContext.Current.CurrentClient.Profile.ClientName; }
				catch { return SiteContext.Current.User.Identity.Name; }
			}
		}
		private string CurrentClientID
		{
			get
			{
				try { return SiteContext.Current.CurrentClient.Profile.ClientId; }
				catch { return SiteContext.Current.User.AcctgID; }
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			divMember.Visible = false;
			if (Context.User.Identity.IsAuthenticated)
			{
				lClientName.Text = CurrentClientName;
				lClientID.Text = CurrentClientID;
				divMember.Visible = true;
			}
		}
	}
}