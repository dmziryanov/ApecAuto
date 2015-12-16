using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Specialized;
using RmsAuto.Common.Web;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Cms.Routing
{

	public static class UrlManager
	{
		public static CatalogItemsDictionary CatalogItems
		{
			get { return CatalogItemsCache.Default; }
		}

		public static string MakeAbsoluteUrl(string virtualPath)
		{
			string host = ConfigurationManager.AppSettings["WebSiteUrl"];
			string appPath = HttpContext.Current.Request.ApplicationPath;
			if (appPath != "/" && virtualPath.StartsWith(appPath, StringComparison.InvariantCultureIgnoreCase))
				virtualPath = virtualPath.Substring(appPath.Length);
			return host + virtualPath;
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			using (routes.GetWriteLock())
			{
				RegisterCmsRoutes(routes);
				// Murkin Daniil 2012.01.30 Закомментировал в связи с тем что мы убираем поиск по VIN на сайте,
				// т.к. перестаем ориентироваться на розницу.
				//RegisterVinRequestRoutes(routes);
				RegisterTecDocRoutes(routes);
				RegisterPrivateOfficeRoutes(routes);
				RegisterStoreRoutes(routes);
                RegisterLaximoRoutes(routes);
			}
		}

        private static void RegisterLaximoRoutes(RouteCollection routes)
        {
            routes.Add(
                "LaximoThreeColumnsCatalogs",
                new Route(
                    @"Rms/Catalogs.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/ThreeColumnsCatalogs.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoCatalog",
                new Route(
                    @"Rms/Catalog.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/Catalog.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoVehicles",
                new Route(
                    @"Rms/Vehicles.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/Vehicles.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoVehicle",
                new Route(
                    @"Rms/Vehicle.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/Vehicle.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoUnit",
                new Route(
                    @"Rms/Unit.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/Unit.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoWizard",
                new Route(
                    @"Rms/Wizard.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/Wizard.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoQuickGroups",
                new Route(
                    @"Rms/QuickGroups.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/QuickGroups.aspx" } },
                    new PageRouteHandler())
                    );
            routes.Add(
                "LaximoQuickDetails",
                new Route(
                    @"Rms/QuickDetails.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/LaximoCatalogs/QuickDetails.aspx" } },
                    new PageRouteHandler())
                    );
        }

		private static void RegisterStoreRoutes(RouteCollection routes)
		{
			routes.Add(
				"SparePart",
				new Route(
					"SparePart.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SparePart.aspx" } },
					new PageRouteHandler())
					);
			// dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
			routes.Add(
				"SparePartDefects",
				new Route(
					"SparePartDefects.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SparePartDefects.aspx" } },
					new PageRouteHandler())
					);
			routes.Add
				(
				"OrderListProcessedSearchRedirect",
				new CatalogDependentRoute(
					CatalogItems.MyGarageCatalogItem.CatalogItemPath,
					"OrderListProcessedSearchRedirect.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/OrderListProcessedSearchRedirect.aspx" } },
					new PageRouteHandler())
				);

			routes.Add(
				"SearchManufacturers",
				new Route(
					"SearchManufacturers.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SearchManufacturers.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"SearchDiscountManufacturers",
				new Route(
					"SearchDiscountManufacturers.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SearchDiscountManufacturers.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"SearchSpareParts",
				new Route(
					"SearchSpareParts.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SearchSpareParts.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"SearchDiscountSpareParts",
				new Route(
					"SearchDiscountSpareParts.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SearchDiscountSpareParts.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"SupplierStatistic",
				new Route(
					"SupplierStatistic.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Store/SupplierStatistic.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"StockSupplierParts",
				new CatalogDependentRoute(
					CatalogItems.StockSuppliersCatalogItem.CatalogItemPath,
					"Parts.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/Store/StockSupplierParts.aspx" } },
					new PageRouteHandler())
					);
			// dan 02.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
			routes.Add(
				"SparePartImage",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"SPImageHandler.ashx",
					null,
					new RouteValueDictionary { { "Url", "~/Store/SPImageHandler.ashx" } },
					new PageRouteHandler())
					);
		}

		private static void RegisterPrivateOfficeRoutes(RouteCollection routes)
		{
			routes.Add(
				"Login",
				new Route(
					"Login.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/Login.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"Registration",
				new CatalogDependentRoute(
					CatalogItems.PrivateOfficeCatalogItem.CatalogItemPath,
					@"Registration.aspx",
					null,
                    new RouteValueDictionary { { "Url", "~/PrivateOffice/RegisterOnline.aspx" } },
					new PageRouteHandler())
					);

            routes.Add(
                "RegistrationFranch",
                new CatalogDependentRoute(
                    CatalogItems.PrivateOfficeCatalogItem.CatalogItemPath,
                    @"RegistrationFranch.aspx",
                    null,
                    new RouteValueDictionary { { "Url", "~/PrivateOffice/RegisterOnlineFranch.aspx" } },
                    new PageRouteHandler())
                    );

            routes.Add(
				"OrderDetails",
				new CatalogDependentRoute(
					CatalogItems.OrdersCatalogItem.CatalogItemPath,
					@"OrderDetails.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/OrderDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"OrderLineTracking",
				new CatalogDependentRoute(
					CatalogItems.OrdersCatalogItem.CatalogItemPath,
					@"Tracking.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/OrderLineTracking.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"CartCheckout",
				new CatalogDependentRoute(
					CatalogItems.CartCatalogItem.CatalogItemPath,
					@"Checkout.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/Checkout.aspx" } },
					new PageRouteHandler())
					);


			routes.Add(
				"CartPrint",
				new CatalogDependentRoute(
					CatalogItems.CartCatalogItem.CatalogItemPath,
					@"Print.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/CartPrint.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"CartImport",
				new CatalogDependentRoute(
					CatalogItems.PrivateOfficeCatalogItem.CatalogItemPath,
					@"CartImport.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/CartImport.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"PaymentOrderPrint",
				new CatalogDependentRoute(
					CatalogItems.OrdersCatalogItem.CatalogItemPath,
					@"PaymentOrderPrint.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/PaymentOrderPrint.aspx" } },
					new PageRouteHandler())
					);

            routes.Add(
                "OptPaymentOrderPrint",
                new CatalogDependentRoute(
                    CatalogItems.OrdersCatalogItem.CatalogItemPath,
                    @"OptPaymentOrderPrint.aspx",
                    null,
                    new RouteValueDictionary { { "Url", "~/Manager/optPaymentOrderPrint.aspx" } },
                    new PageRouteHandler())
                    );



			routes.Add(
				"OrderPrint",
				new CatalogDependentRoute(
					CatalogItems.OrdersCatalogItem.CatalogItemPath,
					@"OrderPrint.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/OrderPrint.aspx" } },
					new PageRouteHandler())
					);
			routes.Add(
				"OrderLinesReadyForDeliveryPrint",
				new CatalogDependentRoute(
					CatalogItems.OrdersCatalogItem.CatalogItemPath,
					@"ReadyForDeliveryPrint.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/OrderLinesReadyForDeliveryPrint.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"ClientActivation",
				new CatalogDependentRoute(
					CatalogItems.PrivateOfficeCatalogItem.CatalogItemPath,
                    @"Activation.aspx",
					null,
                    new RouteValueDictionary { { "Url", "~/PrivateOffice/ActivateUserAccount.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"PasswordRecovery",
				new CatalogDependentRoute(
					CatalogItems.PrivateOfficeCatalogItem.CatalogItemPath,
                    @"PasswordRecovery.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/PasswordRecovery.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"ContractTermsFrame",
				new CatalogDependentRoute(
					CatalogItems.PrivateOfficeCatalogItem.CatalogItemPath,
					"ContractTerms.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/ContractTermsFrame.aspx" } },
					new PageRouteHandler())
					);
            //deas 16.03.2011 task3586
            //изменено на страницу общей настройки
			routes.Add(
				"UserSetting",
				new CatalogDependentRoute(
					CatalogItems.UserSettingCatalogItem.CatalogItemPath,
                    @"UserSetting.aspx",
					null,
                    new RouteValueDictionary { { "Url", "~/PrivateOffice/UserSetting.aspx" } },
					new PageRouteHandler())
					);
            //deas 16.03.2011 task3308
            //добавление страницы с запросом на отправку детального баланса клиента
            routes.Add(
                "DetailBalance",
                new CatalogDependentRoute(
                    CatalogItems.DetailBalanceCatalogItem.CatalogItemPath,
                    @"DetailBalance.aspx",
                    null,
                    new RouteValueDictionary { { "Url", "~/PrivateOffice/DetailBalance.aspx" } },
                    new PageRouteHandler() )
                    );
			//dan 07.07.2011 task4770
			//добавление страницы для работы с возвратами
			routes.Add(
				"Reclamation",
				new CatalogDependentRoute(
					CatalogItems.ReclamationCatalogItem.CatalogItemPath,
					@"Reclamation.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/Reclamation.aspx" } },
					new PageRouteHandler())
					);
			//dan 13.07.2011 task4770
			//добавление страницы для оформления заявки на отказ либо на возврат
			routes.Add(
				"ReclamationRequest",
				new CatalogDependentRoute(
					CatalogItems.ReclamationCatalogItem.CatalogItemPath,
					@"ReclamationRequest.aspx",
					null,
					new RouteValueDictionary { {"Url", "~/PrivateOffice/ReclamationRequest.aspx"} },
					new PageRouteHandler())
				);
			//dan 25.08.2011 task4770
			//печатная форма для заявки на отказ либо на возврат
			routes.Add(
				"ReclamationPrint",
				new CatalogDependentRoute(
					CatalogItems.ReclamationCatalogItem.CatalogItemPath,
					@"ReclamationPrint.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/ReclamationPrint.aspx" } },
					new PageRouteHandler() )
					);

			//dan 09.09.2011
			//добавление страницы для отправки предложений поставщиками
			//TODO раскомментировать при возобновлении задачи отправки предложений поставщикам
			//routes.Add(
			//    "Offers",
			//    new Route(
			//        "Suppliers/Offers.aspx",
			//        null,
			//        null,
			//        new RouteValueDictionary { { "Url", "~/PrivateOffice/Offers.aspx" } },
			//        new PageRouteHandler() )
			//        );
			
			//dan 05.11.2013
			//добавление страницы для тестирования скрипта LiveTex (онлайн чат)
			routes.Add(
				"LiveTexTest",
				new Route(
					"LiveTexTest.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/LiveTexTest.aspx" } },
					new PageRouteHandler())
					);
		}

		private static void RegisterTecDocRoutes(RouteCollection routes)
		{
			routes.Add(
				"TecDocManufacturerHistory",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"{VehicleType}/{UrlCode}.aspx",
					new RouteValueDictionary { { "VehicleType", "(Cars)|(Trucks)" },
									{ "UrlCode", @"[\d\w]+" } },
					new RouteValueDictionary { { "Url", "~/TecDoc/TecDocManufacturerHistory.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"TecDocManufacturerDetails",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"{VehicleType}/{UrlCode}/Models.aspx",
					new RouteValueDictionary { { "VehicleType", "(Cars)|(Trucks)" },
						{ "UrlCode", @"[\d\w]+" } },
					new RouteValueDictionary { { "Url", "~/TecDoc/TecDocManufacturerDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"TecDocModelDetails",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"Model.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/TecDoc/TecDocModelDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"TecDocModificationDetails",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"Modification.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/TecDoc/TecDocModificationDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"TecDocSearchTreeNodeDetails",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"Parts.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/TecDoc/TecDocSearchTreeNodeDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				 "TecDocInfo",
				 new CatalogDependentRoute(
					  CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					 @"PartInfo.aspx",
					 null,
					 new RouteValueDictionary { { "Url", "~/TecDoc/PartInfo.aspx" } },
					 new PageRouteHandler())
					 );

			routes.Add(
				 "TecDocInfoImages",
				 new CatalogDependentRoute(
					  CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					 @"PartInfoImages.aspx",
					 null,
					 new RouteValueDictionary { { "Url", "~/TecDoc/PartInfoImages.aspx" } },
					 new PageRouteHandler())
					 );

			routes.Add(
				 "TecDocInfoCars",
				 new CatalogDependentRoute(
					  CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					 @"PartInfoCars.aspx",
					 null,
					 new RouteValueDictionary { { "Url", "~/TecDoc/PartInfoCars.aspx" } },
					 new PageRouteHandler())
					 );

			routes.Add(
				"TecDocImage",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"ImageHandler.ashx",
					null,
					new RouteValueDictionary { { "Url", "~/TecDoc/ImageHandler.ashx" } },
					new PageRouteHandler())
					);
		}

		private static void RegisterCmsRoutes(RouteCollection routes)
		{
			routes.Add(
				"SeoPartsCatalog",
				new SeoPartsCatalogRoute()
				);

			routes.Add(
				"Catalog",
				new CatalogRoute());

			routes.Add(
				 "NewsDetails",
				 new CatalogDependentRoute(
					 CatalogItems.NewsListCatalogItem.CatalogItemPath,
					 @"{ID}.aspx",
					 new RouteValueDictionary { { "ID", @"\d+" } },
					 new RouteValueDictionary { { "Url", "~/Cms/NewsDetails.aspx" } },
					 new PageRouteHandler())
					 );

			routes.Add (
				"ShopDetails",
				new CatalogDependentRoute(
					CatalogItems.ShopListCatalogItem.CatalogItemPath,
					@"{ID}.aspx",
					new RouteValueDictionary { { "ID", @"\d+" } },
					new RouteValueDictionary { { "Url", "~/Cms/ShopDetails.aspx" } },
					new PageRouteHandler())
					);

            routes.Add(
                    "ShopList",
                    new CatalogDependentRoute(CatalogItems.ShopListCatalogItem.CatalogItemPath,
                        @"{Region}/{ID}.aspx",
                        new RouteValueDictionary { { "ID", @"\d+" } },
                        new RouteValueDictionary { { "Url", "~/Cms/ShopList.aspx" } },
                        new PageRouteHandler())
                        );

            routes.Add(
                      "Maps",
                      new CatalogDependentRoute(CatalogItems.ShopListCatalogItem.CatalogItemPath,
                          @"Map.aspx",
                          new RouteValueDictionary { { "ID", @"\d+" } },
                          new RouteValueDictionary { { "Url", "~/Cms/Shops/YandexMap.aspx" } },
                          new PageRouteHandler())
                          );

            /*routes.Add(
                    "ShopList2",
                    new CatalogDependentRoute(
                        CatalogItems.ShopListCatalogItem.CatalogItemPath,
                        @"City/{ID}",
                        new RouteValueDictionary { { "CityID", @"\d+" } },
                        new RouteValueDictionary { { "Url", "~/Cms/ShopList.aspx" } },
                        new PageRouteHandler())
                        );*/

			routes.Add(
				"VacancyDetails",
				new CatalogDependentRoute(
					CatalogItems.VacancyListCatalogItem.CatalogItemPath,
					@"{ID}.aspx",
					new RouteValueDictionary { { "ID", @"\d+" } },
					new RouteValueDictionary { { "Url", "~/Cms/VacancyDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"ManufacturerDetails",
				new CatalogDependentRoute(
					CatalogItems.OnlineCatalogsCatalogItem.CatalogItemPath,
					@"Manufacturers/{UrlCode}.aspx",
					new RouteValueDictionary { { "UrlCode", @"[\d\w]+" } },
					new RouteValueDictionary { { "Url", "~/Cms/ManufacturerDetails.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"File",
				new Route(
					"Files/{ID}.ashx",
					null,
					new RouteValueDictionary { { "ID", @"^\d+$" } },
					new RouteValueDictionary { { "Url", "~/Cms/File.ashx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"Flash",
				new Route(
					"Files/{ID}.ashx/{fileName}.swf",
					null,
					new RouteValueDictionary { { "ID", @"^\d+$" } },
					new RouteValueDictionary { { "Url", "~/Cms/Flash.ashx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"Thumbnail",
				new Route(
					"Thumbnails/{Key}/{ID}.ashx",
					null,
                    new RouteValueDictionary { { "ID", @"^\d+$" }, { "Key", @"^\w+$" } },
					new RouteValueDictionary { { "Url", "~/Cms/Thumbnail.ashx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"SupplierStat",
				new Route(
					"SupplierStat/Chart.ashx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Cms/ChartSupplierStat.ashx" } },
					new PageRouteHandler())
					);
			/* === BEGIN TEST ==== */
			
			routes.Add(
				"Error403",
				new Route(
					"Error403.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Cms/Error403.aspx" } },
					new PageRouteHandler())
				);

			routes.Add(
				"Error404",
				new Route(
					"Error404.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Cms/Error404.aspx" } },
					new PageRouteHandler())
				);

			routes.Add(
				"Error500",
				new Route(
					"Error500.aspx",
					null,
					null,
					new RouteValueDictionary { { "Url", "~/Cms/Error500.aspx" } },
					new PageRouteHandler())
				);

            routes.Add(
                "ErrorBL",
                new Route(
                    "ErrorBL.aspx",
                    null,
                    null,
                    new RouteValueDictionary { { "Url", "~/Cms/ErrorBL.aspx" } },
                    new PageRouteHandler() )
                );

			/* === END TEST ====== */
		}

		private static void RegisterVinRequestRoutes(RouteCollection routes)
		{
            //Сейчас вин-запросы отключены
            //routes.Add(
            //    "VinRequestDetails",
            //    new CatalogDependentRoute(
            //        CatalogItems.VinRequestsCatalogItem.CatalogItemPath,
            //        String.Format("{{{0}}}.aspx", UrlKeys.VinRequests.RequestId),
            //        new RouteValueDictionary { { UrlKeys.VinRequests.RequestId, @"\d+" } },
            //        new RouteValueDictionary { { "Url", "~/PrivateOffice/VinRequestDetails.aspx" } },
            //        new PageRouteHandler())
            //        );

			routes.Add(
				"VinRequestNew",
				new CatalogDependentRoute(
					CatalogItems.VinRequestsCatalogItem.CatalogItemPath,
					"New.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/VinRequestNew.aspx" } },
					new PageRouteHandler())
					);

			/*routes.Add(
				"VinRequestGarage",
				new CatalogDependentRoute(
					CatalogItems.VinRequestsCatalogItem.CatalogItemPath,
					"MyGarage.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/MyGarage.aspx" } },
					new PageRouteHandler() )
					);*/

			routes.Add(
				"VinRequestGarageNew",
				new CatalogDependentRoute(
					CatalogItems.MyGarageCatalogItem.CatalogItemPath,
					"NewCar.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/MyGarageNew.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"VinRequestGarageEdit",
				new CatalogDependentRoute(
					CatalogItems.MyGarageCatalogItem.CatalogItemPath,
					String.Format("MyGarage/{{{0}}}.aspx", UrlKeys.VinRequests.CarId),
					new RouteValueDictionary { { UrlKeys.VinRequests.CarId, @"\d+" } },
					new RouteValueDictionary { { "Url", "~/PrivateOffice/MyGarageEdit.aspx" } },
					new PageRouteHandler())
					);

			routes.Add(
				"VinRequestGarageView",
				new CatalogDependentRoute(
					CatalogItems.MyGarageCatalogItem.CatalogItemPath,
					String.Format("MyGarage/View/{{{0}}}.aspx", UrlKeys.VinRequests.CarId),
					new RouteValueDictionary { { UrlKeys.VinRequests.CarId, @"\d+" } },
					new RouteValueDictionary { { "Url", "~/PrivateOffice/MyGarageView.aspx" } },
					new PageRouteHandler())
					);

			routes.Add
				(
				"VinRequestAccessDenied",
				new CatalogDependentRoute(
					CatalogItems.MyGarageCatalogItem.CatalogItemPath,
					"VinRequestAccessDenied.aspx",
					null,
					new RouteValueDictionary { { "Url", "~/PrivateOffice/VinRequestAccessDenied.aspx" } },
					new PageRouteHandler())
				);
		}

		public static string GetOrderListProcessedSearchRedirectURL()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"OrderListProcessedSearchRedirect",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetOrderListProcessedSearchRedirectURL(string mfr, string pn, bool searchCounterparts, int excludeSupplierID, int acctgOrderLineID /*orderLineID*/)
		{

			return string.Format("{0}?{1}={2}&{3}={4}&{5}={6}{7}&{8}={9}",
									GetOrderListProcessedSearchRedirectURL(),
									UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(mfr),
									UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode(pn),
									UrlKeys.StoreAndTecdoc.ExcludeSupplierID, excludeSupplierID,
									searchCounterparts ? string.Format("&{0}=1", UrlKeys.StoreAndTecdoc.SearchCounterparts) : string.Empty,
									/*UrlKeys.OrderLineRequests.OrderLineId*/UrlKeys.OrderLineRequests.AcctgOrderLineId, /*orderLineID*/ acctgOrderLineID );

		}
		public static string GetSearchSparePartsUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SearchSpareParts",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetSearchDiscountSparePartsUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SearchDiscountSpareParts",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetSupplierStatisticUrl(string sparePartKey)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SupplierStatistic",
					null);
				return p.VirtualPath + "?ID=" + HttpUtility.HtmlEncode(sparePartKey);
			}
		}

        public static string GetSellerMessageUrl(string InternalFranchName)
        {
                return "Message.aspx?ID=" + InternalFranchName;
        }


		public static string GetSearchSparePartsUrl(string manufacturer, string partNumber)
		{
			return string.Format("{0}?{1}={2}&{3}={4}",
						GetSearchSparePartsUrl(),
						UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(manufacturer),
						UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode(partNumber));
		}

		public static string GetSearchSparePartsUrl(string mfr, string pn, bool searchCounterparts)
		{
			return string.Format("{0}?{1}={2}&{3}={4}{5}",
								 GetSearchSparePartsUrl(),
								 UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(mfr),
								 UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode(pn),
								 searchCounterparts ? string.Format("&{0}=1", UrlKeys.StoreAndTecdoc.SearchCounterparts) : string.Empty);
		}

		public static string GetSearchSparePartsUrl(string mfr, string pn, bool searchCounterparts, int excludeSupplierID)
		{
			return string.Format("{0}?{1}={2}&{3}={4}&{5}={6}{7}",
								 GetSearchSparePartsUrl(),
								 UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(mfr),
								 UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode(pn),
								 UrlKeys.StoreAndTecdoc.ExcludeSupplierID, excludeSupplierID,
								 searchCounterparts ? string.Format("&{0}=1", UrlKeys.StoreAndTecdoc.SearchCounterparts) : string.Empty);
		}

		public static string GetSearchDiscountSparePartsUrl(string mfr)
		{
			return string.Format("{0}?{1}={2}",
									GetSearchDiscountSparePartsUrl(),
									UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(mfr));
		}

		public static string GetSearchManufacturersUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SearchManufacturers",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetSearchManufacturersUrl(string pn, bool searchCounterParts)
		{
			return string.Format(
				"{0}?{1}={2}&{3}={4}",
				GetSearchManufacturersUrl(),
				UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode(pn),
				UrlKeys.StoreAndTecdoc.SearchCounterparts, searchCounterParts ? "1" : "0");
		}

		public static string GetSeoPartsCatalogRootUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SeoPartsCatalog",
					new RouteValueDictionary());
				return p.VirtualPath;
			}
		}

		public static string GetSeoPartsCatalogUrl(int seoPartsCatalogItemId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SeoPartsCatalog",
					new RouteValueDictionary { { "ID", seoPartsCatalogItemId } });
				return p.VirtualPath;
			}
		}

		public static string GetSeoPartsCatalogUrl(SeoPartsCatalogItem[] seoPartsCatalogItems)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SeoPartsCatalog",
					new RouteValueDictionary { { "SeoPartsCatalogItems", seoPartsCatalogItems } });
				return p.VirtualPath;
			}
		}

		public static string GetCatalogUrl(int catalogItemId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"Catalog",
					new RouteValueDictionary { { "ID", catalogItemId } });
				return p.VirtualPath;
			}
		}

		public static string GetMapUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.MapCatalogItem.CatalogItemID);
		}

		public static string GetShopListUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.ShopListCatalogItem.CatalogItemID);
		}

		public static string GetNewsListUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.NewsListCatalogItem.CatalogItemID);
		}

		public static string GetNewsDetailsUrl(int newsItemId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"NewsDetails",
					new RouteValueDictionary { { "ID", newsItemId } });
				return p.VirtualPath;
			}
		}

		public static string GetShopDetailsUrl(int shopId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"ShopDetails",
					new RouteValueDictionary { { "ID", shopId } });
				return p.VirtualPath;
			}
		}

		public static string GetVacancyDetailsUrl(int vacancyId)
		{

			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VacancyDetails",
					new RouteValueDictionary { { "ID", vacancyId } });
				return p.VirtualPath;
			}
		}

		public static string GetManufacturerDetailsUrl(string manufacturerUrlCode)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"ManufacturerDetails",
					new RouteValueDictionary { { "UrlCode", manufacturerUrlCode } }
					);
				return p.VirtualPath;
			}
		}

		#region BC

		public static string GetTecDocManufacturerHistoryUrl(bool isCarModel, string manufacturerUrlCode)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocManufacturerHistory",
					new RouteValueDictionary { 
						{ "VehicleType", isCarModel ? "Cars" : "Trucks" }, 
						{ "UrlCode", manufacturerUrlCode } });
				return p.VirtualPath;
			}
		}

		public static string GetTecDocManufacturerDetailsUrl(bool isCarModel, string manufacturerUrlCode)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocManufacturerDetails",
					new RouteValueDictionary { 
						{ "VehicleType", isCarModel ? "Cars" : "Trucks" }, 
						{ "UrlCode", manufacturerUrlCode } });
				return p.VirtualPath;
			}
		}

		public static string GetTecDocModelDetailsUrl(int modelId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocModelDetails",
					null);
				return string.Format("{0}?{1}={2}", p.VirtualPath, UrlKeys.StoreAndTecdoc.ModelId, modelId);
			}

		}

		public static string GetTecDocModificationDetailsUrl(int modificationId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocModificationDetails",
					null);
				return string.Format("{0}?{1}={2}", p.VirtualPath, UrlKeys.StoreAndTecdoc.CarTypeId, modificationId);
			}
		}

		public static string GetTecDocSearchTreeNodeDetailsUrl(int modificationId, int searchTreeNodeId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocSearchTreeNodeDetails",
					null);
				return string.Format("{0}?{1}={2}&{3}={4}", p.VirtualPath,
					UrlKeys.StoreAndTecdoc.CarTypeId, modificationId,
					UrlKeys.StoreAndTecdoc.SearchTreeNodeId, searchTreeNodeId);
			}
		}

		#endregion

		public static string GetTecDocInfoUrl(int articleId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocInfo",
					null);
				return String.Format("{0}?{1}={2}", p.VirtualPath, UrlKeys.StoreAndTecdoc.ArticleId, articleId);
			}
		}

		public static string GetTecDocInfoImagesUrl(int articleId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocInfoImages",
					null);
				return String.Format("{0}?{1}={2}", p.VirtualPath, UrlKeys.StoreAndTecdoc.ArticleId, articleId);
			}
		}
		
		public static string GetTecDocInfoCarsUrl(int articleId)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocInfoCars",
					null);
				return String.Format("{0}?{1}={2}", p.VirtualPath, UrlKeys.StoreAndTecdoc.ArticleId, articleId);
			}
		}

		public static string GetTecDocImageUrl(int id)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"TecDocImage",
					new RouteValueDictionary { { "id", id } });
				return p.VirtualPath;
			}
		}
		
		// dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
		public static string GetSparePartDefectImageUrl(string spid)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SparePartImage",
					new RouteValueDictionary { { "spid", spid } });
				return p.VirtualPath;
			}
		}

		public static string GetSparePartDetailsUrl(string sparePartKey)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SparePart",
					null
					 );
				return p.VirtualPath + "?ID=" + HttpUtility.HtmlEncode(sparePartKey);
			}
		}

		// dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
		public static string GetSparePartsDefectImagesUrl(string additionaInfoKey)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SparePartDefects",
					null
					);
				return p.VirtualPath + "?id=" + HttpUtility.HtmlEncode(additionaInfoKey);
			}
		}

		//dan 13.07.2011 task4770 Механизм возвратов
		public static string GetReclamationRequestUrl(string orderLineID)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"ReclamationRequest",
					null
					);
				return p.VirtualPath + "?id=" + HttpUtility.HtmlEncode(orderLineID);
			}
		}
		//dan 25.07.2011 task4770 Механизм возвратов
		public static string GetReclamationUrl( string mode )
		{
			using ( RouteTable.Routes.GetReadLock() )
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"Reclamation",
					null
					);
				return p.VirtualPath + (string.IsNullOrEmpty( mode ) ? string.Empty : HttpUtility.HtmlEncode( "?mode=" + mode ));
			}
		}

		public static string GetPrivateOfficeUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.PrivateOfficeCatalogItem.CatalogItemID);
		}

		public static string GetFeedbackUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.FeedbackCatalogItem.CatalogItemID);
		}


		public static string GetCartUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.CartCatalogItem.CatalogItemID);
		}

		public static string GetCartPrintUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"CartPrint",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetCartImportUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"CartImport",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetPaymentOrderPrintUrl(int orderID)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"PaymentOrderPrint",
					null);
				return string.Format("{0}?OrderID={1}", p.VirtualPath, orderID);
			}
		}

        public static string GetPaymentOrderPrintUrlOpt(int orderID)
        {
            
            {
                VirtualPathData p = RouteTable.Routes.GetVirtualPath(
                    null,
                    "OptPaymentOrderPrint",
                    null);
                return string.Format("{0}?OrderID={1}", p.VirtualPath, orderID);
            }
        }

		public static string GetOrderPrintUrl(int orderID)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"OrderPrint",
					null);
				return string.Format("{0}?OrderID={1}", p.VirtualPath, orderID);
			}
		}

		public static string GetReclamationPrintUrl( int reclamationID )
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"ReclamationPrint",
					null );
				return string.Format( "{0}?ReclamationID={1}", p.VirtualPath, reclamationID );
			}
		}

		public static string GetOrderLinesReadyForDeliveryPrintUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"OrderLinesReadyForDeliveryPrint",
					null);
				return p.VirtualPath;
			}
		}


		public static string GetCheckoutUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"CartCheckout",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetRegistrationUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"Registration",
					null);
				return p.VirtualPath;
			}
		}

        public static string GetRegistrationUrlFranch()
        {
            
            {
                VirtualPathData p = RouteTable.Routes.GetVirtualPath(
                    null,
                    "RegistrationFranch",
                    null);
                return p.VirtualPath;
            }
        }
        
        public static string GetOrdersUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.OrdersCatalogItem.CatalogItemID);
		}

		public static string GetOrderDetailsUrl(int orderId, string backUrl)
		{
			return GetOrderDetailsUrl(new int[] { orderId }, backUrl);
		}

		public static string GetOrderDetailsUrl(int[] orderIds, string backUrl)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"OrderDetails",
					null);
				return string.Format("{0}?ID={1}&back_url={2}",
					p.VirtualPath,
					HttpUtility.UrlEncode(string.Join(",", orderIds.Select(id => id.ToString()).ToArray())),
					HttpUtility.UrlEncode(backUrl));
			}
		}

        public static string GetAllOrderDetailsUrl(int[] orderIds, string backUrl)
        {
            
            {
                VirtualPathData p = RouteTable.Routes.GetVirtualPath(
                    null,
                    "AllOrderDetails",
                    null);
                return string.Format("{0}?ID={1}&back_url={2}",
                    p.VirtualPath,
                    HttpUtility.UrlEncode(string.Join(",", orderIds.Select(id => id.ToString()).ToArray())),
                    HttpUtility.UrlEncode(backUrl));
            }
        }

		public static string GetOrderLineTracking(int orderLineId, string backUrl)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"OrderLineTracking",
					null);
				return string.Format("{0}?ID={1}&back_url={2}", p.VirtualPath, orderLineId, HttpUtility.UrlEncode(backUrl));
			}
		}

		public static string GetFileUrl(int fileID)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"File",
					new RouteValueDictionary { { "ID", fileID } });
				return p.VirtualPath;
			}
		}

        /*public static string GetFileUrlRMS(int fileID)
        {
            
            {
                VirtualPathData p = RouteTable.Routes.GetVirtualPath(
                    null,
                    "File",
                    new RouteValueDictionary { { "ID", fileID } });
                return p.VirtualPath+"?r=rms";
            }
        }*/

		public static string GetFlashUrl(int fileID)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"Flash",
					new RouteValueDictionary { { "ID", fileID } });
				return p.VirtualPath;
			}
		}

		public static string GetThumbnailUrl(int fileID, string thumbnailGeneratorKey, string rms)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"Thumbnail",
					new RouteValueDictionary { 
						{ "ID", fileID }, 
						{ "Key", thumbnailGeneratorKey }
                       
					});
                if (rms != "")
                    { return p.VirtualPath + "?r=rms"; }
                else
                    { return p.VirtualPath; }
			}
		}

		public static string GetSupplierStatUrl(string sparePartKey)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"SupplierStat",
					null);
				return p.VirtualPath + "?ID=" + HttpUtility.HtmlEncode(sparePartKey);
			}
		}

        public static string GetClientActivationUrl(Guid activationCode, string FranchCode)
		{
			
			{
                //VirtualPathData p = RouteTable.Routes.GetVirtualPath(
                //    null,
                //    "ClientActivation",
                //    /*null*/
                //    new RouteValueDictionary { 
                //        { UrlKeys.Activation.MaintUid, activationCode.ToString() },
                //        { UrlKeys.Activation.FranchCode, FranchCode }
                //    });
                //return p.VirtualPath;
                
                if (FranchCode == null)
                    FranchCode = "rmsauto";
                    
                
                var httpUrl = (AcctgRefCatalog.RmsFranches[FranchCode].Url.Split(';')[0].Contains("http://") ? " " : "http://") + AcctgRefCatalog.RmsFranches[FranchCode].Url.Split(';')[0].Trim() + "/Cabinet/Activation.aspx";

                if (FranchCode == "rmsauto")
                    FranchCode = "";
				
                return string.Format("{0}?{1}={2}&{3}={4}",
                    httpUrl, UrlKeys.Activation.MaintUid, HttpUtility.UrlEncode(activationCode.ToString()),
					UrlKeys.Activation.FranchCode, HttpUtility.UrlEncode(FranchCode));
			}
		}

		public static string GetPasswordRecoveryUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
                    "PasswordRecovery",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetContractTermsFrameUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"ContractTermsFrame",
					null);
				return p.VirtualPath;
			}
		}

		#region VinRequests

		public static string GetVinRequestDetailsUrl(int id)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VinRequestDetails",
					new RouteValueDictionary { { UrlKeys.VinRequests.RequestId, id } });
				return p.VirtualPath;
			}
		}

		public static string GetVinRequestNewUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VinRequestNew",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetVinRequestNewUrl(int fromCarId)
		{
			return String.Format("{0}?{1}={2}", GetVinRequestNewUrl(), UrlKeys.VinRequests.CarId, fromCarId);
		}

		public static string GetGarageUrl()
		{
			return GetCatalogUrl(CatalogItems.MyGarageCatalogItem.CatalogItemID);
		}

		public static string GetNewGarageCarUrl()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VinRequestGarageNew",
					null);
				return p.VirtualPath;
			}
		}

		public static string GetGarageCarEditUrl(int id)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VinRequestGarageEdit",
					new RouteValueDictionary { { UrlKeys.VinRequests.CarId, id } });
				return p.VirtualPath;
			}
		}

		public static string GetGarageCarViewUrl(int id)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VinRequestGarageView",
					new RouteValueDictionary { { UrlKeys.VinRequests.CarId, id } });
				return p.VirtualPath;
			}
		}

		public static string GetVinRequestsUrl()
		{
			return GetCatalogUrl(UrlManager.CatalogItems.VinRequestsCatalogItem.CatalogItemID);
		}

		public static string GetVinRequestAccessDenied()
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"VinRequestAccessDenied",
					null);
				return p.VirtualPath;
			}
		}

		//public static string GetVinRequestAccessDeniedManager()
		//{
		//    
		//    {
		//        VirtualPathData p = RouteTable.Routes.GetVirtualPath(
		//            null,
		//            "VinRequestAccessDeniedManager",
		//            null);
		//        return p.VirtualPath;
		//    }
		//}

		#endregion

		#region StockSuppliers

		public static string GetStockSupplierPartsPage(string mfr)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"StockSupplierParts",
					null);
				return String.Format("{0}?{1}={2}", p.VirtualPath, UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(mfr));
			}
		}

		public static string GetStockSupplierPartsPage(string mfr, int page)
		{
			
			{
				VirtualPathData p = RouteTable.Routes.GetVirtualPath(
					null,
					"StockSupplierParts",
					null);
				return String.Format("{0}?{1}={2}&{3}={4}", p.VirtualPath, UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode(mfr), UrlKeys.Paging.PageNum, page);
			}
		}

		#endregion

		public static string BuildUrl(string url, NameValueCollection nameValueCollection)
		{
			return url + (url.IndexOf('?') >= 0 ? '&' : '?') + nameValueCollection.ToWwwQueryString();
		}
	}
}
