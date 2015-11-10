using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Laximo.Guayaquil.Data.Entities;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class Wizard : System.Web.UI.Page
    {
        #region properties

        private string WizardId
        {
            get { return Request.QueryString["wizardid"]; }
        }

        private string ValueId
        {
            get { return Request.QueryString["valueid"]; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ICatalog catalog = master.Catalog;
                catalog.Info = master.CatalogProvider.GetCatalogInfo(catalog.Code, null, catalog.Ssd);
                GetWizard wizard = master.CatalogProvider.GetWizard(catalog.Code, WizardId, ValueId);

                Laximo.Guayaquil.Render.Wizard wizardControl = new Laximo.Guayaquil.Render.Wizard(new WizardExtender(), catalog);
                wizardControl.WizardInfo = wizard;

                wizardPanel.Controls.Add(
                    new LiteralControl(String.Format("<h1>{0} - {1}</h1>",
                                                     wizardControl.GetLocalizedString("SearchByWizard"),
                                                     catalog.Info.row.name)));
                wizardPanel.Controls.Add(wizardControl);
            }
        }
    }
}
