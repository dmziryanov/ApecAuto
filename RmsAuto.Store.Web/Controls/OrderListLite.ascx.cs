using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Data;
using RmsAuto.Store.Web.Manager;


namespace RmsAuto.Store.Web.Controls
{
    public partial class OrderListLite : System.Web.UI.UserControl
    {
        public OrderTracking.OrderStatusFilter OrderStatusFilter
		{
			get { return (OrderTracking.OrderStatusFilter)Convert.ToInt32( _objectDataSource.SelectParameters[ "statusFilter" ].DefaultValue ); }
			set { _objectDataSource.SelectParameters[ "statusFilter" ].DefaultValue = Convert.ToString( (int)value ); }
		}

		public OrderTracking.OrderSortFields OrderSortField
		{
			get { return (OrderTracking.OrderSortFields)Convert.ToInt32( _objectDataSource.SelectParameters[ "sort" ].DefaultValue ); }
			set { _objectDataSource.SelectParameters[ "sort" ].DefaultValue = Convert.ToString( (int)value ); }
		}

		public bool ShowPaymentOrderLink
		{
			get
			{
				return OrderStatusFilter == RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders
                    && SiteContext.Current.CurrentClient.Profile.PrepaymentPercent != 0 && SiteContext.Current.InternalFranchName == "rmsauto" ;
			}
		}

		public string GetPaymentOrderPrintUrl( int orderID )
		{
			return UrlManager.GetPaymentOrderPrintUrl( orderID );
		}

		#region Load OrderLines

		private static OrderTracking.OrdersList GetOrdersList( int statusFilter, int sort, int startIndex, int size )
		{
			string key = "RmsAuto.Store.Web.Controls.OrderList.GetOrdersList";

			OrderTracking.OrdersList res = (OrderTracking.OrdersList)HttpContext.Current.Items[ key ];
			if( res == null )
			{
				HttpContext.Current.Items[ key ] = res = OrderTracking.GetOrders(
					   SiteContext.Current.CurrentClient.Profile.ClientId,
					   (OrderTracking.OrderStatusFilter)statusFilter,
					   (OrderTracking.OrderSortFields)sort,
					   startIndex,
					   size );
			}
			return res;
		}

        public static void UpdateOrders(int statusFilter, int sort, int startIndex, int size)
        {

        }

		public static int GetOrdersCount( int statusFilter, int sort, int startIndex, int size )
		{
			return GetOrdersList( statusFilter, sort, startIndex, size ).TotalCount;
		}

		public static Order[] GetOrders( int statusFilter, int sort, int startIndex, int size )
		{
			return GetOrdersList( statusFilter, sort, startIndex, size ).Orders;
		}

		#endregion

		protected string GetThisUrl()
		{
			return ( (IPageUrlState)Page ).UrlStateContainer.GetPageUrl();
		}
	

		protected void Page_Load( object sender, EventArgs e )
		{
			if( !IsPostBack )
			{
				//сортировка
				foreach( OrderTracking.OrderSortFields sortField in Enum.GetValues( typeof( OrderTracking.OrderSortFields ) ) )
				{
					_sortBox.Items.Add( new ListItem( sortField.ToTextOrName(), ( (int)sortField ).ToString() ) );
				}
				_sortBox.SelectedValue = Convert.ToString( (int)OrderTracking.OrderSortFields.OrderIdDesc );
				
				//восстановить состояние
				if( !string.IsNullOrEmpty( Request[ "view" ] ) )
				{
					var view = (PrivateOffice.OrderList.Views)int.Parse( Request[ "view" ] );
					if( view == PrivateOffice.OrderList.Views.ActiveOrdersView && OrderStatusFilter == OrderTracking.OrderStatusFilter.ActiveOrders
						|| view == PrivateOffice.OrderList.Views.ArchiveOrdersView && OrderStatusFilter == OrderTracking.OrderStatusFilter.ArchiveOrders )
					{
						if( !string.IsNullOrEmpty( Request[ "sort" ] ) )
							_sortBox.SelectedValue = Request[ "sort" ];
						if( !string.IsNullOrEmpty( Request[ "start" ] ) && !string.IsNullOrEmpty( Request[ "size" ] ) )
						{
							_dataPager.SetPageProperties( int.Parse( Request[ "start" ] ), int.Parse( Request[ "size" ] ), false );
							_pageSizeBox.SelectedValue = Request[ "size" ];
						}
					}
				}
			}


			SaveUrlState();
		}

