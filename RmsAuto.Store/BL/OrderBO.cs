using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Data;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Configuration;
using RmsAuto.Common.Linq;
using System.Linq.Expressions;
using RmsAuto.Store.Cms.Mail.Messages;
using RmsAuto.Store.Cms.Mail;
using System.Net.Mail;
using RmsAuto.Store.Cms.Dac;
using System.Web;
using System.Web.Caching;
using RmsAuto.Store.Data;
using System.Data;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.BL
{
	public static class OrderBO
	{
		/// <summary>
		/// условие: строки в каких статусах учитываются при рассчёте суммы заказа
		/// </summary>
		/// <remarks>
		/// должны учитываться строки в любых статусах кроме:
		/// техническая отмена, фальшномер, поставка невозможна, отменен клиентом
		/// </remarks>
		public static Expression<Func<OrderLine, bool>> TotalStatusExpression { get; internal set; }

		/// <summary>
		/// условие: финальные статусы
		/// </summary>
		public static Expression<Func<OrderLine, bool>> FinalStatusExpression { get; internal set; }

		/// <summary>
		/// условие: не финальные статусы + строки в статусе "поставка невозможна", если не было клика (Processed = "0")
		/// </summary>
		public static Expression<Func<OrderLine, bool>> WorkStatusExpression { get; internal set; }

		/// <summary>
		/// условие: строки, которые могут требовать подтверждения (для отображения на странице "требует подтверждения")
		/// </summary>
		public static Expression<Func<OrderLine, bool>> PotentiallyRequiresReactionStatusExpression { get; internal set; }

		/// <summary>
		/// условие: строки, которые требуют\требовали подтверждения (факт ответа не учитывается)
		/// </summary>
		public static Expression<Func<OrderLine, bool>> NecessarilyRequiresReactionStatusExpression { get; internal set; }

		static OrderBO()
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				//инициализировать условие для определения финальных статусов, статусов подтверждения, статусов для рассчёта суммы
				var finalStatusExpression = PredicateBuilder.False<OrderLine>();
				var workStatusExpression = PredicateBuilder.False<OrderLine>();
				var requiresReactionStatusExpression = PredicateBuilder.False<OrderLine>();
				var excludeFromTotalSumExpression = PredicateBuilder.False<OrderLine>();
				foreach (var statusElement in dc.DataContext.OrderLineStatuses)
				{
					var status = statusElement.OrderLineStatusID;
					if (statusElement.IsFinal)
					{
						finalStatusExpression = finalStatusExpression.Or(l => l.CurrentStatus == status);
					}
					if (statusElement.IsFinal != true)
					{
						workStatusExpression = workStatusExpression.Or(l => l.CurrentStatus == status);
					}
					if (statusElement.RequiresClientReaction)
					{
						requiresReactionStatusExpression =
							requiresReactionStatusExpression.Or(l => l.CurrentStatus == status);
					}
					if (statusElement.ExcludeFromTotalSum)
					{
						excludeFromTotalSumExpression =
							excludeFromTotalSumExpression.Or(l => l.CurrentStatus == status);
					}
				}

				//Expression<Func<Car, bool>> theCarIsRed = c => c.Color == "Red";
				//Expression<Func<Car, bool>> theCarIsCheap = c => c.Price < 10.0;
				//Expression<Func<Car, bool>> theCarIsRedOrCheap = theCarIsRed.Or(theCarIsCheap);
				//var query = carQuery.Where(theCarIsRedOrCheap);
				//workStatusExpression = workStatusExpression.Or(finalStatusExpression);

                // deas 23.05.2011 task4130 Ускорение работы со статусами
				//var stRefusedBySupplier = OrderLineStatusUtil.StatusByte(dc, "RefusedBySupplier");
                var stRefusedBySupplier = OrderLineStatusUtil.StatusByte( "RefusedBySupplier" );


				workStatusExpression =
					workStatusExpression.Or(
						l => l.CurrentStatus == stRefusedBySupplier
						&& l.Processed == (byte)Processed.NotProcessed);

				FinalStatusExpression = finalStatusExpression;
				WorkStatusExpression = workStatusExpression;
				PotentiallyRequiresReactionStatusExpression = requiresReactionStatusExpression;
				TotalStatusExpression = excludeFromTotalSumExpression.Not();
                // deas 23.05.2011 task4130 Ускорение работы со статусами
                //var stPriceAdjustment = OrderLineStatusUtil.StatusByte( dc, "PriceAdjustment" );
                //var stPartNumberTransition = OrderLineStatusUtil.StatusByte( dc, "PartNumberTransition" );
                var stPriceAdjustment = OrderLineStatusUtil.StatusByte( "PriceAdjustment" );
				var stPartNumberTransition = OrderLineStatusUtil.StatusByte( "PartNumberTransition" );
				NecessarilyRequiresReactionStatusExpression = requiresReactionStatusExpression.And(
					l => l.CurrentStatus != stPriceAdjustment
						 && !(l.CurrentStatus == stPartNumberTransition && !l.StrictlyThisNumber));
			}
		}



		public static Order LoadOrderData(string clientId, int orderId)
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				DataLoadOptions options = new DataLoadOptions();
				options.LoadWith<Order>(o => o.OrderLines);
				options.LoadWith<OrderLine>(l => l.OrderLineStatusChanges);
				options.AssociateWith<OrderLine>(l => l.OrderLineStatusChanges.OrderBy(sc => sc.StatusChangeTime));
				dc.DataContext.LoadOptions = options;

				dc.DataContext.DeferredLoadingEnabled = false;

                Order res;
                
                if (SiteContext.Current.User.Role == SecurityRole.Manager && AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
                {
                    res = dc.DataContext.Orders.Single(o =>
                        o.OrderID == orderId);
                }
                else
                {
                    res = dc.DataContext.Orders.Single(o =>
                        o.OrderID == orderId &&
                        o.ClientID == clientId);
                }

				foreach (var l in res.OrderLines)
				{
					var parent = l.ParentOrderLine;
				}
				return res;
			}
		}

		public static OrderLine LoadOrderLineData(string clientId, int orderLineId, bool TrackEnable)
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				
                DataLoadOptions options = new DataLoadOptions();
				options.LoadWith<OrderLine>(l => l.Order);
				options.LoadWith<OrderLine>(l => l.OrderLineStatusChanges);
				options.AssociateWith<OrderLine>(l => l.OrderLineStatusChanges.OrderBy(sc => sc.StatusChangeTime));
				dc.DataContext.LoadOptions = options;
                

				dc.DataContext.DeferredLoadingEnabled = false;
                dc.DataContext.ObjectTrackingEnabled = TrackEnable;
				
                var OrdLn = dc.DataContext.OrderLines.Where(l =>
					l.OrderLineID == orderLineId &&
					l.Order.ClientID == clientId).ToArray()[0];


                
                if (!TrackEnable)
                {
                    OrderLine res2 = new OrderLine();
                    Order o = new Order();
                    OrderLine res = Serializer.JsonClone<OrderLine>(OrdLn, res2);
                    res.Order = Serializer.JsonClone<Order>(OrdLn.Order, o);
                    return res;
                }
                else
                    return OrdLn;
                //return res;
			}
		}

		public static IEnumerable<int> CreateOrder(
			int userId,
			string clientId,
			string storeNumber,
			string employeeId,
			IEnumerable<ShoppingCartItem> cartItems,
			PaymentMethod paymentMethod,
			string shippingAddress,
			string orderNotes,
			string custOrderNum)
		{
			if (string.IsNullOrEmpty(clientId))
				throw new ArgumentException("Client' ID cannot be empty", "clientId");
			if (cartItems == null)
				throw new ArgumentNullException("cartItems");
			if (cartItems.Count() == 0)
				throw new ArgumentException("Shopping cart cannot be empty", "cartItems");
			if (cartItems.Any(i => i.HasIssues))
				throw new ArgumentException("Shopping cart has unresolved issues", "cartItems");


		    using (var dc = new DCFactory<StoreDataContext>(IsolationLevel.ReadUncommitted, false, null, false))
		    {
		        foreach (var itemGroup in cartItems.GroupBy(x => x.SparePart.InternalFranchName))
		        {
		            var order = new Order()
		            {
		                UserID = userId,
		                ClientID = clientId,
		                StoreNumber = storeNumber,
		                ShippingMethod =
		                    !string.IsNullOrEmpty(shippingAddress)
		                        ? ShippingMethod.CourierDelivery
		                        : ShippingMethod.StorePickup,
		                ShippingAddress = shippingAddress,
		                PaymentMethod = paymentMethod,
		                OrderNotes = orderNotes,
		                CustOrderNum = custOrderNum,
		                OrderDate = DateTime.Now
		            };
                    
                    dc.DataContext.Connection.Open();
		            foreach (var item in itemGroup)
		            {

		                order.OrderLines.Add(
		                    new OrderLine()
		                    {
		                        AcctgOrderLineID = -item.ItemID,
		                        //temporary ID while order has'n been sent yet (to satisfy unique database constraint 
		                        InternalFranchName = item.SparePart.InternalFranchName,
		                        Manufacturer = item.SparePart.Manufacturer,
		                        PartNumber = item.SparePart.PartNumber,
		                        SupplierID = item.SparePart.SupplierID,
		                        ReferenceID = item.ReferenceID,
		                        DeliveryDaysMin = item.SparePart.DeliveryDaysMin,
		                        DeliveryDaysMax = item.SparePart.DeliveryDaysMax,
		                        PartName = item.SparePart.PartName,
		                        PartDescription = item.SparePart.PartDescription,
		                        WeightPhysical = item.SparePart.WeightPhysical,
		                        WeightVolume = item.SparePart.WeightVolume,
		                        //если проставлена "вчерашнаяя" цена, то в заказ уходит она
		                        UnitPrice = item.UnitPriceYesterday.HasValue ? item.UnitPriceYesterday.Value : item.UnitPrice,
		                        Qty = item.Qty,
		                        StrictlyThisNumber = item.StrictlyThisNumber,
		                        VinCheckupData = item.VinCheckupDataID.HasValue
		                            ? ClientCarsDac
		                                .GetGarageCar(item.VinCheckupDataID.Value)
		                                .ToVinOrderLineComment()
		                            : string.Empty,
		                        OrderLineNotes = item.ItemNotes,
		                        // deas 23.05.2011 task4130 Ускорение работы со статусами
		                        //CurrentStatus = OrderLineStatusUtil.StatusByte(dc, "PreOrder" /* "New" */)
		                        CurrentStatus = OrderLineStatusUtil.StatusByte("PreOrder" /* "New" */)
		                    });

		                var itemToDelete = dc.DataContext.ShoppingCartItems.SingleOrDefault(
		                    i =>
		                        i.ItemID == item.ItemID &&
		                        i.ItemVersion == item.ItemVersion);
		                if (itemToDelete == null)
		                    throw new BLException("Ошибка создания заказа. Элемент корзины изменен или удален");
		                dc.DataContext.ShoppingCartItems.DeleteOnSubmit(itemToDelete);
		            }
		            dc.DataContext.Orders.InsertOnSubmit(order);
		            
		            dc.DataContext.Transaction = dc.DataContext.Connection.BeginTransaction();
		            try
		            {
		                dc.DataContext.SubmitChanges();
		                dc.DataContext.Transaction.Commit();
		                LightBO.SetOrderNoXmlSign(order.OrderID);
		            }
		            catch
		            {
		                dc.DataContext.Transaction.Rollback();
		                throw;
		            }

		            finally
		            {
		                if (dc.DataContext.Connection.State == ConnectionState.Open)
		                {
		                    dc.DataContext.Connection.Close();
		                }
		                //Теперь закрываемся в DCFactory
		            }

		            yield return order.OrderID;
		        }
		    }
		}

		/// <summary>
		/// Создание заказа для веб-сервиса франча
		/// </summary>
		/// <returns>ID заказа</returns>
		public static int CreateOrderForService(
			int userId,
			string clientId,
			string storeNumber,
			List<OrderLine> orderLines,
			string custOrderNum)
		{
			var order = new Order()
			{
				UserID = userId,
				ClientID = clientId,
				StoreNumber = storeNumber,
				ShippingMethod = ShippingMethod.StorePickup,
				ShippingAddress = string.Empty,
				PaymentMethod = PaymentMethod.Cash,
				OrderNotes = string.Empty,
				CustOrderNum = custOrderNum,
				OrderDate = DateTime.Now
			};
			foreach (var line in orderLines)
			{
				order.OrderLines.Add( line );
			}
            using (var dc = new DCFactory<StoreDataContext>())
			{
				dc.DataContext.Orders.InsertOnSubmit( order );
                //DCFactory начинает транзакцию и так, может не стоит этого делать?
                dc.DataContext.Transaction = dc.DataContext.Connection.BeginTransaction();

				try
				{
					dc.DataContext.SubmitChanges();
					SendOrder(dc.DataContext, ref order, string.Empty );

					dc.DataContext.SubmitChanges();
					dc.DataContext.Transaction.Commit();
				}
				catch(Exception ex)
				{
					string s = ex.Message;
					dc.DataContext.Transaction.Rollback();
					throw;
				}
				finally
				{
					
				}
				return order.OrderID;
			}
		}

		/// <summary>
		/// Метод для повторной отправки заказа в учетную систему
		/// </summary>
		public static void ResendOrder(int orderId)
		{
			using (var dc = new DCFactory<StoreDataContext>())
			{
                //DataLoadOptions options = new DataLoadOptions();
                //options.LoadWith<Order>(o => o.OrderLines);
                //options.LoadWith<OrderLine>(l => l.SparePart);

                //Order orderSource = dc.DataContext.Orders.Single(o => o.OrderID == orderId);

                //SendOrder(ref orderSource, string.Empty);
                throw new NotImplementedException();
			}
		}

        private static void SendorderFranch(StoreDataContext dc, ref Order order, string employeeId)
        { 
			if (order == null)
				throw new ArgumentNullException("order");
			var acctgOrder = new OrderInfo
			{
				ClientId = order.ClientID,
				OrderNo = order.OrderID.ToString(),
				CustOrderNum = order.CustOrderNum,
				OrderDate = order.OrderDate,
				DeliveryAddress = order.ShippingAddress,
				PaymentType = order.PaymentMethod.ToTextOrName(),
				EmployeeId = employeeId,
				OrderNotes = order.OrderNotes,
				OrderLines = order.OrderLines
				.Select<OrderLine, OrderLineInfo>(
					l => new OrderLineInfo
					{
						WebOrderLineId = l.OrderLineID,
						Article = new ArticleInfo
						{
							PartNumber = l.PartNumber,
							Manufacturer = l.Manufacturer,
							SupplierId = l.SupplierID,
							ReferenceID = l.ReferenceID,
							DeliveryDaysMin = l.DeliveryDaysMin,
							DeliveryDaysMax = l.DeliveryDaysMax,
							Description = l.PartDescription,
							DescriptionOrig = l.PartName,
							InternalPartNumber = l.Part.InternalPartNumber,//l.SparePart.InternalPartNumber,
							SupplierPriceWithMarkup = l.Part.SupplierPriceWithMarkup,//l.SparePart.SupplierPriceWithMarkup,
							SupplierMarkup = l.Part.PriceConstantTerm.GetValueOrDefault(),//l.SparePart.PriceConstantTerm.GetValueOrDefault(),
							WeightPhysical = l.WeightPhysical.GetValueOrDefault(),
							WeightVolume = l.WeightVolume.GetValueOrDefault(),
							DiscountGroup = l.Part.RgCode//l.SparePart.RgCode
						},
						FinalSalePrice = l.UnitPrice,
						Quantity = l.Qty,
						StrictlyThisNumber = l.StrictlyThisNumber ? 1 : 0,
						VinCheckupData = l.VinCheckupData,
						OrderLineNotes = l.OrderLineNotes
					}).ToArray()
			};

			//noxml logic!!!
			if (LightBO.IsLight())
			{
				//для лайтовых франчей формируем xml только в случае если у пользователя выставлен признак "автозаказ"
				//в противном случае xml не формируем и выставляем для заказа признак о том, что у него нет xml
				if (LightBO.GetIsAutoOrder(order.UserID))
				{
					OrderService.SendOrder(dc, ref acctgOrder);

					foreach (var wl in order.OrderLines)
					{
						var al = acctgOrder.OrderLines.Single(l => l.WebOrderLineId == wl.OrderLineID);
						wl.AcctgOrderLineID = al.AcctgOrderLineId;
					}
				}
				else
				{
					LightBO.SetOrderNoXmlSign(order.OrderID);
				}
			}
			else
			{
				//для не лайтовых франчей логика остается старой
				OrderService.SendOrder(dc, ref acctgOrder);

				foreach (var wl in order.OrderLines)
				{
					var al = acctgOrder.OrderLines.Single(l => l.WebOrderLineId == wl.OrderLineID);
					wl.AcctgOrderLineID = al.AcctgOrderLineId;
				}

				if (order.OrderLines.FirstOrDefault(l => !l.AcctgOrderLineID.HasValue) != null)
					throw new BLException("Ошибка отправки заказа. Не принято одна или более позиций заказа");
			}
        }

		private static void SendOrder(StoreDataContext dc, ref Order order, string employeeId)
		{
			if (order == null) throw new ArgumentNullException("order");

            var perm1C = CommonDac.GetPermutations(); //Заполняем словарь перестановок SupplierID
            
            var acctgOrder = new OrderInfo
			{
				ClientId = order.ClientID,
				OrderNo = order.OrderID.ToString(),
				CustOrderNum = order.CustOrderNum,
				OrderDate = order.OrderDate,
				DeliveryAddress = order.ShippingAddress,
				PaymentType = order.PaymentMethod.ToTextOrName(),
				EmployeeId = employeeId,
				OrderNotes = order.OrderNotes,
				OrderLines = order.OrderLines.Select<OrderLine, OrderLineInfo>(
					l => new OrderLineInfo
					{
						WebOrderLineId = l.OrderLineID,
						Article = new ArticleInfo
						{
							PartNumber = ProcessingPACK(l.PartNumber, l.SupplierID), //обрабатываем PACK
							Manufacturer = l.Manufacturer,
							/* Реализована возможность продавать одну и ту же деталь (pn, brand, supplierID) по разным ценам (например если при разной "партионности" разная цена, т.е. 1 шт. - 10р. 10 шт. - 9р.):
							 * в этом случае данная деталь заливается с разными SupplierID (реальный и "виртуальный"). Т.к. УС ничего не знает о данных "виртуальных" SupplierID, то при отправке в УС
							 * должна производиться подмена "виртуальных" SupplierID реальными. */
                            SupplierId = perm1C.ContainsKey(l.SupplierID) ? (int)perm1C[l.SupplierID] : l.SupplierID,
							ReferenceID = l.ReferenceID,
							DeliveryDaysMin = l.DeliveryDaysMin,
							DeliveryDaysMax = l.DeliveryDaysMax,
							Description = l.PartDescription,
							DescriptionOrig = l.PartName,
							InternalPartNumber = l.Part.InternalPartNumber,
							SupplierPriceWithMarkup = l.Part.SupplierPriceWithMarkup,
							SupplierMarkup = l.Part.PriceConstantTerm.GetValueOrDefault(),
							WeightPhysical = l.WeightPhysical.GetValueOrDefault(),
							WeightVolume = l.WeightVolume.GetValueOrDefault(),
							DiscountGroup = l.Part.RgCode

						},
						FinalSalePrice = l.UnitPrice,
						Quantity = l.Qty,
						StrictlyThisNumber = l.StrictlyThisNumber ? 1 : 0,
						VinCheckupData = l.VinCheckupData,
						OrderLineNotes = l.OrderLineNotes
					}).ToArray()
			};

			OrderService.SendOrder(dc, ref acctgOrder);

			foreach (var wl in order.OrderLines)
			{
				var al = acctgOrder.OrderLines.Single(l => l.WebOrderLineId == wl.OrderLineID);
				wl.AcctgOrderLineID = al.AcctgOrderLineId;
			}

			if (order.OrderLines.FirstOrDefault(l => !l.AcctgOrderLineID.HasValue) != null)
				throw new BLException("Ошибка отправки заказа. Не принято одна или более позиций заказа");
		}

		/// <summary>
		/// Процесс несколько изменился: теперь помимо обновления байта processed строки заказа обновляется/записывается этот же байт
		/// в отдельную таблицу (таким образом удается избежать потери данных связанных с потерей значения этого байта при передаче данных
		/// из 1С при обновлении статусов строк заказа)
		/// </summary>
		public static void UpdateOrderLineProcessed(int /*orderLineID*/acctgOrderLineID, byte processed)
		{
			using (var dc = new DCFactory<StoreDataContext>(IsolationLevel.ReadUncommitted, false, null, false))
            //using (var dc = new DCFactory<StoreDataContext>())
			{
				
				//выставляем байт processed в таблице OrderLinesProcessed (она будет использоваться при перезаливке строк из 1С)
				var olProcessed = dc.DataContext.OrderLinesProcesseds.Where( olp => olp.AcctgOrderLineID == acctgOrderLineID ).FirstOrDefault();
				if (olProcessed != null)
				{
					olProcessed.Processed = processed;
				}
				else
				{
					//если такой записи нет то создаем ее
					OrderLinesProcessed olp = new OrderLinesProcessed();
					olp.AcctgOrderLineID = acctgOrderLineID;
					olp.Processed = processed;
					dc.DataContext.OrderLinesProcesseds.InsertOnSubmit( olp );
				}

				// обновляем байт processed самой строки заказа (причем строк с одинаковым AcctgOrderLineID может быть несколько -
				// такое происходит при "дроблении" строк например)
				var ols = dc.DataContext.OrderLines.Where(l => l.AcctgOrderLineID == acctgOrderLineID).ToList();
				foreach (var ol in ols)
				{
					ol.Processed = processed;
				}

				dc.DataContext.Connection.Open();
				dc.DataContext.Transaction = dc.DataContext.Connection.BeginTransaction();
				try
				{
					dc.DataContext.SubmitChanges();
					dc.DataContext.Transaction.Commit();
				}
				catch (Exception)
				{
					dc.DataContext.Transaction.Rollback();
					throw;
				}
				finally
				{
					if (dc.DataContext.Connection.State == ConnectionState.Open)
						dc.DataContext.Connection.Close();
				}
				//dc.SetCommit();
			}

			#region old variant
			//using (var dc = new StoreDataContext())
			//{
			//    dc.Connection.Open();

			//    var el =
			//        from o in dc.OrderLines
			//        where o.OrderLineID == orderLineID
			//        select o;

			//    var newL = el.First();

			//    newL.Processed = processed;

			//    dc.Transaction = dc.Connection.BeginTransaction();
			//    try
			//    {
			//        dc.SubmitChanges();
			//        dc.Transaction.Commit();
			//    }
			//    catch
			//    {
			//        dc.Transaction.Rollback();
			//        throw;
			//    }
			//    finally
			//    {
			//        if (dc.Connection.State == System.Data.ConnectionState.Open)
			//            dc.Connection.Close();
			//    }
			//}
			#endregion
		}

		public static void UpdateOrderLineProcessed(OrderLine orderLine)
		{
			UpdateOrderLineProcessed(orderLine.OrderLineID, orderLine.Processed);
		}

		public static DateTime? GetLastOrderLineStatusChangeTime()
		{
            using (var context = new DCFactory<StoreDataContext>())
			{
				var lastStatus = context.DataContext
                    .OrderLineStatusChanges
					.OrderByDescending(olsc => olsc.StatusChangeTime)
					.FirstOrDefault();
				return lastStatus != null ? lastStatus.StatusChangeTime : new DateTime?();
			}
		}


		public static void ClearTransferChangesLog()
		{
            using (var context = new DCFactory<StoreDataContext>())
			{
				context.DataContext.ExecuteCommand("DELETE FROM dbo.TransferChangesLogEntries");
			}
		}

		//[Obsolete]
		//public static void ReceiveAcctgOrderChanges(DateTime checkpointTime)
		//{
		//    using (var context = new StoreDataContext())
		//    {
		//        var logEntry = new TransferChangesLogEntry
		//        {
		//            CheckpointTime = checkpointTime,
		//            TransferStartTime = DateTime.Now
		//        };

		//        var stopWatch = new System.Diagnostics.Stopwatch();
		//        stopWatch.Start();

		//        var changes = OrderService.GetChanges(checkpointTime).OrderBy(l => l.StatusChangeTime);
		//        logEntry.ChangesReceived = changes.Count();

		//        if (logEntry.ChangesReceived > 0)
		//            logEntry.MinChangeTime = changes.First().StatusChangeTime;

		//        foreach (var line in changes)
		//        {
		//            try
		//            {
		//                var existing = context.OrderLines.SingleOrDefault(
		//                    l => l.AcctgOrderLineID == line.AcctgOrderLineId);
		//                var newStatus = new OrderLineStatusChange
		//                {
		//                    OrderLineStatus = OrderLineStatusUtil.StatusByteFromHansa(line.OrderLineStatus),
		//                    StatusChangeTime = line.StatusChangeTime.Value
		//                };

		//                if (existing != null)
		//                {
		//                    if (!existing.OrderLineStatusChanges.Any(
		//                        olsc => olsc.OrderLineStatus == newStatus.OrderLineStatus &&
		//                            olsc.StatusChangeTime == newStatus.StatusChangeTime))
		//                    {
		//                        existing.OrderLineStatusChanges.Add(newStatus);
		//                        logEntry.StatusChangesAdded++;
		//                    }

		//                    //Обновить статус заказа
		//                    existing.CurrentStatus = newStatus.OrderLineStatus;
		//                    existing.CurrentStatusDate = newStatus.StatusChangeTime;

		//                    //TODO: Может ли прийти пустая дата поставки?
		//                    //TODO: должна ли создаваться новая строчка в заказе при изменении ожидаемой даты поставки?
		//                    if (line.EstSupplyDate.HasValue)
		//                        existing.EstSupplyDate = line.EstSupplyDate.Value;
		//                }
		//                else
		//                {
		//                    if (line.ParentAcctgOrderLineId.HasValue)
		//                    {
		//                        var old = context.OrderLines.SingleOrDefault(
		//                            l => l.AcctgOrderLineID == line.ParentAcctgOrderLineId);
		//                        if (old == null)
		//                        {
		//                            logEntry.TransferChangesErrors.Add(
		//                                new TransferChangesError
		//                                {
		//                                    ErrorMessage =
		//                                    string.Format("Строка не найдена.Не найдена замещаемая строка. OrderLineHW = {0}, OldOrderLineHW = {1}",
		//                                    line.AcctgOrderLineId, line.ParentAcctgOrderLineId.Value)
		//                                });
		//                        }
		//                        else
		//                        {
		//                            var newOrderLine = new OrderLine
		//                            {
		//                                OrderID = old.OrderID,
		//                                ParentOrderLineID = old.OrderLineID,
		//                                AcctgOrderLineID = line.AcctgOrderLineId,
		//                                PartNumber = line.Article.PartNumber,
		//                                Manufacturer = line.Article.Manufacturer,
		//                                SupplierID = line.Article.SupplierId,
		//                                ReferenceID = old.ReferenceID,
		//                                DeliveryDaysMin = line.Article.DeliveryDaysMin,
		//                                DeliveryDaysMax = line.Article.DeliveryDaysMax,
		//                                PartName = line.Article.DescriptionOrig,
		//                                PartDescription = line.Article.Description,
		//                                WeightPhysical = line.Article.WeightPhysical,
		//                                WeightVolume = line.Article.WeightVolume,
		//                                UnitPrice = line.FinalSalePrice,
		//                                Qty = line.Quantity,
		//                                StrictlyThisNumber = line.StrictlyThisNumber == 1,
		//                                VinCheckupData = line.VinCheckupData,
		//                                OrderLineNotes = line.OrderLineNotes,
		//                                EstSupplyDate = line.EstSupplyDate,
		//                                CurrentStatus = newStatus.OrderLineStatus,
		//                                CurrentStatusDate = newStatus.StatusChangeTime
		//                            };

		//                            newOrderLine.OrderLineStatusChanges.Add(newStatus);

		//                            logEntry.OrderLinesAdded++;
		//                            logEntry.StatusChangesAdded++;

		//                            context.OrderLines.InsertOnSubmit(newOrderLine);
		//                        }
		//                    }
		//                    else
		//                    {
		//                        logEntry.TransferChangesErrors.Add(new TransferChangesError
		//                        {
		//                            ErrorMessage = "Строка не найдена.Отсутствует идентификатор замещаемой строки. OrderLineHW = " +
		//                            line.AcctgOrderLineId.ToString()
		//                        });
		//                    }
		//                }
		//                context.SubmitChanges();

		//                logEntry.MaxChangeTime = line.StatusChangeTime;
		//            }
		//            catch (Exception ex)
		//            {
		//                logEntry.TransferChangesErrors.Add(
		//                    new TransferChangesError { ErrorMessage = ex.Message });
		//            }
		//        }

		//        stopWatch.Stop();
		//        logEntry.DurationInMilliseconds = (int)stopWatch.ElapsedMilliseconds;

		//        context.TransferChangesLogEntries.InsertOnSubmit(logEntry);
		//        context.SubmitChanges();
		//    }
		//}

		public static void ApplyStatusChangeClientReaction(
			int orderLineId,
			OrderLineChangeReaction clientReaction,
			DateTime clientReactionTime)
		{
            using (var context = new DCFactory<StoreDataContext>())
            {
                var line = context.DataContext.OrderLines.Single( l => l.OrderLineID == orderLineId );

                if ( !line.AcctgOrderLineID.HasValue )
                    throw new InvalidOperationException( "Строка не имеет идентификатора OrderLineHW.  отправка реакции невозможна" );

                var status = line.OrderLineStatusChanges.OrderBy( olsc => olsc.StatusChangeTime ).Last();

                // deas 28.04.2011 task3981 обновление статусов локально без отправки в 1С
                context.DataContext.spUpdateOrderLinesStatus(false, line.AcctgOrderLineID, line.PartNumber,
                    line.Manufacturer, line.SupplierID, line.UnitPrice, line.Qty, line.EstSupplyDate, line.ReferenceID,
                    status.OrderLineStatuses.NameHansa, line.AcctgOrderLineID, line.PartNumber,
                    line.Manufacturer, line.SupplierID, line.DeliveryDaysMin, line.DeliveryDaysMax,
                    line.PartName, line.PartDescription, line.UnitPrice, line.Qty, line.EstSupplyDate,
                    clientReaction == OrderLineChangeReaction.Accept ? "APPLYCLT" : "CANCEL", DateTime.Now );
                //if (status.ClientReactionPending)
                //{
                //    if (OrderService
                //        .PostOrderLineChangeRequest(
                //        line.AcctgOrderLineID.Value,
                //        clientReaction) == Acknowledgement.OK)
                //    {
                //        status.ClientReaction = (byte)clientReaction;
                //        status.ClientReactionTime = clientReactionTime;
                //        context.SubmitChanges();
                //    }
                //}
                //else
                //{
                //    throw new BLException("Ответ уже был отправлен");
                //}
            }
		}

		public interface ISendOrderLineTrackingAlertsLog
		{
			void LogSuccessfulAlert(string clientId);
			void LogManagerListRequestFailed();
			void LogEmptyEmail(string clientId);
			void LogInvalidEmail(string clientId, string email);
			void LogError(string clientId, Exception ex);
		}

		/// <summary>
		/// Отправка оповещений об изменениях статусов заказов
		/// </summary>
		public static void SendOrderLineTrackingAlerts(ISendOrderLineTrackingAlertsLog log)
		{
			//список менеджеров для отправки копий оповещений
			IDictionary<string, EmployeeInfo> managers = null;

			try
			{
				managers = AcctgRefCatalog.RmsEmployees.Items.ToDictionary(item => item.EmployeeId);
			}
			catch
			{
				log.LogManagerListRequestFailed();
			}

			using (var dc = new DCFactory<StoreDataContext>())
			{
                // deas 23.05.2011 task4130 Ускорение работы со статусами
				//var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte( "Rejected" );
				//подгрузить список клиентов, по которым будут отправляться оповещения
				var hourOfPeriod = DateTime.Now.Hour; //текущий час отправки оповещений (т.е. сервис сработал в этот час)

				//var clients = (from u in dc.Users.Where(user => user.Role == SecurityRole.Client)
				//               join alert in dc.ClientAlertInfos on u.AcctgID equals alert.ClientID
				//               join o in dc.Orders on u.AcctgID equals o.ClientID
				//               join l in dc.OrderLines on o.OrderID equals l.OrderID
				//               join ac in dc.ClientAlertConfigs on o.ClientID equals ac.ClientID //Для фильтрации по периоду отправки !!! тут явно косяк
				//               where l.CurrentStatusDate > alert.OrderTrackingLastAlertDate
				//                   && l.CurrentStatus != stRejected
				//                   && ac.HourOfPeriod == null || ac.HourOfPeriod == -1 || ac.HourOfPeriod == hourOfPeriod //Фильтрация по периоду отправки !!! тут явно косяк
				//               select u.AcctgID)
				//                .Distinct()
				//                .ToArray();

				//var clients = (from u in dc.Users.Where(user => user.Role == SecurityRole.Client)
				//               join alert in dc.ClientAlertInfos on u.AcctgID equals alert.ClientID
				//               join o in dc.Orders on u.AcctgID equals o.ClientID
				//               join l in dc.OrderLines on o.OrderID equals l.OrderID
				//               //join ac in dc.ClientAlertConfigs.DefaultIfEmpty() on o.ClientID equals ac.ClientID into list	//Для фильтрации по периоду отправки
				//               //from listItem in list.DefaultIfEmpty()										//left join
				//               where l.CurrentStatusDate > alert.OrderTrackingLastAlertDate
				//                   && l.CurrentStatus != stRejected
				//                   //&& (listItem != null && (listItem.HourOfPeriod == null || listItem.HourOfPeriod == -1 || listItem.HourOfPeriod == hourOfPeriod)) //Фильтрация по периоду отправки
				//               select u.AcctgID )
				//                .Distinct()
				//                .ToArray();
				//var clients_temp = from u in clients
				//              join ac in dc.ClientAlertConfigs.DefaultIfEmpty() on u equals ac.ClientID
				//              where (ac == null) || (ac.HourOfPeriod == -1)
				//              select u;

                //var clients = (from u in dc.Users.Where(user => user.Role == SecurityRole.Client)
                //               join alert in dc.ClientAlertInfos on u.AcctgID equals alert.ClientID
                //               join o in dc.Orders on u.AcctgID equals o.ClientID
                //               join l in dc.OrderLines on o.OrderID equals l.OrderID
                //               join ac in dc.ClientAlertConfigs.DefaultIfEmpty() on o.ClientID equals ac.ClientID into list	//Для фильтрации по периоду отправки
                //               from listItem in list.DefaultIfEmpty()										                //left join
                //               where l.CurrentStatusDate > alert.OrderTrackingLastAlertDate
                //                   && l.CurrentStatus != stRejected
                //               && (listItem == null || (listItem.HourOfPeriod == null || listItem.HourOfPeriod == -1 || listItem.HourOfPeriod == hourOfPeriod)) //Фильтрация по периоду отправки
                //               select u.AcctgID)
                //                .Distinct()
                //                .ToArray();

                
                // deas 23.05.2011 task4124 Новый вариант настроек фильтра оповещений по статусам
                var clients = (from u in dc.DataContext.Users.Where(user => user.Role == SecurityRole.Client)
                               join alert in dc.DataContext.ClientAlertInfos on u.AcctgID equals alert.ClientID
                               join o in dc.DataContext.Orders on u.AcctgID equals o.ClientID
                               join l in dc.DataContext.OrderLines on o.OrderID equals l.OrderID
                               join ac in dc.DataContext.UserSettings.DefaultIfEmpty() on u.UserID.ToString() equals ac.UserID into list	//Для фильтрации по периоду отправки
                                from listItem in list.DefaultIfEmpty()										                //left join
                                where l.CurrentStatusDate > alert.OrderTrackingLastAlertDate
                                    && l.CurrentStatus != stRejected
                                && ( listItem == null || ( listItem.AlertHourOfPeriod == null || listItem.AlertHourOfPeriod == -1 || listItem.AlertHourOfPeriod == hourOfPeriod ) ) //Фильтрация по периоду отправки
                                select u.AcctgID )
                                .Distinct()
                                .ToArray();

				foreach (var clientId in clients)
				{
					try
					{
						SendOrderLineTrackingAlerts(clientId, managers, log);
					}
					catch (Exception ex)
					{
						log.LogError(clientId, ex);
					}
				}
			}
		}

		private static void SendOrderLineTrackingAlerts(string clientId, IDictionary<string, EmployeeInfo> managers, ISendOrderLineTrackingAlertsLog log)
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<OrderLine>(l => l.Order);
				dlo.LoadWith<OrderLine>(l => l.OrderLineStatusChanges);

				var profile = ClientProfile.Load(clientId);
                var clientAlertInfo = dc.DataContext.ClientAlertInfos.Single(a => a.ClientID == clientId);

				//на всякий случай, для новых пользователей исключить отсылку "старых" оповещений, когда он был офлайн клиентом.
				if (clientAlertInfo == null)
				{
                    dc.DataContext.ClientAlertInfos.InsertOnSubmit(new ClientAlertInfo { ClientID = clientId, OrderTrackingLastAlertDate = DateTime.Now });
                    dc.DataContext.SubmitChanges();
				}
                // deas 23.05.2011 task4130 Ускорение работы со статусами
				//var stRejected = OrderLineStatusUtil.StatusByte(dc, "Rejected");
                var stRejected = OrderLineStatusUtil.StatusByte( "Rejected" );

				byte[] selectedStatuses = OrderLineStatusUtil.GetSelectedStatusesOfClient(dc.DataContext, clientId); // ID-шники выбранных этим клиентом статусов

				//var orderLines = ( from o in dc.Orders
				//                   join l in dc.OrderLines on o.OrderID equals l.OrderID
				//                   where o.ClientID == clientId
				//                      && l.CurrentStatusDate > clientAlertInfo.OrderTrackingLastAlertDate
				//                      && l.CurrentStatus != stRejected
				//                      && (selectedStatuses.Length > 0 && selectedStatuses.Contains(l.CurrentStatus)) // Для фильтрации по выбранным для оповещения статусам
				//                   orderby l.CurrentStatusDate
				//                   select l ).ToArray();

				var orderLines = new OrderLine[] { };
				if (selectedStatuses != null && selectedStatuses.Length > 0)
				{
                    orderLines = (from o in dc.DataContext.Orders
                                  join l in dc.DataContext.OrderLines on o.OrderID equals l.OrderID
								  where o.ClientID == clientId
									 && l.CurrentStatusDate > clientAlertInfo.OrderTrackingLastAlertDate
									 && l.CurrentStatus != stRejected
									 && selectedStatuses.Contains(l.CurrentStatus) // Для фильтрации по выбранным для оповещения статусам
								  orderby l.CurrentStatusDate
								  select l).ToArray();
				}
				else if(selectedStatuses == null) //значит записи в настройках клиента еще нет и тогда отсылаем все статусы (по умолчанию)
				//if (selectedStatuses != null && selectedStatuses.Length == 0) -> запись в настройках клиента есть, но нет выбранных статусов, тогда ничего не отсылаем
				{
                    orderLines = (from o in dc.DataContext.Orders
                                  join l in dc.DataContext.OrderLines on o.OrderID equals l.OrderID
								  where o.ClientID == clientId
									 && l.CurrentStatusDate > clientAlertInfo.OrderTrackingLastAlertDate
									 && l.CurrentStatus != stRejected
								  orderby l.CurrentStatusDate
								  select l).ToArray();
				}

				if (orderLines.Length != 0)
				{
					OrderLineTrackingAlert alert = new OrderLineTrackingAlert();
					if (LightBO.IsLight())
					{
						alert = new OrderLineTrackingAlert(profile.InternalFranchName);
					}
                    var estSupplyDateHint = TextItemsDac.GetTextItem( "Orders.EstSupplyDateHint", "ru-RU" );
					alert.EstSupplyDateHint = estSupplyDateHint != null ? estSupplyDateHint.TextItemBody : "";

                    var blockMailFooter = TextItemsDac.GetTextItem( "OrderLineTrackingAlert.BlockMailFooter", "en-US"/*"ru-RU"*/ );
					alert.BlockMailFooter = blockMailFooter != null ? blockMailFooter.TextItemBody.ToString() : "";

					alert.NumChange = "0"; // считаем, что "перехода номера" не было.

					Dictionary<byte, string> dNames = new Dictionary<byte, string>();
                    foreach (var statusElement in dc.DataContext.OrderLineStatuses)
					{
						var status = statusElement.OrderLineStatusID;
						dNames.Add(status, OrderLineStatusUtil.DisplayName(status));
					}

					alert.OrderLines = orderLines.Select(l =>
							new OrderLineTrackingAlert.OrderLine
							{
								OrderDisplayNumber = RmsAuto.Store.Web.OrderTracking.GetOrderDisplayNumber(l.Order),
								CustOrderNum = l.Order.CustOrderNum,
								OrderDate = l.Order.OrderDate.ToString("dd.MM.yyyy"),

								Manufacturer = l.Manufacturer,
								PartNumber = l.PartNumber,
								PartName = l.PartName,
								UnitPrice = l.UnitPrice.ToString("### ### ##0.00"),
								Qty = l.Qty.ToString(),
								ReferenceID = l.ReferenceID,
								Total = l.Total.ToString("### ### ##0.00"),
								EstSupplyDate = l.EstSupplyDate != null ? l.EstSupplyDate.Value.ToString("dd.MM.yyyy") : "",

								CurrentStatusDate = l.CurrentStatusDate.Value.ToString("dd.MM.yyyy HH:mm:ss"),
								CurrentStatusName = dNames[l.CurrentStatus],
								CurrentStatusDescription = string.Empty,//l.LastStatusChange.StatusChangeInfo, -- закомментированно временно, до тех пор, пока снова не появится "история" по строке заказа

								ParentOrderLine =
								   l.ParentOrderLine == null ? null : new OrderLineTrackingAlert.OrderLine
								   {
									   Manufacturer = l.ParentOrderLine.Manufacturer,
									   PartNumber = l.ParentOrderLine.PartNumber,
									   PartName = l.ParentOrderLine.PartName,
									   UnitPrice = l.ParentOrderLine.UnitPrice.ToString("### ### ##0.00"),
									   Qty = l.ParentOrderLine.Qty.ToString(),
									   ReferenceID = l.ParentOrderLine.ReferenceID,
									   Total = l.ParentOrderLine.Total.ToString("### ### ##0.00"),
									   EstSupplyDate = l.ParentOrderLine.EstSupplyDate != null ? l.ParentOrderLine.EstSupplyDate.Value.ToString("dd.MM.yyyy") : ""
								   }
							}).ToArray();
					//var stPartNumberTransition = dNames[OrderLineStatusUtil.StatusByte(dc, "PartNumberTransition")];

					try
					{
						var el = from o in alert.OrderLines
								 where o.ParentOrderLine != null
								 //where o.CurrentStatusName == stPartNumberTransition
								 select o;

						if (el.Count() > 0)
							if (el.First() != null)
								alert.NumChange = "1"; //родительские строки есть!
					}
					catch (ApplicationException)
					{
					}

					MailAddress clientEmail = GetOrderLineTrackingEmail(profile.Email,
						profile.IsLegalWholesale ? profile.ContactPerson : profile.ClientName, log, clientId);

					MailAddress managerEmail = null;
					if (managers != null && profile.ManagerId != null)
					{
						EmployeeInfo manager;
						if (managers.TryGetValue(profile.ManagerId, out manager) && manager != null)
							managerEmail = GetOrderLineTrackingEmail(manager.Email, manager.FullName, log, "manager " + profile.ManagerId);

					}

					//создаем аттач
					Attachment attach = CreateAttachment(alert.OrderLines);

					//отправляем оповещение
					if ((clientEmail ?? managerEmail) != null)
					{
						/*MailEngine.SendMailWithBcc(
							clientEmail ?? managerEmail,
							clientEmail == null ? null : managerEmail,
							alert );*/

						MailEngine.SendMailWithBccAndAttachments(
							clientEmail ?? managerEmail,
							clientEmail == null ? null : managerEmail,
							new Attachment[] { attach },
							alert);
					}

					//если email не задан или некорректный, то дату последнего оповещения всё равно сдвигаем,
					//чтобы не заспамить клиента, когда в его профиле будет корректный email.
					clientAlertInfo.OrderTrackingLastAlertDate = orderLines.Max(l => l.CurrentStatusDate).Value;

                    dc.DataContext.SubmitChanges();

					log.LogSuccessfulAlert(clientId);
				}
			}
		}

		private static MailAddress GetOrderLineTrackingEmail(string address, string displayName, ISendOrderLineTrackingAlertsLog log, string refId)
		{
			MailAddress email = null;
			try
			{
				email = new MailAddress(address, displayName);
			}
			catch (ArgumentException) // обработка manager.Email = null или String.Empty
			{
				log.LogEmptyEmail(refId);
			}
			catch (FormatException)
			{
				log.LogInvalidEmail(refId, address);
			}
			return email;
		}

		/// <summary>
		/// создает "аттач" в формате .csv для строк заказа (по сути дублируем само письмо в аттаче в формате .csv)
		/// </summary>
		/// <param name="orderLines">строки заказа</param>
		/// <returns>аттач</returns>
		private static Attachment CreateAttachment(OrderLineTrackingAlert.OrderLine[] orderLines)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var line in orderLines)
			{
				sb.Append(line.OrderDisplayNumber + "\t");
				sb.Append(line.CustOrderNum + "\t"); //проверить
				sb.Append(line.OrderDate + "\t");
				sb.Append(line.Manufacturer + "\t");
				sb.Append(line.PartNumber + "\t");
				sb.Append(line.PartName + "\t");
				sb.Append(line.Qty + "\t");
				sb.Append(line.ReferenceID + "\t");
				sb.Append(line.UnitPrice + "\t");
				sb.Append(line.Total + "\t");
				sb.Append(line.EstSupplyDate + "\t");
				sb.Append(line.CurrentStatusName + "\t");
				sb.Append(line.CurrentStatusDescription + "\t");
				sb.Append(line.CurrentStatusDate + "\t");
				sb.Append(Environment.NewLine);
			}

			Encoding win1251 = Encoding.GetEncoding(1251);
			byte[] buffer = win1251.GetBytes(sb.ToString());
			MemoryStream ms = new MemoryStream(buffer);
			Attachment att = new Attachment(ms, "attachment.csv");

			return att;
		}

		/// <summary>
		/// "Отрезает" слово "PACK", если таковое есть у артикулов принадлежащих собственным складам наличия
		/// </summary>
		/// <param name="pn">артикул</param>
		/// <param name="supplierID">код поставщика</param>
		/// <returns>обработанный артикул</returns>
		private static string ProcessingPACK( string pn, int supplierID )
		{
			if (pn.Length > 4 && pn.Contains("PACK") && (StoreRefCatalog.RefOwnStores.Items.Where(x => x.SupplierID == supplierID).ToList().Count > 0))
			{
				return pn.Replace( "PACK", "" );
			}
			return pn;
		}

        /// <summary>
        /// Возвращает строковое представление кол-ва на складе (остатков)
        /// </summary>
        /// <param name="qtyInStock">кол-во на складе</param>
        /// <returns>строковое представление</returns>
        public static string GetQtyInStockString(SparePartFranch sp)
        {
            if (!sp.QtyInStock.HasValue) { return string.Empty; } //эта проверка на всякий случай, т.к. по бизнес-логике не должно быть такого
            else if (StoreRefCatalog.RefSearchOwnStores.Items.Select(x => x.SupplierID).Contains(sp.SupplierID))
            {
                if (sp.QtyInStock.Value < 10) return sp.QtyInStock.Value.ToString();
                if (sp.QtyInStock.Value <= 30) { return ">10"; }
                if (sp.QtyInStock.Value <= 100) { return ">30"; }
                if (sp.QtyInStock.Value <= 1000) { return ">100"; }
                return ">1000"; //Всех остальных случаях

            }
            else { return sp.QtyInStock.Value.ToString(); }
        }
	}
}