using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.FranchSvcs;

namespace TestBOConsole
{
	class Program
	{
		static void Main( string[] args )
		{
			//ImportManager manager = new ImportManager("d:\price_test", "", "", "", "", "", 10, false);
			//manager.do
			//var parts = GetPrices( "WHT002001", "", true );
			//Console.WriteLine( parts != null ? parts.Count.ToString() : "null" );
			//Console.ReadLine();

			//int i = GetOrderStatuses( new int[] { 232996, 238778, 238780, 416440 } ).Count;
			//string s = i.ToString();

			//OrderLineItemSource item1 = new OrderLineItemSource()
			//{
			//    ItemID = 1,
			//    Manufacturer = "KNECHT",
			//    PartNumber = "OC90",
			//    Qty = 1,
			//    ReferenceID = "Dima1",
			//    StrictlyThisBrand = true,
			//    StrictlyThisNumber = true,
			//    StrictlyThisQty = false,
			//    SupplierID = 5500,
			//    UnitPrice = 91.13m
			//};

			//OrderLineItemSource item2 = new OrderLineItemSource()
			//{
			//    ItemID = 2,
			//    Manufacturer = "LPR",
			//    PartNumber = "05P293",
			//    Qty = 40,
			//    ReferenceID = "Dima1",
			//    StrictlyThisBrand = true,
			//    StrictlyThisNumber = true,
			//    StrictlyThisQty = false,
			//    SupplierID = 5230,
			//    UnitPrice = 450m
			//};

			//var result = SendOrder( new OrderLineItemSource[] { item1, item2 } );
			//Console.WriteLine( "номер заказа: " + result.OrderID.ToString() );
			//Console.ReadLine();

			//Dictionary<byte, string> statuses = GetStatuses();
			//foreach (var status in statuses)
			//{
			//    Console.WriteLine( status.Key + " - " + status.Value );
			//}
			//Console.ReadLine();

			////Тестирование отправки оповещений
			//Program p = new Program();
			//p.SendOrderLineTrackingAlers();
			//Console.ReadLine();
			//spTest();
			//TestMD5Hash();
            testDCWrappersFactory();
		}

        	
        static void testDCWrappersFactory()
        {
            using (var dc = new DCFactory<StoreDataContext>())
		    {
				var list = dc.DataContext.PricingMatrixEntries.Select(s => s.SupplierID).Distinct().ToList();
				foreach (var item in list)
				{
				    Console.WriteLine(item);
				}
				Console.ReadLine();
		    }
        }
                

		static void TestMD5Hash()
		{
			string testString = "mnbvcxz";
			Console.WriteLine( "Source string: " + testString );
			Console.WriteLine( "GetMD5Hash: " + testString.GetMD5Hash() );
			Console.WriteLine( "GetMD5Hash.GetMD5Hash: " + testString.GetMD5Hash().GetMD5Hash() );
			Console.WriteLine( "GetMD5Hash.GetMD5Hash.GetMD5Hash: " + testString.GetMD5Hash().GetMD5Hash().GetMD5Hash() );
			Console.ReadLine();
		}

		static void spTest()
		{
			using (var dc = new dcCommonDataContext())
			{
				try
				{
					var results = dc.ExecuteQuery<int>( @"exec spTest" ).ToList();
					foreach (var res in results)
					{
						Console.WriteLine( res );
					}
					Console.ReadLine();
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					if (dc.Connection.State == System.Data.ConnectionState.Open)
						dc.Connection.Close();
				}
			}
		}

		void SendOrderLineTrackingAlers()
		{
			using (SendOrderLineTrackingAlertsLog log = new SendOrderLineTrackingAlertsLog( LogError, LogMessage ))
			{
				OrderBO.SendOrderLineTrackingAlerts( log );
			}
		}

		static List<SparePartResult> GetPrices( string pn, string brand, bool st )
		{
			List<SparePartResult> result = new List<SparePartResult>();
			/*
			 * Если параметр brand - пустая строка и количество производителей на данный артикул > 1, то возвращаем результат
			 * по всем найденным производителям
			 */
			// Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
			string acctgId = ServiceAccountDac.GetClientIDByWcfServiceAccount( "RMS-AUTO\\murkin" );
			//подгружаем данные о клиенте
			ClientProfile profile = ClientProfile.Load( acctgId );
			RmsAuto.Acctg.ClientGroup clientGroup = profile.ClientGroup;
			decimal personalMarkup = profile.PersonalMarkup;
			
			List<string> manufacturers = new List<string>();
			if (string.IsNullOrEmpty( brand ))
			{
				manufacturers = PricingSearch.SearchSparePartManufactures( pn, st ).Select( s => s.Manufacturer ).ToList<string>();
				if (manufacturers.Count == 0) { return result; }
			}
			else
			{
				manufacturers.Add( brand );
			}

			List<SparePartItem> parts = new List<SparePartItem>();
			foreach (var m in manufacturers)
			{
				var items = PricingSearch.SearchSpareParts( pn, m, st );
				parts.AddRange( items );
			}

			foreach (SparePartItem item in parts)
			{
				SparePartResult res = new SparePartResult()
				{
					DeliveryDaysMax = item.SparePart.DeliveryDaysMax,
					DeliveryDaysMin = item.SparePart.DeliveryDaysMin,
					Manufacturer = item.SparePart.Manufacturer,
					MinOrderQty = item.SparePart.MinOrderQty,
					PartDescription = item.SparePart.PartDescription,
					PartNumber = item.SparePart.PartNumber,
					Price = item.SparePart.GetFinalSalePrice( clientGroup, personalMarkup ),
					SparePartType = item.ItemType,
					SupplierID = item.SparePart.SupplierID
				};
				result.Add( res );
			}

			return result;
		}

