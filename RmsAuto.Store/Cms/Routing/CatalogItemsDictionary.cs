using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Web;
using RmsAuto.Store.Acctg;
using System.Configuration;

namespace RmsAuto.Store.Cms.Routing
{
	public class CatalogItemsDictionary
	{
		#region Murkin, добавил для реализации локализации (меню будет одно)
		public CatalogItem RootCatalogItem { get; private set; }
		#endregion
		#region Murkin, закомментировал для реализации локализации
		//string RootCatalogID
		//{
		//    get
		//    {
		//        try
		//        {
		//            switch ( HttpContext.Current.Request.Cookies["CurrentCulture"].Value )
		//            {
		//                case "en-US":
		//                    return "~en";
		//                case "de-DE":
		//                    return "~de";
		//                default:
		//                    return "~";
		//            }
		//        }
		//        catch
		//        {
		//            return "~";
		//        }
		//    }
		//}
		//// deas 05.05.2011 task3996 для локализации
		//public CatalogItem RootCatalogItem
		//{
		//    get
		//    {
		//        switch ( RootCatalogID )
		//        {
		//            case "~en":
		//                return UrlManager.CatalogItems.RootCatalogItemEN;
		//            case "~de":
		//                return UrlManager.CatalogItems.RootCatalogItemDE;
		//            default:
		//                return UrlManager.CatalogItems.RootCatalogItemRU;
		//        }
		//    }
		//}
		//public CatalogItem RootCatalogItemRU { get { return _catalogItemsRU[InternalFranchName].Where(c => c.CatalogItemCode == "~").Single(); } private set { } }
		//public CatalogItem RootCatalogItemEN { get { return _catalogItemsEN[InternalFranchName].Where(c => c.CatalogItemCode == "~en").Single(); } private set { } }
		//public CatalogItem RootCatalogItemDE { get { return _catalogItemsDE[InternalFranchName].Where(c => c.CatalogItemCode == "~de").Single(); } private set { } }
		#endregion

		public CatalogItem NewsListCatalogItem { get; private set; }
		public CatalogItem MapCatalogItem { get; private set; }
		public CatalogItem ShopListCatalogItem { get; private set; }
		public CatalogItem VacancyListCatalogItem { get; private set; }
		public CatalogItem OnlineCatalogsCatalogItem { get; private set; }

		public CatalogItem PrivateOfficeCatalogItem { get; private set; }
		public CatalogItem CartCatalogItem { get; private set; }
		public CatalogItem OrdersCatalogItem { get; private set; }
		public CatalogItem FeedbackCatalogItem { get; private set; }
		public CatalogItem VinRequestsCatalogItem { get; private set; }
		public CatalogItem MyGarageCatalogItem { get; private set; }
		public CatalogItem StockSuppliersCatalogItem { get; private set; }
		//public CatalogItem NotificationConfigCatalogItem { get; private set; }
        public CatalogItem UserSettingCatalogItem { get; private set; }
        public CatalogItem DetailBalanceCatalogItem { get; private set; }
		public CatalogItem ReclamationCatalogItem { get; private set; }
		public CatalogItem OffersCatalogItem { get; private set; }

