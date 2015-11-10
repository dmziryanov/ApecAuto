using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Entities;
using System.Linq.Expressions;
using RmsAuto.Common.Linq;
using System.Data.Linq;
using RmsAuto.Store.BL;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Data;

using RmsAuto.Store.Web.Resource;

namespace RmsAuto.Store.Web
{
	public static class OrderTracking
	{
		#region OrderMethods

		/// <summary>
		/// Сформировать номер заказа для отображения
		/// </summary>
		/// <param name="orderID"></param>
		/// <returns></returns>
		public static string GetOrderDisplayNumber( Order order )
		{
			//return string.Format( "{0}/{1}", order.StoreNumber, order.OrderID );			
            return order.OrderID.ToString();			
		}

		#endregion

		#region OrderLines load methods

		public enum OrderLineSortFields
		{
			[Text( "By order number (asc.), Manufacturer, Partnumber" )]
            [LocalizedDescription("OrderLineSortFields_OrderIDAsc", typeof(EnumResource))]
			OrderIDAsc,

            [Text("By order number (desc.), Manufacturer, Partnumber")]
            [LocalizedDescription("OrderLineSortFields_OrderIDDesc", typeof(EnumResource))]
			OrderIDDesc,

            [Text("By Manufacturer, PartNumber")]
            [LocalizedDescription("OrderLineSortFields_Manufacturer", typeof(EnumResource))]
			Manufacturer,

			[Text( "By description" )]
            [LocalizedDescription("OrderLineSortFields_PartName", typeof(EnumResource))]
			PartName,
		
			[Text( "By price (asc.)" )]
            [LocalizedDescription("OrderLineSortFields_PriceAsc", typeof(EnumResource))]
			PriceAsc,

			[Text( "By price (desc.)" )]
            [LocalizedDescription("OrderLineSortFields_PriceDesc", typeof(EnumResource))]
			PriceDesc,

			[Text( "By delivery date (asc.)" )]
            [LocalizedDescription("OrderLineSortFields_EstSupplyDateAsc", typeof(EnumResource))]
			EstSupplyDateAsc,

            [Text("By delivery date (desc.)")]
            [LocalizedDescription("OrderLineSortFields_EstSupplyDateDesc", typeof(EnumResource))]
			EstSupplyDateDesc,

			[Text( "By status" )]
            [LocalizedDescription("OrderLineSortFields_Status", typeof(EnumResource))]
			Status
		}

		public enum ComplexStatusFilter
		{
			[Text( "All in work" )]
            [LocalizedDescription("ComplexStatusFilter_NotCompleted", typeof(EnumResource))]
			NotCompleted,

			[Text("Require confirm")]
            [LocalizedDescription("ComplexStatusFilter_RequiresReaction", typeof(EnumResource))]
			RequiresReaction,

			[Text("All closed")]
            [LocalizedDescription("ComplexStatusFilter_Completed", typeof(EnumResource))]
			Completed
		}

		public class OrderLineFilter
		{
			public int[] OrderIDs { get; set; }
			public string CustOrderNum { get; set; }
			public DateTime? OrderDate { get; set; }
			public string Manufacturer { get; set; }
			public string PartNumber { get; set; }
			public string PartName { get; set; }
			public DateTime? EstSupplyDate { get; set; }
            public byte? OrderLineStatus { get; set; }
            public byte? Processed { get; set; }
			public ComplexStatusFilter? ComplexStatusFilter { get; set; }
		}

		/// <summary>
		/// фильтр для представления "Анализ заказов"
		/// </summary>
		public class OrderLineAnalysisFilter
		{
			public DateTime? OrderDateStart { get; set; }
			public DateTime? OrderDateEnd { get; set; }
			public Decimal? PartPriceStart { get; set; }
			public Decimal? PartPriceEnd { get; set; }
			public string PartName { get; set; }
			public List<int> OrderIDs { get; set; }
			public List<string> CustOrderNums { get; set; }
			public List<string> PartNumbers { get; set; }
			public List<string> ReferenceIDs { get; set; }
			public List<string> Manufacturers { get; set; }
			public List<int> StatusIDs { get; set; }
			public ComplexStatusFilter? ComplexStatusFilter { get; set; }
		}

		public class OrderLineTotals
		{
			public int TotalCount { get; set; }
			public decimal TotalSum { get; set; }
		}

		public static string[] LoadOrderLinesManufacturers( string clientId, int[] orderIDs )
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
                // deas 23.05.2011 task4130 Ускорение работы со статусами
			    //var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte( "Rejected" );

			    var orderLines = dc.DataContext.OrderLines
                    .Where(l => l.Order.ClientID == clientId && l.CurrentStatus != stRejected);

