using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class Vehicles : System.Web.UI.Page
    {
        #region properties

        private string FindType
        {
            get { return Request.QueryString["ft"]; }
        }

        private string Vin
        {
            get { return Request.QueryString["vin"]; }
        }

        private string Frame
        {
            get { return Request.QueryString["frame"]; }
        }

        private string FrameNo
        {
            get { return Request.QueryString["frameNo"]; }
        }

        private string WizardId
        {
            get { return Request.QueryString["wid"]; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;
                catalog.Info = master.CatalogProvider.GetCatalogInfo(catalog.Code, null, catalog.Ssd);

                VehicleInfo[] vehicleInfos;
                switch (FindType)
                {
                    case "findByVIN":
                        FindVehicleByVIN findVehicleByVin = master.CatalogProvider.FindVehicleByVIN(catalog.Code, Vin, null, catalog.Ssd);
                        vehicleInfos = findVehicleByVin.row;
                        break;
                    case "findByFrame":
                        FindVehicleByFrame findVehicleByFrame = master.CatalogProvider.FindVehicleByFrame(catalog.Code, Frame, FrameNo, null, catalog.Ssd);
                        vehicleInfos = findVehicleByFrame.row;
                        break;
                    case "findByWizard":
                        FindVehicleByWizard findVehicleByWizard = master.CatalogProvider.FindVehicleByWizard(catalog.Code, WizardId, null, catalog.Ssd);
                        vehicleInfos = findVehicleByWizard.row;
                        break;
                    default:
                        throw new ArgumentException(string.Format("Find type '{0}' is not supported", FindType));
                }

                VehiclesExtender extender = new VehiclesExtender();
                if (vehicleInfos == null || vehicleInfos.Length == 0)
                {
                    if (FindType.Equals("findByVIN"))
                    {
                        vehiclesPanel.Controls.Add(new LiteralControl(extender.GetLocalizedString("FINDFAILED", null, Vin)));
                    }
                    else
                    {
                        vehiclesPanel.Controls.Add(
                            new LiteralControl(extender.GetLocalizedString("FINDFAILED", null,
                                                                           String.Format("{0}-{1}", Frame, FrameNo))));
                    }
                }
                else
                {
                    VehiclesList vehiclesList = new VehiclesList(extender, catalog);
                    vehiclesList.QuickGroupsEnable = catalog.Info.row.supportquickgroups;
                    vehiclesList.Vehicles = vehicleInfos;

                    vehiclesPanel.Controls.Add(
                        new LiteralControl(String.Format("<h1>{0}</h1><br>", vehiclesList.GetLocalizedString("Cars"))));
                    vehiclesPanel.Controls.Add(vehiclesList);
                }

                if (catalog.Info.row.supportvinsearch)
                {
                    VinSearchForm vinSearchForm = new VinSearchForm(new VinSearchExtender(), catalog);
                    vinSearchForm.PreviousVin = Vin;

                    vehiclesPanel.Controls.Add(new LiteralControl(String.Format("<h1>{0}</h1>", vinSearchForm.GetLocalizedString("SearchByVIN"))));
                    vehiclesPanel.Controls.Add(vinSearchForm);
                    vehiclesPanel.Controls.Add(new LiteralControl("<br /><br />"));
                }

                if (catalog.Info.row.supportframesearch)
                {
                    FrameSearchForm frameSearchForm = new FrameSearchForm(new FrameSearchExtender(), catalog);
                    frameSearchForm.PreviousFrame = Frame;
                    frameSearchForm.PreviousFrameNo = FrameNo;

                    vehiclesPanel.Controls.Add(new LiteralControl(String.Format("<h1>{0}</h1>", frameSearchForm.GetLocalizedString("SearchByFrame"))));
                    vehiclesPanel.Controls.Add(frameSearchForm);
                    vehiclesPanel.Controls.Add(new LiteralControl("<br /><br />"));
                }
            }
        }
    }
}
