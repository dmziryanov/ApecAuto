using System;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class DetailExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            if (type.Equals("quickgroup"))
            {
                return String.Format("QuickGroups.aspx?c={0}&vid={1}&ssd={2}", catalog.Code, catalog.VehicleId, catalog.Ssd);
            }

            //'window.alert('.FormatLocalizedString('SelectedDetail', $dataItem->oem).')'
            return String.Format("window.alert('{0}')", GetLocalizedString("SelectedDetail", renderer));//dataItem->oem - wtf?
        }
    }
}
