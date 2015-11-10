﻿using System;
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
    public partial class TecDocModificationDetails : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			using( var ctx = new StoreDataContext() )
			{
				var modificationId = Request.QueryString.Get<int>( UrlKeys.StoreAndTecdoc.CarTypeId );
				var modification = TecDocAggregator.GetModificationById( modificationId );

				if( modification == null )
					throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );

				var model = modification.Model;
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
				CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( modification.Name.Tex_Text, UrlManager.GetTecDocModificationDetailsUrl( modification.ID ) ) );

				//Заголовок
				_titleLabel.Text = isCarModel ? "Легковые автомобили" : "Грузовые автомобили и автобусы";
				_subTitleLabel.Text = Server.HtmlEncode( modification.Name.Text );

				//Список модификаций
				_tecDocPartsTree.ModificationId = modification.ID;

				//Ссылка на AutoXP
				_autoXPPlaceHolder.Visible = brand != null && !string.IsNullOrEmpty( brand.AutoXPUrl );
				if( brand != null ) _autoXPLink.NavigateUrl = brand.AutoXPUrl;

				//параметры страницы
				var text = SeoTecDocTextTemplatesCache.Default.GetPageText( Request.RawUrl, modification.Name.Text );
				CmsContext.Current.PageFields = new PageFields
				{
					Title = text.PageTitle,
					Footer = text.PageFooter
				};
			}
		}

	}
}
