using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
	public partial class ListSparePartImagesMgmt : Security.BasePage
	{
        // количество строк с фото на странице, можно менять при необходимости
        static private int ddlNumLineInPage = 10;

        public SparePartImage[] listSparePartImages, // список всех фотографий
                        displayListSparePartImages; // список отображаемых фотографий
        // текущая страница 
        public int id;

        // количество всех строк фото
        public int numAllRowsPhoto;
        // количество всех страниц
        public int numAllPage;
        // остаток при делении количества всех строк фото на количество строк с фото на странице
        public int numPageRemainder;

        // количество страниц в строке пейджинга, можно менять при необходимости
        public int numPageInLine = 1;
        // текущая позиция блока пейджинга
        public int currentNumPossitoin = 1;
        // минимальная позиция блока пейджинга
        public int minPossition = 1;
        // максимальная позиция блока пейджинга
        public int maxPossition = 1;
        // остаток при деление количества страниц на количество страниц в строке пейджинга 
        public int possitionRemainder;

        // значение полученное из TextBox, id = TBManufacturer
        static private string tbManufacturer = "";
        // значение полученное из TextBox, id = TBPartNumber
        static private string tbPartNumber = "";
        // значение полученное из DropDownList, id = DDLSupplierID
        static private string ddlSupplierID = "";

        string action, idPage;

        protected void Page_Load(object sender, EventArgs e)
		{
            // действие
            action = Page.Request.QueryString["action"];
            // ключ для бд
            string manufacturer = Page.Request.QueryString["manufacturer"];
            string partNumber = Page.Request.QueryString["partnumber"];
            string supplierId = Page.Request.QueryString["supplierid"];
            string imageNumber = Page.Request.QueryString["imagenumber"];
            // номер страницы для пейджинга
            idPage = Page.Request.QueryString["id"];

            if (!IsPostBack)
            {
                if (tbManufacturer != "") TBManufacturer.Text = tbManufacturer;
                if (tbPartNumber != "") TBPartNumber.Text = tbPartNumber;
                if (ddlSupplierID != "")
                {
                    DDLSupplierID.Items.Add(new ListItem { Text = "Все", Value = ""});
                    DDLSupplierID.Items.Add(new ListItem { Text = "Уценка 20%", Value = "1212" });
                    DDLSupplierID.Items.Add(new ListItem { Text = "Уценка 50%", Value = "1215" });
                    DDLSupplierID.SelectedValue = ddlSupplierID;
                }
                else
                {
                    DDLSupplierID.Items.Add(new ListItem { Text = "Все", Value = "", Selected = true });
                    DDLSupplierID.Items.Add(new ListItem { Text = "Уценка 20%", Value = "1212" });
                    DDLSupplierID.Items.Add(new ListItem { Text = "Уценка 50%", Value = "1215" });
                }
                DDLNumLineInPage.SelectedValue = ddlNumLineInPage.ToString();
            }
            else
            {
                tbManufacturer = TBManufacturer.Text.Trim();
                tbPartNumber = TBPartNumber.Text.Trim();
                ddlSupplierID = DDLSupplierID.SelectedItem.Value;
                ddlNumLineInPage = Convert.ToInt32(DDLNumLineInPage.SelectedItem.Value);
                // переход на первую страницу
                idPage = "1";
            }

            
            using(var store = new DCFactory<StoreDataContext>())
            {
            
                var param = store.DataContext;
                delPhoto(ref param, action, manufacturer, partNumber, supplierId, imageNumber);
                
                // расчитываем что будет выведено на экран
                displayListSparePartImages = pageImages(ref param, ddlNumLineInPage);
            }
		}
        
        SparePartImage[] pageImages(ref StoreDataContext store, int count)
        {
            //Parallel
            SparePartImage[] ia = null;
            if (tbManufacturer != "") ia = store.SparePartImages.Where(i => i.Manufacturer == tbManufacturer).ToArray();
            if (tbPartNumber != "")
            {
                if (ia == null) ia = store.SparePartImages.Where(i => i.PartNumber == tbPartNumber).ToArray();
                else ia = ia.Where(i => i.PartNumber == tbPartNumber).ToArray();
            }
            if (ddlSupplierID != "")
            {
                if (ia == null) ia = store.SparePartImages.Where(i => i.SupplierID == Convert.ToInt32(ddlSupplierID)).ToArray();
                ia = ia.Where(i => i.SupplierID == Convert.ToInt32(ddlSupplierID)).ToArray();
            }
            int from = ia == null ? getPositionFrom(store.SparePartImages.Count()) : getPositionFrom(ia.Length);
            if (ia == null) ia = store.SparePartImages.Skip(from).Take(count).ToArray();
            else ia = ia.Skip(from).Take(count).ToArray(); return ia;
        }
        int getPositionFrom(int numAllRowsPhoto)
        {
            // получаем число всех фотографий numAllRowsPhoto

            // определяем количество страниц для пейджинга
            numAllPage = numAllRowsPhoto / ddlNumLineInPage;
            numPageRemainder = numAllRowsPhoto % ddlNumLineInPage;

            if (numPageRemainder != 0) numAllPage++;

            // пейжжинг
            maxPossition = numAllPage / numPageInLine;
            possitionRemainder = numAllPage % numPageInLine;
            if (possitionRemainder != 0) maxPossition++;

            FromToPs.Text = " из " + maxPossition + ' ';

            // определение начала выбора фото для отображения на текужей странице
            int startChoisePhoto = id = 1;

            int id_2 = id; try
            {
                id_2 = Convert.ToInt32(PageNumberID.Text); if (id_2 < minPossition) id_2 = minPossition;
                else if (id_2 > maxPossition) id_2 = maxPossition; id = id_2;
            }
            catch { /*id_2 = id;*/  }

            // задаем критерии выборки для соответствующей страницы
            if (action == "page" && idPage != null) id = Convert.ToInt32(idPage);
            startChoisePhoto = (id - 1) * ddlNumLineInPage + 1;
            PageNumberID.Text = id.ToString();

            // вычисляем текущую позицию блока страниц в пейджинге
            for (int i = 1; i <= maxPossition; i++)
            if ((id >= (i - 1) * numPageInLine + 1) && (id <= i * numPageInLine)) currentNumPossitoin = i;

            return startChoisePhoto - 1;
        }
        void delPhoto(ref StoreDataContext store, string a, string m, string p, string id, string nm)
        {
            // проверяем есть ли фотография на удаление
            if (a == "del" && m != null && p != null && id != null && nm != null)
            {
                // достаем фотографию из бд
                SparePartImage deleteImage = store.SparePartImages.SingleOrDefault(q => q.Manufacturer == m && q.PartNumber == p && q.SupplierID == Convert.ToInt32(id) && q.ImageNumber == Convert.ToInt32(nm));

                // если фотография есть, удаляем ee
                if (deleteImage != null)
                {
                    store.SparePartImages.DeleteOnSubmit(deleteImage);
                    store.SubmitChanges();
                    string storage = id == "1212" ? "Уценка 20%" : "Уценка 50%";
                    LblMessage.Text = "Фотография с параметрами: Производитель=" + m + ", Артикул=" + p + ", Склад уценки=" + storage + ", Номер фотографии=" + nm + " - удалена.";
                    LblMessage.Visible = true;
                }
            }
        }
    }
}
