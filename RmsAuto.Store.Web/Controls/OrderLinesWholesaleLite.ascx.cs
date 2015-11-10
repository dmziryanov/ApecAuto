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
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using System.Collections.Specialized;
using RmsAuto.Common.Web.UrlState;
using System.Net;
using RmsAuto.Store.Data;
using System.Collections.Generic;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Controls
{
	public partial class OrderLinesWholesaleLite : System.Web.UI.UserControl
	{
		public enum ViewModes
		{
			OrderDetailsMode,
			WholesaleMode
		}

		public ViewModes ViewMode { get; set; }

		public bool ShowFilters { get { return ViewMode == ViewModes.WholesaleMode; } }

		public bool ShowCustOrderNum { get { return ViewMode == ViewModes.WholesaleMode; } }

		public int[] OrderIDs
		{
			get
			{
				var str = _objectDataSource.SelectParameters[ "strOrderIDs" ].DefaultValue;
				if( !string.IsNullOrEmpty( str ) )
				{
					return str.Split( ',' ).Select( s => int.Parse( s ) ).ToArray();
				}
				else
				{
					return null;
				}
			}
			set
			{
				if( IsPostBack ) throw new Exception( "Cannot change OrderIDs property" );
				_objectDataSource.SelectParameters[ "strOrderIDs" ].DefaultValue = string.Join( ",", value.Select( id => id.ToString() ).ToArray() );
			}
		}

		#region Load OrderLines

		public static OrderTracking.OrderLineTotals CurrentOrderLineTotals
		{
			get { return (OrderTracking.OrderLineTotals)HttpContext.Current.Items[ "RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLineTotals" ]; }
			set { HttpContext.Current.Items[ "RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLineTotals" ] = value; }
		}

		public static OrderLine[] CurrentOrderLines
		{
			get { return (OrderLine[])HttpContext.Current.Items[ "RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLines" ]; }
			set { HttpContext.Current.Items[ "RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLines" ] = value; }
		}

		public static int GetOrderLinesCount( string strOrderIDs, int? orderID, string custOrderNum, DateTime? orderDate, string manufacturer, string partNumber, string partName, DateTime? estSupplyDate, int sort, string status, int startIndex, int size )
		{
			if( CurrentOrderLineTotals == null )
			{
				var filter = new OrderTracking.OrderLineFilter();
				if( orderID != null ) filter.OrderIDs = new int[] { orderID.Value };
				if( strOrderIDs != null )
				{
					var orderIDs = strOrderIDs.Split( ',' ).Select( s => int.Parse( s ) ).ToArray();
					if( filter.OrderIDs != null ) filter.OrderIDs = filter.OrderIDs.Intersect( orderIDs ).ToArray();
					else filter.OrderIDs = orderIDs;
				}
				filter.CustOrderNum = custOrderNum;
				filter.OrderDate = orderDate;
				filter.Manufacturer = manufacturer;
				filter.PartNumber = partNumber;
				filter.PartName = partName;
				filter.EstSupplyDate = estSupplyDate;
				if( status != null && status != "all" )
				{
					if( status[ 0 ] == 's' )
						filter.OrderLineStatus = (byte) int.Parse( status.Substring( 1 ) );
					else if( status[ 0 ] == 'c' )
						filter.ComplexStatusFilter = (OrderTracking.ComplexStatusFilter)int.Parse( status.Substring( 1 ) );
                    else if (status[ 0 ] == 'p' )
                        filter.Processed = (byte) int.Parse( status.Substring( 1 ) );
				}
                CurrentOrderLineTotals = OrderTracking.GetOrderLinesCountForRMM(filter);
			}
			return CurrentOrderLineTotals.TotalCount;
		}

		public static OrderLine[] GetOrderLines( string strOrderIDs, int? orderID, string custOrderNum, DateTime? orderDate, string manufacturer, string partNumber, string partName, DateTime? estSupplyDate, string status, int sort, int startIndex, int size )
		{
			if( CurrentOrderLines == null )
			{
				var filter = new OrderTracking.OrderLineFilter();
				if( orderID != null ) filter.OrderIDs = new int[] { orderID.Value };
				if( strOrderIDs != null )
				{
					var orderIDs = strOrderIDs.Split( ',' ).Select( s => int.Parse( s ) ).ToArray();
					if( filter.OrderIDs != null ) filter.OrderIDs = filter.OrderIDs.Intersect( orderIDs ).ToArray();
					else filter.OrderIDs = orderIDs;
				}
				filter.CustOrderNum = custOrderNum;
				filter.OrderDate = orderDate;
				filter.Manufacturer = manufacturer;
				filter.PartNumber = partNumber;
				filter.PartName = partName;
				filter.EstSupplyDate = estSupplyDate;
				if( status != null && status != "all" )
				{
                    if (status[0] == 's')
                        filter.OrderLineStatus = (byte) int.Parse(status.Substring(1));
                    else if (status[0] == 'c')
                        filter.ComplexStatusFilter = (OrderTracking.ComplexStatusFilter)int.Parse(status.Substring(1));
                    else if (status[0] == 'p')
                        filter.Processed = (byte) int.Parse(status.Substring(1));
				}

				CurrentOrderLines = OrderTracking.GetOrderLinesForLiteRMM(filter, (OrderTracking.OrderLineSortFields)sort, startIndex, size );
			}
			return CurrentOrderLines;
		}

		#endregion

		protected string GetThisUrl()
		{
			return ( (IPageUrlState)Page ).UrlStateContainer.GetPageUrl();
		}

        protected void Page_Init(object sender, EventArgs e)
        {
            //this. += new System.EventHandler(this.Page_PreLoad);
        }

        public void SaveStatusChanges()
        {

            var mas = SavedStatuses.Value.Split(';');
            if (mas.Length > 1)
            {
                using (var ctxStore = new DCWrappersFactory<StoreDataContext>())
                {
                    foreach (var pair in mas)
                    {
                        var curPair = pair.Split(',');
                        int i;
                        byte StatusID;

                        if (int.TryParse(curPair[0], out i) && byte.TryParse(curPair[1], out StatusID))
                        {
                            OrderLine CurOrderLine = ctxStore.DataContext.OrderLines.Where(x => x.OrderLineID == i).FirstOrDefault();
                            CurOrderLine.ChangeLiteStatus(StatusID);
                            CurOrderLine.CurrentStatusDate = DateTime.Today;
                            ctxStore.DataContext.SubmitChanges();
                            ctxStore.SetCommit();
                        }
                    }
                    CurrentOrderLines = null;
                }
            }
            
        }

        public override void DataBind()
        {
            SaveStatusChanges();
            base.DataBind();
        }


		protected void Page_Load( object sender, EventArgs e )
		{
          
            if (!IsPostBack)
			{

                lDateOnStock.Text = lDateOnStock.Text + " " + AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Name;
				
                //сортировка
				foreach(OrderTracking.OrderLineSortFields sortField in Enum.GetValues( typeof( OrderTracking.OrderLineSortFields ) ) )
				{
					_sortBox.Items.Add( new ListItem( sortField.GetDescription(), ( (int)sortField ).ToString() ) );
				}

				_sortBox.SelectedValue = Convert.ToString((int)OrderTracking.OrderLineSortFields.OrderIDDesc);

				//фильтр по статусам
				_filterStatusBox.Items.Add(new ListItem( "All", "all" ));
				foreach( OrderTracking.ComplexStatusFilter status in Enum.GetValues( typeof( OrderTracking.ComplexStatusFilter ) ) )
				{
					_filterStatusBox.Items.Add( new ListItem( status.ToTextOrName(), "c" + (int)status ) );
				}
                using (var dc = new DCWrappersFactory<StoreDataContext>())
                {
                    foreach (var statusElement in dc.DataContext.OrderLineStatuses)
                    {
                        byte status = statusElement.OrderLineStatusID;

                        if (status != OrderLineStatusUtil.StatusByte("Rejected"))
                        {
                            string displayName = OrderLineStatusUtil.DisplayName(status);
                            _filterStatusBox.Items.Add(new ListItem(displayName, "s" + status));
                        }
                    }

                    // deas 23.05.2011 task4130 Ускорение работы со статусами
                    //var stRefusedBySupplier = OrderLineStatusUtil.DisplayName( dc, OrderLineStatusUtil.StatusByte( "RefusedBySupplier" ) );
                    var stRefusedBySupplier = OrderLineStatusUtil.DisplayName( OrderLineStatusUtil.StatusByte( "RefusedBySupplier" ) );

                    _filterStatusBox.Items.Add(new ListItem(stRefusedBySupplier + ", reorder",
                        "p" + (byte)Processed.NotProcessed));

                    
                    _filterStatusBox.Items.Add(new ListItem(stRefusedBySupplier + ", processed by client",
                        "p" + (byte)Processed.Client));

                    _filterStatusBox.Items.Add(new ListItem(stRefusedBySupplier + ", processed by manager",
                        "p" + (byte)Processed.Manager));
                }

                
                if( ShowFilters )
					_filterStatusBox.SelectedValue = "all";

				//фильтр по номерам заказов
				if( OrderIDs != null )
				{
					_filterOrderIdDropDownBox.Items.Add( new ListItem( "", "" ) );
					foreach( int id in OrderIDs.OrderBy( id => id ) )
					{
						var order = OrderBO.LoadOrderData( SiteContext.Current.CurrentClient.Profile.ClientId, id );
						_filterOrderIdDropDownBox.Items.Add( new ListItem( OrderTracking.GetOrderDisplayNumber( order ), id.ToString() ) );
					}
				}

				//фильтр по производителям
				_filterManufacturerBox.Items.Add( new ListItem( "", "" ) );
				foreach( string manufacturer in OrderTracking.LoadOrderLinesManufacturers( SiteContext.Current.CurrentClient.Profile.ClientId, OrderIDs ) )
				{
					_filterManufacturerBox.Items.Add( new ListItem( manufacturer, manufacturer ) );
				}

				//восстановить состояние
				if( string.IsNullOrEmpty( Request[ "view" ] )
					|| (PrivateOffice.OrderList.Views)int.Parse( Request[ "view" ] ) == PrivateOffice.OrderList.Views.WholesaleView )
				{
					if( !string.IsNullOrEmpty( Request[ "sort" ] ) )
						_sortBox.SelectedValue = Request[ "sort" ];
					if( !string.IsNullOrEmpty( Request[ "start" ] ) && !string.IsNullOrEmpty( Request[ "size" ] ) )
					{
						_dataPager.SetPageProperties( int.Parse( Request[ "start" ] ), int.Parse( Request[ "size" ] ), false );
						_pageSizeBox.SelectedValue = Request[ "size" ];
					}
					if( ShowFilters )
					{
						if( OrderIDs == null )
						{
							_filterOrderIdTextBox.Text = Request[ "orderid" ];
						}
						else
						{
							_filterOrderIdDropDownBox.SelectedValue = Request[ "orderid" ];
						}
						_filterOrderDateBox.Text = Request[ "orderdate" ];
						_filterManufacturerBox.SelectedValue = Request[ "manufacturer" ];
						_filterPartNumberBox.Text = Request[ "partnumber" ];
						_filterPartNameBox.Text = Request[ "partname" ];
						_filterEstSupplyDateBox.Text = Request[ "estsupplydate" ];
						if( !string.IsNullOrEmpty( Request[ "status" ] ) )
							_filterStatusBox.SelectedValue = Request[ "status" ];
					}
				}
				ApplyFilters(false);
            
			}

            //Если выбранный заказ один, то делаем текущего клиента выбранным
            if (OrderIDs.Length == 1)
            {
                Order o = OrderTracking.GetOrderById(OrderIDs[0]);

                var context = (ManagerSiteContext)SiteContext.Current;
                if (!context.ClientSet.Contains(o.ClientID))
                    { context.ClientSet.AddClient(o.ClientID); }

                context.ClientSet.SetDefaultClient(o.ClientID);
            }
         
              
		}

        protected void Page_Prerender(object sender, EventArgs e)
        {
           
         
        }

		protected void OnClientChangeReaction( object sender, ChangeReactionEventArgs e )
		{
			if( SiteContext.Current.User.Role != SecurityRole.Client )
				throw new HttpException((int)HttpStatusCode.Forbidden, "Access denied" );

			try
			{
				OrderBO.ApplyStatusChangeClientReaction( e.OrderLineId, e.Reaction, DateTime.Now );
				_dataPager.SetPageProperties( _dataPager.StartRowIndex, _dataPager.PageSize, true );
			}
			
            catch( Exception )
			{
				//TODO: отображение сообщения об ошибке
				throw;
			}
		}


		protected void _searchButton_Click( object sender, EventArgs e )
		{
			if( Page.IsValid )
			{
				ApplyFilters( true );
			}
		}

		protected void _clearFilterButton_Click( object sender, EventArgs e )
		{
			_filterOrderIdTextBox.Text = "";
			_filterOrderIdDropDownBox.Text = "";
			_filterOrderDateBox.Text = "";
			_filterManufacturerBox.SelectedValue = "";
			_filterPartNumberBox.Text = "";
			_filterPartNameBox.Text = "";
			_filterEstSupplyDateBox.Text = "";
			_filterStatusBox.SelectedValue = "c" + (int)OrderTracking.ComplexStatusFilter.NotCompleted;
			ApplyFilters( true );
		}

		private void ApplyFilters( bool bind )
		{
			if( ShowFilters )
			{
				if( _filterOrderIdTextBox.Visible )
				{
					string[] p = _filterOrderIdTextBox.Text.Split( '/' );
					if( p.Length == 1 )
						_objectDataSource.SelectParameters[ "orderID" ].DefaultValue = p[ 0 ];
					else if( p.Length == 2 )
						_objectDataSource.SelectParameters[ "orderID" ].DefaultValue = p[ 1 ];
					else
						_objectDataSource.SelectParameters[ "orderID" ].DefaultValue = "";
				}
				else if( _filterOrderIdDropDownBox.Visible )
				{
					_objectDataSource.SelectParameters[ "orderID" ].DefaultValue = _filterOrderIdDropDownBox.SelectedValue;
				}
				_objectDataSource.SelectParameters[ "custOrderNum" ].DefaultValue = _filterCustOrderNumBox.Text;
				_objectDataSource.SelectParameters[ "orderDate" ].DefaultValue = _filterOrderDateBox.Text;
				_objectDataSource.SelectParameters[ "manufacturer" ].DefaultValue = _filterManufacturerBox.SelectedValue;
				_objectDataSource.SelectParameters[ "partNumber" ].DefaultValue = _filterPartNumberBox.Text;
				_objectDataSource.SelectParameters[ "partName" ].DefaultValue = _filterPartNameBox.Text;
				_objectDataSource.SelectParameters[ "estSupplyDate" ].DefaultValue = _filterEstSupplyDateBox.Text;
				_objectDataSource.SelectParameters[ "status" ].DefaultValue = _filterStatusBox.SelectedValue;
			}
			if( bind )
			{
				_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
			}
		}

		protected void _sortBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
		}

		protected void _pageSizeBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, Convert.ToInt32( _pageSizeBox.SelectedValue ), true );
		}
		protected void Page_PreRender( object sender, EventArgs e )
		{
            var mas = SavedStatuses.Value.Split(';');
            if (mas.Length > 1)
            {
                _objectDataSource.DataBind();
                _listView.DataBind();
                SavedStatuses.Value = "";
            }

            _filters1Block.Visible = _filters2Block.Visible = ShowFilters;
			_filterOrderIdTextBox.Visible = OrderIDs == null;
			_filterOrderIdDropDownBox.Visible = OrderIDs != null;

			_headerCustOrderNumPlaceHolder.Visible = ShowCustOrderNum;
			_filterCustOrderNumPlaceHolder.Visible = ShowCustOrderNum;
		}

		protected void _listView_DataBinding( object sender, EventArgs e )
		{
			//сохранить информацию о состоянии
           

            IPageUrlState page = (IPageUrlState)Page;
			page.UrlStateContainer[ "sort" ] = _sortBox.SelectedValue;
			page.UrlStateContainer[ "start" ] = _dataPager.StartRowIndex.ToString();
			page.UrlStateContainer[ "size" ] = _dataPager.PageSize.ToString();

			page.UrlStateContainer[ "orderid" ] = _objectDataSource.SelectParameters[ "orderID" ].DefaultValue;
			page.UrlStateContainer[ "orderdate" ] = _objectDataSource.SelectParameters[ "orderDate" ].DefaultValue;
			page.UrlStateContainer[ "manufacturer" ] = _objectDataSource.SelectParameters[ "manufacturer" ].DefaultValue;
			page.UrlStateContainer[ "partnumber" ] = _objectDataSource.SelectParameters[ "partNumber" ].DefaultValue;
			page.UrlStateContainer[ "partname" ] = _objectDataSource.SelectParameters[ "partName" ].DefaultValue;
			page.UrlStateContainer[ "estsupplydate" ] = _objectDataSource.SelectParameters[ "estSupplyDate" ].DefaultValue;
			page.UrlStateContainer[ "status" ] = _objectDataSource.SelectParameters[ "status" ].DefaultValue;
		}		
		
		protected void _listView_DataBound( object sender, EventArgs e )
		{
            if( CurrentOrderLineTotals != null )
			{
                _totalLabel.Text = string.Format("{0:### ### ##0.00}", CurrentOrderLineTotals.TotalSum);
			}
			if( CurrentOrderLines != null )
			{
                _pageTotalLabel.Text = string.Format("{0:### ### ##0.00}", CurrentOrderLines.AsQueryable().Where(OrderBO.TotalStatusExpression).Sum(l => l.Total));
                //foreach (var orderLine in CurrentOrderLines)
                //{
                //    if (orderLine.CurrentStatus == OrderLineStatusUtil.StatusByte("PartNumberTransition") && orderLine.ParentOrderLine != null)
                //    {
                //        PartNumberTransitionHintPlaceHolder.Visible = true;
                //        break;
                //    }
                //}
			}

            
		}

		protected void _objectDataSource_Selected( object sender, ObjectDataSourceStatusEventArgs e )
		{
			if( e.ReturnValue != null && e.ReturnValue.GetType() == typeof( int ) )
			{
				int count = Convert.ToInt32( e.ReturnValue );
				_totalsBlock.Visible = count != 0;
				_dataPager.Visible = count != 0; 
				_pagerSettingsBlock.Visible = count != 0;
			}
		}

        protected void ImageButton2_Click(object sender, EventArgs e)
        {

            foreach (var o in _listView.Items)
            {
                var orderLine = (OrderLine)o.DataItem;
                if (orderLine.CurrentStatus == OrderLineStatusUtil.StatusByte("PartNumberTransition") && orderLine.ParentOrderLine != null)
                {
                    PartNumberTransitionHintPlaceHolder.Visible = true;
                    break;
                }
            }
        }

        protected void _listView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var row = e.Item.Controls[1].Controls[0];
            var o = ((DropDownList)row.FindControl("_StatusBoxList")).SelectedValue;
            var oID = ((HtmlInputHidden)row.FindControl("OrderLineID")).Value;

            int i;
            byte StatusID;
            if (int.TryParse(oID, out i) && byte.TryParse(o, out StatusID))
            {
                using (var ctxStore = new DCWrappersFactory<StoreDataContext>())
                {

                    OrderLine CurOrderLine = ctxStore.DataContext.OrderLines.Where(x => x.OrderLineID == i).FirstOrDefault();
                    
                    CurOrderLine.ChangeLiteStatus(StatusID);
                    CurOrderLine.CurrentStatusDate = DateTime.Today;
                    ctxStore.DataContext.SubmitChanges();
                    ctxStore.SetCommit();
                        //    var pos = Array.IndexOf(CurrentOrderLines, CurrentOrderLines.Where(x => x.OrderLineID == i).FirstOrDefault() );
                    //CurrentOrderLines[pos].CurrentStatus = StatusID; // Класть сюда весь объект нельзя, так как он будет использоваться после  диспоузинга датаконтекста
                }
                
            }
         
        }

        protected void _listView_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            
        }

        public static void UpdateOrderLines(string strOrderIDs, int? orderID, string custOrderNum, DateTime? orderDate, string manufacturer, string partNumber, string partName, DateTime? estSupplyDate, string status, int sort)
        {
               
        }

        private void InitializeComponent()
        {

        }

        private void OrderLinesWholesaleLite_Init(object sender, EventArgs e)
        {
            
        }

        protected void _listView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
          
        }

        protected void _listView_ItemDataBound1(object sender, ListViewItemEventArgs e)
        {
            var row = e.Item.Controls[1].Controls[0];
            int curID;
            int.TryParse(((HtmlInputHidden)row.FindControl("OrderLineStatusID")).Value, out curID);

            
            if (curID != 240)
                ((DropDownList)row.FindControl("_StatusBoxList")).SelectedValue = curID.ToString();
        }

	}
}