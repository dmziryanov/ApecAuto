using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RmsAuto.Common.References;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Acctg.Entities
{
    public class ServiceProxy
    {
        private static readonly Dictionary<Type, ProxyType> _proxyTypeCache = new Dictionary<Type, ProxyType>();
        private static readonly object _sync = new object();
        private ProxyType _proxyType;

        public static readonly ServiceProxy Default = new ServiceProxy();

        public ServiceProxy()
        {
            Type type = this.GetType();
            if (!_proxyTypeCache.ContainsKey(type))
                lock (_sync)
                    if (!_proxyTypeCache.ContainsKey(type))
                        _proxyTypeCache.Add(type, new ProxyType(type));
            _proxyType = _proxyTypeCache[type];
        }

        public ServiceProxy(string url, string username, string password, int? timeout)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Acctg_service_url cannot be empty", "url");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Acctg_service_username cannot be empty", "username");
         

            

            Type type = this.GetType();
            if (!_proxyTypeCache.ContainsKey(type))
                lock (_sync)
                    if (!_proxyTypeCache.ContainsKey(type))
                        _proxyTypeCache.Add(type, new ProxyType(type));
            _proxyType = _proxyTypeCache[type];
        }

        /// <summary>
        /// Новый метод отправки заказа (без использования сервиса, просто сохраняем этот же xml-ник в БД)
        /// </summary>
        [ServiceMethod("SendOrder", typeof(SendOrderEnvelope), typeof(SendOrderResultsEnvelope))]
        public void SendOrder(StoreDataContext dc, OrderInfo order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            SaveRequestToDB(dc, "order", "SendOrder", new SendOrderEnvelope { ClientId = order.ClientId, Order = order });
        }

        [ServiceMethod("SendClientBalance", typeof(SendClientBalanceEnvelope), typeof(AcknowledgementEnvelope))]
        public string SendClientBalance(string clientId, DateTime dateStart, DateTime dateEnd)
        {
            SaveRequestToDB(
                "balance",
                "SendClientBalance",
                new SendClientBalanceEnvelope
                {
                    ClientId = clientId,
                    DateStart = dateStart.ToString("yyyyMMdd"),
                    DateEnd = dateEnd.ToString("yyyyMMdd")
                });
            return "OK";
        }

		[ServiceMethod("SendNonDeliveryRequest", typeof(SendClientNonDeliveryRequestEnvelope), typeof(AcknowledgementEnvelope))]
		public string SendNonDeliveryRequest(string clienId, string orderId, string dataForXml)
		{
			SaveRequestToDB(
				"nondelivery",
				"SendNonDeliveryRequest",
				new SendClientNonDeliveryRequestEnvelope
				{
					ClientId = clienId,
					OrderId = orderId,
					Data = dataForXml
				});
			return "OK";
		}

        [ServiceMethod("SendFranchBlankRequest", typeof(FranchEnvelope), typeof(AcknowledgementEnvelope))]
        public string SendFranchBlankRequest(FranchEnvelope fe)
        {
            SaveRequestToDB( "franch", "SendFranchBlankRequest", fe );
            return "OK";
        }

		private void SaveRequestToDB(string requestType, string methodName, Envelope argsEnvelope)
		{
			var method = _proxyType.GetMethod( methodName );
			var messageSerializer = new MessageSerializer( method );

			// получаем xml-запроса как и раньше
			string requestXml = messageSerializer.SerializeRequest("", "", argsEnvelope);
			// убираем xml-заголовок (<?xml version="1.0" encoding="utf-16"?>), чтобы сохранить в БД
			requestXml = Regex.Replace(requestXml, @"<\?xml.*\?>", string.Empty);

			// теперь просто сохраняем его в БД
			using (var dc = new DCFactory<StoreDataContext>())
			{
				try
				{
					dc.DataContext.ExecuteCommand( "INSERT into Acctg.Requests VALUES ({0}, {1}, GETDATE())", requestType, requestXml );
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
                    //Теперь закрываемся в фабрике
                    //
				}
			}
		}

		/// <summary>
		/// Для отправки заказа использовать этот метод, т.к. в него проброшен DataContext для соблюдения танзакции
		/// </summary>
		/// <param name="dc">DataContext</param>
		/// <param name="requestType">Тип запроса</param>
		/// <param name="methodName">Имя метода</param>
		/// <param name="argsEnvelope">Данные</param>
		private void SaveRequestToDB(StoreDataContext dc, string requestType, string methodName, Envelope argsEnvelope)
		{
			var method = _proxyType.GetMethod(methodName);
			var messageSerializer = new MessageSerializer(method);

			// получаем xml-запроса как и раньше
			string requestXml = messageSerializer.SerializeRequest("", "", argsEnvelope);
			// убираем xml-заголовок (<?xml version="1.0" encoding="utf-16"?>), чтобы сохранить в БД
			requestXml = Regex.Replace(requestXml, @"<\?xml.*\?>", string.Empty);

			// теперь просто сохраняем его в БД
			//using (var dc = new DCFactory<StoreDataContext>())
			//{
				try
				{
					dc.ExecuteCommand("INSERT into Acctg.Requests VALUES ({0}, {1}, GETDATE())", requestType, requestXml);
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					//Теперь закрываемся в фабрике
					//
				}
			//}
		}

    }

    public class AcctgItemProxy<TItem> : IItemsProxy<TItem>
    {

        #region IItemsProxy<TItem> Members

        public IEnumerable<TItem> GetItems()
        {
            if ((typeof(TItem).FullName) == typeof(CurrencyRate).FullName)
            {
                return GetCurrencyRates() as IEnumerable<TItem>;
            }
            
            if ((typeof(TItem).FullName) == typeof(StoreInfo).FullName)
            {
                return GetStores() as IEnumerable<TItem>;
            }

            if ((typeof(TItem).FullName) == typeof(City).FullName)
            {
                return GetCities() as IEnumerable<TItem>;
            }

            if ((typeof(TItem).FullName) == typeof(Region).FullName)
            {
                return GetRegions() as IEnumerable<TItem>;
            }

            if ((typeof(TItem).FullName) == typeof(EmployeeInfo).FullName)
            {
                return GetEmployees() as IEnumerable<TItem>;
            }

            if ((typeof(TItem).FullName) == typeof(spSelFranchesFromRmsResult).FullName)
            {
                return GetFranches() as IEnumerable<TItem>;
            }

            throw new Exception("Не найден метод поддерживающий данный тип");
        }

        #endregion

     
        public IEnumerable<CurrencyRate> GetCurrencyRates()
        {
            //Теперь курс валют забираем из своей БД, а вот там ежедневно (или по мере изменения) его должна обновлять 1С посредством хранимки
            CurrencyRate[] res = null;
            using (var dc = new StoreDataContext())
            {
                try
                {
                    IEnumerable<CurrencyRate> rates = dc.ExecuteQuery<CurrencyRate>
                        (@"SELECT CurrencyCode, CurrencyName, Rate, LastDate FROM Acctg.CurrencyRates");

                    res = rates.ToArray();
                }
                
                catch
                {
                    //не падаем, просто возвращаем null
                }

                finally
                {
                    //if (dc.DataContext.Connection.State == System.Data.ConnectionState.Open)
                    //    dc.DataContext.Connection.Close();
                }
            }
            return res;
        }

      
        public IEnumerable<StoreInfo> GetStores()
        {
          
            //Теперь список магазинов забираем из своей БД, а вот там ежедневно (или по мере изменения) его должна обновлять 1С посредством хранимки

            StoreInfo[] res = null;
            using (var dc = new DCFactory<StoreDataContext>())
            {
                IEnumerable<StoreInfo> stores;
                try
                {
                    if (SiteContext.Current.InternalFranchName == "rmsauto" || SiteContext.Current.InternalFranchName == null)
                    {
                        stores = dc.DataContext.ExecuteQuery<StoreInfo>
                        ("SELECT StoreId, StoreNumber, StoreName, Address, Phone, IsRetail, IsWholesale FROM Acctg.Shops");
                    }
                    else
                    {
                        if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
                        {
                            stores = dc.DataContext.ExecuteQuery<StoreInfo>
                            ("SELECT StoreId, StoreNumber, ShopName, ShopAddress as Address, ShopPhones as Phone, IsRetail, IsWholesale FROM Cms.Shops where FranchName='" + SiteContext.Current.InternalFranchName+"'");
                        }
                        else
                        {
                            stores = dc.DataContext.ExecuteQuery<StoreInfo>
                            ("SELECT StoreId, StoreNumber, ShopName, ShopAddress as Address, ShopPhones as Phone, IsRetail, IsWholesale FROM Cms.Shops");
                        }
                    }

                    res = stores.ToArray();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    //не падаем, просто возвращаем null
                }
                finally
                {
                    //Теперь закрываемся в DCFactory
                }
            }
            return res;
        }

        public IEnumerable<RmsAuto.Store.Entities.City> GetCities()
        {
            IEnumerable<City> rezItems = null;
            // теперь забираем данные из "своей" таблицы, а синхронизация - проблема УС
            using (var dc = new dcCommonDataContext())
            {
                try
                {
                    rezItems = dc.Cities.Select(x => x).OrderBy(x=> x.Name).ToArray();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (dc.Connection.State == System.Data.ConnectionState.Open)
                        dc.Connection.Close();
                }
            }

            return rezItems;
        }

        public IEnumerable<RmsAuto.Store.Entities.Region> GetRegions()
        {
            IEnumerable<Region> rezItems = null;
            // теперь забираем данные из "своей" таблицы, а синхронизация - проблема УС
            using (var dc = new RmsAuto.Store.Entities.dcCommonDataContext())
            {
                try
                {
                    rezItems = dc.Regions.Select(x => x).OrderBy(x => x.RegionName).ToArray();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (dc.Connection.State == System.Data.ConnectionState.Open)
                        dc.Connection.Close();
                }
            }

            return rezItems;
        }

        public IEnumerable<EmployeeInfo> GetEmployees()
        {
            IEnumerable<EmployeeInfo> rezItems = null;
            // теперь забираем данные из "своей" таблицы, а синхронизация - проблема УС
            using (var dc = new DCFactory<StoreDataContext>())
            {
                try
                {
					if (LightBO.IsLight())
					{
						if (SiteContext.Current != null)
							rezItems = dc.DataContext.ExecuteQuery<EmployeeInfo>("SELECT * FROM Acctg.EmployeesRef where InternalFranchName = '" + SiteContext.Current.InternalFranchName + "'").ToArray();
						else
							rezItems = dc.DataContext.ExecuteQuery<EmployeeInfo>("SELECT * FROM Acctg.EmployeesRef").ToArray();
					}
					else
					rezItems = dc.DataContext.ExecuteQuery<EmployeeInfo>("SELECT * FROM Acctg.EmployeesRef").ToArray();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                   //теперь закрываемся в фабрике
                }
            }

            return rezItems;
        }

        public IEnumerable<spSelFranchesFromRmsResult> GetFranches()
        {
            IEnumerable<spSelFranchesFromRmsResult> rezItems = null;

            //справочник франчей забираем из rmsauto всегда
            using (var dc = new CmsDataContext())
            {
                try
                {
                    rezItems = dc.spSelFranchesFromRms().ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (dc.Connection.State == System.Data.ConnectionState.Open)
                        dc.Connection.Close();
                }
            }

            return rezItems;
        }
    }
}
