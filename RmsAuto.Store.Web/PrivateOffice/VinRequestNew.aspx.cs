using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Configuration;
using RmsAuto.Common.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Common.Misc;
using System.Collections.Generic;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class VinRequestNew : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        public StoreDataContext DataContext { get; private set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.DataContext = (new DCWrappersFactory<StoreDataContext>()).DataContext;
        }
        
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.DataContext != null)
                this.DataContext.Dispose();
        }

        public void vrNewSaveRequest(object sender, VinRequestEventArgs e)
        {
            if (IsValid)
            {
                VinRequest rq = e.VinRequest;

                rq.RequestDate = DateTime.Now;
                var profile = SiteContext.Current.CurrentClient.Profile;
                rq.ClientId = profile.ClientId;
                rq.StoreId = profile.RmsStoreId;
				rq.ManagerId = profile.ManagerId;

                this.DataContext.VinRequests.InsertOnSubmit(rq);
                this.DataContext.SubmitChanges();

                vrNew.Visible = false;
                _pValidResult.Visible = true;

                //Response.Redirect(UrlManager.GetVinRequestDetailsUrl(rq.Id));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            new VinRequestUtil().CanVinRequest(this);
        }
    }
}
