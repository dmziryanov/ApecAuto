using System;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class WizardExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            GetWizard wizard = dataItem as GetWizard;
            if (wizard == null)
            {
                throw new ArgumentNullException("wizard");
            }

            return
                String.Format(
                    type.Equals("vehicles")
                        ? "Vehicles.aspx?ft=findByWizard&c={0}&wid={1}&ssd="
                        : "Wizard.aspx?c={0}&wizardid={1}&valueid=$valueid$&ssd=", catalog.Code, wizard.row[0].wizardid);
        }
    }
}
