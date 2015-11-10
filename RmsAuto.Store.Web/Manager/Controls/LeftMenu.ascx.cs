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
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class LeftMenu : System.Web.UI.UserControl
    {

		protected void Page_Load(object sender, EventArgs e)
		{
			//_vinRequestsLink.NavigateUrl = RmsAuto.Store.Web.Manager.VinRequestList.GetUrl();
			if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
			{
				hlSelectClient.Text = Resources.Texts.CustomersList;
			}
			else
			{
				hlSelectClient.Text = Resources.Texts.SearchСustomer;
				hlAllOrders.Visible = false;
				hlUploadStatuses.Visible = false;
                hlClientLoad.Visible = false;
                hlClientPayments.Visible = false;
                p_reports.Visible = false;
                p_reports_header.Visible = false;
			}
		}
    }
}