		void InitializeCatalogItems()
		{
			#region Murkin, добавлено для реализации локализации (меню будет одно)
			this.RootCatalogItem = _catalogItems.Where(c => c.CatalogItemCode == "~").Single();
			#endregion
			this.NewsListCatalogItem = GetSingleCatalogItemByPath("~/Cms/NewsList.aspx");
			this.MapCatalogItem = GetSingleCatalogItemByPath("~/Cms/Map.aspx");
			this.ShopListCatalogItem = GetSingleCatalogItemByPath("~/Cms/ShopList.aspx");
			this.VacancyListCatalogItem = GetSingleCatalogItemByPath("~/Cms/VacancyList.aspx");
			this.OnlineCatalogsCatalogItem = GetSingleCatalogItemByPath("~/Cms/OnlineCatalogs.aspx");
			this.PrivateOfficeCatalogItem = GetSingleCatalogItemByPath("~/PrivateOffice/Default.aspx");
			this.CartCatalogItem = GetSingleCatalogItemByPath("~/PrivateOffice/Cart.aspx");
			this.OrdersCatalogItem = GetSingleCatalogItemByPath("~/PrivateOffice/OrderList.aspx");
			this.FeedbackCatalogItem = GetSingleCatalogItemByPath("~/PrivateOffice/Feedback.aspx");
			this.VinRequestsCatalogItem = GetSingleCatalogItemByPath("~/PrivateOffice/VinRequests.aspx");
			this.MyGarageCatalogItem = GetSingleCatalogItemByPath("~/PrivateOffice/MyGarage.aspx");
			this.StockSuppliersCatalogItem = GetSingleCatalogItemByPath("~/Store/StockSuppliers.aspx");
            //this.NotificationConfigCatalogItem = GetSingleCatalogItemByPath( "~/PrivateOffice/NotificationConfig.aspx" );
            this.UserSettingCatalogItem = GetSingleCatalogItemByPath( "~/PrivateOffice/UserSetting.aspx" );
            this.DetailBalanceCatalogItem = GetSingleCatalogItemByPath( "~/PrivateOffice/DetailBalance.aspx" );
			this.ReclamationCatalogItem = GetSingleCatalogItemByPath( "~/PrivateOffice/Reclamation.aspx" );
			//if (SiteContext.Current.InternalFranchName == "rmsauto" )
            //TODO: Это надо подключить как-то на нашем сайте     this.OffersCatalogItem = GetSingleCatalogItemByPath("~/Cms/OffersList.aspx");
		}

		#region Murkin, добавил для реализации локализации (меню будет одно)
		IDictionary<string, ICollection<CatalogItem>> _catalogItemsDict = new Dictionary<string, ICollection<CatalogItem>>();
		IDictionary<string, IDictionary<int, CatalogItem>> _dictionaryDict = new Dictionary<string, IDictionary<int, CatalogItem>>();

		public IDictionary<int, CatalogItem> _dictionary
		{
			get { return _dictionaryDict[InternalFranchName]; }
		}
		public ICollection<CatalogItem> _catalogItems
		{
			get { return _catalogItemsDict[InternalFranchName]; }
		}
		#endregion


		#region Murkin, закомментировал для реализации локализации (меню будет одно)
		//// deas 16.05.2011 task3996 для локализации
		//public ICollection<CatalogItem> _catalogItems
		//{
		//    get
		//    {
		//        switch ( RootCatalogID )
		//        {
		//            case "~en":
		//                return _catalogItemsEN[InternalFranchName];
		//            case "~de":
		//                return _catalogItemsDE[InternalFranchName];
		//            default:
		//                return _catalogItemsRU[InternalFranchName];
		//        }
		//    }
		//}

		//IDictionary<string, ICollection<CatalogItem>> _catalogItemsRU = new Dictionary<string, ICollection<CatalogItem>>();
		//IDictionary<string, ICollection<CatalogItem>> _catalogItemsEN = new Dictionary<string, ICollection<CatalogItem>>();
		//IDictionary<string, ICollection<CatalogItem>> _catalogItemsDE = new Dictionary<string, ICollection<CatalogItem>>();
		#endregion

		public string InternalFranchName
        {
            get
            {
                return GetInternalFranchName();
            }
		}

		#region Murkin, закомментировал для реализации локализации (меню будет одно)
		//// deas 16.05.2011 task3996 для локализации
		//public IDictionary<int, CatalogItem> _dictionary
		//{
		//    get
		//    {
                
		//        switch ( RootCatalogID )
		//        {
		//            case "~en":
		//                return _dictionaryEN[InternalFranchName];
		//            case "~de":
		//                return _dictionaryDE[InternalFranchName];
		//            default:
		//                return _dictionaryRU[InternalFranchName];
		//        }
		//    }
		//}
		
		//IDictionary<string,IDictionary<int, CatalogItem>> _dictionaryRU = new Dictionary<string,IDictionary<int, CatalogItem>>() ;
		//IDictionary<string, IDictionary<int, CatalogItem>> _dictionaryEN = new Dictionary<string, IDictionary<int, CatalogItem>>();
		//IDictionary<string, IDictionary<int, CatalogItem>> _dictionaryDE = new Dictionary<string, IDictionary<int, CatalogItem>>();
		#endregion

