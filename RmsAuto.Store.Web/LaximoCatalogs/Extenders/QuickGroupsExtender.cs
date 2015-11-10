using System;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class QuickGroupsExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            string link;
            if (type.Equals("vehicle"))
            {
                link = String.Format("Vehicle.aspx?c={0}&vid={1}&ssd={2}", catalog.Code, catalog.VehicleId, catalog.Ssd);
            }
            else
            {
                t_row row = dataItem as t_row;
                if (row == null)
                {
                    throw new ArgumentException(String.Format("Expected type 't_row'. Actual type is '{0}'.",
                                                              dataItem.GetType()));
                }

                link = String.Format("QuickDetails.aspx?c={0}&gid={1}&vid={2}&ssd={3}", catalog.Code,
                                     row.quickgroupid, catalog.VehicleId, catalog.Ssd);
            }

            if (ItemId > 0)
            {
                link += string.Format("&ItemId={0}", ItemId);
            }

            return link;
        }
    }
}
