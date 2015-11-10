using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Entities.Helpers;

namespace RmsAuto.Store.Web.Controls
{
	public class SearchDiscountResultItem
	{
		public DiscountSparePart SparePart { get; set; }
		public AdditionalInfoExt AdditionalInfoExt { get; set; }
		public decimal FinalSalePrice { get; set; }
	}

	public partial class DiscountSpareParts : System.Web.UI.UserControl
	{
		protected bool IsRestricted
		{
			get { return SiteContext.Current.CurrentClient.Profile.IsRestricted; }
		}

		protected string CurrentBrand
		{
			get { return Request.QueryString[UrlKeys.StoreAndTecdoc.ManufacturerName]; }
		}

		private int PageSize { get { return 100; } }
		private int CommonCount
		{
			get { return DiscountSparePartDac.GetDiscountSparePartCount(CurrentBrand); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			//добавление товара в корзину
			if (SiteContext.Current.CurrentClient.Cart != null && !IsRestricted)
			{
				var queryArgs = new NameValueCollection(Request.QueryString);

				string partKey = queryArgs.GetAndRemove("p");
				string qty = queryArgs.GetAndRemove("q");

				if (!string.IsNullOrEmpty(partKey) && !string.IsNullOrEmpty(qty))
				{
					SiteContext.Current.CurrentClient.Cart.Add(
						SparePartPriceKey.Parse(partKey),
						Convert.ToInt32(qty), true);
					Response.Redirect(Request.Url.AbsolutePath + "?" + queryArgs.ToWwwQueryString());
				}
			}
		}

		public void Page_PreRender(object sender, EventArgs e)
		{
			//устанавливаем значения пейджера
			int count = CommonCount;
			_searchResultPager.Visible = count / PageSize + 1 > 1;
			if (count % PageSize > 0)
			{
				_searchResultPager.MaxIndex = count / PageSize + 1;
			}
			else
			{
				_searchResultPager.MaxIndex = count / PageSize;
			}
			if (_searchResultPager.CurrentIndex > 0)
			{
				_partsRepeater.DataSource = GetDataSource(_searchResultPager.CurrentIndex);
				_partsRepeater.DataBind();
			}
		}

		protected void _partsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "AddToCart")
			{
				var qty = int.Parse(((TextBox)e.Item.FindControl("_txtQty")).Text);
				var defaultOrderQty = int.Parse(((Label)e.Item.FindControl("_lblDefaultOrderQty")).Text);

				string error = null;
				if (qty < defaultOrderQty)
					error = "Количество не должно быть меньше числа деталей в комплекте";
				else if (qty % defaultOrderQty != 0)
				{
					error = "Количество должно быть кратным числу деталей в комплекте (" +
						defaultOrderQty
						.Progression(defaultOrderQty, 5)
						.Select(i => i.ToString())
						.Aggregate((acc, s) => acc + "," + s) + " и т.д.)";
				}

				if (error != null)
				{
					ShowMessage(error);
					return;
				}

				var key = SparePartPriceKey.Parse(((Label)e.Item.FindControl("_lblKey")).Text);
				SiteContext.Current.CurrentClient.Cart.Add(key, qty, true);
			}
		}

		protected void _partsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item ||
				e.Item.ItemType == ListItemType.AlternatingItem)
			{
				string valGroup = "QtyValGroup_" + e.Item.ItemIndex.ToString();
				//((ImageButton)e.Item.FindControl("_btnAddToCart")).ValidationGroup = 
				//((BaseValidator)e.Item.FindControl("_qtyReqValidator")).ValidationGroup = valGroup;

				var part = ((SearchDiscountResultItem)e.Item.DataItem).SparePart;
				var partKey = new SparePartPriceKey(
					part.Manufacturer,
					part.PartNumber,
					part.SupplierID);
				((Label)e.Item.FindControl("_lblKey")).Text = partKey.ToString();

				var txtQty = (TextBox)e.Item.FindControl("_txtQty");
				var btnAddToCart = (ImageButton)e.Item.FindControl("_btnAddToCart");
				var qtyPlaceHolder = (PlaceHolder)e.Item.FindControl("_qtyPlaceHolder");

				btnAddToCart.OnClientClick = string.Format("return validate_qty('{0}');", txtQty.ClientID);

				if (!SiteContext.Current.IsAnonymous &&
					SiteContext.Current.User.Role == SecurityRole.Manager)
				{
					if (SiteContext.Current.CurrentClient.IsGuest ||
					   !((ManagerSiteContext)SiteContext.Current)
					   .ClientDataSectionEnabled(ClientDataSection.Cart))
					{
						btnAddToCart.Visible = false;
						qtyPlaceHolder.Visible = false;
					}
				}
			}
		}

		protected string GetOldPriceString(decimal currentPrice, int supplierID)
		{
			switch(supplierID)
			{
				case 1212:
					return string.Format("{0:### ### ##0.00}", currentPrice / 0.8m);
				case 1215:
					return string.Format("{0:### ### ##0.00}", currentPrice / 0.5m);
			}
			return string.Empty;
		}

		private void ShowMessage(string message)
		{
			Page.ClientScript.RegisterStartupScript(
				this.GetType(),
				"__messageBox",
				"<script type='text/javascript'>alert('" + message + "');</script>");
		}

		private IEnumerable<SearchDiscountResultItem> GetDataSource(int pageIndex)
		{
			var discountParts =
					DiscountSparePartDac.GetDiscountSparePart(CurrentBrand, (pageIndex - 1) * PageSize, PageSize);

			//пересчитываем цены, подгружаем дополнительную информацию
            RmsAuto.Acctg.ClientGroup clientGroup = SiteContext.Current.CurrentClient.Profile.ClientGroup;
			decimal personalMarkup = SiteContext.Current.CurrentClient.Profile.PersonalMarkup;
			var additionalInfosExt = RmsAuto.Store.Entities.Helpers.SearchHelper.GetAdditionalInfoExt(discountParts.Select(p => new SparePartKeyExt(p.Manufacturer, p.PartNumber, p.SupplierID)));

			return discountParts.Select(
				p => new SearchDiscountResultItem
				{
					SparePart = p,
					AdditionalInfoExt = additionalInfosExt.ContainsKey(new SparePartKeyExt(p.Manufacturer, p.PartNumber, p.SupplierID)) ?
									additionalInfosExt[new SparePartKeyExt(p.Manufacturer, p.PartNumber, p.SupplierID)] : null,
					FinalSalePrice = p.GetFinalSalePrice(clientGroup, personalMarkup)
				});
		}
	}
}