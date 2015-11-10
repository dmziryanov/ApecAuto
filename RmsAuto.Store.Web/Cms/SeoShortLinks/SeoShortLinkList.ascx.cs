using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms.SeoShortLinks
{
    public partial class SeoShortLinkList : System.Web.UI.UserControl
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            try
            {
				var links = SeoShortLinksDac.GetLastLink( 8, SiteContext/*.Current*/.CurrentCulture );
				if (links.Count == 0) { this.Visible = false; return; }

				lvShortLinks.DataSource = links;
				lvShortLinks.DataBind();
            }
            catch
            {
                Visible = false;
            }
        }
    }
}