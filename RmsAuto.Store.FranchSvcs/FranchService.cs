using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.FranchSvcs
{
	//[ServiceBehavior( AddressFilterMode = AddressFilterMode.Any )]
	public class FranchService : IFranchService
	{
		//Статические поля
        private const string _SEMAPHORE = "PRICE_LOAD_LOCK";
        private static string TimeFormat = ConfigurationManager.AppSettings["DateTimeFormat"];
        private static SqlConnection connection { get; set; }

        //Эти поля нельзя сделать статиками, так как  при инициалиции экземпляра веб-сервиса они заполнятся 1 раз
        private string LoggerTemplate = @"User: " + CurrentIdentityName +  ";\rStep of excecution: {2};\r{0};\rDateTime: {1};";
        string acctgId;
        string[] Rights;
        ClientProfile profile;

        public FranchService()
        {
            acctgId = ServiceAccountDac.GetClientIDByWcfServiceAccount(CurrentIdentityName);
            Rights =  ServiceAccountDac.getRightsByClientID(acctgId);  
            profile = ClientProfile.Load( acctgId );
        }

        /// <summary>
        /// Коды результата обработки строки заказа
        /// </summary>
        private enum ItemResultCode : int
        {
            [Text("Позиция принята успешно.")]
            Successs = 1,				//"Позиция принята успешно."
            [Text("Запчасть не найдена.")]
            SparePartNotFound = 2,		//"Запчасть не найдена."
            [Text("Минимальное количество для заказа: {0}.")]
            MinOrderQtyError = 3,		//"Минимальное количество для заказа: {0}."
            [Text("Количество должно быть кратным числу деталей в комплекте.")]
            DefaultOrderQtyError = 4,	//"Количество должно быть кратным числу деталей в комплекте."
            [Text("Заказанное количество превышает остатки склада, доступно: {0}.")]
            QtyInStockError = 5,		//"Заказанное количество превышает остатки склада, доступно: {0}."
            [Text("Несоответствие цены ФТП-прайсу.")]
            PriceError = 6				//"Несоответствие цены ФТП-прайсу"
        }

        public object TestMethod()
        {
            try
            {
                return "test method call successful";
            }
            catch (Exception e)
            {
                Object o = new Object();
                o = "EXCEPTION MESSAGE: " + e.Message + Environment.NewLine +
                    "EXCEPTION STACK TRACE: " + e.StackTrace + Environment.NewLine +
                    "EXCEPTION SOURCE: " + e.Source + Environment.NewLine +
                    "INNER EXCEPTION: " + (e.InnerException == null ? "NULL" : "MESSAGE " + e.InnerException.Message);
                return o;
            }
        }

        public List<SupplierResult> SupplierTimes()
        {
            if (!Rights.Contains("5"))
            {
                //TODO: убрать Exception logger  Logger.WriteError(CurrentIdentityName + ": " + "Access to webservice Denied", EventLogerID.BLException, EventLogerCategory.WebServiceError);
                throw new Exception("Access Denied");
            }

            var supplierInfos = FranchBO.GetSuppliers();
			return supplierInfos.ConvertAll<SupplierResult>(
                s => new SupplierResult()
                {
                    SupplierID = s.SupplierID,
                    DeliveryDaysMin = s.DeliveryMinDays,
                    DeliveryDaysMax = s.DeliveryMaxDays
                });
        }

        #region === old GetPrices (one brand) ===
        //public List<SparePartResult> GetPrices( string pn, string brand, bool st )
        //{
        //    List<SparePartResult> result = new List<SparePartResult>();
        //    /*
        //     * Если параметр brand - пустая строка и количество производителей на данный артикул > 1, то возвращается результат
        //     * по первому из них
        //     */
        //    // Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
        //    string acctgId = ServiceAccountDac.GetClientIDByWcfServiceAccount( CurrentIdentityName );
        //    //подгружаем данные о клиенте
        //    ClientProfile profile = ClientProfile.Load( acctgId );
        //    ClientGroup clientGroup = profile.ClientGroup;
        //    decimal personalMarkup = profile.PersonalMarkup;

        //    string manufacturer = string.Empty;
        //    if (string.IsNullOrEmpty( brand ))
        //    {
        //        var manufacturers = PricingSearch.SearchSparePartManufactures( pn, st );

        //        if (manufacturer.Length > 0) { manufacturer = manufacturers[0].Manufacturer; }
        //        else { return result; }
        //    }
        //    else
        //    {
        //        manufacturer = brand;
        //    }
        //    var parts = PricingSearch.SearchSpareParts( pn, manufacturer, st );

        //    foreach (SparePartItem item in parts)
        //    {
        //        SparePartResult res = new SparePartResult()
        //        {
        //            DeliveryDaysMax = item.SparePart.DeliveryDaysMax,
        //            DeliveryDaysMin = item.SparePart.DeliveryDaysMin,
        //            Manufacturer = item.SparePart.Manufacturer,
        //            MinOrderQty = item.SparePart.MinOrderQty,
        //            PartDescription = item.SparePart.PartDescription,
        //            PartNumber = item.SparePart.PartNumber,
        //            Price = item.SparePart.GetFinalSalePrice(clientGroup, personalMarkup),
        //            SparePartType = item.ItemType,
        //            SupplierID = item.SparePart.SupplierID
        //        };
        //        result.Add( res );
        //    }

        //    return result;
        //}
        #endregion

        public List<SparePartResult> GetPrices(string pn, string brand, bool st)
        {
            if (!Rights.Contains("1")) {
                //TODO: убрать Exception logger Logger.WriteError(CurrentIdentityName + ": " + "Access to webservice Denied", EventLogerID.BLException, EventLogerCategory.WebServiceError);
                throw new Exception("Access Denied");
            }
            
            List<SparePartResult> result = new List<SparePartResult>();
            /*
             * Если параметр brand - пустая строка и количество производителей на данный артикул > 1, то возвращаем результат
             * по всем найденным производителям
             */
            // Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте

            RmsAuto.Acctg.ClientGroup clientGroup = profile.ClientGroup;
            decimal personalMarkup = profile.PersonalMarkup;

            List<string> manufacturers = new List<string>();
            if (string.IsNullOrEmpty(brand))
            {
                manufacturers = PricingSearch.SearchSparePartManufactures(pn, st).Select(s => s.Manufacturer).ToList<string>();
                OperationContext ctx = OperationContext.Current;
                MessageProperties messageProperties = ctx.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                SearchSparePartsLogDac.AddWebServiceLog(DateTime.Now, pn, brand, endpointProperty.Address, acctgId);
                if (manufacturers.Count == 0) { return result; }
            }
            else
            {
                manufacturers.Add(brand);
            }

            List<SparePartItem> parts = new List<SparePartItem>();
            foreach (var m in manufacturers)
            {
                var items = PricingSearch.SearchSpareParts(pn, m, st);
                parts.AddRange(items);
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
                    Price = item.SparePart.GetFinalSalePrice(clientGroup, personalMarkup),
                    SparePartType = item.ItemType,
                    SupplierID = item.SparePart.SupplierID,
                    QtyInStock = item.SparePart.QtyInStock,
                    PriceDate = item.SparePart.PriceDate
                };
                result.Add(res);
            }

            return result;
        }

        public List<OrderLineStatusResult> GetOrderStatuses(int[] orderids)
        {
            if (!Rights.Contains("2"))
            {
                //TODO: убрать Exception logger Logger.WriteError(CurrentIdentityName + ": " + "Access Denied", EventLogerID.BLException, EventLogerCategory.WebServiceError);
                throw new Exception("Access Denied");
            }
            
            List<OrderLineStatusResult> result = new List<OrderLineStatusResult>();
            // Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
           

			foreach (int id in orderids)
			{
				RmsAuto.Store.Entities.Order o = OrderBO.LoadOrderData( acctgId, id );
				var lines = o.OrderLines.ToList();
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

		//TODO: Здесь диспозиться как-то объект, из-за этого метод нельзя вызвать 2 раза за сессию
        public OrderResult SendOrder(OrderLineItemSource[] items, string custOrderNum)
		{
            if (!Rights.Contains("3"))
            {
                var ex = new Exception("Access Denied");
                Logger.WriteError(CurrentIdentityName + ": Access Denied", EventLogerID.BLException, EventLogerCategory.WebServiceError, ex);
                throw ex;
            }
            
			OrderResult result = null;
			_getAppLock(new StoreDataContext().Connection.ConnectionString);
			try
			{
                RmsAuto.Acctg.ClientGroup clientGroup = profile.ClientGroup;
                Logger.WriteInformation(string.Format(LoggerTemplate, "Order lines to process: " + items.Length, DateTime.Now.ToString(TimeFormat), "Enter 'SendOrder' method of web-service"), EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceError, new Object[] { true });
				if (items.Length == 0) {
                    var ex = new Exception("Количество строк заказа должно быть  > 0");
					Logger.WriteError(string.Format(LoggerTemplate, items.Length, DateTime.Now.ToString(TimeFormat), "Error checking number of lines: must be > 0"), EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceError, ex);
                    throw ex; 
                }
				// Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
                 
				decimal personalMarkup = profile.PersonalMarkup;
				//подгружаем "настройки" клиента
				StoreDataContext DC = new StoreDataContext();

				var userSet = DC.spSelUserSetting(profile.UserId).FirstOrDefault();
				byte PrcExcessPrice = userSet == null ? (byte)0 : userSet.PrcExcessPrice;
				//подгружаем список "собственных складов наличия"
				List<int> ownStores = StoreRefCatalog.RefOwnStores.Items.Select(x => x.SupplierID).ToList();

				//TODO: Вынести логику проверки в общий метод для загрузки через Excel
				List<OrderItemResult> infos = new List<OrderItemResult>();
				List<OrderLine> orderLines = new List<OrderLine>();
				bool existSuccesItems = false;
				foreach (var line in items)
				{
					//подгружаем инфу о детали
					SparePartPriceKey key = new SparePartPriceKey(line.Manufacturer, line.PartNumber, line.SupplierID);
					var part = SparePartsDac.Load(DC, key);
					//производим количественные проверки, и проверку наличия детали
					if (part == null)
					{
                        infos.Add(
						new OrderItemResult()
						{
							ItemID = line.ItemID,
							ResultInfo = ItemResultCode.SparePartNotFound.ToTextOrName(),
							ResultCode = (int)ItemResultCode.SparePartNotFound
						});
					}
					else if (line.Qty < part.DefaultOrderQty)
					{
                        infos.Add(
							new OrderItemResult()
							{
								ItemID = line.ItemID,
								ResultInfo = string.Format(ItemResultCode.MinOrderQtyError.ToTextOrName(), part.DefaultOrderQty),
								ResultCode = (int)ItemResultCode.MinOrderQtyError
							});
					}
					else if (line.Qty % part.DefaultOrderQty != 0)
					{
                        infos.Add(
							new OrderItemResult()
							{
								ItemID = line.ItemID,
								ResultInfo = ItemResultCode.DefaultOrderQtyError.ToTextOrName(),
								ResultCode = (int)ItemResultCode.DefaultOrderQtyError
							});
					}
					else if (part.QtyInStock.GetValueOrDefault() > 0 && line.Qty > part.QtyInStock)
					{
                        infos.Add(
							new OrderItemResult()
							{
								ItemID = line.ItemID,
								ResultInfo = string.Format(ItemResultCode.QtyInStockError.ToTextOrName(), part.QtyInStock),
								ResultCode = (int)ItemResultCode.QtyInStockError
							});
					}
					else
					{
						decimal? priceClientYesterday = null; // "Вчерашная" цена
						bool isErrorPrice = false;
						//проверка цены
						//(т.е. если текущая цена > цена клиента + допустимый % превышения цены, то это факт превышения цены)
						if (part.GetFinalSalePrice(profile.ClientGroup, profile.PersonalMarkup) >
							line.UnitPrice + Math.Round(line.UnitPrice * PrcExcessPrice / 100, 2) &&
							//т.е. расчет "вчерашних" цен производится ТОЛЬКО для собственных складов наличия
							ownStores != null && ownStores.Contains(part.SupplierID))
						{
							using (var dc = new dcCommonDataContext())
							{
								try
								{
									//получаем список "вчерашних" и сегодняшних цен фтп для сравнения
									//если загрузка производится в понедельник, то "вчерашние" цены фтп должны браться по пятнице, субботе и воскресенью
									string queryTemplate = "exec srvdb4.az.dbo.spGetPricesOnPeriod @Number='{0}', @Brand='{1}', @DateFrom='{2}', @DateTo='{3}', @PriceColumn={4}";
 									DateTime dateFrom = (DateTime.Now.DayOfWeek == DayOfWeek.Monday) ? DateTime.Now.AddDays(-3) : DateTime.Now.AddDays(-1);
									DateTime dateTo = DateTime.Now;
									List<decimal> prices = dc.ExecuteQuery<decimal>(
											string.Format(queryTemplate,
												part.PartNumber,
												part.Manufacturer,
												dateFrom.ToString("yyyyMMdd"),
												dateTo.ToString("yyyyMMdd"),
												(int)profile.ClientGroup))
											.ToList<decimal>();

									//для каждой из цен производим сравнение с ценой клиента +- 1 копейка (чтобы учесть возможные погрешности при округлении)
									//TODO: Переписать через LINQ

									foreach (var price in prices)
									{
										decimal pClient = Math.Round(line.UnitPrice, 2);	//цена клиента в 'Excel'
										decimal pFtp = Math.Round(price, 2);				//одна из вчерашних цен нашего Ftp
										if (pClient > 0 && (pClient == pFtp ||
											pClient == (pFtp - 0.01M) ||
											pClient == (pFtp + 0.01M)))
										{
											//добавляем допустимый % превышения (доли копеек отбрасываем)
											priceClientYesterday = pFtp + pFtp * PrcExcessPrice / 100;
											priceClientYesterday = decimal.Truncate((decimal)priceClientYesterday * 100) / 100;
											break;
										}
									}
									//если ни одного совпадения не найдено, то позиция отваливается с ошибкой "Несоответствие цены ФТП-прайсу"
									isErrorPrice = (prices.Count() > 0 && !priceClientYesterday.HasValue);

									if (prices.Count() == 0) //Если цен не найдено, ошибка
										isErrorPrice = true;
								}
								catch (Exception ex)
								{
                                    Logger.WriteError(@"Error while calculating 'Yesterday' price!", EventLogerID.UnknownError, EventLogerCategory.UnknownCategory, ex);
								}
								finally
								{
								
                                    if (dc.Connection.State == System.Data.ConnectionState.Open)
										dc.Connection.Close();
								}
							}
						}

						if (isErrorPrice)
						{
							infos.Add(
							new OrderItemResult()
							{
								ItemID = line.ItemID,
								ResultInfo = ItemResultCode.PriceError.ToTextOrName(),
								ResultCode = (int)ItemResultCode.PriceError
							});
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
								UnitPrice = (priceClientYesterday.HasValue ? priceClientYesterday.Value : part.GetFinalSalePrice(clientGroup, personalMarkup)),
								Qty = line.Qty,
								StrictlyThisNumber = line.StrictlyThisNumber,
								VinCheckupData = string.Empty,
								OrderLineNotes = string.Empty,
								CurrentStatus = OrderLineStatusUtil.StatusByte("PreOrder")
							};
							orderLines.Add(ol);

							infos.Add(
								new OrderItemResult()
								{
									ItemID = line.ItemID,
									ResultInfo = ItemResultCode.Successs.ToTextOrName(),
									ResultCode = (int)ItemResultCode.Successs
								});
							existSuccesItems = true;
						}
					}
				}

				int? orderID = null;
				if (existSuccesItems)
				{
					orderID = OrderBO.CreateOrderForService(profile.UserId, profile.ClientId, "1", orderLines, custOrderNum);
				}

				result = new OrderResult()
				{
					OrderID = orderID,
					ItemResults = infos.ToArray()
				};

				if (DC.Connection.State == System.Data.ConnectionState.Open)
				{
					DC.Connection.Close();
				}

				_releaseAppLock();
			}
			catch (Exception ex)
			{
				if (connection.State == System.Data.ConnectionState.Open)
				{
					try
					{
						_releaseAppLock();
					}
                    catch (Exception ex1)
					{
						connection.Close();
                        Logger.WriteError(string.Format(LoggerTemplate, result.ItemResults.Length, DateTime.Now.ToString(TimeFormat), "Error while release lock"), EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceError, ex1);
					}
				}

                Logger.WriteError(string.Format(LoggerTemplate, "Order lines processed: not defined", DateTime.Now.ToString(TimeFormat), "Exception while executing web-service"), EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceError, ex);
			}

            //Записываем сколько на выходе
            //TODO: возможно это можно переписать через какое-то LINQ
            var q1 =  result.ItemResults.Where(x => x.ResultCode == (int)ItemResultCode.Successs).Count();
            var q2 =  result.ItemResults.Where(x => x.ResultCode == (int)ItemResultCode.SparePartNotFound).Count();
            var q3 =  result.ItemResults.Where(x => x.ResultCode == (int)ItemResultCode.QtyInStockError).Count();
            var q4 =  result.ItemResults.Where(x => x.ResultCode == (int)ItemResultCode.PriceError).Count();
            var q5 =  result.ItemResults.Where(x => x.ResultCode == (int)ItemResultCode.MinOrderQtyError).Count();

            Logger.WriteInformation(string.Format(LoggerTemplate, "OrderID: " + (result.OrderID) + "\rOrder lines processed: " + result.ItemResults.Count() + "\r  Success: " + q1 + ",\r  SparePartNotFound: " + q2 + ",\r  QtyInStockError: " + q3 + ",\r  PriceError: " + q4 + ",\r  MinOrderQtyError: " + q5, DateTime.Now.ToString(TimeFormat), "Successful exit of 'SendOrder' method"), EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceLogic, new Object[] { true });
			return result;
		}

		public Dictionary<byte, string> GetStatuses()
		{
            if (!Rights.Contains("4"))
            {
                //TODO: убрать Exception logger Logger.WriteError(CurrentIdentityName + ": " + "Access Denied", EventLogerID.BLException, EventLogerCategory.BLError);
                throw new Exception("Access Denied");
            }
			
            using (var dc = new StoreDataContext())
			{
				return dc.OrderLineStatuses
					.Where( s => s.ClientShow )
					.OrderBy( s => s.ClientShowOrder )
					.Select( s => new { s.OrderLineStatusID, s.Client } )
					.ToDictionary( kvp => kvp.OrderLineStatusID, kvp => kvp.Client );
			}
		}

		private static string CurrentIdentityName
		{
			get
			{
				try
				{

					OperationContext ctx = OperationContext.Current;
					return ctx.ServiceSecurityContext.PrimaryIdentity.Name;
				}
				catch (Exception ex)
				{
                    Logger.WriteError(ex.Message, EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceError);
                    throw ex;

				}
			}
		}

		private static void _getAppLock(string connectionString)
		{
			//Выставляем "семафор" средствами SQL-Server
			string query = "exec @result = sp_getapplock @Resource = '" + _SEMAPHORE + "', @LockMode = 'Exclusive', @LockOwner = 'Session', @LockTimeout = 1";

			//string query = "exec sp_releaseapplock @Resource = '" + _SEMAPHORE + "', @LockOwner = 'Session'";
			if (connection == null)
			{
				connection = new SqlConnection(connectionString);
			}
			SqlCommand cmd = new SqlCommand(query, connection);
			SqlParameter p = new SqlParameter("@result", System.Data.SqlDbType.Int);
			p.Direction = System.Data.ParameterDirection.Output;
			cmd.Parameters.Add(p);

			if (connection.State == System.Data.ConnectionState.Closed)
			{
				connection.Open();
			}
			cmd.ExecuteNonQuery();
			int lockResult = Convert.ToInt32(cmd.Parameters[0].Value);
			if (lockResult < 0)
			{
                var ex = new Exception("В данный момент заказ невозможен. Повторите попытку позже.");
                Logger.WriteError("Message: Error while acquiring DataBase lock;\rDateTime: " + DateTime.Now.ToString(TimeFormat), EventLogerID.AdditionalLogic, EventLogerCategory.WebServiceError, ex);
                throw ex;
			}
		}

		private static void _releaseAppLock()
		{
			string query = "exec sp_releaseapplock @Resource = '" + _SEMAPHORE + "', @LockOwner = 'Session'";

			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.ExecuteNonQuery();
		}
	}
}
