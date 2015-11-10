using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Acctg;
using System.IO;

namespace RmsAuto.Store.Web.Controls
{
    public partial class AcctgPing : System.Web.UI.UserControl
    {
#if DEBUG
        protected void Page_Load(object sender, EventArgs e)
		{
            ltLastModifiedDate.Text = String.Format("<font style=\"font-size:9px\">Версия от {0}</font>", File.GetLastWriteTime(Server.MapPath("bin/RmsAuto.Store.Web.dll")).ToString());
		}
#endif
    }
}