﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml.Xsl;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
	public partial class TecDocManufacturers : System.Web.UI.UserControl
	{
		protected string GetManufacturerUrl( object objBrandItem )
		{
			var brandItem = (BrandItem)objBrandItem;
			return UrlManager.GetTecDocManufacturerHistoryUrl( brandItem.Brand.VehicleType == VehicleType.Car, brandItem.Brand.UrlCode );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			var brandItems = TecDocAggregator.GetBrands();
			_carDataList.DataSource = brandItems.Where( it => it.Brand.VehicleType == VehicleType.Car );
			_carDataList.DataBind();
			_truckDataList.DataSource = brandItems.Where( it => it.Brand.VehicleType == VehicleType.Truck );
			_truckDataList.DataBind();
		}

	}
}