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
    public partial class TecDocModelDetails : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			var modelId = Request.QueryString.Get<int>( UrlKeys.StoreAndTecdoc.ModelId );
			var model = TecDocAggregator.GetModelById( modelId );

			if( model == null )
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );

			var manufacturer = model.Manufacturer;
			var isCarModel = model.IsCarModel == 1;

			var brandItem = TecDocAggregator.GetBrandByManufacturerID( manufacturer.ID, isCarModel ? VehicleType.Car : VehicleType.Truck );
			if( brandItem == null )
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );

			var brand = brandItem.Brand;

			//Хлебные крошки
			CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( manufacturer.Name, UrlManager.GetTecDocManufacturerHistoryUrl( isCarModel, brand.UrlCode ) ) );
			CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( "Каталог неоригинальных запчастей", UrlManager.GetTecDocManufacturerDetailsUrl( isCarModel, brand.UrlCode ) ) );
			CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( model.Name.Tex_Text, UrlManager.GetTecDocModelDetailsUrl( model.ID ) ) );

			//Заголовок
			_titleLabel.Text = isCarModel ? "Легковые автомобили" : "Грузовые автомобили и автобусы";

			StringBuilder sbSubTitle = new StringBuilder();
            if (model.Name.Tex_Text.IndexOf(manufacturer.Name, StringComparison.CurrentCultureIgnoreCase) < 0)
				sbSubTitle.AppendFormat( "{0} ", manufacturer.Name );
            sbSubTitle.Append(model.Name.Tex_Text);
			_subTitleLabel.Text = Server.HtmlEncode( sbSubTitle.ToString() );

			//Список модификаций
			_tecDocModifications.ModelId = model.ID;

			//Ссылка на AutoXP
			_autoXPPlaceHolder.Visible = brand != null && !string.IsNullOrEmpty( brand.AutoXPUrl );
			if( brand != null ) _autoXPLink.NavigateUrl = brand.AutoXPUrl;

			//параметры страницы
			var text = SeoTecDocTextTemplatesCache.Default.GetPageText( Request.RawUrl, sbSubTitle.ToString() );
			CmsContext.Current.PageFields = new PageFields
			{
				Title = text.PageTitle,
				Footer = text.PageFooter
			};

		}

	}
}
