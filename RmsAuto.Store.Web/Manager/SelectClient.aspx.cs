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
using RmsAuto.Store.Acctg;

using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    public partial class SelectClient : RMMPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
            {
                ClientPicker1.Visible = false;
                ClientPicker2.Visible = true;
            }
            else
            {
                ClientPicker1.Visible = true;
                ClientPicker2.Visible = false;
            }
        }
    }
}
