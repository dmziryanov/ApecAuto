using System;
using System.Collections.Generic;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;
using RmsAuto.Store.Web.LaximoCatalogs.RMSRenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class QuickDetails : System.Web.UI.Page
    {
        private string QuickGroupId
        {
            get
            {
                return Request.QueryString["gid"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;

                GetVehicleInfo vehicleInfo = master.CatalogProvider.GetVehicleInfo(catalog.Code, catalog.VehicleId, null, catalog.Ssd);
                ListQuickDetail quickDetail = master.CatalogProvider.GetListQuickDetail(catalog.Code, catalog.VehicleId,
                                                                                        QuickGroupId, true, null, catalog.Ssd);
                QuickDetailsExtender extender = new QuickDetailsExtender();
                QuickDetailsList quickDetailsList = new QuickDetailsList(extender, catalog);
                quickDetailsList.QuickDetailList = quickDetail;

                Dictionary<string, int> columns = new Dictionary<string, int>();
                columns.Add("Toggle", 1);
                columns.Add("PNC", 3);
                //columns.Add("OEM", 2);
                columns.Add("Name", 3);
                //columns.Add("Cart", 1);
                //columns.Add("Price", 3);
                columns.Add("Note", 2);
                columns.Add("Tooltip", 1);
                columns.Add("Designation", 2);
                columns.Add("Applicablemodels", 2);
                columns.Add("Partspec", 2);
                columns.Add("Color", 2);
                columns.Add("Shape", 2);
                columns.Add("Standard", 2);
                columns.Add("Material", 2);
                columns.Add("Size", 2);
                columns.Add("Featuredescription", 2);
                columns.Add("Prodstart", 2);
                columns.Add("Prodend", 2);
                columns.Add("Flag", 2);
                columns.Add("Amount", 2);
                columns.Add("OEM", 1);

                DetailsListRMS detailsList = new DetailsListRMS(extender, catalog);
                detailsList.FilterByGroup = true;
                detailsList.Columns = columns;

                quickDetailsList.DetailsList = detailsList;
                quickDetailsList.Controls.Add(detailsList);

                qdetailsPanel.Controls.Add(
                    new LiteralControl(String.Format("<h1>{0}</h1>",
                                                     quickDetailsList.GetLocalizedString("GroupDetails",
                                                                                         vehicleInfo.row[0].name))));
                qdetailsPanel.Controls.Add(new LiteralControl("<div id=\"pagecontent\">"));
                qdetailsPanel.Controls.Add(quickDetailsList);
                qdetailsPanel.Controls.Add(new LiteralControl("</div>"));
            }
        }
    }
}
