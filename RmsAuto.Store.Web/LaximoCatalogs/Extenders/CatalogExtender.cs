using System;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public sealed class CatalogExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            ListCatalogsRow catalogsRow = dataItem as ListCatalogsRow;
            if (catalogsRow == null)
            {
                throw new ArgumentException(String.Format("Expected type 'ListCatalogsRow'. Actual type is '{0}'.", dataItem.GetType()));
            }

            string link = String.Format("Catalog.aspx?c={0}&ssd=", catalogsRow.code);
            if (catalogsRow.supportparameteridentifocation)
            {
                link += "&spi=t";
            }
            return link;
        }
    }
}
