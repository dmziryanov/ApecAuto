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
using RmsAuto.Store.BL;
using System.Net;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class OrderPrint : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		public bool ShowPartNumbers
		{
			get { return Page.User.Identity.IsAuthenticated; }
		}

		public string Phone
		{
			get
			{
				return AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone;
			}
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			Response.Cache.SetCacheability( HttpCacheability.NoCache );

			var orderId = Convert.ToInt32( Request[ "OrderID" ] );
			var client = SiteContext.Current.CurrentClient;

			_clientNameLabel.Text = Server.HtmlEncode( client.Profile.ClientName );

			Order order = OrderBO.LoadOrderData( client.Profile.ClientId, orderId );
			if( order == null )
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );

            // deas 23.03.2011 task3419
            // печать слова копия при печати заказа в день отличный от размещения
            if ( order.OrderDate.Date == DateTime.Now.Date )
            {
                _orderCopy.Visible = false;
            }

			_orderNumLabel.Text = OrderTracking.GetOrderDisplayNumber( order );
			if( !string.IsNullOrEmpty( order.CustOrderNum ) )
			{
				_customOrderNumLiteral.Text = Server.HtmlEncode( order.CustOrderNum );
				_customOrderNumPlaceHolder.Visible = true;
			}
			else
			{
				_customOrderNumPlaceHolder.Visible = false;
			}
			var orderLines = order.OrderLines.AsQueryable().Where( OrderBO.TotalStatusExpression );
			int totalCount = orderLines.Sum( l=>l.Qty );
			decimal totalSum = orderLines.Sum( l => l.Total );
			_totalCountLiteral.Text = totalCount.ToString();
			_totalSumLiteral.Text = string.Format( "{0:### ### ##0.00}", totalSum );

			_cartItemsRepeater.DataSource = orderLines;
			_cartItemsRepeater.DataBind();
		}
	}
}
