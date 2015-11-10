using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class ClientOrdelinesLoad : ClientBoundPage, IPageUrlState
    {
        public static string GetUrl()
        {
            return "~/Manager/ClientOrdelinesLoad.aspx";
        }

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.ClientOrdelinesLoad; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region IPageUrlState Members

        public UrlStateContainer UrlStateContainer
        {
            get { return _urlStateContainer; }
        }
        
        UrlStateContainer _urlStateContainer;

        #endregion
    }
}