				if( orderIDs != null )
				{
					Expression<Func<OrderLine, bool>> condition = PredicateBuilder.False<OrderLine>();
					orderIDs.Each( orderID => condition = condition.Or<OrderLine>( o => o.OrderID == orderID ) );
					orderLines = orderLines.Where( condition );
				}

				return orderLines.Select( l => l.Manufacturer )
					.Distinct()
					.OrderBy( m => m )
					.ToArray();
			}
		}

		/// <summary>
		/// GetOrderLinesCount
		/// </summary>
		/// <param name="clientId">ID клиента</param>
		/// <param name="filter">фильтр (возможны 2 типа OrderLineFilter и OrderLineAnalysisFilter)</param>
		/// <returns>количество строк заказов</returns>
		public static OrderLineTotals GetOrderLinesCount( string clientId, /*OrderLineFilter filter*/ object filter )
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
                // deas 23.05.2011 task4130 Ускорение работы со статусами
			    //var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte( "Rejected" );

                var orderLines = dc.DataContext.OrderLines.Where(l => l.Order.ClientID == clientId && l.CurrentStatus != stRejected);
				/*if( filter != null )
					orderLines = ApplyOrderLineFilter( orderLines, filter );*/
				if (filter != null)
				{
					if (filter is OrderLineFilter)
					{
						orderLines = ApplyOrderLineFilter(orderLines, (OrderLineFilter)filter);
					}
					else if (filter is OrderLineAnalysisFilter)
					{
						orderLines = ApplyOrderLineAnalysisFilter(orderLines, (OrderLineAnalysisFilter)filter);
					}
					else
					{
						throw new Exception("Incorrect filter object.");
					}
				}

				OrderLineTotals res = new OrderLineTotals();
				res.TotalCount = orderLines.Count();
				//Быи
                if( orderLines.Where( OrderBO.TotalStatusExpression ).Count() != 0 )
					res.TotalSum = orderLines.Where( OrderBO.TotalStatusExpression ).Sum( l => l.UnitPrice * l.Qty );
				return res;
			}
		}

		/// <summary>
		/// GetOrderLines
		/// </summary>
		/// <param name="clientId">ID клиента</param>
		/// <param name="filter">фильтр (возможны 2 типа OrderLineFilter и OrderLineAnalysisFilter)</param>
		/// <param name="sortField">поле сортировки</param>
		/// <param name="startIndex">номер "страницы данных"</param>
		/// <param name="size">размер "страницы данных"</param>
		/// <returns>строки заказов</returns>
		public static OrderLine[] GetOrderLines( string clientId, /*OrderLineFilter filter*/ object filter, OrderLineSortFields sortField, int startIndex, int size )
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				DataLoadOptions options = new DataLoadOptions();
				options.LoadWith<OrderLine>( l => l.OrderLineStatusChanges );
				options.LoadWith<OrderLine>( l => l.Order );
				dc.DataContext.LoadOptions = options;

                // deas 23.05.2011 task4130 Ускорение работы со статусами
			    //var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte( "Rejected" );

                var orderLines = dc.DataContext.OrderLines.Where(l => l.Order.ClientID == clientId && l.CurrentStatus != stRejected);

				/*if( filter != null )
					orderLines = ApplyOrderLineFilter( orderLines, filter );*/
				if (filter != null)
				{
					if (filter is OrderLineFilter)
					{
						orderLines = ApplyOrderLineFilter(orderLines, (OrderLineFilter)filter);
					}
					else if (filter is OrderLineAnalysisFilter)
					{
						orderLines = ApplyOrderLineAnalysisFilter(orderLines, (OrderLineAnalysisFilter)filter);
					}
					else
					{
						throw new Exception("Incorrect filter object.");
					}
				}

				orderLines = ApplyOrderLineSort( orderLines, sortField );

				var res = orderLines.Skip( startIndex ).Take( size ).ToArray();
				foreach( var l in res )
				{
					var parent = l.ParentOrderLine;
				}
				return res;
			}
		}

		static IQueryable<OrderLine> ApplyOrderLineFilter( IQueryable<OrderLine> orderLines, OrderLineFilter filter )
		{
			if( filter.OrderIDs != null )
			{
				Expression<Func<OrderLine, bool>> condition = PredicateBuilder.False<OrderLine>();

				filter.OrderIDs.Each( orderID => condition = condition.Or<OrderLine>( o => o.OrderID == orderID ) );

				orderLines = orderLines.Where( condition );
			}
			if( !string.IsNullOrEmpty( filter.CustOrderNum ) )
			{
				orderLines = orderLines.Where( l => l.Order.CustOrderNum == filter.CustOrderNum );
			}
			if( filter.OrderDate.HasValue )
			{
				orderLines = orderLines.Where( l => filter.OrderDate.Value.Date <= l.Order.OrderDate && l.Order.OrderDate < filter.OrderDate.Value.Date.AddDays( 1 ) );
			}
			if( !string.IsNullOrEmpty( filter.Manufacturer ) )
			{
				orderLines = orderLines.Where( l => l.Manufacturer == filter.Manufacturer );
			}
			if( !string.IsNullOrEmpty( filter.PartNumber ) )
			{
				orderLines = orderLines.Where( l => l.PartNumber.StartsWith( filter.PartNumber ) );
			}
			if( !string.IsNullOrEmpty( filter.PartName ) )
			{
				orderLines = orderLines.Where( l => l.PartName.Contains( filter.PartName ) );
			}
			if( filter.EstSupplyDate.HasValue )
			{
				orderLines = orderLines.Where( l => l.EstSupplyDate == filter.EstSupplyDate.Value );
			}
			if( filter.OrderLineStatus.HasValue )
			{
				orderLines = orderLines.Where( l => l.CurrentStatus == filter.OrderLineStatus.Value );
			}
            var stRefusedBySupplier = OrderLineStatusUtil.StatusByte("RefusedBySupplier");
            if (filter.Processed.HasValue)
            {
                orderLines = orderLines.Where(l => l.Processed == filter.Processed.Value && l.CurrentStatus == stRefusedBySupplier);
            }

			if( filter.ComplexStatusFilter.HasValue )
			{
				if( filter.ComplexStatusFilter.Value == ComplexStatusFilter.NotCompleted )
				{
					//orderLines = orderLines.Where( OrderBO.FinalStatusExpression.Not() );
                    orderLines = orderLines.Where( OrderBO.WorkStatusExpression );
				}
				else if( filter.ComplexStatusFilter.Value == ComplexStatusFilter.RequiresReaction )
				{
					orderLines = orderLines.Where( OrderBO.NecessarilyRequiresReactionStatusExpression );
				}
				else if( filter.ComplexStatusFilter.Value == ComplexStatusFilter.Completed )
				{
					orderLines = orderLines.Where( OrderBO.FinalStatusExpression );
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			return orderLines;
		}

		/// <summary>
		/// фильтрация строк заказов для вкладки "Анализ заказов"
		/// </summary>
		/// <param name="orderLines">исходные строки заказов</param>
		/// <param name="filter">фильтр</param>
		/// <returns>отлфильтрованные строки заказов</returns>
		static IQueryable<OrderLine> ApplyOrderLineAnalysisFilter(IQueryable<OrderLine> orderLines, OrderLineAnalysisFilter filter)
		{
			if (filter.OrderIDs != null)
			{
				Expression<Func<OrderLine, bool>> condition = PredicateBuilder.False<OrderLine>();

				filter.OrderIDs.Each(orderID => condition = condition.Or<OrderLine>(o => o.OrderID == orderID));

				orderLines = orderLines.Where(condition);
			}
			if (filter.CustOrderNums != null)
			{
				Expression<Func<OrderLine, bool>> condition = PredicateBuilder.False<OrderLine>();

				filter.CustOrderNums.Each(custOrderNum => condition = condition.Or<OrderLine>(o => o.Order.CustOrderNum == custOrderNum));

				orderLines = orderLines.Where(condition);
			}
			if (filter.OrderDateStart.HasValue)
			{
				orderLines = orderLines.Where(l => filter.OrderDateStart.Value.Date <= l.Order.OrderDate);
			}
			if (filter.OrderDateEnd.HasValue)
			{
				orderLines = orderLines.Where(l => filter.OrderDateEnd.Value.Date >= l.Order.OrderDate);
			}
			if (filter.PartPriceStart.HasValue)
			{
				orderLines = orderLines.Where(l => filter.PartPriceStart.Value <= l.UnitPrice);
			}
			if (filter.PartPriceEnd.HasValue)
			{
				orderLines = orderLines.Where(l => filter.PartPriceEnd.Value >= l.UnitPrice);
			}
			if (!string.IsNullOrEmpty(filter.PartName))
			{
				orderLines = orderLines.Where(l => l.PartName.Contains(filter.PartName));
			}
			if (filter.PartNumbers != null && filter.PartNumbers.Count > 0)
			{
				orderLines = orderLines.Where(l => filter.PartNumbers.Contains(l.PartNumber));
			}
			if (filter.ReferenceIDs != null && filter.ReferenceIDs.Count > 0)
			{
				orderLines = orderLines.Where(l => filter.ReferenceIDs.Contains(l.ReferenceID));
			}
			if (filter.Manufacturers != null && filter.Manufacturers.Count > 0)
			{
				orderLines = orderLines.Where(l => filter.Manufacturers.Contains(l.Manufacturer));
			}

			if (filter.ComplexStatusFilter.HasValue)
			{
				if (filter.ComplexStatusFilter.Value == ComplexStatusFilter.NotCompleted)
				{
					//orderLines = orderLines.Where( OrderBO.FinalStatusExpression.Not() );
					orderLines = orderLines.Where(OrderBO.WorkStatusExpression);
				}
				//else if (filter.ComplexStatusFilter.Value == ComplexStatusFilter.RequiresReaction)
				//{
				//    orderLines = orderLines.Where(OrderBO.NecessarilyRequiresReactionStatusExpression);
				//}
				else if (filter.ComplexStatusFilter.Value == ComplexStatusFilter.Completed)
				{
					orderLines = orderLines.Where(OrderBO.FinalStatusExpression);
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			else if (filter.StatusIDs != null && filter.StatusIDs.Count > 0)
			{
				Expression<Func<OrderLine, bool>> condition = PredicateBuilder.False<OrderLine>();

				filter.StatusIDs.Each(statusID => condition = condition.Or<OrderLine>(o => o.CurrentStatus == statusID));

				orderLines = orderLines.Where(condition);
			}

			return orderLines;
		}

		private static IQueryable<OrderLine> ApplyOrderLineSort( IQueryable<OrderLine> orderLines, OrderLineSortFields sortField )
		{
			switch( sortField )
			{
				case OrderLineSortFields.OrderIDAsc: orderLines = orderLines.OrderBy( l => l.Order.OrderID ).ThenBy( l=>l.Manufacturer ).ThenBy(l=>l.PartNumber); break;
				case OrderLineSortFields.OrderIDDesc: orderLines = orderLines.OrderByDescending( l => l.Order.OrderID ).ThenBy( l => l.Manufacturer ).ThenBy( l => l.PartNumber ); break;
				case OrderLineSortFields.Manufacturer: orderLines = orderLines.OrderBy( l => l.Manufacturer ).ThenBy( l => l.PartNumber ); break;
				case OrderLineSortFields.PartName: orderLines = orderLines.OrderBy( l => l.PartName ); break;
				case OrderLineSortFields.PriceAsc: orderLines = orderLines.OrderBy( l => l.UnitPrice ); break;
				case OrderLineSortFields.PriceDesc:	orderLines = orderLines.OrderByDescending( l => l.UnitPrice ); break;
				case OrderLineSortFields.EstSupplyDateAsc: orderLines = orderLines.OrderBy( l => l.EstSupplyDate );	break;
				case OrderLineSortFields.EstSupplyDateDesc: orderLines = orderLines.OrderByDescending( l => l.EstSupplyDate ); break;
				case OrderLineSortFields.Status: orderLines = orderLines.OrderBy( l => l.CurrentStatus ); break;
			}
			return orderLines;
		}

		#endregion

		#region Orders load methods

		public enum OrderSortFields
		{
			[Text( "By order number (ASC.)" )]
            [LocalizedDescription("OrderSortFields_OrderIdAsc", typeof(EnumResource))]
			OrderIdAsc,

			[Text( "By order number (DESC.)" )]
            [LocalizedDescription("OrderSortFields_OrderIdDesc", typeof(EnumResource))]
			OrderIdDesc,
	
			/*[Text( "по дате заказа, возр." )]
			OrderDateAsc,
			
			[Text( "по дате заказа, убыв." )]
			OrderDateDesc,
			
			[Text( "по сумме, возр." )]
			TotalAsc,
			
			[Text( "по сумме, убыв." )]
			TotalDesc,*/
			
			[Text( "By status" )]
            [LocalizedDescription("OrderSortFields_Status", typeof(EnumResource))]
			Status
		}

		public enum OrderStatusFilter
		{
			ActiveOrders,
			ArchiveOrders
		}

		public class OrdersList
		{
			public Order[] Orders { get; set; }
			public int StartIndex { get; set; }
			public int TotalCount { get; set; }
		}

		public class OrderTotals
		{
			/// <summary>
			/// Сумма заказов в статусе "В обработке"
			/// </summary>
			public decimal ProcessingOrdersSum { get; set; }

			/// <summary>
			/// Сумма заказов в статусе "Завершен"
			/// </summary>
			public decimal CompletedOrdersSum { get; set; }

			/// <summary>
			/// Сумма позиций в нефинальных статусах заказов в статусе "В обработке"
			/// </summary>
			public decimal ProcessingOrderLinesSum { get; set; }

			/// <summary>
			/// Сумма позиций в статусе "получено клиентом" заказов в статусе "Завершен".
			/// </summary>
			public decimal ReceivedByClientSum { get; set; }
		}

		public static OrdersList GetOrders( string clientId, OrderStatusFilter statusFilter, OrderSortFields sort, int startIndex, int size )
		{
			using( var dc = new DCWrappersFactory<StoreDataContext>() )
			{
				DataLoadOptions options = new DataLoadOptions();
				options.LoadWith<Order>( o => o.OrderLines );
				dc.DataContext.LoadOptions = options;
                dc.DataContext.Log = new DebugTextWriter();
				var orders = dc.DataContext.Orders.Where( o => o.ClientID == clientId ).ToArray().AsQueryable();
				
				//применить фильтры
				orders = statusFilter == OrderStatusFilter.ActiveOrders ? orders.Where( o => o.Status != OrderStatus.Completed ) : orders.Where( o => o.Status == OrderStatus.Completed );

				//применить сортировку
				switch( sort )
				{
					case OrderSortFields.OrderIdAsc: orders = orders.OrderBy( o => o.OrderID ); break;
					case OrderSortFields.OrderIdDesc: orders = orders.OrderByDescending( o => o.OrderID ); break;
					//case OrderSortFields.OrderDateAsc: orders = orders.OrderBy( o => o.OrderDate ); break;
					//case OrderSortFields.OrderDateDesc: orders = orders.OrderByDescending( o => o.OrderDate ); break;
					//case OrderSortFields.TotalAsc: orders = orders.OrderBy( o => o.Total ); break;
					//case OrderSortFields.TotalDesc: orders = orders.OrderByDescending( o => o.Total ); break;
					case OrderSortFields.Status: orders = orders.OrderBy( o => o.Status ).ThenBy( o => o.OrderID ); break;
					//case OrderSortFields.StatusDesc: orders = orders.OrderByDescending( o => o.Status ).ThenBy( o => o.OrderID ); break;
				}

                
				OrdersList res = new OrdersList();
				res.TotalCount = orders.Count();
				res.Orders = orders.Skip( startIndex ).Take( size ).ToArray();
				res.StartIndex = startIndex;
				return res;
			}
		}

		/// <summary>
		/// Получить сумму заказов в статусе "новый"
		/// <returns>Сумма заказов</returns>
		/// //byOutsourcers эквайринг
		public static decimal GetNewOrdersSum(string clientId)
		{
			decimal sum = 0;
			using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				try
				{
					sum = dc.DataContext.Orders.Where(o => o.ClientID == clientId && o.Status == OrderStatus.New).Sum(o => o.Total);
				}
				catch (Exception ex) { }
			};

			return sum;
		}

		/// <summary>
        /// Получение итогов по заказам
        /// </summary>
        /// <param name="clientId">Код клиента</param>
        /// <param name="getCurrent">Для текущих заказов или для архивных</param>
        /// <returns>Итоги по заказам</returns>
		public static OrderTotals GetOrderTotals( string clientId, bool getCurrent )
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                OrderTotals res = new OrderTotals();

                if ( getCurrent )
                {
                    try
                    {
						res.ProcessingOrdersSum = dc.DataContext.Orders.Where(o => o.ClientID == clientId && o.Status == OrderStatus.Processing).Sum(o => o.Total);
                        // при пустом результате возникает ошибка приведения, но так исполнятеся только один запрос
                        res.ProcessingOrderLinesSum = ( from o in dc.DataContext.Orders
                                                        join ol in dc.DataContext.OrderLines on o.OrderID equals ol.OrderID
                                                        join ols in dc.DataContext.OrderLineStatuses on ol.CurrentStatus equals ols.OrderLineStatusID
                                                        where o.ClientID == clientId
                                                        && o.Status == OrderStatus.Processing
                                                        && !ols.IsFinal
                                                        select ol ).Sum( t => t.Qty * t.UnitPrice );
                    }
                    catch { }
                }
                else
                {
                    try
                    {
						res.CompletedOrdersSum = dc.DataContext.Orders.Where(o => o.ClientID == clientId && o.Status == OrderStatus.Completed).Sum(o => o.Total);
                        // при пустом результате возникает ошибка приведения, но так исполнятеся только один запрос
                        res.ReceivedByClientSum = ( from o in dc.DataContext.Orders
                                                    join ol in dc.DataContext.OrderLines on o.OrderID equals ol.OrderID
                                                    where o.ClientID == clientId
                                                    && o.Status == OrderStatus.Processing
                                                    && ol.CurrentStatus == (byte)160
                                                    select ol ).Sum( t => t.Qty * t.UnitPrice );
                    }
                    catch { }
                }
                return res;
            }
            //using( var dc = new StoreDataContext() )
            //{
            //    DataLoadOptions options = new DataLoadOptions();
            //    options.LoadWith<Order>( o => o.OrderLines );
            //    dc.LoadOptions = options;

            //    var orders = dc.Orders.Where( o => o.ClientID == clientId ).ToArray().AsQueryable();

            //    OrderTotals res = new OrderTotals();

            //    res.ProcessingOrdersSum = orders.Where( o => o.Status == OrderStatus.Processing ).Sum( o => o.Total );

            //    res.CompletedOrdersSum = orders.Where( o => o.Status == OrderStatus.Completed ).Sum( o => o.Total );

            //    res.ProcessingOrderLinesSum = orders
            //        .Where( o => o.Status == OrderStatus.Processing )
            //        .Sum( o => o.OrderLines.AsQueryable().Where( OrderBO.FinalStatusExpression.Not() ).Sum( l => l.Total ) );

            //    // deas 23.05.2011 task4130 Ускорение работы со статусами
            //    //var stReceivedByClient = OrderLineStatusUtil.StatusByte(dc, "ReceivedByClient");
            //    var stReceivedByClient = OrderLineStatusUtil.StatusByte( "ReceivedByClient" );

            //    res.ReceivedByClientSum = orders
            //        .Where( o => o.Status == OrderStatus.Completed )
            //        .Sum(o => o.OrderLines.Where(l => l.CurrentStatus == stReceivedByClient).Sum(l => l.Total));
            //    return res;
            //}
		}


		#endregion

		#region OrderLines requires reaction

		public static int GetOrderLinesRequiresReactionCount( string clientId )
		{
			using( var dc = new DCWrappersFactory<StoreDataContext>() )
			{
				var lines = dc.DataContext.OrderLines
					.Where( l => l.Order.ClientID == clientId ).Where( OrderBO.PotentiallyRequiresReactionStatusExpression );
				return lines.Count();
			}
		}

        public static Order GetOrderById(int OrderId)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                var order = dc.DataContext.Orders.Where(l => l.OrderID == OrderId).FirstOrDefault();
                return order;
            }
        }

        public static Order GetOrderByAcctgOrderlineId(int? AcctgId)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                var order = dc.DataContext.OrderLines.Where(l => l.AcctgOrderLineID == AcctgId).FirstOrDefault().Order;
                return order;
            }
        }

		public static OrderLine[] GetOrderLinesRequiresReaction( string clientId, OrderLineSortFields sortField )
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				DataLoadOptions options = new DataLoadOptions();
				options.LoadWith<OrderLine>( l => l.OrderLineStatusChanges );
				options.LoadWith<OrderLine>( l => l.Order );
				dc.DataContext.LoadOptions = options;

				var res = dc.DataContext.OrderLines
					.Where( l => l.Order.ClientID == clientId ).Where( OrderBO.PotentiallyRequiresReactionStatusExpression )
					.OrderByDescending( l => l.OrderLineStatusChanges.Max( s => s.StatusChangeTime ) )
					.ToArray();

				foreach( var l in res )
				{
					var parent = l.ParentOrderLine;
				}

				res = ApplyOrderLineSort( res.AsQueryable(), sortField ).ToArray();

				return res;
			}
		}

		#endregion


		public static void GetOrderLinesSummary( string clientId, byte status, out int linesCount, out decimal totalSum )
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				var lines = dc.DataContext.OrderLines
					.Where( l => l.Order.ClientID == clientId && l.CurrentStatus == status );

				linesCount = lines.Count();
				if( linesCount != 0 )
					totalSum = lines.Sum( l => l.UnitPrice * l.Qty );
				else
					totalSum = 0;
			}
		}

        public static OrdersList GetOrdersByName(string ClientName, int statusFilter, OrderSortFields sort, DateTime lowDate, DateTime hiDate, int startIndex, int size, int AcctgOrderLineId)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<Order>(o => o.OrderLines);
                dc.DataContext.LoadOptions = options;
                dc.DataContext.Log = new DebugTextWriter();

                IEnumerable<Order> orders = new List<Order>();

                int TotalCount = 0;
                int ClientCount = 0;

                    
                var Clients = dc.DataContext.Users.Where(x => x.Clientname.Contains(ClientName)).Select(x => x.AcctgID).AsEnumerable();

                try
                { 
                   ClientCount = Clients.Count();
                }
                catch
                {

                
                }

                

                //применить сортировку
                string SortExpr = "";
                string StatusFilter = "";
                string OrderlineAcctgIdFilter = "";

                if (AcctgOrderLineId > 0)
                {
                    OrderlineAcctgIdFilter = "and (select count(*) from dbo.OrderLines c where c.OrderId = a.OrderId and c.AcctgOrderLineId = " + AcctgOrderLineId.ToString() + ") > 0";
                }

                switch (sort)
                {
                    //case OrderSortFields.OrderIdAsc: orders = orders.OrderBy(o => o.OrderID); break;
                    //case OrderSortFields.OrderIdDesc: orders = orders.OrderByDescending(o => o.OrderID); break;
                    //case OrderSortFields.OrderDateAsc: orders = orders.OrderBy( o => o.OrderDate ); break;
                    //case OrderSortFields.OrderDateDesc: orders = orders.OrderByDescending(o => o.OrderDate); break;
                    //case OrderSortFields.TotalAsc: orders = orders.OrderBy( o => o.Total ); break;
                    //case OrderSortFields.TotalDesc: orders = orders.OrderByDescending( o => o.Total); break;
                    //case OrderSortFields.Status: orders = orders.OrderBy(o => o.Status).ThenBy(o => o.OrderID); break;
                    //case OrderSortFields.StatusDesc: orders = orders.OrderByDescending( o => o.Status ).ThenBy( o => o.OrderID ); break;
                    case OrderSortFields.OrderIdAsc: SortExpr = "ORDER BY OrderID"; break;
                    case OrderSortFields.OrderIdDesc: SortExpr = "ORDER BY OrderID DESC"; break;
                    case OrderSortFields.Status: SortExpr = "ORDER BY Status"; break;
                }

                switch (statusFilter)
                {

                    case 0: StatusFilter = "AND a.Status > 0"; break; //Все
                    case 1: StatusFilter = "AND a.Status = 1"; break; //Новый
                    case 2: StatusFilter = "AND a.Status = 2"; break; //В работе
                    case 3: StatusFilter = "AND a.Status = 3"; break; //Архив
                    case 4: StatusFilter = "AND a.Status > 0 AND a.Status < 3"; break; //Активные
                }
                
                
                if (string.IsNullOrEmpty(ClientName))
                   {
                        orders = dc.DataContext.ExecuteQuery<Order>(@"SELECT  *
                                                    FROM    (SELECT    ROW_NUMBER() OVER ( "+ SortExpr +@" ) AS RowNum, a.*, b.ClientName
                                                    FROM    dbo.Orders a, dbo.Users b
                                                    WHERE   OrderDate >= {0} AND OrderDate <= {1} AND a.UserId = b.UserId AND b.InternalFranchName = {4} " + OrderlineAcctgIdFilter + " " + StatusFilter + @") AS RowConstrainedResult
                                                    WHERE   RowNum > {2}
                                                    AND RowNum <= {2} + {3}
                                                    ORDER BY RowNum", lowDate, hiDate, startIndex, size, SiteContext.Current.InternalFranchName);

                        
                    
                        TotalCount = dc.DataContext.ExecuteQuery<int>(@"SELECT  COUNT(*)
                                                                       FROM    dbo.Orders a, dbo.Users b
                                                                       WHERE   OrderDate >= {0} AND OrderDate <= {1} AND  a.UserId = b.UserId AND b.InternalFranchName = {2} " + OrderlineAcctgIdFilter + " " + StatusFilter,
                                                                       lowDate,
                                                                       hiDate,
                                                                       SiteContext.Current.InternalFranchName).FirstOrDefault();
                    }
                    else
                    //orders = dc.DataContext.Orders.Where(o => Clients.Contains(o.UserID)).ToArray().AsEnumerable();
                    {
                        if (ClientCount > 0)
                        {
                            orders = dc.DataContext.ExecuteQuery<Order>(@"SELECT  *
                                                    FROM    (SELECT    ROW_NUMBER() OVER ( " + SortExpr + @" ) AS RowNum, a.*, b.ClientName
                                                    FROM    dbo.Orders a, dbo.Users b
                                                    WHERE   OrderDate >= {0} AND OrderDate <= {1} " + OrderlineAcctgIdFilter + " " + StatusFilter + @" AND  a.UserID = b.UserId AND b.InternalFranchName = {4} AND a.ClientId  in ('" + Clients.Select(x => x.ToString()).Aggregate((prev, next) => prev + "','" + next  ) +
                                                            @"')) AS RowConstrainedResult
                                                    WHERE   RowNum > {2} AND RowNum <=  {2} + {3} 
                                                    ORDER BY RowNum", lowDate, hiDate, startIndex, size, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].InternalFranchName);

                            try
                            {
                                TotalCount = dc.DataContext.ExecuteQuery<int>(@"SELECT  COUNT(*)
                                                                FROM    dbo.Orders a, dbo.Users b 
                                                                WHERE   OrderDate >= {0} " + OrderlineAcctgIdFilter + " " + StatusFilter + @" AND  a.UserId = b.UserId AND  OrderDate <= {1} AND CAST(a.ClientID as NVarChar(50)) in ('" + Clients.Select(x => x.ToString()).Aggregate((prev, next) => prev + "','" + next  )+ "')", lowDate, hiDate).FirstOrDefault();
                            }
                            catch
                            {

                            }
                        }
                    }
                    //применить фильтры
                    // orders = statusFilter == OrderStatusFilter.ActiveOrders ? orders.Where(o => o.Status != OrderStatus.Completed) : orders.Where(o => o.Status == OrderStatus.Completed);

              
                
                
                OrdersList res = new OrdersList();
                res.Orders = orders.ToArray();
                res.Orders.Each((x) => x.ClientName = dc.DataContext.Users.Where(y => y.AcctgID ==x.ClientID).FirstOrDefault().Clientname); //Нужно загрузить здесь, так как после диспоуза дтаконтекста юзер будет не доступен
                res.Orders.Each((x) => x.ClientSaldo = decimal.Round(LightBO.GetUserLightBalance(dc.DataContext.Users.Where(y => y.AcctgID == x.ClientID).FirstOrDefault().UserID, DateTime.Now)));
                res.Orders.Each((x) => x.ActiveOrdersSum = decimal.Round(LightBO.GetUserActiveOrdersSum(dc.DataContext.Users.Where(y => y.AcctgID == x.ClientID).FirstOrDefault().UserID)));
                res.Orders.Each((x) => x.PrepaymentPercent = dc.DataContext.ExecuteQuery<decimal>("select PrepaymentPercent from dbo.Users where UserId = {0}", dc.DataContext.Users.Where(y => y.AcctgID == x.ClientID).FirstOrDefault().UserID).FirstOrDefault());
                res.Orders.Each((x) => x.ActiveOrdersSumToDate = LightBO.GetUserActiveOrdersSumToDate /*GetUserLightDebt*/(dc.DataContext.Users.Where(y => y.AcctgID == x.ClientID).FirstOrDefault().UserID));
                res.Orders.Each((x) => x.ClientSaldoToDate = LightBO.GetUserLightBalanceWithDelay(dc.DataContext.Users.Where(y => y.AcctgID == x.ClientID).FirstOrDefault().UserID));
                res.Orders.Each((x) => x.PaymentLimit = LightBO.GetPaymentLimit(dc.DataContext.Users.Where(y => y.AcctgID == x.ClientID).FirstOrDefault().UserID));
                
                res.TotalCount = TotalCount;
                res.StartIndex = startIndex;
                return res;
            }
        }

        public static OrderLine[] GetOrderLinesForLiteRMM(OrderLineFilter filter, OrderLineSortFields orderLineSortFields, int startIndex, int size)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<OrderLine>(l => l.OrderLineStatusChanges);
                options.LoadWith<OrderLine>(l => l.Order);
                dc.DataContext.LoadOptions = options;

                // deas 23.05.2011 task4130 Ускорение работы со статусами
                //var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte("Rejected");

                var orderLines = dc.DataContext.OrderLines.AsQueryable(); //пока закоментировано .Where(l => l.CurrentStatus != stRejected);

                /*if( filter != null )
                    orderLines = ApplyOrderLineFilter( orderLines, filter );*/
                if (filter != null)
                {
                    if (filter is OrderLineFilter)
                    {
                        orderLines = ApplyOrderLineFilter(orderLines, (OrderLineFilter)filter);
                    }

                                     
                    else
                    {
                        throw new Exception("Incorrect filter object.");
                    }
                }

                orderLines = ApplyOrderLineSort(orderLines, orderLineSortFields);

                //orderLines = ApplyOrderLineSort(orderLines, sortField);
                

                return orderLines.Skip(startIndex).Take(size).ToArray();
            }
        }

        public static OrderLineTotals GetOrderLinesCountForRMM(OrderLineFilter filter)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<OrderLine>(l => l.OrderLineStatusChanges);
                options.LoadWith<OrderLine>(l => l.Order);
                dc.DataContext.LoadOptions = options;

                // deas 23.05.2011 task4130 Ускорение работы со статусами
                //var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte("Rejected");

                var orderLines = dc.DataContext.OrderLines.Where(l => l.CurrentStatus != stRejected);

                /*if( filter != null )
                    orderLines = ApplyOrderLineFilter( orderLines, filter );*/
                if (filter != null)
                {
                    if (filter is OrderLineFilter)
                    {
                        orderLines = ApplyOrderLineFilter(orderLines, (OrderLineFilter)filter);
                    }

                    else
                    {
                        throw new Exception("Incorrect filter object.");
                    }
                }

              

                OrderLineTotals res = new OrderLineTotals();
                res.TotalCount = orderLines.Count();
                if (orderLines.Where(OrderBO.TotalStatusExpression).Count() != 0)
                    res.TotalSum = orderLines.Where(OrderBO.TotalStatusExpression).Sum(l => l.UnitPrice * l.Qty);
                return res;
            }
        }
    }
}
