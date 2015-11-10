using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Data;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities.Helpers;
using RmsAuto.Common.Misc;
using System.Configuration;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public partial class TireSearch : System.Web.UI.UserControl
    {
        public int MaxSearchCount = 100;
        public int CurrentSearchCount = 0;
        public bool ExceedMaxresult = false;
       
    
        protected PagedDataSource PagedDataSource;

        protected bool IsRestricted
        {
            get { return SiteContext.Current.CurrentClient.Profile.IsRestricted; }
        }

        CurrencyRate GetCurrentCurrencyRate()
        {
            CurrencyRate res;
            if (SiteContext.Current.CurrencyCode != CurrencyRate.RurRate.CurrencyCode)
            {
                try
                {
                    res = AcctgRefCatalog.CurrencyRates[SiteContext.Current.CurrencyCode];
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
        

        public Control FindControlRecursive(Control top, string name)
        {
            Control ctrl = top.FindControl(name);
            if (ctrl != null)
                return ctrl;
            else
                foreach (Control control in top.Controls)
                    return FindControlRecursive(control, name);
            return null;
        }


        public void DoSearch(string Company, string Height, string Width, string Radius, string Season)
        {
            List<TireWithPrice> sourceList = null;

            using (CmsDataContext cms = new CmsDataContext())
            {
                try
                {
                    //Алгоритм фильтрации 1) сначала выбираем все
                    List<Tires> mQuery = null;
                    IEnumerable<Tires> query = query = cms.Tires.Select(x => x);
                    // 2)Выбираем только те, которые соответствуют названию производителя
                    
                    // 3)Выбираем только те, которые соответствуют заданной высоте
                    if (!String.IsNullOrEmpty(Height) && !Height.ToLower().Contains("все"))
                        query = query.Where(x => x.Height == Convert.ToDouble(Height));

                    // 4) и т.д.
                    if (!String.IsNullOrEmpty(Width) && !Width.ToLower().Contains("все"))
                        query = query.Where(x => x.Profile != null && x.Profile.ToString() == Width);

                    if (!String.IsNullOrEmpty(Radius) && !Radius.ToLower().Contains("все"))
                        query = query.Where(x => x.Radius != null && x.Radius.Contains(Radius));

                    if (!String.IsNullOrEmpty(Season) && !Season.ToLower().Contains("все"))
                        query = query.Where(x => x.Season != null && x.Season.ToString().ToUpper() == Season.ToUpper());

                    if (!String.IsNullOrEmpty(Company) && !Company.ToLower().Contains("все"))
                        //Проверяем нличие брэнда в метаданных и списке брендов он должен быть настроен видимым
                        query = query.Where(x => x.Manufacturer != null && x.Manufacturer.ToUpper().Contains(Company.ToUpper()) && cms.TireBrands.Where(y => y.IsVisible).Select(y => y.Name.ToUpper()).Contains(Company.ToUpper()));
                    else
                        query = query.Where(x => x.Manufacturer != null && cms.TireBrands.Where(y => y.IsVisible).Select(y => y.Name.ToUpper()).Contains(x.Manufacturer.ToUpper()));

                    //Конец фильтрации

                    mQuery = query.ToList();

                    sourceList = (from n in mQuery

                                  select new TireWithPrice
                                  {
                                      Manufacturer = n.Manufacturer,
                                      ModelName = n.ModelName,
                                      Profile = n.Profile,
                                      Radius = n.Radius,
                                      Height = n.Height,
                                      Season = n.Season,
                                      TireIndex = n.TireIndex,
                                      Price = 0,
                                      Width = n.Profile,
                                      TireNumber = n.TireNumber,
                                      ImageUrl = n.ImageUrl,
                                      Ref = String.Format(UrlManager.GetSearchSparePartsUrl(n.Manufacturer.ToUpper(), n.TireNumber))
                                  }).ToList();

                }
                catch
                { 
                }
                finally
                {
                    if (cms.Connection.State == System.Data.ConnectionState.Open)
                        cms.Connection.Close();
                }
            }

            using (StoreDataContext store = new StoreDataContext())
            {

                try
                {
                    List<TireWithPrice> TirePriceSearchResult = new List<TireWithPrice>();
                    ExceedMaxresult = false;
                    foreach (TireWithPrice item in sourceList)
                    {
                        List<SearchResultItem> list;
                        if (Cache.Get(item.Manufacturer + item.TireNumber) == null)
                        {
                            PartKey searchPartKey = new PartKey(item.Manufacturer, item.TireNumber);

                            SparePartItem[] parts = new SparePartItem[0];

							if (item.Manufacturer != null)
							{
								//TODO: Рассмотреть применение последнего параметра -- поиска аналогов
								// со слов Беляева если нет уточнения по параметрам, т.е. зима, размер и т.д., то "поиск аналогов" в отношении шин не имеет смысла,
								// в связи с этим рассмотреть возможность передачи false в нижележащий метод:
								parts = PricingSearch.SearchSpareParts(item.TireNumber, item.Manufacturer.ToUpper(), true);
							}

                            //пересчитать цены, подгрузить дополнительную информацию о деталях 
                            RmsAuto.Acctg.ClientGroup clientGroup = SiteContext.Current.CurrentClient.Profile.ClientGroup;
                            decimal personalMarkup = SiteContext.Current.CurrentClient.Profile.PersonalMarkup;
                            //var additionalInfos = RmsAuto.TechDoc.Facade.GetAdditionalInfo(new[] { searchPartKey }.Union(parts.Select(p => new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber))));
                            //var additionalInfosExt = RmsAuto.Store.Entities.Helpers.SearchHelper.GetAdditionalInfoExt(parts.Select(p => new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)));

                            //курс валюты
                            CurrencyRate currencyRate = GetCurrentCurrencyRate();

                            list = parts.Select(
                                p => new RmsAuto.Store.Web.Controls.SearchResultItem
                                {
                                    ItemType = p.ItemType,
                                    SparePart = p.SparePart,
                                    AdditionalInfo = null,/*additionalInfos.ContainsKey(new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber)) ? additionalInfos[new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber)] : null,*/
                                    AdditionalInfoExt = null, /*additionalInfosExt.ContainsKey(new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)) ?
                                        additionalInfosExt[new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)] : null,*/
                                    FinalSalePriceRUR = p.SparePart.GetFinalSalePrice(clientGroup, personalMarkup),
                                    FinalSalePrice = p.SparePart.GetFinalSalePrice(clientGroup, personalMarkup) / currencyRate.Rate,
                                    ShowPrices = true,
                                    ShowInfo = true
                                }).Where(p => p.FinalSalePriceRUR > 0).ToList(); //чтобы убрать позиции с нулевой ценой из результатов поиска


                            Cache.Insert(item.Manufacturer + item.TireNumber, list, null, DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CacheDuration"])), TimeSpan.Zero);
                        }
                        else
                        {
                            list = (List<SearchResultItem>)Cache.Get(item.Manufacturer + item.TireNumber);
                        }

                        if (TirePriceSearchResult.Count < MaxSearchCount)
                        {
                            foreach (var item2 in list)
                            {
                                TireWithPrice tmp = new TireWithPrice()
                                {
                                    Radius = item.Radius,
                                    Height = item.Height,
                                    Manufacturer = item.Manufacturer,
                                    ModelName = item.ModelName,
                                    Price = item2.FinalSalePriceRUR,
                                    Profile = item.Profile,
                                    Ref = item.Ref,
                                    Season = item.Season,
                                    TireIndex = item.TireIndex,
                                    TireNumber = item.TireNumber,
                                    Width = item.Width,
                                    SparePart = item2.SparePart,
                                    ImageUrl = item.ImageUrl,
                                   
                                };

                                TirePriceSearchResult.Add(tmp);
                                break;
                            }
                        }
                        else
                        {
                            ExceedMaxresult = true;
                            break;
                        }
                    }
                    
					//отсортировать результаты поиска
                    CurrentSearchCount = TirePriceSearchResult.Count;
                    PagedDataSource = new PagedDataSource();
                    PagedDataSource.DataSource = TirePriceSearchResult.OrderBy(x => x.Price).ToList();
                    PagedDataSource.AllowPaging = true;

                    int count = 0;
					if (!String.IsNullOrEmpty(Request.Params["PageSize"]) && int.TryParse(Request.Params["PageSize"], out count))
					{
						PagedDataSource.PageSize = Convert.ToInt32(Request.Params["PageSize"]);
					}
					if (PagedDataSource.PageSize == 0)
					{
						PagedDataSource.PageSize = 10;
					}

                    TirePagerControl.Visible = TirePriceSearchResult.Count / PagedDataSource.PageSize + 1 > 1;

                    //Установка значения пейджера
                    if (TirePriceSearchResult.Count % PagedDataSource.PageSize > 0)
                        TirePagerControl.MaxIndex = TirePriceSearchResult.Count / PagedDataSource.PageSize + 1;
                    else
                        TirePagerControl.MaxIndex = TirePriceSearchResult.Count / PagedDataSource.PageSize + 0;

                    if (TirePagerControl.CurrentIndex > 0)
                    {
                        PagedDataSource.CurrentPageIndex = TirePagerControl.CurrentIndex - 1;
                        rptSearchResults.DataSource = PagedDataSource;
                        rptSearchResults.DataBind();
                    }

                    if (TirePriceSearchResult.Count > 0)
                        rptSearchResults.Visible = true;
                    else
                    {
                        var labelResult = new Label();
                        labelResult.Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                        labelResult.Style.Add(HtmlTextWriterStyle.Color, "Green");
                        labelResult.Text = "Ничего не найдено, попробуйте изменить параметры поиска.";
                        _resultsPlaceHolder.Controls.Add(labelResult);
                        rptSearchResults.Visible = false;
                    }
                }
                catch(Exception ex)
                {
					Logger.WriteError("При поиске шин произошла ошибка.", EventLogerID.UnknownError, EventLogerCategory.UnknownCategory, ex);
				}
                finally
                {
					if (store.Connection.State == System.Data.ConnectionState.Open) { store.Connection.Close(); }
                }
            }              

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 0) { return; }

            var Company = Request.Params["mfr"];
            var Height = Request.Params["Height"];
            var Width = Request.Params["Width"];
            var Radius = Request.Params["Radius"];
            var Season = Request.Params["Season"];
            DoSearch(Company, Height, Width, Radius, Season);
        }

        ///Возможно понадобиться, если делать заказ в корзину с этой страницы
        //protected void rptSearchResults_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    //TODO: Надо вынести кнопку заказа и количества в контрол
        //    if (e.CommandName == "AddToCart")
        //    {
        //        var qty = int.Parse(((TextBox)e.Item.FindControl("_txtQty")).Text);
              
        //        var defaultOrderQty = int.Parse(((Label)e.Item.FindControl("_lblDefaultOrderQty")).Text);

        //        string error = null;
        //        if (qty < defaultOrderQty)
        //            error = "Количество не должно быть меньше числа деталей в комплекте";
        //        else if (qty % defaultOrderQty != 0)
        //        {
        //            error = "Количество должно быть кратным числу деталей в комплекте (" +
        //                defaultOrderQty
        //                .Progression(defaultOrderQty, 5)
        //                .Select(i => i.ToString())
        //                .Aggregate((acc, s) => acc + "," + s) + " и т.д.)";
        //        }

        //        if (error != null)
        //        {
        //            ShowMessage(error);
        //            return;
        //        }

        //        var key = SparePartPriceKey.Parse(((Label)e.Item.FindControl("_lblKey")).Text);
        //        // deas 27.04.2011 task3929 добавление в корзине флага отправить в заказ
        //        SiteContext.Current.CurrentClient.Cart.Add(key, qty, true);
        //    }
        //}

        //protected void rptSearchResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item ||
        //        e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        string valGroup = "QtyValGroup_" + e.Item.ItemIndex.ToString();
        //        //((ImageButton)e.Item.FindControl("_btnAddToCart")).ValidationGroup = 
        //        //((BaseValidator)e.Item.FindControl("_qtyReqValidator")).ValidationGroup = valGroup;               

        //        var part = ((TireWithPrice)e.Item.DataItem).SparePart;
        //        var partKey = new SparePartPriceKey(
        //            part.Manufacturer,
        //            part.PartNumber,
        //            part.SupplierID);
        //       // ((Label)e.Item.FindControl("_lblKey")).Text = partKey.ToString();

        //       // var txtQty = (TextBox)e.Item.FindControl("_txtQty");
        //        //var btnAddToCart = (ImageButton)e.Item.FindControl("_btnAddToCart");
        //       // var qtyPlaceHolder = (PlaceHolder)e.Item.FindControl("_qtyPlaceHolder");

        //     //   btnAddToCart.OnClientClick = string.Format("return validate_qty('{0}');", txtQty.ClientID);

        //        if (!SiteContext.Current.IsAnonymous &&
        //            SiteContext.Current.User.Role == SecurityRole.Manager)
        //        {
        //            if (SiteContext.Current.CurrentClient.IsGuest ||
        //               !((ManagerSiteContext)SiteContext.Current)
        //               .ClientDataSectionEnabled(ClientDataSection.Cart))
        //            {
        //       //         btnAddToCart.Visible = false;
        //               // qtyPlaceHolder.Visible = false;
        //            }
        //        }
        //    }
        //}

        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }

        private void InitializeComponent()
        {
            //this.rptSearchResults.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(rptSearchResults_ItemCommand);
        }
    }
}