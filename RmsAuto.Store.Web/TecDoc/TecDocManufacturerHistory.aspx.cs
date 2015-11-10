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
    public partial class TecDocManufacturerHistory : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
        public string FieldNum;

        protected void Page_Load(object sender, EventArgs e)
        {
            var manufacturerUrlCode = CmsContext.Current.PageParameters["UrlCode"];//Request.QueryString.Get<int>(UrlKeys.StoreAndTecdoc.ManufacturerId);
            var vehicleType = CmsContext.Current.PageParameters["VehicleType"] == "Cars" ? VehicleType.Car : VehicleType.Truck;// Request.QueryString.Get<int>( UrlKeys.StoreAndTecdoc.IsCarModelId ) == 1;

            var brandItem = TecDocAggregator.GetBrandByUrlCode(manufacturerUrlCode, vehicleType);
            if (brandItem == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not found");
            }

            var brand = brandItem.Brand;
            var manufacturer = brandItem.Manufacturer;

            var isCarModel = vehicleType == VehicleType.Car;

            //Хлебные крошки
            CmsContext.Current.BreadCrumbSuffix.Add(new BreadCrumbItem(manufacturer.Name, UrlManager.GetTecDocManufacturerDetailsUrl(isCarModel, brand.UrlCode)));

            //Заголовок
            _titleLabel.Text = isCarModel ? "Легковые автомобили" : "Грузовые автомобили и автобусы";
            _subTitleLabel.Text = Server.HtmlEncode(manufacturer.Name);

            //Информация о бренде, ссылка на AutoXP
            _autoXPPlaceHolder.Visible = brand != null && !string.IsNullOrEmpty(brand.AutoXPUrl);
            _logoImagePlaceHolder.Visible = brand != null && brand.LogoFileID.HasValue;
            if (brand != null)
            {
                FieldNum = brand.AutoXPUrl;
                _infoLabel.Text = brand.Info;
                if (brand.LogoFileID.HasValue)
                    _logoImage.ImageUrl = UrlManager.GetFileUrl(brand.LogoFileID.Value);
            }

            //Ссылка на текдок
            _tecdocLink.NavigateUrl = UrlManager.GetTecDocManufacturerDetailsUrl(isCarModel, manufacturerUrlCode);

            //параметры страницы
            CmsContext.Current.PageFields = new PageFields
            {
                Title = brand != null ? (brand.PageTitle ?? brand.Name) : manufacturer.Name,
                Keywords = brand != null ? brand.PageKeywords : null,
                Description = brand != null ? brand.PageDescription : null,
                Footer = brand != null ? brand.PageFooter : null
            };

        }

        protected void XPButton_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("FranchName", UrlManager.MakeAbsoluteUrl("")));
            HttpContext.Current.Response.Cookies["FranchName"].Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies["FranchName"].Domain = ".rmsauto.ru";
            Response.Redirect(FieldNum);
        }
    }
}
