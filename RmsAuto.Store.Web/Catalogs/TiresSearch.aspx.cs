using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web
{
    public partial class TiresSearch : LocalizablePage
    {
        private const int numOfBrandColumns = 4; //Количество колонок выравнивания брендов

		protected void Page_PreInit(object sender, EventArgs e)
		{
			if (SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager)
			{
				Page.MasterPageFile = "~/Manager/Manager.Master";
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			PriceWarningText.Visible = SiteContext.Current.CurrentClient.IsGuest;
			this.TireSearchControl.MaxSearchCount = 100;
			this.QuantityWarningText.Text = "&mdash; результатов поиска слишком много, выведено " + TireSearchControl.MaxSearchCount.ToString() + " товаров, пожалуйста уточните параметры поиска. ";
			this.PreRender += new System.EventHandler(this.TiresSearch_PreRender);
			List<TireBrand> cmsTireBrandsList;
			using (CmsDataContext cms = new CmsDataContext())
			{
				cmsTireBrandsList = cms.TireBrands.Where(x => x.IsVisible).ToList();
				companies.DataSource = cmsTireBrandsList;
				companies.DataTextField = "Name";
				companies.DataBind();
				companies.Items.Insert(0, "Все производители");
			}

			int RowCount = 0;
			if (cmsTireBrandsList.Count % numOfBrandColumns > 0)
				RowCount = cmsTireBrandsList.Count / numOfBrandColumns + 1;
			else
				RowCount = cmsTireBrandsList.Count / numOfBrandColumns;
			int curIndex = 0;
			for (int i = 0; i < RowCount; i++)
			{
				TableRow trLogo = new TableRow();
				BrandsTable.Rows.Add(trLogo);
				TableRow trText = new TableRow();
				BrandsTable.Rows.Add(trText);
				for (int j = 0; j < numOfBrandColumns; j++)
				{
					if (curIndex == cmsTireBrandsList.Count) { break; }
					TableCell tcLogo = new TableCell();
					trLogo.Cells.Add(tcLogo);
					Image im = new Image();
					im.ImageUrl = UrlManager.GetFileUrl(cmsTireBrandsList[curIndex].ImageId.Value) + "?r=rms";
					im.CssClass = "imgCat";
					im.AlternateText = cmsTireBrandsList[curIndex].Name;

					tcLogo.Controls.Add(im);

					TableCell tcText = new TableCell();
					trText.Cells.Add(tcText);
					tcText.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
					tcText.Style.Add(HtmlTextWriterStyle.MarginBottom, "10");
					tcText.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
					tcText.Style.Add(HtmlTextWriterStyle.Height, "30px");

					Label btn = new Label();
					btn.Text = cmsTireBrandsList[curIndex].Name;
					btn.CssClass = "imgCat";
					tcText.Controls.Add(btn);
					curIndex++;
				}
			}
		}

        private void TiresSearch_PreRender(object sender, EventArgs e)
        {
            Quantity.Text = TireSearchControl.CurrentSearchCount.ToString();
            QuantityWarningText.Visible = TireSearchControl.ExceedMaxresult;
            QuantityText.Visible = !TireSearchControl.ExceedMaxresult;
            Quantity.Visible = !TireSearchControl.ExceedMaxresult;
            Elements_Count.Visible = TireSearchControl.CurrentSearchCount > 0;
            info_block.Visible = TireSearchControl.CurrentSearchCount > 0;
        }
 
		#region +++ Парсинг html (на данный момент не используется +++
		/* •——————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————•
           | protected List<Tire> ParseHtml(string res)                                                                                   |
           | {                                                                                                                            |
           |     HtmlDocument html = new HtmlDocument();                                                                                  |
           |     html.LoadHtml(res);                                                                                                      |
           |     var table = html.DocumentNode.ChildNodes.Where(x => x.Attributes.Any(y => y.Value == "tiressearch")).FirstOrDefault();   |
           |     var tablebody = table.ChildNodes.Where(x => x.Name == "tbody").FirstOrDefault();                                         |
           |                                                                                                                              |
           |     var l = new List<Tire>();                                                                                                |
           |     var trs = tablebody.ChildNodes.Where(x => x.Name == "tr");                                                               |
           |     int j = 0;                                                                                                               |
           |     foreach (var tr in trs)                                                                                                  |
           |     {                                                                                                                        |
           |       var tds = tr.ChildNodes.Where(x => x.Name == "td");                                                                    |
           |       Tire Tire = new Tire();                                                                                                |
           |       int i = 0;                                                                                                             |
           |       foreach (var td in tds)                                                                                                |
           |       {                                                                                                                      |
           |           i++;                                                                                                               |
           |           switch (i)                                                                                                         |
           |           {                                                                                                                  |
           |               case 1:                                                                                                        |
           |                   Tire.Radius = td.InnerText;                                                                                |
           |                   if (td.InnerText.Contains("&nbsp") || td.InnerText == "")                                                  |
           |                   { Tire.Radius = l[j-1].Radius; }                                                                           |
           |               break;                                                                                                         |
           |               case 2: Tire.WidthAndProfile = td.InnerText;                                                                   |
           |               break;                                                                                                         |
           |               case 3: Tire.Index = td.InnerText;                                                                             |
           |               break;                                                                                                         |
           |               case 4: Tire.Articul = td.InnerText;                                                                           |
           |               break;                                                                                                         |
           |               case 5: Tire.Weather = td.InnerText;                                                                           |
           |               break;                                                                                                         |
           |           }                                                                                                                  |
           |       }                                                                                                                      |
           |       l.Add(Tire);                                                                                                           |
           |       j++;                                                                                                                   |
           |     }                                                                                                                        |
           |                                                                                                                              |
           |   return l;                                                                                                                  |
         | }                                                                                                                              |
           •——————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————• */
		#endregion
	}

    public class Tire
    {
        public object Company { get; set; }
        public object Code { get; set; }
        public object Weather { get; set; }
        public object Radius { get; set; }
        public object WidthAndProfile { get; set; }
        public object Index { get; set; }
        public object Articul { get; set; }
    }
   
    public class TireWithPrice
    {
        public SparePartFranch SparePart { get; set; }
        public string Manufacturer {get; set;}
        public string ModelName  {get; set;}
        public Double? Profile  {get; set;}
        public string Radius  {get; set;}
        public Double? Height { get; set; }
        public string Season { get; set; }
        public string TireIndex { get; set; }
        public Decimal Price { get; set; }
        public Double? Width { get; set; }
        public string TireNumber { get; set; }
        public string Ref { get; set; }
        public int? ImageUrl { get; set; }
    }
}
