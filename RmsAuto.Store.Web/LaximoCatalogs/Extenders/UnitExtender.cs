using System;
using System.Web;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class UnitExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            ListUnitsRow unit = dataItem as ListUnitsRow;
            if (unit == null)
            {
                throw new ArgumentException(String.Format("Expected type 'ListUnitsRow'. Actual type is '{0}'.", renderer.GetType()));
            }

            string link;

            if (type.Equals("filter"))
            {
                link = String.Format("UnitFilter.aspx?c={0}&vid={1}&uid={2}&cid={3}&ssd={4}&path_id={5}&f={6}",
                                     catalog.Code, catalog.VehicleId, unit.unitid, catalog.CategoryId, unit.ssd,
                                     catalog.PathId, HttpUtility.UrlEncode(unit.filter));
            }
            else
            {
                link = String.Format("Unit.aspx?c={0}&vid={1}&uid={2}&cid={3}&ssd={4}&path_id={5}",
                                     catalog.Code, catalog.VehicleId, unit.unitid, catalog.CategoryId, unit.ssd,
                                     catalog.PathId);
            }

            if (ItemId > 0)
            {
                link += string.Format("&ItemId={0}", ItemId);
            }

            return link;
        }
    }
}
