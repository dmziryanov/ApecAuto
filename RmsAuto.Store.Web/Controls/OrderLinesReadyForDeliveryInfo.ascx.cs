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
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
	public partial class OrderLinesReadyForDeliveryInfo : System.Web.UI.UserControl
	{
		protected void Page_PreRender( object sender, EventArgs e )
		{
			int linesCount;
			decimal totalSum;
			OrderTracking.GetOrderLinesSummary( SiteContext.Current.CurrentClient.Profile.ClientId, OrderLineStatusUtil.StatusByte("ReadyForDelivery"), out linesCount, out totalSum );

			if( linesCount != 0 )
			{
				_readyForDeliveryLink.Text = string.Format(
					/*"Готово к выдаче {0} позиций на сумму {1:### ### ##0.00} руб.",*/
					Resources.Texts.OrderLinesReadyForDelivery + " " + Resources.Texts.DollarShort,
					linesCount,
					totalSum );
				_readyForDeliveryLink.NavigateUrl = UrlManager.GetOrderLinesReadyForDeliveryPrintUrl();
				_placeHolder.Visible = true;
			}
			else
			{
				_placeHolder.Visible = false;
			}

		}
	}
}