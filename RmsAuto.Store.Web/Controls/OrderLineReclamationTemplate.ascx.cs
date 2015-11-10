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
using RmsAuto.Store.Configuration;
using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Controls
{
	public partial class OrderLineReclamationTemplate : System.Web.UI.UserControl
	{
		public string ThisPageUrl { get; set; }

		public bool ShowCustOrderNum { get; set; }

		protected string GetSparePartDetailsUrl( OrderLine line )
		{
			if( line != null )
			{
				SparePartPriceKey key = new SparePartPriceKey(
					line.Manufacturer,
					line.PartNumber,
					line.SupplierID );
				return UrlManager.GetSparePartDetailsUrl( key.ToUrlString() );
			}
			else
			{
				return null;
			}
		}

		protected string GetOrderDisplayNumber( Order order )
		{
			return OrderTracking.GetOrderDisplayNumber( order );
		}

		protected string GetStatusName( byte status )
		{
            return OrderLineStatusUtil.DisplayName(status);
		}

		protected string InitRedirectButton(byte currentStatus, DateTime orderLineStatusDate, int acctgOrderLineID)
		{
			if (ReclamationTracking.IsReclamationExist( acctgOrderLineID ))
			{
				btnToRequest.Enabled = false;
				return string.Empty;
			}
			else
			{

				btnToRequest.CommandName = "redirect";

				if (currentStatus == OrderLineStatusUtil.StatusByte( "ReceivedByClient" )) //запрос на возврат требует дополнительной проверки на разрешенный срок
				{
					TimeSpan ts = DateTime.Now - orderLineStatusDate;
					
					if (ts.Days > SiteContext.Current.CurrentClient.Profile.ReclamationPeriod)
					{
						btnToRequest.CommandName = "alert";
						btnToRequest.OnClientClick = string.Format( "window.alert('Все рекламации принимаются не позднее {0} календарных дней со дня отгрузки товара!');",
							SiteContext.Current.CurrentClient.Profile.ReclamationPeriod );
					}
				}

				return string.Empty;
			}
		}

		protected void btnToRequest_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			if ( btn.CommandName == "redirect" )
			{
				//Response.Redirect(UrlManager.GetReclamationRequestUrl(btn.CommandArgument), true);
				string url = string.Empty;
				switch (SiteContext.Current.User.Role)
				{
					case SecurityRole.Client:
						url = UrlManager.GetReclamationRequestUrl( btn.CommandArgument );
						break;
					case SecurityRole.Manager:
						url = string.Format(
							"~/Manager/ClientReclamationRequest.aspx?id={0}",
							HttpUtility.UrlEncode( btn.CommandArgument ) );
						break;
					default:
						throw new Exception( "Unknown user role" );
				}
				Response.Redirect( url, true );
			}
		}
 
		protected void Page_Load( object sender, EventArgs e )
		{
		}
	}
}