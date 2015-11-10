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
using RmsAuto.Store.BL;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager
{
    public partial class ClientProfile : ClientBoundPage
    {
		public static string GetUrl()
		{
//            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
//                { return "~/Manager/ClientOrders.aspx"; }
//            else
                { return "~/Manager/ClientProfile.aspx"; }

		}

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.Profile; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
            {
                _clientProfileView.Visible = false; _clientProfileViewLite.Visible = true;
                
            }
            else
            {
                _clientProfileViewLite.Visible = false;
                _clientProfileView.Visible = true;
            }
                _clientProfileView.data = ClientData;
            _clientProfileViewLite.data = ClientData;
			_clientAccountInfo.Data = ClientData;
        }
    }
}
