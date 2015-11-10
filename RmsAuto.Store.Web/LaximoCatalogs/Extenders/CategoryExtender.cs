using System;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class CategoryExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            string link;
            if (type.Equals("quickgroup"))
            {
                link = String.Format("QuickGroups.aspx?c={0}&vid={1}&ssd={2}", catalog.Code, catalog.VehicleId, catalog.Ssd);
            }
            else
            {
                ListCategoriesRow category = dataItem as ListCategoriesRow;
                if (category == null)
                {
                    throw new ArgumentException(String.Format("Expected type 'ListCategoriesRow'. Actual type is '{0}'.", renderer.GetType()));
                }
                link = String.Format("Vehicle.aspx?c={0}&vid={1}&cid={2}&ssd={3}", catalog.Code, catalog.VehicleId,
                                     category.categoryid,
                                     catalog.Ssd);
            }

            if (ItemId > 0)
            {
                link += string.Format("&ItemId={0}", ItemId);
            }

            return link;
        }
    }
}
