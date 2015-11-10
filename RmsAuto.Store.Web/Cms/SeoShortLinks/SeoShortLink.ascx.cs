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
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Dac;
using System.ComponentModel;

namespace RmsAuto.Store.Web.Cms.SeoShortLinks
{
    public partial class SeoShortLink : System.Web.UI.UserControl
    {
        public int SeoLinksID
        {
            get { return (int)ViewState["SeoLinksID"]; }
            set { ViewState["SeoLinksID"] = value; }
        }

        public string NameLink
        {
            get { return (string)ViewState["NameLink"]; }
            set { ViewState["NameLink"] = value; }
        }

        public string Url
        {
            get { return (string)ViewState["Url"]; }
            set { ViewState["Url"] = value; }
        }

        protected void Page_PreRender( object sender, EventArgs e )
        {
            var seoShortLink = SeoShortLinksDac.GetSeoShortLink( SeoLinksID );

            hlSeoLink.Text = seoShortLink.NameLink;
            hlSeoLink.NavigateUrl = seoShortLink.Url;
        }
    }
}