		//Загрузка словарей из базы
        public CatalogItemsDictionary()
		{
			#region Murkin, закомментировал для реализации локализации (меню будет одно)
			//List<String> FranchNames = AcctgRefCatalog.RmsFranches.Items.Select(x => x.InternalFranchName).ToList();
			//foreach (string s in FranchNames)
			//{
			//    ICollection<CatalogItem> catalogItemsRU = CatalogItemsDac.GetCatalogItems("~", s);
			//    _catalogItemsRU[s] = catalogItemsRU;
			//    _dictionaryRU[s] = catalogItemsRU.ToDictionary(c => c.CatalogItemID);
			//    ICollection<CatalogItem> catalogItemsEN = CatalogItemsDac.GetCatalogItems("~en", s);
			//    _catalogItemsEN[s] = catalogItemsEN;
			//    _dictionaryEN[s] = catalogItemsEN.ToDictionary(c => c.CatalogItemID);
			//    ICollection<CatalogItem> catalogItemsDE = CatalogItemsDac.GetCatalogItems("~de", s);
			//    _catalogItemsDE[s] = catalogItemsDE;
			//    _dictionaryDE[s] = catalogItemsDE.ToDictionary(c => c.CatalogItemID);
			//}

			//InitializeCatalogItems();
			#endregion

			#region Murkin, добавил для реализации локализации (меню будет одно)
			List<String> franchNames = AcctgRefCatalog.RmsFranches.Items.Select(x => x.InternalFranchName).ToList();
			foreach (string s in franchNames)
			{
				ICollection<CatalogItem> catalogItems = CatalogItemsDac.GetCatalogItems("~", s);
				
					//вынимаем все локализации для Текущего франча
					var locs = CatalogItemsDac.GetCatalogItemsAllLoc(s);
					//заполняем для каждого CatalogItem его локализации
					foreach (var ci in catalogItems)
					{
						var ciLocs = locs.Where(c => c.CatalogItemID == ci.CatalogItemID);
						ci.LocalizationData = ciLocs.ToDictionary(c => c.Localization);
					}
				

				_catalogItemsDict[s] = catalogItems;
				_dictionaryDict[s] = catalogItems.ToDictionary(c => c.CatalogItemID);
			}

			InitializeCatalogItems();
			#endregion
		}

		//public CatalogItemsDictionary(ICollection<CatalogItem> catalogItems)
		//{
		//    _catalogItems = catalogItems;
		//    _dictionary = catalogItems.ToDictionary(c => c.CatalogItemID);

		//    InitializeCatalogItems();
		//}

		public CatalogItem GetCatalogItemByVirtualPath(string virtualPath)
		{
			string[] parts = virtualPath.Split('/');

			CatalogItem catalogItem = this.RootCatalogItem;
			for (int i = 0; i < parts.Length; ++i)
			{
				catalogItem = _catalogItems.Where(c => c.ParentItemID == catalogItem.CatalogItemID && c.CatalogItemCode == parts[i]).FirstOrDefault();

				if (catalogItem == null || catalogItem != null && !catalogItem.CatalogItemVisible)
				{
					catalogItem = null;
					break;
				}
			}
			
            return catalogItem;
		}

		public string GetVirtualPathForCatalogItem(int catalogItemId)
		{
			List<string> path = new List<string>();

			for (CatalogItem item = GetCatalogItem(catalogItemId);
			#region Murkin, заменил для реализации локализации (меню будет одно)
				//item.ParentItemID != null;
				item.CatalogItemID != RootCatalogItem.CatalogItemID;
			#endregion
 item = GetCatalogItem(item.ParentItemID.Value))
			{
				path.Add(item.CatalogItemCode);
			}
			path.Reverse();

			return string.Join("/", path.ToArray());
		}

		private CatalogItem GetSingleCatalogItemByPath(string path)
		{
			try
			{
				return _catalogItems.Where(c => c.CatalogItemPath == path).Single();
			}
			catch (InvalidOperationException)
			{
				throw new Exception("Catalog not found: " + path);
			}
		}

		public CatalogItem GetCatalogItemByPath(string path)
		{
			return _catalogItems.Where(c => c.CatalogItemPath == path).FirstOrDefault();
		}

		//public CatalogItem GetCatalogItemByCatalogItemName( string catalogItemName)
		//{
		//    return _catalogItems.Where(c => c.CatalogItemName == catalogItemName).FirstOrDefault();
		//}

		/// <summary>
		/// Возвращает видимые дочерние разделы (кроме служебных)
		/// </summary>
		public IEnumerable<CatalogItem> GetCatalogItems(int parentItemId, CatalogItemMenuType mType)
		{
			return _catalogItems.Where(c => c.ParentItemID == parentItemId && c.CatalogItemMenuType == mType && c.CatalogItemVisible && !c.IsServicePage).OrderBy(c => c.CatalogItemPriority);
		}

