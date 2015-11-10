using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class Catalog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;

                GetCatalogInfo catalogInfo = master.CatalogProvider.GetCatalogInfo(catalog.Code, null, catalog.Ssd);
                catalog.Info = catalogInfo;

                if (catalogInfo.row.supportvinsearch)
                {
                    VinSearchForm vinSearchForm = new VinSearchForm(new VinSearchExtender(), catalog);

                    catalogPanel.Controls.Add(new LiteralControl(String.Format("<h1>{0}</h1>", vinSearchForm.GetLocalizedString("SearchByVIN"))));
                    catalogPanel.Controls.Add(vinSearchForm);
                    catalogPanel.Controls.Add(new LiteralControl("<br /><br />"));
                }
                if (catalogInfo.row.supportframesearch)
                {
                    FrameSearchForm frameSearchForm = new FrameSearchForm(new FrameSearchExtender(), catalog);

                    catalogPanel.Controls.Add(new LiteralControl(String.Format("<h1>{0}</h1>", frameSearchForm.GetLocalizedString("SearchByFrame"))));
                    catalogPanel.Controls.Add(frameSearchForm);
                    catalogPanel.Controls.Add(new LiteralControl("<br /><br />"));
                }
                if (catalogInfo.row.supportparameteridentifocation)
                {
                    WizardExtended wizard = new WizardExtended(new WizardExtender(), catalog);
                    wizard.WizardInfo = master.CatalogProvider.GetWizard(catalog.Code, string.Empty, string.Empty);

                    catalogPanel.Controls.Add(new LiteralControl(String.Format("<h1>{0}</h1>", wizard.GetLocalizedString("SearchByWizard"))));
                    catalogPanel.Controls.Add(wizard);
                    catalogPanel.Controls.Add(new LiteralControl("<br /><br />"));
                }
            }
        }
    }

    public class WizardExtended : Laximo.Guayaquil.Render.Wizard
    {
        public WizardExtended(IGuayaquilExtender extender, ICatalog catalog)
            : base(extender, catalog)
        {
        }

        protected override string GetWizardRowDescription(GetWizardRow wizardRow)
        {
            return string.Empty;
        }

        protected override void WriteVehicleListLink(HtmlTextWriter writer)
        {
        }
    }
}
