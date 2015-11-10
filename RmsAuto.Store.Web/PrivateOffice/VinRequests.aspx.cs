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
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Configuration;
using RmsAuto.Common.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Common.Misc;
using System.Collections.Generic;

using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class VinRequests : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vrList.DataSource = VinRequestsDac.GetRequests(SiteContext.Current.CurrentClient.Profile.ClientId);
            vrList.DataBind();
        }

        protected void Page_PreRender(object source, EventArgs e)
        {
            // Можем ли получить доступ к VIN запросам?
            new VinRequestUtil().CanVinRequest(this);
        }
    }
}