		static List<OrderLineStatusResult> GetOrderStatuses( int[] orderids )
		{
			List<OrderLineStatusResult> result = new List<OrderLineStatusResult>();
			// Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
			string acctgId = ServiceAccountDac.GetClientIDByWcfServiceAccount( "RMS-AUTO\\murkin" );

			foreach (int id in orderids)
			{
				RmsAuto.Store.Entities.Order o = OrderBO.LoadOrderData( acctgId, id );
				var lines = o.OrderLines.ToList();//.AsQueryable().ToList();//.Where( OrderBO.TotalStatusExpression );
				foreach (var line in lines)
				{
					OrderLineStatusResult l = new OrderLineStatusResult()
					{
						CurrentStatus = line.CurrentStatus,
						Manufacturer = line.Manufacturer,
						OrderID = line.OrderID,
						PartNumber = line.PartNumber,
						Qty = line.Qty,
						ReferenceID = line.ReferenceID,
						StrictlyThisBrand = false,
						StrictlyThisNumber = line.StrictlyThisNumber,
						StrictlyThisQty = false,
						SupplierID = line.SupplierID,
						UnitPrice = line.UnitPrice
					};
					result.Add( l );
				}
			}
			
			return result;
		}

		static OrderResult SendOrder( OrderLineItemSource[] items )
		{
			// Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
			string acctgId = ServiceAccountDac.GetClientIDByWcfServiceAccount( "RMS-AUTO\\franch01" );
			//подгружаем данные о клиенте
			ClientProfile profile = ClientProfile.Load( acctgId );
			RmsAuto.Acctg.ClientGroup clientGroup = profile.ClientGroup;
			decimal personalMarkup = profile.PersonalMarkup;

			//Dictionary<int, string> infos = new Dictionary<int, string>();
			List<OrderItemResult> infos = new List<OrderItemResult>();
			List<OrderLine> orderLines = new List<OrderLine>();
			bool existSuccesItems = false;
			foreach (var line in items)
			{
				StoreDataContext DC = new StoreDataContext();
				//подгружаем инфу о детали
				SparePartPriceKey key = new SparePartPriceKey( line.Manufacturer, line.PartNumber, line.SupplierID );

				var part = SparePartsDac.Load( DC, key );
				if (part == null)
				{
					infos.Add(
						new OrderItemResult()
						{
							ItemID = line.ItemID,
							ResultInfo = "Запчасть не найдена.",
							ResultCode = 2
						} );
				}
				else if (line.Qty < part.DefaultOrderQty)
				{
					infos.Add(
						new OrderItemResult()
						{
							ItemID = line.ItemID,
							ResultInfo = string.Format( "Минимальное количество для заказа: {0}.", part.DefaultOrderQty ),
							ResultCode = 3
						} );
				}
				else if (line.Qty % part.DefaultOrderQty != 0)
				{
					infos.Add(
						new OrderItemResult()
						{
							ItemID = line.ItemID,
							ResultInfo = "Количество должно быть кратным числу деталей в комплекте.",
							ResultCode = 4
						} );
				}
				else if (part.QtyInStock.GetValueOrDefault() > 0 && line.Qty > part.QtyInStock)
				{
					infos.Add(
						new OrderItemResult()
						{
							ItemID = line.ItemID,
							ResultInfo = string.Format( "Заказанное количество превышает остатки склада, доступно: {0}.", part.QtyInStock ),
							ResultCode = 5
						} );
				}
				else
				{
					OrderLine ol = new OrderLine()
					{
						AcctgOrderLineID = -line.ItemID, //временный ID пока УС не обработает заказ и не проставит свой ID
						Manufacturer = part.Manufacturer,
						PartNumber = part.PartNumber,
						SupplierID = part.SupplierID,
						ReferenceID = line.ReferenceID,
						DeliveryDaysMin = part.DeliveryDaysMin,
						DeliveryDaysMax = part.DeliveryDaysMax,
						PartName = part.PartName,
						PartDescription = part.PartDescription,
						WeightPhysical = part.WeightPhysical,
						WeightVolume = part.WeightVolume,
						UnitPrice = part.GetFinalSalePrice( clientGroup, personalMarkup ),
						Qty = line.Qty,
						StrictlyThisNumber = line.StrictlyThisNumber,
						VinCheckupData = string.Empty,
						OrderLineNotes = string.Empty,
						CurrentStatus = OrderLineStatusUtil.StatusByte( "PreOrder" )
					};
					orderLines.Add( ol );

					infos.Add(
						new OrderItemResult()
						{
							ItemID = line.ItemID,
							ResultInfo = "Позиция принята успешно.",
							ResultCode = 1
						} );
					existSuccesItems = true;
				}
			}
			////если нет ни одной ошибки, то размещаем заказ
			//int? orderID = null;
			//if (infos.Count == 0)
			//{
			int? orderID = null;
			if (existSuccesItems)
			{
				orderID = OrderBO.CreateOrderForService(profile.UserId, profile.ClientId, "1", orderLines, "123" );
			}
			//}
			OrderResult result = new OrderResult()
			{
				OrderID = orderID,
				ItemResults = infos.ToArray()
			};
			return result;
		}

