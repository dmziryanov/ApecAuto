using System;
using System.Collections.Generic;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;
using RmsAuto.Store.Web.LaximoCatalogs.RMSRenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class Unit : System.Web.UI.Page
    {
        private string UnitId
        {
            get { return Request.QueryString["uid"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;

                GetCatalogInfo catalogInfo = master.CatalogProvider.GetCatalogInfo(catalog.Code, null, catalog.Ssd);
                catalog.Info = catalogInfo;

                GetUnitInfo unitInfo = master.CatalogProvider.GetUnitInfo(catalog.Code, UnitId, catalog.Ssd);
                ListDetailsByUnit listDetailsByUnit = master.CatalogProvider.GetListDetailByUnit(catalog.Code, UnitId, catalog.Ssd);
                ListImageMapByUnit listImageMapByUnit = master.CatalogProvider.GetListImageMapByUnit(catalog.Code, UnitId, catalog.Ssd);
                DetailExtender extender = new DetailExtender();

                Laximo.Guayaquil.Render.Unit unit = new Laximo.Guayaquil.Render.Unit(extender, catalog);

                UnitImage unitImage = new UnitImage(extender, catalog);
                unitImage.UnitInfo = unitInfo;
                unitImage.ImageMap = listImageMapByUnit;
                unitImage.ContainerHeight = unit.ContainerHeight;
                unitImage.ContainerWidth = unit.ContainerWidth;

                unit.UnitImage = unitImage;
                unit.Controls.Add(unitImage);

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
				detailsList.Details = listDetailsByUnit.row;
				detailsList.Columns = columns;

                unit.DetailsList = detailsList;
                unit.Controls.Add(detailsList);

                unitPanel.Controls.Add(
                    new LiteralControl(String.Format("<h1>{0}</h1>",
                                                     unit.GetLocalizedString("UnitName",
                                                                             String.Concat(unitInfo.FirstRow.code, ": ", unitInfo.FirstRow.name)))));
                unitPanel.Controls.Add(unit);
            }
        }
    }
}