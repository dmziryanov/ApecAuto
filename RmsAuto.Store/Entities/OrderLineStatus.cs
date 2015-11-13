using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Data;
using RmsAuto.Store.Web;
using System.Threading;
using System.Globalization;

namespace RmsAuto.Store.Entities
{
	public class OrderLineStatusUtil
	{
        private static Dictionary<string, List<OrderLineStatuses>> _allStatusesDict = new Dictionary<string, List<OrderLineStatuses>>();
        
        public static List<OrderLineStatuses> _allStatuses
		{ 
			get
			{
				if(SiteContext.Current != null)
					return _allStatusesDict[SiteContext.Current.InternalFranchName];
				else //значит обращение происходит в сервисе и соответственно SiteContext отстутствует
					return _allStatusesDict[ConfigurationManager.AppSettings["InternalFranchName"]];
			}
		}

        public static List<OrderLineStatuses> LiteStatuses { get { return _allStatusesDict[SiteContext.Current.InternalFranchName] /*.Where(x => x.OrderLineStatusID > 160 && x.OrderLineStatusID < 170).ToList();*/; } }

        public static int FinalRMSState = 160;
        public static int LowBoundary = 160;
        public static int HiBoundary = 170;

        static OrderLineStatusUtil()
        {
            List<String> FranchNames = AcctgRefCatalog.RmsFranches.Items.Select(x => x.InternalFranchName).ToList();
            foreach (string s in FranchNames)
            {
				//Нет пока никаих "лайтов" для эмиратов
				if (AcctgRefCatalog.RmsFranches[s].isLite)
				{
				    using (var dc = new DCFactory<StoreDataContext>(s))
				    {
                        DataLoadOptions dlo = new DataLoadOptions();
						dlo.LoadWith<OrderLineStatuses>(ols => ols.OrderLineStatusesLocs);
						dc.DataContext.LoadOptions = dlo;
						dc.DataContext.DeferredLoadingEnabled = false;
				        _allStatusesDict[s] = dc.DataContext.OrderLineStatuses.ToList<OrderLineStatuses>();
				    }
				}
				else
				{
                    using (var dc = new StoreDataContext())
					{
						#region === Подгружаем локализации ===
						DataLoadOptions dlo = new DataLoadOptions();
						dlo.LoadWith<OrderLineStatuses>(ols => ols.OrderLineStatusesLocs);
						dc.LoadOptions = dlo;
						dc.DeferredLoadingEnabled = false;
						#endregion
						_allStatusesDict[s] = dc.OrderLineStatuses.ToList<OrderLineStatuses>();
                    }
                }
            }
        }
  
		/// <summary>
		/// ОrderLineStatus in byte presentation
		/// </summary>
		/// <param name="dc"></param>
		/// <param name="nameRMS"></param>
		/// <returns></returns>
        //public static byte StatusByte(StoreDataContext dc, String nameRMS)
        //{
        //    var el =
        //            from o in dc.OrderLineStatuses
        //            where o.NameRMS == nameRMS
        //            select o.OrderLineStatusID;

        //    return (byte)el.First();
        //}

		/// <summary>
		/// ОrderLineStatus in byte presentation
		/// </summary>
		/// <param name="nameRMS"></param>
		/// <returns></returns>
		public static byte StatusByte(String nameRMS)
		{
            //using (var dc = new StoreDataContext())
            //{
            //    return StatusByte(dc, nameRMS);
            //}
            var el =
                    from o in _allStatuses
                    where o.NameRMS == nameRMS
                    select o.OrderLineStatusID;

            return (byte)el.First();
		}

		/// <summary>
		/// OrderLineStatusNameRMS
		/// </summary>
		/// <param name="dc"></param>
		/// <param name="statusID"></param>
		/// <returns></returns>
        //public static String StatusName(StoreDataContext dc, byte statusID)
        //{
        //    var el =
        //           from o in dc.OrderLineStatuses
        //           where o.OrderLineStatusID == statusID
        //           select o.NameRMS;

        //    return (String)el.First();
        //}

		/// <summary>
		/// OrderLineStatusNameRMS
		/// </summary>
		/// <param name="statusID"></param>
		/// <returns></returns>
		//public static String StatusName(byte statusID)
		//{
		//    //using (var dc = new StoreDataContext())
		//    //{
		//    //    return StatusName(dc, statusID);
		//    //}
		//    var el =
		//           from o in _allStatuses
		//           where o.OrderLineStatusID == statusID
		//           select o.NameRMS;

		//    return (String)el.First();
		//}

