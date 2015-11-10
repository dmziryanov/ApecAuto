using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Entities.Helpers;
using RmsAuto.Common.Data;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Configuration;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Controls
{
    public partial class BatterySearch : System.Web.UI.UserControl
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

        public void DoSearch(string Company, string Capacity, string Polarity, string Cleat)
        {
            List<Battery> mQuery = null;

            using (CmsDataContext cms = new CmsDataContext())
            {
                try
                {
                    //Алгоритм фильтрации 1) сначала выбираем все
                
                    IEnumerable<Battery> query = query = cms.Batteries.Select(x => x);
                    // 2)Выбираем только те, которые соответствуют названию производителя
                    
                    // 3)Выбираем только те, которые соответствуют заданной высоте
                    if (!String.IsNullOrEmpty(Capacity) && !Capacity.ToLower().Contains("все"))
                    {
                        var low  = Capacity.Split('-')[0];
                        var high = Capacity.Split('-')[1];
                        query = query.Where(x => x.Capacity >= Convert.ToDecimal(low) && x.Capacity <= Convert.ToDecimal(high));
                    }

                    // 4) и т.д.
                    if (!String.IsNullOrEmpty(Polarity) && !Polarity.ToLower().Contains("все"))
                        query = query.Where(x => x.polarity != null && x.polarity.ToLower().Contains(Polarity.ToLower()));

                    if (!String.IsNullOrEmpty(Cleat) && !Cleat.ToLower().Contains("все"))
                        query = query.Where(x => x.cleat != null && x.cleat.ToLower().Contains(Cleat.ToLower()));

                    if (!String.IsNullOrEmpty(Company) && !Company.ToLower().Contains("все"))
                        //Проверяем нличие брэнда в метаданных и списке брендов он должен быть настроен видимым
                        query = query.Where(x => x.Manufacturer != null && x.Manufacturer.ToUpper().Contains(Company.ToUpper()) && cms.BatteryBrands.Where(y => y.IsVisible).Select(y => y.Name.ToUpper()).Contains(Company.ToUpper()));
                    else
                        query = query.Where(x => x.Manufacturer != null && cms.BatteryBrands.Where(y => y.IsVisible).Select(y => y.Name.ToUpper()).Contains(x.Manufacturer.ToUpper()));

                    //Конец фильтрации

                    mQuery = query.ToList();
                }
                catch
                { }
                finally
                {
                   //Теперь закрываемся в фабрике
                    if (cms.Connection.State == System.Data.ConnectionState.Open)
                        cms.Connection.Close();
                }
            }

                try
                {
                    List<Battery> BatteryPriceSearchResult = new List<Battery>();
                    ExceedMaxresult = false;
                    
                    foreach (Battery item in mQuery)
                    {
                        List<SearchResultItem> list;
                        if (Cache.Get(item.Manufacturer + item.PartNumber) == null)
                        {
                            PartKey searchPartKey = new PartKey(item.Manufacturer, item.PartNumber);

                            SparePartItem[] parts = new SparePartItem[0];

							if (item.Manufacturer != null)
							{
								//TODO: Рассмотреть применение последнего параметра -- поиска аналогов
								parts = PricingSearch.SearchSpareParts(item.PartNumber, item.Manufacturer.ToUpper(), false);
							}

                            //пересчитать цены, подгрузить дополнительную информацию о деталях 
                            RmsAuto.Acctg.ClientGroup clientGroup = SiteContext.Current.CurrentClient.Profile.ClientGroup;
                            decimal personalMarkup = SiteContext.Current.CurrentClient.Profile.PersonalMarkup;
                            var additionalInfos = RmsAuto.TechDoc.Facade.GetAdditionalInfo(new[] { searchPartKey }.Union(parts.Select(p => new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber))));
                            // dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
                            var additionalInfosExt = RmsAuto.Store.Entities.Helpers.SearchHelper.GetAdditionalInfoExt( parts.Select(p => new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)));

                            //курс валюты
                            CurrencyRate currencyRate = GetCurrentCurrencyRate();


                            list = parts.Select(
                                p => new RmsAuto.Store.Web.Controls.SearchResultItem
                                {
                                    ItemType = p.ItemType,
                                    SparePart = p.SparePart,
                                    AdditionalInfo = additionalInfos.ContainsKey(new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber)) ? additionalInfos[new PartKey(p.SparePart.Manufacturer, p.SparePart.PartNumber)] : null,
                                    // dan 01.06.2011 task4253 Механизм отображения фотографий брака в результатах поиска.
                                    AdditionalInfoExt = additionalInfosExt.ContainsKey(new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)) ?
                                        additionalInfosExt[new SparePartKeyExt(p.SparePart.Manufacturer, p.SparePart.PartNumber, p.SparePart.SupplierID)] : null,
                                    FinalSalePriceRUR = p.SparePart.GetFinalSalePrice(clientGroup, personalMarkup),
                                    FinalSalePrice = p.SparePart.GetFinalSalePrice(clientGroup, personalMarkup) / currencyRate.Rate,

                                    ShowPrices = true,
                                    ShowInfo = true
                                }).Where(p => p.FinalSalePriceRUR > 0).ToList(); //dan 19.09.2011 добавил условие чтобы убрать позиции с нулевой ценой из результатов поиска


                            Cache.Insert(item.Manufacturer + item.PartNumber, list, null, DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CacheDuration"])), TimeSpan.Zero);
                        }
                        else
                        {
                            list = (List<SearchResultItem>)Cache.Get(item.Manufacturer + item.PartNumber);
                        }

                        if (BatteryPriceSearchResult.Count < MaxSearchCount)
                        {
                            foreach (var item2 in list)
                            {
                                Battery tmp = new Battery()
                                {
                                    Capacity = item.Capacity,
                                    Manufacturer = item.Manufacturer,
                                    ModelName = item.ModelName,
                                    Price = item2.FinalSalePriceRUR,
                                    cleat = item.cleat,
                                    Size = item.Size,
                                    polarity = item.polarity,
                                    PartNumber = item.PartNumber,
                                    SparePart = item2.SparePart,
                                    ImageUrl = item.ImageUrl,
                                    Ref = String.Format(UrlManager.GetSearchSparePartsUrl(item.Manufacturer.ToUpper(), item.PartNumber))
                                };

                                BatteryPriceSearchResult.Add(tmp);
                                
                                
                                if (BatteryPriceSearchResult.Count >= MaxSearchCount)
                                {
                                    ExceedMaxresult = true;
                                    break;
                                }

                                break; //Ищем только одно предложение
                            }

                        }
                        else
                        {
                            ExceedMaxresult = true;
                            break;
                        }
                    }
                    
					//отсортировать результаты поиска
                    CurrentSearchCount = BatteryPriceSearchResult.Count;
                    PagedDataSource = new PagedDataSource();
                    PagedDataSource.DataSource = BatteryPriceSearchResult.OrderBy(x => x.Price).ToList();
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

                    TirePagerControl.Visible = BatteryPriceSearchResult.Count / PagedDataSource.PageSize + 1 > 1;

                    //Установка значения пейджера
                    if (BatteryPriceSearchResult.Count % PagedDataSource.PageSize > 0)
                        TirePagerControl.MaxIndex = BatteryPriceSearchResult.Count / PagedDataSource.PageSize + 1;
                    else
                        TirePagerControl.MaxIndex = BatteryPriceSearchResult.Count / PagedDataSource.PageSize + 0;

                    if (TirePagerControl.CurrentIndex > 0)
                    {
                        PagedDataSource.CurrentPageIndex = TirePagerControl.CurrentIndex - 1;
                        rptSearchResults.DataSource = PagedDataSource;
                        rptSearchResults.DataBind();
                    }

                    if (BatteryPriceSearchResult.Count > 0)
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
                catch (Exception ex)
                {
                    Logger.WriteError(ex.Message, EventLogerID.BLException, EventLogerCategory.BLError);
				}
                finally
                {

                }

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 0) { return; }

            var Company = Request.Params["mfr"];
            var Cleat = Request.Params["cleat"];
            var polarity = Request.Params["polarity"];
            var Capacity = Request.Params["Capacity"];
            DoSearch(Company, Capacity, polarity, Cleat);
        }

        //Сейчас это отключено
        protected void rptSearchResults_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //TODO: Надо вынести кнопку заказа и количества в контрол
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
                // deas 27.04.2011 task3929 добавление в корзине флага отправить в заказ
                SiteContext.Current.CurrentClient.Cart.Add(key, qty, true);
            }
        }

        protected void rptSearchResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string valGroup = "QtyValGroup_" + e.Item.ItemIndex.ToString();
                //((ImageButton)e.Item.FindControl("_btnAddToCart")).ValidationGroup = 
                //((BaseValidator)e.Item.FindControl("_qtyReqValidator")).ValidationGroup = valGroup;               

                SparePartFranch part = (SparePartFranch)((Battery)e.Item.DataItem).SparePart;
                var partKey = new SparePartPriceKey(
                    part.Manufacturer,
                    part.PartNumber,
                    part.SupplierID);
                ((Label)e.Item.FindControl("_lblKey")).Text = partKey.ToString();

                var txtQty = (TextBox)e.Item.FindControl("_txtQty");
                var btnAddToCart = (ImageButton)e.Item.FindControl("_btnAddToCart");
                var qtyPlaceHolder = (PlaceHolder)e.Item.FindControl("_qtyPlaceHolder");

                btnAddToCart.OnClientClick = string.Format("return validate_qty('{0}');", txtQty.ClientID);

                if (!SiteContext.Current.IsAnonymous && SiteContext.Current.User.Role == SecurityRole.Manager)
                {
                    if (SiteContext.Current.CurrentClient.IsGuest ||
                       !((ManagerSiteContext)SiteContext.Current)
                       .ClientDataSectionEnabled(ClientDataSection.Cart))
                    {
                        btnAddToCart.Visible = false;
                       // qtyPlaceHolder.Visible = false;
                    }
                }
            }
        }

        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }

        private void InitializeComponent()
        {
            this.rptSearchResults.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(rptSearchResults_ItemCommand);
        }

     
    }

        
 }
    
