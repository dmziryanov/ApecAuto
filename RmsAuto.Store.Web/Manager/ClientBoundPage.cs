using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    [Serializable()]
    public abstract class ClientBoundPage : RMMPage
    {
        protected ClientData ClientData
        {
            get { return SiteContext.Current.CurrentClient; }
        }
        
        protected override void OnInit(EventArgs e)
        {
            if (!((ManagerSiteContext)SiteContext.Current).ClientDataSectionEnabled(DataSection))
            {
                Response.Redirect("~/Manager/ClientAccessDenied.aspx", true);
            }

            base.OnInit(e);
        }

        public abstract ClientDataSection DataSection { get; }
    }
}
