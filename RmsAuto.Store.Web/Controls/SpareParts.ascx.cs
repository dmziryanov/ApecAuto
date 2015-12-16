using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Data;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Entities.Helpers;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.Store.Web.Controls
{

    public class SearchResultItem
    {
        public SparePartItemType ItemType { get; set; }
        public SparePartFranch SparePart { get; set; }
        public AdditionalInfo AdditionalInfo { get; set; }
        public AdditionalInfoExt AdditionalInfoExt { get; set; } // dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
        /// <summary>
        /// true - деталь с собственных складов наличия
        /// </summary>
        public bool IsOwnStore { get; set; }
        public decimal FinalSalePriceRUR { get; set; }
        public decimal FinalSalePrice { get; set; }
        public decimal SalePrice1 { get; set; }
        public decimal SalePrice2 { get; set; }
        public decimal SalePrice3 { get; set; }

        public bool ShowInfo { get; set; }
        public bool ShowPrices { get; set; }
        public bool ShowSupplierStatistic
        {
            get
            {
                if (SparePart.BeforeTime != null && SparePart.OnTime != null && SparePart.Delay != null && SparePart.NonDelivery != null)
                    return true;
                else
                    return false;
            }
        }

        //индекс группы подсветки
        public int? SparePartGroupIndex { get; set; }
        public SparePartGroup SparePartGroup { get; set; }

    }

    public partial class SpareParts : System.Web.UI.UserControl
	{
		public bool SearchCounterParts
		{
			get { return Request[ UrlKeys.StoreAndTecdoc.SearchCounterparts ] == "1"; }
		}
		public string EnteredPartNumber
		{
			get { return Request[ UrlKeys.StoreAndTecdoc.EnteredPartNumber ] ?? ""; }
		}
		public string Manufacturer
		{
			get { return Request[ UrlKeys.StoreAndTecdoc.ManufacturerName ] ?? ""; }
		}
		public int? ExcludeSupplierID
		{
			get { return !string.IsNullOrEmpty( Request[ UrlKeys.StoreAndTecdoc.ExcludeSupplierID ] ) ? (int?)Convert.ToInt32( Request[ UrlKeys.StoreAndTecdoc.ExcludeSupplierID ] ) : null; }
		}
		protected bool IsManagerMode
		{
			get { return SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager; }
		}
		protected bool IsRestricted
		{
			get { return SiteContext.Current.CurrentClient.Profile.IsRestricted; }
		}
		protected bool IsGuest
		{
			get { return SiteContext.Current.CurrentClient.IsGuest; }
		}

        protected RmsAuto.Acctg.ClientGroup DiscountGroup1 { get { return DiscountGroupsConfiguration.Current.DiscountGroup1; } }
        protected RmsAuto.Acctg.ClientGroup DiscountGroup2 { get { return DiscountGroupsConfiguration.Current.DiscountGroup2; } }
        protected RmsAuto.Acctg.ClientGroup DiscountGroup3 { get { return DiscountGroupsConfiguration.Current.DiscountGroup3; } }
		protected string DiscountName1 { get { return DiscountGroupsConfiguration.Current.DiscountName1; } }
		protected string DiscountName2 { get { return DiscountGroupsConfiguration.Current.DiscountName2; } }
		protected string DiscountName3 { get { return DiscountGroupsConfiguration.Current.DiscountName3; } }

	    protected PagedDataSource PagedDataSource;

		protected string GetSparePartDetailsUrl( object objSparePart )
		{
			var sparePart = (SparePartFranch)objSparePart;
			SparePartPriceKey key = new SparePartPriceKey(
				sparePart.Manufacturer,
				sparePart.PartNumber,
				sparePart.SupplierID );
			return UrlManager.GetSparePartDetailsUrl( key.ToUrlString() );
		}

        protected string GetSupplierStatisticUrl(object objSparePart)
        {
            var sparePart = (SparePartFranch)objSparePart;
            SparePartPriceKey key = new SparePartPriceKey(
                sparePart.Manufacturer,
                sparePart.PartNumber,
                sparePart.SupplierID);
            return UrlManager.GetSupplierStatisticUrl(key.ToString());
        }

        protected string GetSellerMessageUrl(string InternalFranchName)
        {
            return UrlManager.GetSellerMessageUrl(InternalFranchName);
        }

		protected void Page_Init( object sender, EventArgs e )
		{
			if( !IsPostBack )
			{
				InitCurrencyDD();
			}
		}

		private void InitCurrencyDD()
		{
			var rates = new List<CurrencyRate>();
			rates.Add( CurrencyRate.RurRate );
			try
			{
				rates.AddRange( AcctgRefCatalog.CurrencyRates.Items );
			}
			catch { }

			var currentCode = SiteContext.Current.CurrencyCode;

			rates.ForEach(
				rate =>
				_currencyBox.Items.Add( new ListItem()
				{
					Value = rate.CurrencyCode,
					Text = rate.CurrencyName,
					Selected = rate.CurrencyCode == currentCode
				} ) );
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			//добавление товара в корзину
			if( SiteContext.Current.CurrentClient.Cart != null && !IsRestricted )
			{
				var queryArgs = new NameValueCollection( Request.QueryString );

				string partKey = queryArgs.GetAndRemove( "p" );
				string qty = queryArgs.GetAndRemove( "q" );

				if( !string.IsNullOrEmpty( partKey ) && !string.IsNullOrEmpty( qty ) )
				{
                    // deas 27.04.2011 task3929 добавление в корзине флага отправить в заказ
					SiteContext.Current.CurrentClient.Cart.Add(
						SparePartPriceKey.Parse(partKey),
						Convert.ToInt32( qty), true );
					Response.Redirect( Request.Url.AbsolutePath + "?" + queryArgs.ToWwwQueryString() );
				}
			}
            //deas 16.03.2011 task3302
            //вывод сообщения пользователю
            if ( Session["FindRedirectMess"] != null )
            {
                ShowMessage( Session["FindRedirectMess"].ToString() );
                Session["FindRedirectMess"] = null;
            }
		}

		public void _currencyBox_OnSelectedIndexChanged( object sender, EventArgs e )
		{
			DropDownList current = (DropDownList)sender;
			SiteContext.Current.CurrencyCode = current.SelectedValue;
		}

		public void Page_PreRender( object sender, EventArgs e )
		{
            //if (!IsPostBack)    //Для того, чтобы при добавлении позиции в корзину поиск не производился повторно
			//if(HttpContext.Current.Request.RequestType != "POST") //Поменяли чтобы при добавлении в корзину поиск не производился повторно и при этом работал пейджинг
            {
                if (!IsGuest)
                {
                    TextItemControl_2.Visible = true;
                    Label_2.Visible = true;
                }

                _guestPriceHint_TextItemControl.Visible = _discountNotes_TextItemControl.Visible = false;
                _callToManagerHint_TextItemControl.Visible = false;

                string partNumber = PricingSearch.NormalizePartNumber(EnteredPartNumber);
                if (!String.IsNullOrEmpty(partNumber) &&
                    !String.IsNullOrEmpty(Manufacturer))
                {
					IEnumerable<SearchResultItem> list;
					//if (Cache.Get(partNumber+Manufacturer) == null)
					//{
						SearchSparePartsLogDac.AddLog(DateTime.Now, partNumber, Manufacturer, Request.UserHostAddress);


						PartKey searchPartKey = new PartKey(Manufacturer, partNumber);
						var parts = PricingSearch.SearchSpareParts(partNumber, Manufacturer, SearchCounterParts);

						//исключить запрошенный артикул, если он поставляется указанным поставщиком
						if (ExcludeSupplierID != null)
						{
							parts = parts.Where(p => !(string.Compare(p.SparePart.PartNumber, partNumber, true) == 0 && string.Compare(p.SparePart.Manufacturer, Manufacturer, true) == 0 && p.SparePart.SupplierID == ExcludeSupplierID)).ToArray();
						}

						_searchCodeLabel.Text = Server.HtmlEncode(string.Format("{0} ({1})", partNumber, Manufacturer));
						_countLabel.Text = parts.Length.ToString();
						_guestPriceHint_TextItemControl.Visible = _discountNotes_TextItemControl.Visible = IsGuest && parts.Length > 0;
						_callToManagerHint_TextItemControl.Visible = parts.Length == 0;

						//пересчитать цены, подгрузить дополнительную информацию о деталях 
                        RmsAuto.Acctg.ClientGroup clientGroup = SiteContext.Current.CurrentClient.Profile.ClientGroup;
						decimal personalMarkup = SiteContext.Current.CurrentClient.Profile.PersonalMarkup;
						var additionalInfos = RmsAuto.TechDoc.Facade.GetAdditionalInfo(new[] { searchPartKey }.Union(parts.Select(p => new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber))));
						// dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
						var additionalInfosExt = RmsAuto.Store.Entities.Helpers.SearchHelper.GetAdditionalInfoExt(parts.Select(p => new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)));

						//курс валюты
						CurrencyRate currencyRate = GetCurrentCurrencyRate();

						//получаем список "собственных складов наличия"
						List<int> ownStores = StoreRefCatalog.RefOwnStores.Items.Select(x => x.SupplierID).ToList(); ;//OrderBO.OwnStoresForSearchResults;
						/*var*/ list = parts.Select(
							p => new SearchResultItem
							{
								ItemType = p.ItemType,
								//выставляем признак относится ли деталь к собственным складам наличия
								IsOwnStore = ownStores.Contains(p.SparePart.SupplierID),
								SparePart = p.SparePart,
								AdditionalInfo = additionalInfos.ContainsKey(new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber)) ? additionalInfos[new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber)] : null,
								// dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
								AdditionalInfoExt = additionalInfosExt.ContainsKey(new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)) ?
									additionalInfosExt[new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)] : null,
								FinalSalePriceRUR = p.SparePart.GetFinalSalePrice(clientGroup, personalMarkup),
								FinalSalePrice = p.SparePart.GetFinalSalePrice(clientGroup, personalMarkup) / currencyRate.Rate,
								SalePrice1 = p.SparePart.GetFinalSalePrice(DiscountGroup1, personalMarkup) / currencyRate.Rate,
								SalePrice2 = p.SparePart.GetFinalSalePrice(DiscountGroup2, personalMarkup) / currencyRate.Rate,
								SalePrice3 = p.SparePart.GetFinalSalePrice(DiscountGroup3, personalMarkup) / currencyRate.Rate,
								ShowPrices = true,
								ShowInfo = true
							}).Where(p => p.FinalSalePriceRUR > 0); //dan 19.09.2011 добавил условие чтобы убрать позиции с нулевой ценой из результатов поиска

						//добавить фиктивный запрошенный артикул в блок собственных складов наличия
						if (!list.Any(p => p.ItemType == SparePartItemType.Exact && p.IsOwnStore))
						{
                            AdditionalInfo additionalInfo = additionalInfos.ContainsKey(searchPartKey) ? additionalInfos[searchPartKey] : null;

							list = list.Union(new[]{
							new SearchResultItem
							{
								ItemType = SparePartItemType.Exact,
								IsOwnStore = true,
								SparePart = new SparePartFranch { 
									Manufacturer=searchPartKey.Manufacturer, 
									PartNumber=searchPartKey.PartNumber,
									PartDescription = additionalInfo!=null ? additionalInfo.PartDescription : ""
								},
								AdditionalInfo = additionalInfo,
								ShowPrices = false,
								ShowInfo = additionalInfo!=null,
								FinalSalePrice = -1 //по данной "фиктивной" цене, в следующем блоке мы будем понимать что в блок собственных складов наличия уже добавлен фиктивный артикул
							}
						});
						}
						
                    //добавить фиктивный запрошенный артикул (если запрошенного артикула нет среди собственных складов наличия)
						if (!list.Any(p => p.ItemType == SparePartItemType.Exact && p.FinalSalePrice != -1))
						{
                            AdditionalInfo additionalInfo = /*additionalInfos.ContainsKey(searchPartKey) ? additionalInfos[searchPartKey] : */ null;

							list = list.Union(new[]{
							new SearchResultItem
							{
								ItemType = SparePartItemType.Exact,
								SparePart = new SparePartFranch { 
									Manufacturer=searchPartKey.Manufacturer, 
									PartNumber=searchPartKey.PartNumber,
									PartDescription = additionalInfo!=null ? additionalInfo.PartDescription : ""
								},
								AdditionalInfo = additionalInfo,
								ShowPrices = false,
								ShowInfo = additionalInfo!=null
							}
							});
						}
						//Cache.Insert(partNumber + Manufacturer, list,
						//    null,
						//    DateTime.UtcNow.AddMinutes(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SearchResultsCachDuration"])),
						//    TimeSpan.Zero);
					//}
					//else
					//{
						//list = (IEnumerable<SearchResultItem>)Cache.Get(partNumber+Manufacturer);
						
						_searchCodeLabel.Text = Server.HtmlEncode(string.Format("{0} ({1})", partNumber, Manufacturer));
						int partsCount = list.Count() - list.Where(l => l.ItemType == SparePartItemType.Exact && l.FinalSalePrice <= 0).Count();
						_countLabel.Text = partsCount.ToString();
						_guestPriceHint_TextItemControl.Visible = _discountNotes_TextItemControl.Visible = IsGuest && partsCount > 0;
						_callToManagerHint_TextItemControl.Visible = partsCount == 0;
					//}

                    //отсортировать результаты поиска
                    var orderedList = SortSearchResults(list, _sortOptionsBox.SelectedValue);

                    //подгрузить группы подсветки
                    var groups = SparePartGroupsDac.GetSparePartGroups().ToDictionary(g => g.SparePartGroupID);
                    var usedGroups = new List<SparePartGroup>();
                    foreach (var item in orderedList)
                    {
                        if (item.SparePart.SparePartGroupID.HasValue && groups.ContainsKey(item.SparePart.SparePartGroupID.Value))
                        {
                            var group = groups[item.SparePart.SparePartGroupID.Value];
                            int index = usedGroups.IndexOf(group);
                            if (index < 0)
                            {
                                index = usedGroups.Count;
                                usedGroups.Add(group);
                            }
                            item.SparePartGroupIndex = index;
                            item.SparePartGroup = usedGroups[index];
                        }
                    }

                    PagedDataSource = new PagedDataSource();
                    PagedDataSource.DataSource = orderedList;
                    PagedDataSource.AllowPaging = true;
                    PagedDataSource.PageSize = 100;

					////Номер страницы можно передавать, например, методом GET. Отображение нужной страницы выглядит так:

					//try
					//{
					//    Int32? curIndex = !string.IsNullOrEmpty(Request.QueryString.Get("pageindex")) ? (int?)Convert.ToInt32(Request.QueryString.Get("pageindex")) : null;

					//    if (curIndex != null)
					//        if ((int)curIndex >= 0 || (int)curIndex <= PagedDataSource.DataSourceCount)
					//            PagedDataSource.CurrentPageIndex = (int)curIndex;
					//        else
					//            PagedDataSource.CurrentPageIndex = 0;
					//}
					//catch (Exception)
					//{
					//    // строка в неверном формате
					//}

					//Установка значения пейджера
					_searchResultPager.Visible = orderedList.Length / PagedDataSource.PageSize + 1 > 1;
					if (orderedList.Length % PagedDataSource.PageSize > 0)
					{
						_searchResultPager.MaxIndex = orderedList.Length / PagedDataSource.PageSize + 1;
					}
					else
					{
						_searchResultPager.MaxIndex = orderedList.Length / PagedDataSource.PageSize + 0;
					}
					if (_searchResultPager.CurrentIndex > 0)
					{
						PagedDataSource.CurrentPageIndex = _searchResultPager.CurrentIndex - 1;
						_partsRepeater.DataSource = PagedDataSource;
						_partsRepeater.DataBind();
					}
					
                    //прибиндить результаты
					//_partsRepeater.DataSource = PagedDataSource;
					//_partsRepeater.DataBind();

                    //прибиндить легенду групп подсветки
                    _usedGroupsRepeater.DataSource = usedGroups;
                    _usedGroupsRepeater.DataBind();
                }
            }
		}

        [Obsolete]
		protected String GetPagingLinks()
        {
            Int32 countPages = (int)Math.Ceiling((double)PagedDataSource.DataSourceCount / (double)PagedDataSource.PageSize);

            if (countPages > 1)
            {
                List<String> links = new List<String>();

                String rUrl = Request.RawUrl;
                if (!rUrl.Contains("?"))
                    rUrl += "?";
                int start = rUrl.LastIndexOf("&pageindex");
                if (start > 0)
                {
                    rUrl = rUrl.Remove(start, rUrl.Length - start);
                }

                String cText = "";
                String cUrl = "";

                for (int i = 1; i <= countPages; i++)
                {
                    cText = i.ToString();
                    cUrl = rUrl + "&pageindex=" + (i-1).ToString();
                    links.Add(String.Format("<a href=\"{0}\">{1}</a>&#160;", cUrl, cText));
                }

                String rez = "";

                foreach (var l in links)
                {
                    rez += l;
                }

                return rez;
            }
            return "";
        }

	    SearchResultItem[] SortSearchResults( IEnumerable<SearchResultItem> list, string sortOption )
		{
			//старый вариант до "собственных складов наличия"
            //var orderedList = list.OrderBy( p => p.ItemType );
            var orderedList = list.OrderByDescending(p => p.IsOwnStore).ThenBy(p => p.ItemType);
			switch( sortOption )
			{
				case "FinalSalePriceRUR":
					orderedList = orderedList.ThenBy( p => p.FinalSalePriceRUR );
					break;
				case "FinalSalePriceRUR desc":
					orderedList = orderedList.ThenByDescending( p => p.FinalSalePriceRUR );
					break;
				case "Manufacturer,PartNumber":
					orderedList = orderedList.ThenBy( p => p.SparePart.Manufacturer ).ThenBy( p => p.SparePart.PartNumber );
					break;
				case "PartNumber":
					orderedList = orderedList.ThenBy( p => p.SparePart.PartNumber );
					break;
				case "PartDescription":
					orderedList = orderedList.ThenBy( p => p.SparePart.PartDescription );
					break;
				case "QtyInStock":
					orderedList = orderedList.ThenBy( p => p.SparePart.QtyInStock );
					break;
				case "DeliveryDaysMin":
					orderedList = orderedList.ThenBy( p => p.SparePart.DeliveryDaysMin );
					break;
				case "FinalSalePriceRUR,DeliveryDaysMin":
					orderedList = orderedList.ThenBy( p => p.FinalSalePriceRUR ).ThenBy( p => p.SparePart.DeliveryDaysMin );
					break;
                default: orderedList = orderedList.ThenBy(p => p.FinalSalePriceRUR).ThenBy(p => p.SparePart.DeliveryDaysMin);
                    break;
					//throw new Exception( "Unknown sort order: " + _sortOptionsBox.SelectedValue );
			}
			return orderedList.ToArray();
		}

		CurrencyRate GetCurrentCurrencyRate()
		{
			CurrencyRate res;
			if( SiteContext.Current.CurrencyCode != CurrencyRate.RurRate.CurrencyCode )
			{
				try
				{
					res = AcctgRefCatalog.CurrencyRates[ SiteContext.Current.CurrencyCode ];
				}
				catch
				{
					res = CurrencyRate.RurRate;
					SiteContext.Current.CurrencyCode = CurrencyRate.RurRate.CurrencyCode;
				}
			}
			else
			{
				res = CurrencyRate.RurRate;
			}
			return res;
		}

		protected void _partsRepeater_ItemCommand( object source, RepeaterCommandEventArgs e )
		{
			if( e.CommandName == "AddToCart" )
			{
				var qty = int.Parse( ( (TextBox)e.Item.FindControl( "_txtQty" ) ).Text );
				var defaultOrderQty = int.Parse( ( (Label)e.Item.FindControl( "_lblDefaultOrderQty" ) ).Text );

				string error = null;
				if( qty < defaultOrderQty )
                    error = "Ordered quantity cannot be less than MPQ";
				else if( qty % defaultOrderQty != 0 )
				{
                    error = "The quantity shall be divisible by quantity is sets (" +
						defaultOrderQty
						.Progression( defaultOrderQty, 5 )
						.Select( i => i.ToString() )
						.Aggregate( ( acc, s ) => acc + "," + s ) + " etc.)";
				}

				if( error != null )
				{
					ShowMessage( error );
					return;
				}

				var key = SparePartPriceKey.Parse( ( (Label)e.Item.FindControl( "_lblKey" ) ).Text );
                // deas 27.04.2011 task3929 добавление в корзине флага отправить в заказ
				SiteContext.Current.CurrentClient.Cart.Add( key, qty, true );
			}
		}

		protected void _partsRepeater_ItemDataBound( object sender, RepeaterItemEventArgs e )
		{
			if( e.Item.ItemType == ListItemType.Item ||
				e.Item.ItemType == ListItemType.AlternatingItem )
			{
				string valGroup = "QtyValGroup_" + e.Item.ItemIndex.ToString();
				//((ImageButton)e.Item.FindControl("_btnAddToCart")).ValidationGroup = 
				//((BaseValidator)e.Item.FindControl("_qtyReqValidator")).ValidationGroup = valGroup;               

				var part = ( (SearchResultItem)e.Item.DataItem ).SparePart;
				var partKey = new SparePartPriceKey(
					part.Manufacturer,
					part.PartNumber,
					part.SupplierID );
				( (Label)e.Item.FindControl( "_lblKey" ) ).Text = partKey.ToString();

				var txtQty = (TextBox)e.Item.FindControl( "_txtQty" );
				var btnAddToCart = (LinkButton)e.Item.FindControl( "_btnAddToCart" );
				var qtyPlaceHolder = (PlaceHolder)e.Item.FindControl( "_qtyPlaceHolder" );

				btnAddToCart.OnClientClick = string.Format( "return validate_qty('{0}');", txtQty.ClientID );

				if( !SiteContext.Current.IsAnonymous &&
					SiteContext.Current.User.Role == SecurityRole.Manager )
				{
					if( SiteContext.Current.CurrentClient.IsGuest ||
					   !( (ManagerSiteContext)SiteContext.Current )
					   .ClientDataSectionEnabled( ClientDataSection.Cart ) )
					{
						btnAddToCart.Visible = false;
						qtyPlaceHolder.Visible = false;
					}
				}
			}
		}

		private void ShowMessage( string message )
		{
			Page.ClientScript.RegisterStartupScript(
				this.GetType(),
				"__messageBox",
				"<script type='text/javascript'>alert('" + message + "');</script>" );
		}

        protected Boolean GetInfoStatVisibility(SearchResultItem dataItem)
        {
            if (dataItem.ShowInfo == true || dataItem.ShowSupplierStatistic == true)
                return true;
            else
                return false;
        }
	}
}