		/// <summary>
		/// OrderLineStatus DisplayName by Role
		/// </summary>
		/// <param name="dc"></param>
		/// <param name="statusID"></param>
		/// <returns></returns>
        //public static String DisplayName(StoreDataContext dc, byte statusID)
        //{
        //    try
        //    {
        //        switch (SiteContext.Current.User.Role)
        //        {
        //            case SecurityRole.Manager:
        //                {
        //                    var el =
        //                           from o in dc.OrderLineStatuses
        //                           where o.OrderLineStatusID == statusID
        //                           select o.Manager;

        //                    return (String)el.First();
        //                }
        //            case SecurityRole.Client:
        //                {
        //                    var el =
        //                           from o in dc.OrderLineStatuses
        //                           where o.OrderLineStatusID == statusID
        //                           select o.Client;

        //                    return (String)el.First();
        //                }
        //            default:
        //                throw new Exception("Unknown user role");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        var el =
        //               from o in dc.OrderLineStatuses
        //               where o.OrderLineStatusID == statusID
        //               select o.Client;

        //        return (String)el.First();
        //    }

        //}
         
		/// <summary>
		/// OrderLineStatus DisplayName by Role
		/// </summary>
		/// <param name="statusID"></param>
		/// <returns></returns>
		public static String DisplayName(byte statusID)
		{
            //using (var dc = new StoreDataContext())
            //{
            //    return DisplayName(dc, statusID);
            //}
            try
            {
                switch ( SiteContext.Current.User.Role )
                {
                    case SecurityRole.Manager:
                        {
                            var el =
                                   from o in _allStatuses
                                   where o.OrderLineStatusID == statusID
                                   select o.Manager;

                            return (String)el.First();
                        }
                    case SecurityRole.Client:
                        {
                            var el =
                                   from o in _allStatuses
                                   where o.OrderLineStatusID == statusID
                                   select o.Client;

                            return (String)el.First();
                        }
                    default:
                        throw new Exception( "Unknown user role" );
                }
            }
            catch (Exception ex)
            {
                
                var el =
                       from o in _allStatuses
                       where o.OrderLineStatusID == statusID
                       select o.Client;

                return (String)el.First();
            }
		}

        //deas 30.03.2011 task3586
        // Добавлены фукции для определения видимости статуса в зависимости от роли

        /// <summary>
        /// Определение отображения статуса в зависимости от роли
        /// </summary>
        /// <param name="dc">Контекст данных</param>
        /// <param name="statusID">Код статуса</param>
        /// <returns>Отображать</returns>
        //public static bool IsShow( StoreDataContext dc, byte statusID )
        //{
        //    try
        //    {
        //        switch ( SiteContext.Current.User.Role )
        //        {
        //            case SecurityRole.Manager:
        //                {
        //                    return true;
        //                }
        //            case SecurityRole.Client:
        //                {
        //                    var tmp = dc.OrderLineStatuses.FirstOrDefault( t => t.OrderLineStatusID == statusID );
        //                    return tmp == null ? false : tmp.ClientShow;
        //                }
        //            default:
        //                throw new Exception( "Unknown user role" );
        //        }
        //    }
        //    catch ( Exception )
        //    {
        //        return false;
        //    }

        //}

        /// <summary>
        /// Определение отображения статуса в зависимости от роли
        /// </summary>
        /// <param name="statusID">Код статуса</param>
        /// <returns>Отображать</returns>
        public static bool IsShow( byte statusID )
        {
            //using ( var dc = new StoreDataContext() )
            //{
            //    return IsShow( dc, statusID );
            //}
            try
            {
                switch ( SiteContext.Current.User.Role )
                {
                    case SecurityRole.Manager:
                        {
                            return true;
                        }
                    case SecurityRole.Client:
                        {
                            var tmp = _allStatuses.FirstOrDefault( t => t.OrderLineStatusID == statusID );
                            return tmp == null ? false : tmp.ClientShow;
                        }
                    default:
                        throw new Exception( "Unknown user role" );
                }
            }
            catch ( Exception )
            {
                return false;
            }
        }

		/// <summary>
		/// Return OrderLineStatus from Hansa format in byte presentation in RMS format
		/// </summary>
		/// <param name="dc"></param>
		/// <param name="nameHansa"></param>
		/// <returns></returns>
        //public static byte StatusByteFromHansa(StoreDataContext dc, String nameHansa)
        //{
        //    var el =
        //           from o in dc.OrderLineStatuses
        //           where o.NameHansa == nameHansa
        //           select o.OrderLineStatusID;

        //    return (byte)el.First();
        //}

		/// <summary>
		/// Return OrderLineStatus from Hansa format in byte presentation in RMS format
		/// </summary>
		/// <param name="nameHansa"></param>
		/// <returns></returns>
		//public static byte StatusByteFromHansa(String nameHansa)
		//{
		//    //using (var dc = new StoreDataContext())
		//    //{
		//    //    return StatusByteFromHansa(dc, nameHansa);
		//    //}
		//    var el =
		//           from o in _allStatuses
		//           where o.NameHansa == nameHansa
		//           select o.OrderLineStatusID;