		static Dictionary<byte, string> GetStatuses()
		{			
			using (var dc = new StoreDataContext())
			{
				return dc.OrderLineStatuses
					.Where( s => s.ClientShow )
					.OrderBy( s => s.ClientShowOrder )
					.Select( s => new { s.OrderLineStatusID, s.Client } )
					.ToDictionary( kvp => kvp.OrderLineStatusID, kvp => kvp.Client );
			}
		}

		class SendOrderLineTrackingAlertsLog : OrderBO.ISendOrderLineTrackingAlertsLog, IDisposable
		{
			Action<Exception> _logErrorAction;
			Action<string> _logMessageAction;
			Stopwatch _stopWatch;

			int _successfulCount = 0;
			int _managerListFailedCount = 0;
			int _emptyEmailCount = 0;
			int _invalidEmailCount = 0;
			int _errorCount = 0;

			public SendOrderLineTrackingAlertsLog( Action<Exception> logErrorAction, Action<string> logMessageAction )
			{
				_logErrorAction = logErrorAction;
				_logMessageAction = logMessageAction;
				_stopWatch = Stopwatch.StartNew();
			}


			public void LogSuccessfulAlert( string clientId )
			{
				_successfulCount++;
			}

			public void LogManagerListRequestFailed()
			{
				_managerListFailedCount++;
				_logMessageAction( "Manager list request failed" );
			}

			public void LogEmptyEmail( string clientId )
			{
				_emptyEmailCount++;
				_logMessageAction( string.Format( "Send alert to client {0}: empty email", clientId ) );
			}

			public void LogInvalidEmail( string clientId, string email )
			{
				_invalidEmailCount++;
				_logMessageAction( string.Format( "Send alert to client {0}: invalid email: {1}", clientId, email ) );
			}

			public void LogError( string clientId, Exception ex )
			{
				_errorCount++;
				_logErrorAction( ex );
			}

			public void Dispose()
			{
				_stopWatch.Stop();

				StringBuilder sb = new StringBuilder();
				sb.AppendFormat( "Send alerts completed.\r\n" );
				sb.AppendFormat( "Successful alerts: {0}\r\n", _successfulCount );
				sb.AppendFormat( "Failed manager list requests: {0}\r\n", _managerListFailedCount );
				sb.AppendFormat( "Empty emails: {0}\r\n", _emptyEmailCount );
				sb.AppendFormat( "Invalid emails: {0}\r\n", _invalidEmailCount );
				sb.AppendFormat( "Errors: {0}\r\n", _errorCount );
				sb.AppendFormat( "Elapsed time: {0}", _stopWatch.Elapsed );

				_logMessageAction( sb.ToString() );
			}
		}

		private void LogEvent( string msg, EventLogEntryType etype )
		{
			var eventSource = "RmsAuto.ru";

			if (!EventLog.SourceExists( eventSource ))
				EventLog.CreateEventSource( eventSource, null );

			EventLog.WriteEntry( eventSource, msg, etype );
		}

		protected void LogMessage( string msg )
		{
			LogEvent( msg, EventLogEntryType.Information );
		}

		protected void LogMessage( string msg, params object[] args )
		{
			LogEvent( string.Format( msg, args ), EventLogEntryType.Information );
		}

		protected void LogError( Exception ex )
		{
			LogEvent( ex.ToString(), EventLogEntryType.Error );
		}
	}
}
