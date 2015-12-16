using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using System.Data.SqlClient;
using System.Collections;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;
using System.Data.Linq;
using RmsAuto.Common.Misc;
using System.Configuration;
using RmsAuto.Common.Data;
using RmsAuto.Store.Dac;
using System.Data;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using RmsAuto.Store.Cms.Entities;

using RmsAuto.Store.Web.Resource;

namespace RmsAuto.Store.BL
{

    [Serializable()]
    public class SaleReportString : ReportString
    {
        //Важно чтобы эти поля были свойствами
      

        
    }

    [Serializable()]
    public class SupplyReportString : ReportString
    {
       // public static explicit operator SupplyReportString(ReportString value)
       // {
           // SupplyReportString newValue;
                //newValue.AcctgOrderLineID = value.AcctgOrderLineID;
                //newValue.Manufacturer = value.Manufacturer;
          

         //   return newValue;
        //}  
    }


    [Serializable()]
    public class ReportString
    {
        public decimal ProfitSumm { get; set; }
        public decimal ProfitPercent { get; set; }
        public int AcctgOrderLineID { get; set; }
        public string Manufacturer { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string SupplierName { get; set; }
        public decimal SupplyPrice { get; set; }
        public decimal SupplyTotal { get; set; }
        public int SupplierId { get; set; }
        public int OrderId { get; set; }
        public const string TotalString = "Total:";

        public string ClientNameDecor
        {
            get
            {
                if (ClientName != TotalString) return ClientName + ClientID.WithBrackets();
                return TotalString;
            }
            
            set {
                
            }
        }

        public int SortAttribute { get; set; }
        public DateTime StatusChangeTime { get; set; }
        public string ClientName { get; set; }
        public string ClientID { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

        public decimal? SupplierPriceWithMarkup
        {
            get
            {
                using (var dc = new DCFactory<StoreDataContext>())
                {
                    return dc.DataContext.ExecuteQuery(typeof(decimal?), "select SupplierPriceWithMarkup from dbo.OrderLines where AcctgOrderLineID = {0} ", this.AcctgOrderLineID).OfType<decimal?>().FirstOrDefault();
                }
            }

            set
            {

            }
        }
    }


    public static class LightBO
    {
        
        public const int SupplyOrderLineStatus = 161;
        public const int SaleOrderLineStatus = 162;
        
        /// <summary>
        /// Вставляем данные во временную таблицу
        /// </summary>
        /// <param name="items">данные прочитанные из Excel-файла</param>
        public static void InsertOrderLinesNewStatuses(List<TempOLStatuse> items)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.DataContext.TempOLStatuses.InsertAllOnSubmit(items);
                dc.DataContext.SubmitChanges();
            }
        }


        public static void FillSupplyPrice<T>(IEnumerable<T> ordlnsdto) where T : ReportString
        {
            using (var dc = new DCFactory<StoreDataContext>(IsolationLevel.ReadCommitted, true, "rmsauto", false))
            {
                //Для Жмеринки сделали исключение
                if (SiteContext.Current.InternalFranchName == "zhm01") { dc.DataContext.Connection.ConnectionString = dc.DataContext.Connection.ConnectionString.Replace("sqlwebcluster", "1CTest"); }
                dc.DataContext.Connection.Open();

                //Выбираем лайтовых пользователей
                var AcctgUserId = dc.DataContext.ExecuteQuery<string>("select AcctgUserId from dbo.LightUsers where InternalFranchName = {0}", SiteContext.Current.InternalFranchName).FirstOrDefault();
                //Выбираем строку заказа из базу РМС, где цена это и есть закупочная цена франча
                var OrderIds = dc.DataContext.Orders.Where(x => x.ClientID == AcctgUserId).Select(x => x.OrderID);

                foreach (var ordln in ordlnsdto)
                {
                    if (ordln != null)
                    {
                        var d = dc.DataContext.OrderLines.Where(x => OrderIds.Contains(x.OrderID) && x.ReferenceID.Replace(" ", "") == (ordln.AcctgOrderLineID.ToString() ?? "")).FirstOrDefault();
                        if (d != null)
                        {
                            ordln.SupplyPrice = d.UnitPrice;
                            ordln.SupplierName = "  Supplier";
                        }
                        else
                        {
                            //TODO !!! Продумать момент когда локальный поставщик изчез (в этом случае невозможно посчитать закупочную цену), сейчас в этом случае проставляется поставщик не найден
                            if (ordln.SupplierPriceWithMarkup != null)
                            {

                            }
                            else
                            {
                                SparePartFranch sparePartsDacLoad = SparePartsDac.Load(new SparePartPriceKey(ordln.Manufacturer, ordln.PartNumber, ordln.SupplierId));
                                if (sparePartsDacLoad != null)
                                {
                                    ordln.SupplyPrice = sparePartsDacLoad.SupplierPriceWithMarkup;
                                    ordln.SupplierName = "Local supplier #" + ordln.SupplierId;
                                }
                                else
                                {
                                    ordln.SupplyPrice = 1;
                                    ordln.SupplierName = "Supplier is not found";
                                }
                            }

                        }
                        
                        ordln.SupplyTotal = ordln.Qty * ordln.SupplyPrice;
                    }
                }
            }
        }


