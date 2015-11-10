using System;
using System.Web;
using System.Web.UI;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public class VinRequestUtil : System.Web.UI.Page
    {
        public void CanVinRequest(Page page)
        {
            if (ClientBO.CanVinRequest(SiteContext.Current.CurrentClient.Profile))
                page.Response.Redirect(UrlManager.MakeAbsoluteUrl(UrlManager.GetVinRequestAccessDenied()));
        }
    }
}