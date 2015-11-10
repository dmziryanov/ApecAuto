using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class Vehicle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;
                catalog.Info = master.CatalogProvider.GetCatalogInfo(catalog.Code, null, catalog.Ssd);

                GetVehicleInfo vehicleInfo = master.CatalogProvider.GetVehicleInfo(catalog.Code, catalog.VehicleId, null, catalog.Ssd);
                ListCategories categories = master.CatalogProvider.GetCategoriesList(catalog.Code, catalog.VehicleId, catalog.CategoryId,
                                                                                  catalog.Ssd);
                ListUnits units = master.CatalogProvider.GetUnitsList(catalog.Code, catalog.VehicleId, catalog.CategoryId, catalog.Ssd);

                CategoriesList categoriesList = new CategoriesList(new CategoryExtender(), catalog);
                categoriesList.Categories = categories;

                UnitsList unitsList = new UnitsList(new UnitExtender(), catalog);
                unitsList.Units = units;
                unitsList.ImageSize = 175;

                string vehicleName = (vehicleInfo != null && vehicleInfo.row != null && vehicleInfo.row.Length > 0)
                                         ? vehicleInfo.row[0].name
                                         : string.Empty;

                vehiclePanel.Controls.Add(
                    new LiteralControl(String.Format("<h1>{0}</h1>",
                                                     categoriesList.GetLocalizedString("CarName", vehicleName))));
                vehiclePanel.Controls.Add(new LiteralControl("<div id=\"pagecontent\">"));
                vehiclePanel.Controls.Add(categoriesList);
                vehiclePanel.Controls.Add(unitsList);
                vehiclePanel.Controls.Add(new LiteralControl("</div>"));
            }
        }
    }
}