        public static int GetStringCount(DateTime bdate, DateTime edate, int CurrentPageSize, int CurrentPageIndex, string UserID, string OrderId, int StatusID)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {

                return ctx.DataContext.ExecuteQuery<int>(@"SELECT count(*) FROM dbo.Orders ord, dbo.OrderLines ordlns, dbo.Users u, dbo.OrderLineStatusChanges ordstsc
                                 WHERE ord.ClientID = u.AcctgID and ordlns.OrderID = ord.OrderID and ordstsc.OrderLineID = ordlns.AcctgOrderLineID and ordlns.CurrentStatus >= {2} " + ((UserID == "-1") ? " " : " and u.UserID = " + UserID) +
                                 @" " + (string.IsNullOrEmpty(OrderId) ? "" : ("and ordlns.OrderId =" + OrderId)) +
                                 @" and u.InternalFranchName  = {3}" +
                                @" and ordstsc.OrderLineStatusChangeID  =
                                (select top 1 OrderLineStatusChangeID from dbo.OrderLineStatusChanges a where a.OrderLineID = ordstsc.OrderLineID  AND a.OrderLineStatus = {2}
                                AND StatusChangeTime >= {0} AND StatusChangeTime <= {1})", bdate, edate, StatusID, SiteContext.Current.InternalFranchName ).ToList().FirstOrDefault();
            }
        }


        public static List<ReportString> GetRefId(DateTime bdate, DateTime edate, int CurrentPageSize, int CurrentPageIndex, string sortExpression, string direction, string UserID, string OrderId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return ctx.DataContext.ExecuteQuery<ReportString>(@"select * FROM (SELECT Manufacturer, AcctgOrderLineID, SupplierID, ordlns.OrderId, ordlns.Qty*ordlns.UnitPrice as Total, PartNumber, ordlns.Qty FROM dbo.Orders ord, dbo.OrderLines ordlns, dbo.Users u, dbo.OrderLineStatusChanges ordstsc
                                 WHERE ord.ClientID = u.AcctgID and ordlns.OrderID = ord.OrderID and ordstsc.OrderLineID = ordlns.AcctgOrderLineID and ordlns.CurrentStatus >= {2}" + ((UserID == "-1") ? " " : " and u.UserID = " + UserID) +
                                 (string.IsNullOrEmpty(OrderId) ? "" : (" and ordlns.OrderId =" + OrderId)) +
                                 @" and u.InternalFranchName  = {3}" +
                                 @" and ordstsc.OrderLineStatusChangeID  =
                                (select top 1 OrderLineStatusChangeID from dbo.OrderLineStatusChanges a where a.OrderLineID = ordstsc.OrderLineID  AND a.OrderLineStatus = {2} 
                                AND StatusChangeTime >= {0} AND StatusChangeTime <= {1})  ) tmp", bdate, edate, SaleOrderLineStatus, SiteContext.Current.InternalFranchName).ToList();
            }
        }


        
        public static List<ReportString> GetSaleStrings(DateTime bdate, DateTime edate, int CurrentPageSize, int CurrentPageIndex, string sortExpression, string direction, string UserID, string OrderId)
        {
            return GetCommonStrings(bdate, edate, CurrentPageSize, CurrentPageIndex, sortExpression, direction, UserID, OrderId, SaleOrderLineStatus);
        }

