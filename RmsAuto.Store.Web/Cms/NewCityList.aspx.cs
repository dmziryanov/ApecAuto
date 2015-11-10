using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using System.Web.UI.HtmlControls;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web
{
    public partial class NewCityList : System.Web.UI.Page
    {
        List<RmsAuto.Store.Entities.Region> Regions = null;
        List<RmsAuto.Store.Cms.Entities.Shop> AllShops = new List<RmsAuto.Store.Cms.Entities.Shop>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: Отрефакторить названия с маленькой бувы локальноые переменные с большой пропертя
            List<RmsAuto.Store.Entities.Region> RegionsThroughCities = null;
            RmsAuto.Store.Entities.City[] Cities = null;

            //генерация разметки регионов
            var StoreCtx = new CmsDataContext();
            var CommonCtx = new RmsAuto.Store.Entities.dcCommonDataContext();

            //Извлекаем наборы данных в списки, так как LINQ to SQL не дает выполнять запросы к различным контекстам
            AllShops = StoreCtx.Shops.Where(x => x.ShopVisible).ToList();
            var Regs = CommonCtx.Regions.Select(x => x).ToList();
            var Cits = CommonCtx.Cities.Select(x => x).ToList();

            Regions = (from shops in AllShops
                       join regions in Regs on shops.RegionID equals regions.RegionID
                       where shops.ShopVisible
                       select regions).ToList();

            RegionsThroughCities = (from shops in AllShops
                                    join cities in Cits on shops.CityID equals cities.CityID
                                    join regions in Regs on cities.RegionID equals regions.RegionID
                                    where shops.ShopVisible
                                    select regions).ToList();

            Regions = Regions.Union(RegionsThroughCities).Distinct().OrderBy(x => x.RegionName).ToList();

            Cities = (from shops in AllShops
                      join cities in Cits on shops.CityID equals cities.CityID
                      orderby cities.Name
                      where shops.ShopVisible
                      select cities).Distinct().ToArray();

            Cities = Cities.Union(Cits.Where(x => x.Visible.HasValue && x.Visible.Value)).OrderBy(x => x.Name).Distinct().ToArray();//.ToList();


            foreach (RmsAuto.Store.Entities.Region r in Regions)
            {
                LinkButton c = new LinkButton() { Text = r.RegionName, ID = r.RegionID.ToString(), CssClass = "GrayTextStyle" };
                c.Click += (object se, EventArgs ev) =>
                Response.Redirect(UrlManager.MakeAbsoluteUrl("/About/Shops/Region/" + (se as LinkButton).ID + ".aspx"));
                HtmlGenericControl d = new HtmlGenericControl("div");
                d.Controls.Add(c); PanelRegionList.Controls.Add(d);
            }

            //генерация разметки городов
            if (Cities.Length == 0) return; //городов нет выходим

            Cities = Cities.OrderBy(c => c.Name).ToArray();
            var g = Cities.GroupBy(c => c.Name[0]);

            int gar = TableCities.Rows[0].Cells.Count, gar2 = Cities.Length + g.Count(),
                citiesInColumn = (gar2 % gar != 0) ? gar2 / gar + 1 : gar2 / gar,
                CurrentColumn = 0, CurrentRow = 0;

            foreach (var g2 in g)
            {
                if (CurrentRow != 0)
                {
                    TableCities.Rows[0].Cells[CurrentColumn].Controls.Add(new HtmlGenericControl("br"));
                    if (CurrentRow++ > citiesInColumn) { CurrentRow = 0; ++CurrentColumn; }
                }
                RmsAuto.Store.Entities.City[] ca = g2.ToArray();
                for (int i = 0; i < ca.Length; i++)
                    TableCities.Rows[0].Cells[CurrentColumn].Controls.Add(getCityDiv(i == 0, ca[i]));
                CurrentRow += ca.Length;
            }
        }
        HtmlGenericControl getCityDiv(bool fl, RmsAuto.Store.Entities.City city)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            Label l = new Label() { Width = 20, CssClass = "_CityFirstLetterStyle" };
            if (fl) l.Text = city.Name[0].ToString();
            div.Controls.Add(l);
            LinkButton c = new LinkButton() { Text = city.Name + " " };
            bool CityHasButNotRmsShop = (AllShops.FirstOrDefault(o => ((o.CityID == city.CityID) && (o.isRMS != true))) != null);
            c.Attributes.Add("class", CityHasButNotRmsShop ? "VipTextStyle" : "GrayTextStyle");
            c.PostBackUrl = CityHasButNotRmsShop ? UrlManager.MakeAbsoluteUrl("/About/Shops/City/" + city.CityID + ".aspx") :
            UrlManager.MakeAbsoluteUrl("/About/NewCityList/Form.aspx");
            div.Controls.Add(c);
            if (city.isNEW) div.Controls.Add(new Image() { ImageUrl = "/images/new_icon.png", ToolTip = "Открылся новый офис продаж", ImageAlign = ImageAlign.Bottom });
            return div;
        }
    }
}
