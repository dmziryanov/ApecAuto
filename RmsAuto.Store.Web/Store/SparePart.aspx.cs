using System;
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

using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using System.Net;

namespace RmsAuto.Store.Web
{
    public partial class SparePartPage : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			SparePartPriceKey key = SparePartPriceKey.Parse( Request["ID"] );
			SparePartFranch part = SparePartsDac.Load( key );
			if( part != null )
			{
				_sparePartView.ActualPrice = part.GetFinalSalePrice(
					SiteContext.Current.CurrentClient.Profile.ClientGroup,
					SiteContext.Current.CurrentClient.Profile.PersonalMarkup
					);
				_sparePartView.DisplayDeliveryDaysMax = part.DisplayDeliveryDaysMax;
				_sparePartView.DisplayDeliveryDaysMin = part.DisplayDeliveryDaysMin;
				_sparePartView.Manufacturer = part.Manufacturer;
				_sparePartView.MinOrderQty = part.MinOrderQty;
				_sparePartView.PartName = part.PartName;
				_sparePartView.PartNumber = part.PartNumber;
				_sparePartView.PartDescription = part.PartDescription;
				_sparePartView.QtyInStock = OrderBO.GetQtyInStockString(part);
				_sparePartView.SupplierId = part.SupplierID;
				_sparePartView.WeightPhysical = part.WeightPhysical;
				_sparePartView.WeightVolume = part.WeightVolume;
				_sparePartView.DataBind();
			}
			else
			{
				//throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );
                //deas 16.03.2011 task3302
                //перенаправление пользователя на страницу поиска при не нахождении позиции
                //SparePartsDac 
                HttpContext.Current.Response.Clear();
                if ( PricingSearch.SearchSpareParts( key.PN, key.Mfr, false ).Count() > 0 )
                {
                    HttpContext.Current.Session["FindRedirectMess"] = "Данная деталь отсутствует у запрашиваемого поставщика. Вы можете выбрать её у других поставщиков.";
                    HttpContext.Current.Response.Redirect( "~/SearchSpareParts.aspx?mfr=" + key.Mfr + "&pn=" + key.PN + "&st=1", true );
                }
                else
                {
                    HttpContext.Current.Session["FindRedirectMess"] = "Данная деталь отсутствует у запрашиваемого производителя. Попробуйте подобрать аналоги у других производителей.";
                    HttpContext.Current.Response.Redirect( "~/SearchManufacturers.aspx?pn=" + key.PN + "&st=1", true );
                }
			}
		}
	}
}
