using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web
{
    public partial class PageThreeColumnsNEW : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((int?) Session["UserPageViewsCount"]).HasValue)
                Session["UserPageViewsCount"] = (int) Session["UserPageViewsCount"] + 1;
            else
                Session["UserPageViewsCount"] = 0;
        }
    }
}