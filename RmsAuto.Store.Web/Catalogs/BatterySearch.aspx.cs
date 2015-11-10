using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.Store
{
	//TODO (для разметки) Вынести общие css-классы в отдельный файлик
	public partial class BatterySearch : LocalizablePage
    {

        private const int numOfBrandColumns = 3; //Количество колонок выравнивания брендов

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
            this.BatterySearchControl.MaxSearchCount = 100;
            this.QuantityWarningText.Text = "&mdash; результатов поиска слишком много, выведено " + BatterySearchControl.MaxSearchCount.ToString() + " товаров, пожалуйста уточните параметры поиска. ";
            this.PreRender += new System.EventHandler(this.BatterySearch_PreRender);
            List<BatteryBrand> cmsBatteryBrandsList;
            using (var cms = new CmsDataContext())
            {
                cmsBatteryBrandsList = cms.BatteryBrands.Where(x => x.IsVisible).ToList();
                companies.DataSource = cmsBatteryBrandsList;
                companies.DataTextField = "Name";
                companies.DataBind();
                companies.Items.Insert(0, "Все производители");
            }

            int RowCount = 0;
            if (cmsBatteryBrandsList.Count % numOfBrandColumns > 0)
                RowCount = cmsBatteryBrandsList.Count / numOfBrandColumns + 1;
            else
                RowCount = cmsBatteryBrandsList.Count / numOfBrandColumns;
            int curIndex = 0;
            for (int i = 0; i < RowCount; i++)
            {
                TableRow trLogo = new TableRow();
                BrandsTable.Rows.Add(trLogo);
                TableRow trText = new TableRow();
                BrandsTable.Rows.Add(trText);
                for (int j = 0; j < numOfBrandColumns; j++)
                {
                    if (curIndex == cmsBatteryBrandsList.Count) { break; }
                    TableCell tcLogo = new TableCell();
                    trLogo.Cells.Add(tcLogo);
                    Image im = new Image();
                    im.ImageUrl = UrlManager.GetFileUrl(cmsBatteryBrandsList[curIndex].ImageId.Value)+"?r=rms";
                    im.CssClass = "imgCat";
                    im.AlternateText = cmsBatteryBrandsList[curIndex].Name;

                    tcLogo.Controls.Add(im);

                    TableCell tcText = new TableCell();
                    trText.Cells.Add(tcText);
                    tcText.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    tcText.Style.Add(HtmlTextWriterStyle.MarginBottom, "10");
                    tcText.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                    tcText.Style.Add(HtmlTextWriterStyle.Height, "30px");

                    Label btn = new Label();
                    btn.Text = cmsBatteryBrandsList[curIndex].Name;
                    btn.CssClass = "imgCat";
                    tcText.Controls.Add(btn);
                    curIndex++;
                }

            }
        }

        private void BatterySearch_PreRender(object sender, EventArgs e)
        {
            Quantity.Text = BatterySearchControl.CurrentSearchCount.ToString();
            QuantityWarningText.Visible = BatterySearchControl.ExceedMaxresult;
            QuantityText.Visible = !BatterySearchControl.ExceedMaxresult;
            Quantity.Visible = !BatterySearchControl.ExceedMaxresult;
            Elements_Count.Visible = BatterySearchControl.CurrentSearchCount > 0;
            info_block.Visible = BatterySearchControl.CurrentSearchCount > 0;
        }
    }
}