		//    return (byte)el.First();
		//}

		/// <summary>
		/// Return selected for notification statuses in byte presentation for Client
		/// </summary>
		/// <param name="statuses"></param>
		/// <returns></returns>
		public static byte[] GetSelectedStatusesOfClient(StoreDataContext dc, string clientID)
		{
			List<byte> statusBytes = new List<byte>();
			string[] selectedStatuses = { };
			//var alertConfig = dc.ClientAlertConfigs.Where(ca => ca.ClientID == clientID).SingleOrDefault();
            var client = dc.Users.FirstOrDefault( t => t.AcctgID == clientID );
            var alertConfig = dc.spSelUserSetting( client.UserID ).SingleOrDefault();
			if (alertConfig != null && alertConfig.AlertStatusIDs != null)
			{
				selectedStatuses = alertConfig.AlertStatusIDs.Split(';');
			}
			else
			{
				return null;
			}

			foreach (var status in selectedStatuses)
			{
				byte statusByte = 0;
				if (byte.TryParse(status, out statusByte))
				{
					statusBytes.Add(statusByte);
				}
			}

			return statusBytes.ToArray();
		}
	}

	[ScaffoldTable(true)]
	[MetadataType(typeof(OrderLineStatusesMetadata))]
    [Serializable()]
    public partial class OrderLineStatuses 
	{
		public string Manager
		{
			get
			{

                //Принудительно выставляем английскую локаль, можно сделать LocalizableClass как базовый и там в конструктуре
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
                
                var curloc = Thread.CurrentThread.CurrentCulture.Name;
				if (curloc != "ru-RU")
				{
					var locale = this.OrderLineStatusesLocs.Where(loc => loc.Localization == curloc).SingleOrDefault();
					if (locale != null && !string.IsNullOrEmpty(locale.Manager)) return locale.Manager;
				}
				return ManagerRU;
			}
			set { ManagerRU = value; }
		}
		public string Client
		{
			get
			{

                //Принудительно выставляем английскую локаль
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
                
                var curloc = Thread.CurrentThread.CurrentCulture.Name;
				if (curloc != "ru-RU")
				{
					var locale = this.OrderLineStatusesLocs.Where(loc => loc.Localization == curloc).SingleOrDefault();
					if (locale != null && !string.IsNullOrEmpty(locale.Client)) return locale.Client;
				}
				return ClientRU;
			}
			set { ClientRU = value; }
		}
	}

	[DisplayName("Статусы строк заказов")]
	public partial class OrderLineStatusesMetadata
	{
		[ScaffoldColumn(false)]
		[DisplayName("ID в Хансе")]
		[Required(ErrorMessage = "Не указано значение поля")]
		[UIHint("Text", null, "BindingOptions", "NameHansa")]
		public object NameHansa { get; set; }

		[ScaffoldColumn(false)]
		[DisplayName("ID статуса")]
		[Required(ErrorMessage = "Не указано значение поля")]
		[UIHint("Text", null, "BindingOptions", "OrderLineStatusID")]
		public object OrderLineStatusID { get; set; }

		[ScaffoldColumn(false)]
		[DisplayName("ID в RMS")]
		[Required(ErrorMessage = "Не указано значение поля")]
		[UIHint("Text", null, "BindingOptions", "NameRMS")]
		public object NameRMS { get; set; }

		//"Custom/OrderLineStatusID_Edit"
		[ScaffoldColumn(true)]
		[DisplayName("Менеджер")]
		[Required(ErrorMessage = "Не указано значение поля")]
		[UIHint("Text", null, "BindingOptions", "Manager")]
		public object Manager { get; set; }

		[ScaffoldColumn(true)]
		[DisplayName("Клиент")]
		[Required(ErrorMessage = "Не указано значение поля")]
		[UIHint("Text", null, "BindingOptions", "Client")]
		public object Client { get; set; }

		[ScaffoldColumn(false)]
		public object IsFinal { get; set; }

		[ScaffoldColumn(false)]
		public object RequiresClientReaction { get; set; }

		[ScaffoldColumn(false)]
		public object ExcludeFromTotalSum { get; set; }
	}

	//public enum OrderSortFields
	//    {
	//        [Text( "по номеру заказа (возр.)" )]
	//        OrderIdAsc,

	//        [Text( "по номеру  заказа (убыв.)" )]
	//        OrderIdDesc,

	public enum Processed : byte
	{
		//[Text("не обработано")]
		[Text("not processed")]
		NotProcessed = 0,
		//[Text("обработано клиентом")]
		[Text("processed by customer")]
		Client = 1,
		//[Text("обработано менеджером")]
		[Text("processed by manager")]
		Manager = 2
	}
}