		/// <summary>
		/// Возвращает видимые дочерние разделы (кроме служебных)
		/// </summary>
		public IEnumerable<CatalogItem> GetCatalogItems(int parentItemId)
		{
			return _catalogItems.Where(c => c.ParentItemID == parentItemId && c.CatalogItemVisible && !c.IsServicePage).OrderBy(c => c.CatalogItemPriority);
		}

		public CatalogItem GetCatalogItem(int catalogItemId)
		{
			#region Murkin, убрал для реализции локализации (меню будет одно)
			//catalogItemId = ConvertLocaleCatalogToCurrent( catalogItemId );
			#endregion

			return _dictionary[catalogItemId];
		}

		public CatalogItem[] GetCatalogPathItems(int catalogItemId)
		{
			#region Murkin, убрал для реализации локализации (меню будет одно)
			//catalogItemId = ConvertLocaleCatalogToCurrent( catalogItemId );
			#endregion
			List<CatalogItem> res = new List<CatalogItem>();

			for (CatalogItem item = GetCatalogItem(catalogItemId); item != null && item.CatalogItemID != this.RootCatalogItem.CatalogItemID; item = GetCatalogItem(item.ParentItemID.Value))
			{
				res.Add(item);
			}

			res.Reverse();

			return res.ToArray();
		}

		#region Murkin, убрал для реализации локализации (меню будет одно)
		///// <summary>
		///// Поиск и замена ИД элемента меню на ИД соответсвенного элемента в текущей локализации
		///// </summary>
		///// <param name="ciID">ИД строки</param>
		///// <returns>ИД строки в текущей локали</returns>
		//public int ConvertLocaleCatalogToCurrent(int ciID)
		//{
		//    var cutCat = _catalogItems.FirstOrDefault( t => t.CatalogItemID == ciID );

		//    if ( cutCat == null )
		//    {
                
		//        if ( ( RootCatalogID == "~en" ) || ( RootCatalogID == "~de" ) )
		//        {
		//            var tCI = (from cL in _catalogItemsRU[InternalFranchName]
		//                        join cC in _catalogItems on cL.CatalogItemPath equals cC.CatalogItemPath
		//                        where cL.CatalogItemID == ciID
		//                        select cC ).FirstOrDefault();
		//            if ( tCI != null )
		//            {
		//                return tCI.CatalogItemID;
		//            }
		//        }
		//        if ( ( RootCatalogID == "~" ) || ( RootCatalogID == "~de" ) )
		//        {
		//            var tCI = (from cL in _catalogItemsEN[InternalFranchName]
		//                        join cC in _catalogItems on cL.CatalogItemPath equals cC.CatalogItemPath
		//                        where cL.CatalogItemID == ciID
		//                        select cC ).FirstOrDefault();
		//            if ( tCI != null )
		//            {
		//                return tCI.CatalogItemID;
		//            }
		//        }
		//        if ( ( RootCatalogID == "~" ) || ( RootCatalogID == "~en" ) )
		//        {
		//            var tCI = (from cL in _catalogItemsDE[InternalFranchName]
		//                        join cC in _catalogItems on cL.CatalogItemPath equals cC.CatalogItemPath
		//                        where cL.CatalogItemID == ciID
		//                        select cC ).FirstOrDefault();
		//            if ( tCI != null )
		//            {
		//                return tCI.CatalogItemID;
		//            }
		//        }
		//    }

		//    //TODO: На случай если ИД с одного сайта а Итемы берутся с другого.  Такого быть не должно
		//    if (cutCat == null) { return _catalogItems.FirstOrDefault().CatalogItemID; }

		//    return cutCat.CatalogItemID;
		//}
		#endregion


        private string GetInternalFranchName()
        {
            string internalFranchName = ConfigurationManager.AppSettings["InternalFranchName"];
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies["InternalFranchName"] != null)
                {
                    internalFranchName = HttpContext.Current.Request.Cookies["InternalFranchName"].Value;
                }
                else if (SiteContext._internalFranchName == null)
                {
                    internalFranchName = ConfigurationManager.AppSettings["InternalFranchName"];
                }
                else
                {
                    internalFranchName = SiteContext._internalFranchName;
                }
            }
            catch
            {

            }

            return internalFranchName;
        }
	}

}
