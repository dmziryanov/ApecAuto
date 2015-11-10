using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Store.Configuration;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Controls
{
    public partial class OrderLineTracking : System.Web.UI.UserControl
    {
		public int OrderLineId { get; set; }

		protected List<object> _list;
		protected void Page_PreRender( object sender, EventArgs e )
		{

			var currentOrderLine = OrderBO.LoadOrderLineData(
					SiteContext.Current.CurrentClient.Profile.ClientId,
					OrderLineId, true );

			_orderNumberLabel.Text = OrderTracking.GetOrderDisplayNumber( currentOrderLine.Order );
			_orderDateLabel.Text = string.Format( "{0:dd.MM.yyyy}", currentOrderLine.Order.OrderDate );
			_manufacturerLabel.Text = Server.HtmlEncode( currentOrderLine.Manufacturer );

			//подгрузить историю всей цепочки позиций
			List<OrderLine> lines = new List<OrderLine>();
			for( var orderLine = currentOrderLine ;
				orderLine != null ;
				orderLine = orderLine.ParentOrderLineID.HasValue ? OrderBO.LoadOrderLineData(
					SiteContext.Current.CurrentClient.Profile.ClientId,
					orderLine.ParentOrderLineID.Value, true ) : null )
			{
				lines.Add( orderLine );
			}
			lines.Reverse();

			_list = new List<object>();
			OrderLineStatusChange prevStatusChange = null;
			foreach( var orderLine in lines )
				foreach( var statusChange in orderLine.OrderLineStatusChanges.OrderBy( s => s.StatusChangeTime ) )
				{
					_list.Add(
						new
						{
							Data = statusChange,
							IsSameSparePart = prevStatusChange != null
												&& prevStatusChange.OrderLine.Manufacturer == statusChange.OrderLine.Manufacturer
												&& prevStatusChange.OrderLine.PartNumber == statusChange.OrderLine.PartNumber
												&& prevStatusChange.OrderLine.PartName == statusChange.OrderLine.PartName,
							IsSamePrice = prevStatusChange != null && prevStatusChange.OrderLine.UnitPrice == statusChange.OrderLine.UnitPrice,
							IsSameQty = prevStatusChange != null && prevStatusChange.OrderLine.Qty == statusChange.OrderLine.Qty,
							IsSameTotal = prevStatusChange != null && prevStatusChange.OrderLine.UnitPrice == statusChange.OrderLine.UnitPrice && prevStatusChange.OrderLine.Qty == statusChange.OrderLine.Qty,
							IsSameEstSupplyDate = prevStatusChange != null && prevStatusChange.OrderLine.EstSupplyDate == statusChange.OrderLine.EstSupplyDate
						} );
					prevStatusChange = statusChange;
				}

			_statusChangesRepeater.DataSource = _list;
			_statusChangesRepeater.DataBind();
		}

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

		protected string GetStatusName( byte status )
		{
		    return OrderLineStatusUtil.DisplayName(status);
		}
    }
}