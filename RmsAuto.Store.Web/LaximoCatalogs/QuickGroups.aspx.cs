using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class QuickGroups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;

                GetVehicleInfo vehicleInfo = master.CatalogProvider.GetVehicleInfo(catalog.Code, catalog.VehicleId, null, catalog.Ssd);
                ListQuickGroups quickGroups = master.CatalogProvider.GetListQuickGroup(catalog.Code, catalog.VehicleId, null, catalog.Ssd);

                QuickGroupsList quickGroupsList = new QuickGroupsList(new QuickGroupsExtender(), catalog);
                quickGroupsList.Groupses = quickGroups;

                qgroupsPanel.Controls.Add(
                    new LiteralControl(String.Format("<h1>{0}</h1>",
                                                     quickGroupsList.GetLocalizedString("CarName",
                                                                                        vehicleInfo.row[0].name))));
                qgroupsPanel.Controls.Add(new LiteralControl("<div id=\"pagecontent\">"));
                qgroupsPanel.Controls.Add(quickGroupsList);
                qgroupsPanel.Controls.Add(new LiteralControl("</div>"));
            }
        }
    }
}
