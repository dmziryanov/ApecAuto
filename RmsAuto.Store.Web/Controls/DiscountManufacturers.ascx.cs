using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
	public partial class DiscountManufacturers : System.Web.UI.UserControl
	{
		protected string GetSearchDiscountSparePartsUrl(string mfr)
		{
			return UrlManager.GetSearchDiscountSparePartsUrl(mfr);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			string[] brands = DiscountSparePartDac.GetDiscountSparePartBrands();

			if (brands.Length > 0)
			{
				_rptDiscountBrands.DataSource = brands;
				_rptDiscountBrands.DataBind();
			}
			else { _emptyLabel.Text = "Ничего не найдено"; _emptyLabel.Visible = true; _rptDiscountBrands.Visible = false; }
		}
	}
}