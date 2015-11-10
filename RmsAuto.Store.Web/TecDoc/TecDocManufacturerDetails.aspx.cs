using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Xml.Xsl;
using System.Linq;
using RmsAuto.TechDoc;
using RmsAuto.Store.Cms.Routing;
using System.Web;
using RmsAuto.Store.Web.TecDoc;
using RmsAuto.TechDoc.Entities.Helpers;
using System.Collections;
using System.Text;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.BL;
using System.Net;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.TecDoc
{
    public partial class TecDocManufacturerDetails : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			var manufacturerUrlCode = CmsContext.Current.PageParameters[ "UrlCode" ];//Request.QueryString.Get<int>(UrlKeys.StoreAndTecdoc.ManufacturerId);
			var vehicleType = CmsContext.Current.PageParameters[ "VehicleType" ] == "Cars" ? VehicleType.Car : VehicleType.Truck;// Request.QueryString.Get<int>( UrlKeys.StoreAndTecdoc.IsCarModelId ) == 1;

			var brandItem = TecDocAggregator.GetBrandByUrlCode( manufacturerUrlCode, vehicleType );
			if( brandItem == null )
			{
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );
			}

			var brand = brandItem.Brand;
			var manufacturer = brandItem.Manufacturer;

			var isCarModel = vehicleType == VehicleType.Car;

			//Хлебные крошки
			CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( manufacturer.Name, UrlManager.GetTecDocManufacturerHistoryUrl( isCarModel, brand.UrlCode ) ) );
			CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( "Каталог неоригинальных запчастей", UrlManager.GetTecDocManufacturerDetailsUrl( isCarModel, brand.UrlCode ) ) );

			//Заголовок
			_titleLabel.Text = isCarModel ? "Легковые автомобили" : "Грузовые автомобили и автобусы";
			_subTitleLabel.Text = Server.HtmlEncode( manufacturer.Name );

			//Список моделей
			_tecDocModels.ManufacturerId = manufacturer.ID;
			_tecDocModels.IsCarModel = isCarModel;

			//Информация о бренде, ссылка на AutoXP
			_autoXPPlaceHolder.Visible = brand != null && !string.IsNullOrEmpty( brand.AutoXPUrl );
			if( brand != null )
			{
				_autoXPLink.NavigateUrl = brand.AutoXPUrl;
			}

			//параметры страницы
			var text = SeoTecDocTextTemplatesCache.Default.GetPageText( Request.RawUrl, manufacturer.Name );
			CmsContext.Current.PageFields = new PageFields
			{
				Title = text.PageTitle,
				Footer = text.PageFooter
			};

		}

	}
}