        //Общий метод извлечения строк
        public static List<ReportString> GetCommonStrings(DateTime bdate, DateTime edate, int CurrentPageSize, int CurrentPageIndex, string sortExpression, string direction, string UserID, string OrderId, int StatusId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return ctx.DataContext.ExecuteQuery<ReportString>(@"select * FROM (SELECT U.ClientName, 
                                                                                          u.AcctgID as ClientID, 
                                                                                          ordstsc.StatusChangeTime, 
                                                                                          ordlns.Qty*ordlns.UnitPrice as Total, 
                                                                                          ordlns.AcctgOrderLineID, 
                                                                                          ordlns.Manufacturer, 
                                                                                          ordlns.PartNumber, 
                                                                                          ordlns.PartName, 
                                                                                          Qty, 
                                                                                          SupplierId, 
                                                                                          ordlns.UnitPrice, 
                                                                                          ordlns.OrderID, 
                                                                                          ROW_NUMBER() OVER(order by " + (!string.IsNullOrEmpty(sortExpression) ? (sortExpression) : "StatusChangeTime") + " " + (direction ?? "") + @") as prt 
                                                                                          FROM dbo.Orders ord, dbo.OrderLines ordlns, dbo.Users u, dbo.OrderLineStatusChanges ordstsc
                                 WHERE ord.ClientID = u.AcctgID and ordlns.OrderID = ord.OrderID and ordstsc.OrderLineID = ordlns.AcctgOrderLineID and ordlns.CurrentStatus >= {4} " + ((UserID == "-1") ? " " : " and u.UserID = " + UserID) +
                                 @" and u.InternalFranchName  = {5}" +
                                 (string.IsNullOrEmpty(OrderId) ? "" : (" and ordlns.OrderId =" + OrderId)) +
                                 @" and ordstsc.OrderLineStatusChangeID  =
                                (select top 1 OrderLineStatusChangeID from dbo.OrderLineStatusChanges a where a.OrderLineID = ordstsc.OrderLineID  AND a.OrderLineStatus = {4}
                                AND StatusChangeTime >= {0} AND StatusChangeTime <= {1})  ) tmp WHERE prt > {2} and prt <= {3}", bdate, edate, CurrentPageSize * (CurrentPageIndex), CurrentPageSize * (CurrentPageIndex + 1), StatusId, SiteContext.Current.InternalFranchName).ToList();
            }
        }

        
        public static List<ReportString> GetSupplyStrings(DateTime bdate, DateTime edate, int CurrentPageSize, int CurrentPageIndex, string sortExpression, string direction, string UserID, string OrderId)
        {
                return GetCommonStrings(bdate, edate, CurrentPageSize, CurrentPageIndex, sortExpression, direction, UserID, OrderId, SupplyOrderLineStatus);
        }

        public static void ProcessShipment(int[] ids, string CurrentClientId)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.SetUnCommit();

                var printLines = from ol in dc.DataContext.OrderLines
                                 where ids.Contains(ol.OrderLineID)
                                 select ol;

                if (printLines.Any(x => x.CurrentStatus == 162))
                {
                    throw new Exception("Data was updated ealier, refresh the page");
                }

                printLines.Each(x => x.CurrentStatus = 162);
                printLines.Each(x => x.CurrentStatusDate = DateTime.Now);
                var summ = printLines.ToArray().Sum(pl => pl.Total);
                dc.DataContext.SubmitChanges();
                //Вносим платеж
                var userID = dc.DataContext.Users.Where(u => u.AcctgID.Equals(CurrentClientId)).Select(u => u.UserID).Single();
                AddUserLightPayment(new UserLightPayment()
                {
                    PaymentDate = DateTime.Now,
                    PaymentSum = summ,
                    PaymentType = LightPaymentType.Shipping,
                    UserID = userID
                }, dc);

                dc.SetCommit();
            }
        }


        /// <summary>
        /// Зачищаем временную таблицу по выбранному менеджеру
        /// </summary>
        /// <param name="managerUserID">UserID менеджера</param>
        public static void DeleteOrderLinesNewStatuses(int managerUserID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                var items = dc.DataContext.TempOLStatuses.Where(i => i.ManagerUserID == managerUserID).ToList();
                dc.DataContext.TempOLStatuses.DeleteAllOnSubmit(items);
                dc.DataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="managerUserID"></param>
        public static void DeleteOrder(int OrderID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                var items = dc.DataContext.OrderLines.Where(i => i.OrderID == OrderID).ToList();
                dc.DataContext.OrderLines.DeleteAllOnSubmit(items);

                var Order = dc.DataContext.Orders.Where(i => i.OrderID == OrderID).ToList();
                dc.DataContext.Orders.DeleteAllOnSubmit(Order);

                dc.DataContext.SubmitChanges();
            }
        }

        public static IEnumerable<TempOLStatuse> SelectOrderLinesNewStatuses(int managerUserID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.spLightSelOLStatuses {0}";
                return dc.DataContext.ExecuteQuery<TempOLStatuse>(query, managerUserID).ToList();
            }
        }

        public static int UpdateOrderLinesNewStatuses(int managerUserID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.spLightUpdOLStatuses {0}";
                return dc.DataContext.ExecuteQuery<int>(query, managerUserID).FirstOrDefault();
            }
        }

        public static int UpdateUserLimit(int UserID, decimal PaymentLimit)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.[spLightUpdPaymentLimit] {0}, {1}";
                return dc.DataContext.ExecuteQuery<int>(query, UserID, PaymentLimit).FirstOrDefault();
            }
        }

        //TODO: вынести данные методы в загрузку профиля клиента
        public static int GetPaymentDelayDays(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"select PaymentDelayDays from dbo.UserSettings where UserID = {0}";
                return dc.DataContext.ExecuteQuery<int?>(query, userID).FirstOrDefault() ?? 0;
            }
        }

        //TODO: вынести данные методы в загрузку профиля клиента
        public static string GetAdditionalEmail(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"select AdditionalEmail from dbo.UserSettings where UserID = {0}";
                return dc.DataContext.ExecuteQuery<string>(query, userID).FirstOrDefault();
            }
        }

        //TODO: вынести данные методы в загрузку профиля клиента
        public static int? GetPaymentLimit(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"select PaymentLimit from dbo.UserSettings where UserID = {0}";
                return dc.DataContext.ExecuteQuery<int?>(query, userID).FirstOrDefault();
            }
        }

        //TODO: вынести данные методы в загрузку профиля клиента
        public static bool GetIsAutoOrder(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"select IsAutoOrder from dbo.UserSettings where UserID = {0}";
                return dc.DataContext.ExecuteQuery<bool>(query, userID).FirstOrDefault();
            }
        }

        public static bool IsLight()
        {
            if (SiteContext.Current != null)
            {
                return AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite;
            }

            return Convert.ToBoolean(ConfigurationManager.AppSettings["IsLight"]);
        }


        /// <summary>
        /// TODO: Возвращает ИД пользователя из под которого делаются заказы в базу РМС.
        /// </summary>
        /// <returns></returns>
        public static string LightUserAcctgId()
        {
            if (SiteContext.Current != null)
            {
                //TODO !!! дописать хранимку spSelFranches
                //   return AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].AcctgID;
            }

            return "Define AcctgId is not possible";
        }

        /// <summary>
        /// Обвертка над получением значения
        /// </summary>
        /// <returns></returns>

        public static string InternalFranchName()
        {
            if (SiteContext.Current != null)
            {

                //return AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName];
                return "";
            }
            return "";
        }

        public static void SetOrderNoXmlSign(int orderID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"insert into dbo.OrdersNoXml (OrderID) values ({0})";
                dc.DataContext.ExecuteCommand(query, orderID);
            }
        }

        public static bool GetOrderNoXmlSign(int orderID)
        {
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"select OrderID from dbo.OrdersNoXml where OrderID = {0}";
                var res = dc.DataContext.ExecuteQuery<int>(query, orderID).FirstOrDefault();
                if (res > 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Метод для отправки заказа в работу (формирование xml, внесение его в таблицу Acctg.Requests и удаление признака NoXml у заказа)
        /// </summary>
        /// <param name="orderId">ID заказа</param>
        public static void FormXmlForOrder(int orderId)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.SetUnCommit();
                string query = @"delete from dbo.OrdersNoXml where OrderID = {0}";
                dc.DataContext.ExecuteCommand(query, orderId);
                query = @"Update dbo.Orders SET Status = {1} where OrderID = {0}";
                dc.DataContext.ExecuteCommand(query, orderId, OrderStatus.Processing);
                dc.SetCommit();
            }
        }

        /// <summary>
        /// Возвращает платежи
        /// </summary>
        /// <param name="userID">ID пользователя</param>
        /// <param name="dateFrom">начальная дата</param>
        /// <param name="dateTo">конечная дата</param>
        /// <param name="startIndex">номер страницы данных</param>
        /// <param name="pageSize">размер страницы данных</param>
        /// <returns>страница данных</returns>
        public static List<UserLightPayment> GetUserLightPayments(int userID, DateTime dateFrom, DateTime dateTo, int startIndex, int pageSize)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.DataContext.Log = new DebuggerWriter();
                string query = @"	select T.ID, T.ClientName, T.PaymentDate, T.PaymentSum, T.PaymentType, T.PaymentMethod, T.UserID from
									(
										select ROW_NUMBER() over (order by ID) as RowNum, b.ClientName, ID, PaymentDate, a.PaymentMethod, PaymentSum, PaymentType, a.UserID
										from dbo.UserPayments a, dbo.Users b where a.UserId = b.UserId and a.UserID = {0} and PaymentDate >= {1} and PaymentDate <= {2}
									) as T
									where RowNum between {3} and {3} + {4} - 1
									order by RowNum ";

                if (userID < 0)
                {
                    query = @"	select T.ClientName, T.ID, T.PaymentDate, T.PaymentSum, T.PaymentType, T.PaymentMethod, T.UserID from
									(
										select ROW_NUMBER() over (order by ID) as RowNum,  b.ClientName, ID, PaymentDate, a.PaymentMethod, PaymentSum, PaymentType, a.UserID
										from dbo.UserPayments a, dbo.Users b where a.UserId = b.UserId and PaymentDate >= {0} and PaymentDate <= {1} and b.InternalFranchName = {4}
									) as T
									where RowNum between {2} and {2} + {3} - 1
									order by RowNum ";
                    return dc.DataContext.ExecuteQuery<UserLightPayment>(query, dateFrom, dateTo, startIndex, pageSize, SiteContext.Current.InternalFranchName).ToList();
                }

                return dc.DataContext.ExecuteQuery<UserLightPayment>(query, userID, dateFrom, dateTo, startIndex, pageSize).ToList();
            }
        }

        /// <summary>
        /// Возвращает кол-во платежей
        /// </summary>
        /// <param name="userID">ID пользователя</param>
        /// <param name="dateFrom">начальная дата</param>
        /// <param name="dateTo">конечная дата</param>
        /// <returns>кол-во</returns>
        public static int GetUserLightPaymentsCount(int userID, DateTime dateFrom, DateTime dateTo)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                if (userID > 0)
                {
                    string query = @" select COUNT(*) from dbo.UserPayments where UserID = {0} and PaymentDate >= {1} and PaymentDate <= {2} ";
                    return dc.DataContext.ExecuteQuery<int>(query, userID, dateFrom, dateTo).Single();
                }
                else
                {
                    string query = @" select COUNT(*) from dbo.UserPayments a, dbo.Users b where PaymentDate >= {0} and PaymentDate <= {1} and  a.UserId = b.UserId and b.InternalFranchName = {2}";
                    return dc.DataContext.ExecuteQuery<int>(query, dateFrom, dateTo, SiteContext.Current.InternalFranchName).Single();
                }
            }
        }

        /// <summary>
        /// Добавляет платеж в таблицу платежей
        /// </summary>
        /// <param name="payment">Платеж</param>
        public static int AddUserLightPayment(UserLightPayment payment, DCFactory<StoreDataContext> dc)
        {
            if (payment.UserID <= 0) return -1; //Ничего не делаем, пользователь не валидный



            string query = @" insert into dbo.UserPayments (PaymentDate, PaymentSum, PaymentType, UserID, PaymentMethod) values ({0}, {1}, {2}, {3}, {4}) ";
            dc.DataContext.ExecuteCommand(query,
                payment.PaymentDate,
                payment.PaymentSum,
                payment.PaymentType,
                payment.UserID,
                payment.PaymentMethod);

            return 1;
        }


        /// <summary>
        /// Возвращает сальдо пользователя
        /// </summary>
        /// <param name="userID">ID пользователя</param>
        /// <param name="dateTo">конечная дата</param>
        /// <returns>сальдо</returns>
        public static decimal GetUserLightBalance(int userID, DateTime dateTo)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                string query = "select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentDate <= {1} ";
                var result = dc.DataContext.ExecuteQuery<decimal?>(query, userID, dateTo).SingleOrDefault();
                return /*decimal.Round(*/result.HasValue ? result.Value : 0.0m/*)*/;
            }
        }

        //Считает сумму платежей и вычитает отгрузки, кроме тех которые были после дня оплаты
        public static decimal GetUserLightBalanceWithDelay(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                var dd = LightBO.GetPaymentDelayDays(userID);

                string query1 = "select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentDate <= {1}";
                var result = dc.DataContext.ExecuteQuery<decimal?>(query1, userID, DateTime.Now).SingleOrDefault();

                string query2 = "select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentDate > {1} and PaymentType = 0";
                var result2 = dc.DataContext.ExecuteQuery<decimal?>(query2, userID, DateTime.Now.AddDays(-dd)).SingleOrDefault();
                return decimal.Round((result.HasValue ? result.Value : 0.0m) + (result2.HasValue ? result2.Value : 0.0m));
            }
        }

        public static decimal GetUserActiveOrdersSum(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                string query = "select SUM(Qty*UnitPrice) from dbo.Orders a, dbo.OrderLines b where a.OrderId = b.OrderID and UserID = {0} and not CurrentStatus in (80, 162) and Status = 2";
                var result = dc.DataContext.ExecuteQuery<decimal?>(query, userID).SingleOrDefault();
                return result.HasValue ? decimal.Round(result.Value) : 0.0m;
            }
        }

        #region === Старый (Зыряновский) вариант
        ////Считает сумму активных заказов до даты
        public static decimal GetUserActiveOrdersSumToDate(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                //var dd = LightBO.GetPaymentDelayDays(userID);
                //string query = "select SUM(Total) from dbo.Orders where UserID = {0} and Status = 2 and OrderDate < {1}";
                //var result = dc.DataContext.ExecuteQuery<decimal?>(query, userID, DateTime.Now.AddDays(-dd)).SingleOrDefault();
                //return result.HasValue ? decimal.Round(result.Value) : 0.0m;
                var dd = LightBO.GetPaymentDelayDays(userID);
                if (dd == 0)
                {
                    return GetUserLightBalance(userID, DateTime.Now);
                }
                string query = @"select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentType <> {1}";
                var res1 = dc.DataContext.ExecuteQuery<decimal?>(query, userID, 0 /*отгрузка*/).SingleOrDefault();
                query = @"select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentType = {1} and PaymentDate < {2}";
                var res2 = dc.DataContext.ExecuteQuery<decimal?>(query, userID, 0 /*отгрузка*/, DateTime.Now.AddDays(-dd)).SingleOrDefault();
                var result = (res1.HasValue ? res1.Value : 0.0m) + (res2.HasValue ? res2.Value : 0.0m);
                return result;

                //                var dd = LightBO.GetPaymentDelayDays(userID);
                //                if (dd == 0)
                //                {
                //                    return GetUserLightBalance(userID, DateTime.Now);
                //                }
                //                string query = @"
                //select
                //	(select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentType <> {1})
                //	+
                //	isnull( (select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentType = {1} and PaymentDate < '{2}'), 0 )
                //";
                //                var result = dc.DataContext.ExecuteQuery<decimal?>(query, userID, 0 /*отгрузка*/, DateTime.Now.AddDays(-dd)).SingleOrDefault();
                //                return result.HasValue ? decimal.Round(result.Value) : 0.0m;
            }
        }
        #endregion

        /// <summary>
        /// Возвращает задолженность по пользователю
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static decimal GetUserLightDebt(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                var dd = LightBO.GetPaymentDelayDays(userID);
                if (dd == 0)
                {
                    return GetUserLightBalance(userID, DateTime.Now);
                }
                string query = @"
select
	(select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentType <> {1})
	+
	isnull( (select SUM(PaymentSum) from dbo.UserPayments where UserID = {0} and PaymentType = {1} and PaymentDate < {2}), 0 )
";
                var result = dc.DataContext.ExecuteQuery<decimal?>(query, userID, 0 /*отгрузка*/, DateTime.Now.AddDays(-dd)).SingleOrDefault();
                return result.HasValue ? decimal.Round(result.Value) : 0.0m;
            }
        }


        public static string BeforeSendCheckOrderLine(string[] OrderlineID)
        {
            bool FirstRule;
            bool SecondRule;
            bool FirdRule;

            using (var dc = new DCFactory<StoreDataContext>())
            {
                var OrderID = dc.DataContext.OrderLines.Where(x => OrderlineID[0] == x.OrderLineID.ToString()).FirstOrDefault().OrderID;
                var Total = Math.Round(dc.DataContext.OrderLines.Where(x => OrderlineID.Contains(x.OrderLineID.ToString())).Sum(y => y.Qty * y.UnitPrice),2);

                var ClientID = dc.DataContext.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault().ClientID;
                //var Total = dc.DataContext.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault().Total;
                var ClientSaldo = decimal.Round(LightBO.GetUserLightBalance(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID, DateTime.Now), 2);
                var ActiveOrdersSum = decimal.Round(LightBO.GetUserActiveOrdersSum(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID), 2);
                var PrepaymentPercent = dc.DataContext.ExecuteQuery<decimal>("select PrepaymentPercent from dbo.Users where UserId = {0}", dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID).FirstOrDefault();
                var ActiveOrdersSumToDate = LightBO.GetUserActiveOrdersSumToDate /*GetUserLightDebt*/(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);
                var ClientSaldoToDate = LightBO.GetUserLightBalanceWithDelay(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);
                var PaymentLimit = LightBO.GetPaymentLimit(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);



                FirstRule = ((Total) <= -1 * ClientSaldo);
                //var SecondRule = ActiveOrdersSumToDate <= -1 * ClientSaldoToDate; //Просрочка
                SecondRule = (ActiveOrdersSumToDate <= 0); //Просрочка
                FirdRule = ActiveOrdersSum <= PaymentLimit; //Сумма активных заказов меньше лимита по заказам

                string firstRuleText = FirstRule ? "Balance is favourable." : "Balance is adverse.";
                string secondRuleText = SecondRule ? "No expiration." : "There is expiration";
                string firdRuleText = FirdRule ? "Limit is not exceeded" : "Order limit is exceeded";

                var p = LightBO.GetPaymentDelayDays(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);

                //TODO: возможно это надо отрефакторить в стратегию
                if (p == 0)
                {
                    if (FirstRule && FirdRule)
                        return "Ok";
                    else
                    {
                        return "Present customer has: " + firstRuleText + "\nNo expiration.\n" + firdRuleText + "\nTotal orders amount in process: " + ActiveOrdersSum + " usd." +
                "\nClient’s balance: " + (-1 * ClientSaldo) + " usd." +
                "\nDispatch amount: " + Total + " usd.";
                    }
                }
                else

                if ((FirstRule || SecondRule) && FirdRule)
                {
                    return "Ok";
                }
                else
                {
                    return "Present customer has: " + firstRuleText + "\n" + secondRuleText + "\n" + firdRuleText + "\nTotal orders amount in process: " + ActiveOrdersSum + " usd." +
                    "\nClient’s balance: " + (-1 * ClientSaldo) + " usd." +
                    "\nDispatch amount: " + Total + " usd.";
                }
            }
        }

        /// <summary>
        /// Проверка заказа перед отправкой
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
         
        public static string BeforeSendCheckOrder(int OrderID)
        {
            bool FirstRule;
            bool SecondRule;
            bool FirdRule;



            using (var dc = new DCFactory<StoreDataContext>())
            {
                var ClientID = dc.DataContext.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault().ClientID;
                var Total = dc.DataContext.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault().Total;
                var ClientSaldo = decimal.Round(LightBO.GetUserLightBalance(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID, DateTime.Now));
                var ActiveOrdersSum = decimal.Round(LightBO.GetUserActiveOrdersSum(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID));
                var PrepaymentPercent = dc.DataContext.ExecuteQuery<decimal>("select PrepaymentPercent from dbo.Users where UserId = {0}", dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID).FirstOrDefault();
                var ActiveOrdersSumToDate = LightBO.GetUserActiveOrdersSumToDate /*GetUserLightDebt*/(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);
                var ClientSaldoToDate = LightBO.GetUserLightBalanceWithDelay(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);
                var PaymentLimit = LightBO.GetPaymentLimit(dc.DataContext.Users.Where(y => y.AcctgID == ClientID).FirstOrDefault().UserID);



                FirstRule = ((Total + ActiveOrdersSum) * (PrepaymentPercent / 100) <= -1 * ClientSaldo);
                //var SecondRule = ActiveOrdersSumToDate <= -1 * ClientSaldoToDate; //Просрочка
                SecondRule = (ActiveOrdersSumToDate <= 0); //Просрочка
                FirdRule = ActiveOrdersSum + Total <= PaymentLimit; //Сумма активных заказов меньше лимита по заказам

                string firstRuleText = FirstRule ? "Balance is favourable." : "Balance is adverse.";
                string secondRuleText = SecondRule ? "No expiration." : "There is expiration";
                string firdRuleText = FirdRule ? "Limit is not exceeded" : "Order limit is exceeded";

                if (FirstRule && SecondRule && FirdRule)
                {
                    return "Ok";
                }
                else
                {
                    return "Present customer has: " + firstRuleText + "\n" + secondRuleText + "\n" + firdRuleText + "\nOrder amount in process: " + ActiveOrdersSum + " usd." +
                    "\nClient’s balance: " + (-1 * ClientSaldo) + " usd." +
                    "\nOrder amount: " + Total + " usd.";
                }

            }


        }

         public static List<OrdersDTO> GetPaymentList(int start, int PageSize, DateTime EndDate, string ClientName)
         {
             using (var dc = new DCFactory<StoreDataContext>())
             {
//                return dc.DataContext.ExecuteQuery<OrdersDTO>(@"
//                select * from
//                (
//                    Select *,  ROW_NUMBER() over (ORDER BY DateOfBegin) as num,  Count(*) over (PARTITION BY 1) as cnt  from (
//                    SELECT b.UserID, d.ClientName, b.PaymentSum, CONVERT(varchar(10), b.PaymentDate, 104) as DateOfBegin, c.PaymentDelayDays, SUM(a.PaymentSum) as DebtSumm 
//                    FROM [dbo].[UserPayments] b, [dbo].[UserPayments] a, [dbo].[UserSettings] c, [dbo].Users d
//                    where a.PaymentDate <= DATEADD(DAY, c.PaymentDelayDays, b.PaymentDate) and a.UserID = b.UserID AND b.PaymentType = 0 AND b.UserID = c.UserID and c.UserID = d.UserID
//                    and b.PaymentDate < {2}
//                    GROUP by b.UserID,  d.ClientName, b.PaymentSum, b.PaymentDate, c.PaymentDelayDays) tt  where DebtSumm > 0 
//                    ) tt1 where num >= {0} and num < {1} Order by DateOfBegin", start+1, start + PageSize+1, EndDate).ToList();

                 string txt;

                 if (String.IsNullOrEmpty(ClientName))
                 {
                    txt = System.IO.File.ReadAllText(@"C:\SiteQueries\DebtReport.txt");
                     return dc.DataContext.ExecuteQuery<OrdersDTO>(txt, start + 1, start + PageSize + 1, EndDate, SiteContext.Current.InternalFranchName).ToList();
                 }
                 else
                 {
                     txt = System.IO.File.ReadAllText(@"C:\SiteQueries\DebtReportWithClient.txt");
                     return dc.DataContext.ExecuteQuery<OrdersDTO>(txt, start + 1, start + PageSize + 1, EndDate, ClientName).ToList();
                 }


                 

                 return dc.DataContext.ExecuteQuery<OrdersDTO>(@"
  
                    Declare @DateOfReport DateTime;
                    SET @DateOfReport = {2};

                     select * FROM
                    ( select sab.UserID, sab.ClientName, sab.PaymentSum, sab.DocSumm, convert(varchar(25), DateOfBegin, 120) as DateOfBegin, CASE WHEN DATEDIFF(DAY, DateOfBegin,  @DateOfReport) - PaymentDelayDays > 0 THEN DATEDIFF(DAY, DateOfBegin,  @DateOfReport) - PaymentDelayDays Else 0 END as DaysDelayed, ROW_NUMBER() over (ORDER BY  ClientName, DateOfBegin) as num,  Count(*) over (PARTITION BY 1) as cnt, CASE WHEN DateOfBegin < DATEADD(DAY, -PaymentDelayDays,  @DateOfReport) THEN DocSumm ELSE  0 END DelayedSumm   
					 from
                    (
                    select sa1.*, CASE WHEN sa1.PaymentSum < sa1.DebtSumm + Total THEN sa1.PaymentSum ELSE sa1.DebtSumm + Total END DocSumm
					
                    FROM
						(Select d.UserID, ClientName, PaymentSum, DebtSumm, DateOfBegin, c.PaymentDelayDays FROM
						    (
							    SELECT b.UserID, b.PaymentSum, SUM(a.PaymentSum) as DebtSumm, b.PaymentDate as DateOfBegin
							    FROM  [dbo].[UserPayments] a, [dbo].[UserPayments] b
							    where a.PaymentSum > 0 and b.PaymentSum > 0
							    and a.PaymentDate <= b.PaymentDate and a.UserID = b.UserID
							    and b.PaymentDate < @DateOfReport
							    GROUP by b.UserID,  b.PaymentSum, b.PaymentDate
						    ) sa, [dbo].Users d, dbo.UserSettings c where d.UserID = sa.UserID and c.UserID = sa.UserID) sa1 INNER JOIN (select UserID, SUM(PaymentSum) as Total from dbo.UserPayments where PaymentSum < 0 and PaymentDate < @DateOfReport Group by UserID) e
                        ON sa1.UserID = e.UserID
                        ) sab where DocSumm > 0) sab2 where num >= {0} and num < {1} ORder by ClientName, DateOfBegin ", start + 1, start + PageSize + 1, EndDate).ToList();
             }
         }

         public static List<OrdersDTO> GetSummaryPaymentList(int start, int PageSize, DateTime EndDate)
         {
             using (var dc = new DCFactory<StoreDataContext>())
             {

                 var txt = System.IO.File.ReadAllText(@"C:\SiteQueries\DebtReportSummary.txt");


                 return dc.DataContext.ExecuteQuery<OrdersDTO>(txt, start + 1, start + PageSize + 1, EndDate).ToList();

                return dc.DataContext.ExecuteQuery<OrdersDTO>(@"
                select ClientName, /*SUM(DebtSumm)*/ 100.00 as DebtSum from
                (
                    Select *,  ROW_NUMBER() over (ORDER BY DateOfBegin) as num,  Count(*) over (PARTITION BY 1) as cnt  from (
                    SELECT b.UserID, d.ClientName, b.PaymentSum, CONVERT(varchar(10), b.PaymentDate, 104) as DateOfBegin, c.PaymentDelayDays, SUM(a.PaymentSum) as DebtSumm 
                    FROM [dbo].[UserPayments] b, [dbo].[UserPayments] a, [dbo].[UserSettings] c, [dbo].Users d
                    where a.PaymentDate <= DATEADD(DAY, c.PaymentDelayDays, b.PaymentDate) and a.UserID = b.UserID AND b.PaymentType = 0 AND b.UserID = c.UserID and c.UserID = d.UserID
                    and b.PaymentDate < {2}
                    GROUP by b.UserID,  d.ClientName, b.PaymentSum, b.PaymentDate, c.PaymentDelayDays) tt  where DebtSumm > 0 
                    ) tt1 Group by ClientName", start, start + PageSize, EndDate).ToList();
             }
         }

         public static List<OrdersDTO> GetSummaryRow(string ClientName, DateTime EndDate)
         {
             using (var dc = new DCFactory<StoreDataContext>())
             {
                 return dc.DataContext.ExecuteQuery<OrdersDTO>(@"exec [Light].[spSelSummaryString] {0}, {1}, {2}   ", ClientName, EndDate, SiteContext.Current.InternalFranchName).ToList();
             }
         }
    }


        [DataContract]
        public class PaymentsList
        {
            [DataMember]
            public int totalCount {get; set;}
            [DataMember]
            public List<OrdersDTO> items { get; set; }
            [DataMember]
            public List<OrdersDTO> summaryData { get; set; }
        }
       

	/// <summary>
	/// Платеж
	/// </summary>
	public class UserLightPayment
	{
		
        public int ID { get; set; }
		public decimal PaymentSum { get; set; }
		public DateTime PaymentDate { get; set; }
		public LightPaymentType PaymentType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
		public int UserID { get; set; }
        public int num { get; set; }
        public string ClientName { get; set; }
       
	}

    /// <summary>
	/// Платеж
	/// </summary>
    public class OrdersDTO
    {
        [NonSerialized()]
        private DateTime _DateOfBegin;
        public int OrderID;
        public string ClientName;
        public int DaysDelayed;
        public decimal PaymentSum;
        public String DateOfBegin;
        //{
        //    get { return _DateOfBegin <= DateTime.MinValue ? new DateTime(2000, 1, 1) : _DateOfBegin; } 
        //    set { _DateOfBegin = value; }
        //}
        public decimal DebtSumm;
        public decimal DocSumm;
        public decimal DelayedSumm;
        public int cnt;
    }

	/// <summary>
	/// Типы платежа
	/// </summary>
	public enum LightPaymentType : byte
	{
        [Text("Dispatch")]
        [LocalizedDescription("LightPaymentType_Shipping", typeof(EnumResource))]
		Shipping = 0,		//отгрузка

        [Text("Payment")]
        [LocalizedDescription("LightPaymentType_Payment", typeof(EnumResource))]
		Payment = 1,		//оплата
        
        [Text("Goods return")]
        [LocalizedDescription("LightPaymentType_GoodsReturn", typeof(EnumResource))]
		GoodsReturn = 2,	//возврат товара
        
        [Text("Refund")]
        [LocalizedDescription("LightPaymentType_PayBack", typeof(EnumResource))]
		PayBack = 3			//возврат денег
	}
}
