using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
	public partial class CartTop : System.Web.UI.UserControl
	{
		protected void Page_PreRender(object sender, EventArgs e)
		{
			lCartQty.Text = SiteContext.Current.CurrentClientTotals.PartsCount.ToString();
			lCartTotal.Text = string.Format("{0:### ### ##0.00}", SiteContext.Current.CurrentClientTotals.Total);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}