        public List<OrderLineStatuses> GetStatuses()
        {
            return OrderLineStatusUtil.LiteStatuses.Where(x => x.ClientShow == true && ((new int[] { 40, 200  }).Contains(x.OrderLineStatusID) || x.OrderLineStatusID > 160 && x.OrderLineStatusID < 170) ).ToList();
        }

		void SaveUrlState()
		{
			//сохранить информацию о состоянии
			IPageUrlState page = (IPageUrlState)Page;
			page.UrlStateContainer[ "sort" ] = _sortBox.SelectedValue;
			page.UrlStateContainer[ "start" ] = _dataPager.StartRowIndex.ToString();
			page.UrlStateContainer[ "size" ] = _dataPager.PageSize.ToString();
		}

		protected void _sortBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
		}

		protected void _pageSizeBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, Convert.ToInt32( _pageSizeBox.SelectedValue ), true );
		}

		protected void _showOrdersButton_Click( object sender, EventArgs e )
		{
			if( !string.IsNullOrEmpty( Request[ "order_id" ] ) )
			{
				int[] ids = Request[ "order_id" ].Split( ',' ).Select( s => int.Parse( s ) ).ToArray();
				Response.Redirect( GetOrderDetailsUrl( ids ), true );
			}
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			if( OrderStatusFilter == OrderTracking.OrderStatusFilter.ActiveOrders )
			{
                var totals = OrderTracking.GetOrderTotals( SiteContext.Current.CurrentClient.Profile.ClientId, true );
                _activeOrderTotalsBlock.Visible = true;
                _processingOrdersTotalLabel.Text = string.Format("{0:### ### ##0.00}", totals.ProcessingOrdersSum);
                _processingLinesTotalLabel.Text = string.Format("{0:### ### ##0.00}", totals.ProcessingOrderLinesSum);
			}
			else if( OrderStatusFilter == OrderTracking.OrderStatusFilter.ArchiveOrders )
			{
                var totals = OrderTracking.GetOrderTotals( SiteContext.Current.CurrentClient.Profile.ClientId, false );
                _archiveOrderTotalsBlock.Visible = true;
                _completedOrdersTotalLabel.Text = string.Format("{0:### ### ##0.00}", totals.CompletedOrdersSum);
                _receivedByClientSumLabel.Text = string.Format("{0:### ### ##0.00}", totals.ReceivedByClientSum);
			}
		}

		protected string GetOrderDisplayNumber( Order order )
		{
			return OrderTracking.GetOrderDisplayNumber( order );
		}

		protected string GetOrderDetailsUrl( int orderId )
		{
			return GetOrderDetailsUrl( new int[] { orderId } );
		}

		protected string GetOrderDetailsUrl( int[] orderIds )
		{
			switch( SiteContext.Current.User.Role )
			{
				case SecurityRole.Client:
					return UrlManager.GetOrderDetailsUrl( orderIds, GetThisUrl() );
				case SecurityRole.Manager:
					return ClientOrderDetails.GetUrl( orderIds, GetThisUrl() );
				default:
					throw new Exception( "Unknown user role" );
			}
		}

		protected void _listView_DataBinding( object sender, EventArgs e )
		{
			
            SaveUrlState();
		}

		protected void _objectDataSource_Selected( object sender, ObjectDataSourceStatusEventArgs e )
		{
			if( e.ReturnValue.GetType() == typeof( int ) )
			{
				int count = Convert.ToInt32( e.ReturnValue );
				_sortBlock.Visible = count != 0;
				_dataPager.Visible = count != 0;
				_footerBlock.Visible = count != 0;
			}
		}

        protected void _listView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //TODO: доделать
            var row = e.Item.Controls[1].Controls[0];
            var o = ((DropDownList)row.FindControl("_StatusBoxList")).SelectedValue;
            var oID = ((HyperLink)row.FindControl("_orderLink")).Text.Split(' ')[1].TrimStart('№');

            byte i;
            byte StatusID;
            if (byte.TryParse(oID, out i) && byte.TryParse(o, out StatusID))
            {
                using (var ctxStore = new DCWrappersFactory<StoreDataContext>())
                {
                    List<OrderLine> OrderLinesLoaded = ctxStore.DataContext.OrderLines.Where(x => x.OrderID == i).ToList();
                    foreach (OrderLine ordln in OrderLinesLoaded)
                    {
                        ordln.CurrentStatus = StatusID;
                        ordln.CurrentStatusDate = DateTime.Today;
                        ctxStore.DataContext.SubmitChanges();
                        ctxStore.SetCommit();
                    }
                }
            }
        }

        protected void _listView_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {

        }

        protected void _listView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
          
        }

      
    }
}