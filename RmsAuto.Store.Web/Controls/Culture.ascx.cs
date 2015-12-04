using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
    public partial class Culture : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteContext.CurrentCulture == "en-US") lnkCultureEN.CssClass = "active";
            if (SiteContext.CurrentCulture == "ru-RU") lnkCultureRU.CssClass = "active";
            if (SiteContext.CurrentCulture == "de-DE") lnkCultureDE.CssClass = "active";
        }

        protected void Culture_Click(object sender, EventArgs e)
        {
            SiteContext.CurrentCulture = ((LinkButton)sender).CommandArgument;
            Response.Redirect(Request.Url.OriginalString);
        }
    }
}