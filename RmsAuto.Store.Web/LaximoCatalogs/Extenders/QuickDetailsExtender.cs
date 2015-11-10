using System;
using System.Text;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class QuickDetailsExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            string link;
            switch (type)
            {
                case "vehicle":
                    link = String.Format("Vehicle.aspx?c={0}&vid={1}&ssd={2}", catalog.Code, catalog.VehicleId, catalog.Ssd);
                    break;
                case "category":
                    ListQuickDetailCategory category = dataItem as ListQuickDetailCategory;
                    if (category == null)
                    {
                        throw new ArgumentException(String.Format("Expected type 'ListQuickDetailCategory'. Actual type is '{0}'.",
                                                                                  dataItem.GetType()));
                    }
                    link = string.Format("Vehicle.aspx?c={0}&vid={1}&cid={2}&ssd={3}", catalog.Code,
                                         catalog.VehicleId, category.categoryid, catalog.Ssd);
                    break;
                case "unit":
                    ListQuickDetailCategoryUnit unit = dataItem as ListQuickDetailCategoryUnit;
                    if (unit == null)
                    {
                        throw new ArgumentException(String.Format("Expected type 'ListQuickDetailCategoryUnit'. Actual type is '{0}'.",
                                                                                  dataItem.GetType()));
                    }
                    StringBuilder sb = new StringBuilder();
                    Array.ForEach(unit.Detail, delegate(DetailInfo detail)
                    {
                        sb.AppendFormat("{0},", detail.codeonimage);
                    });
                    link = String.Format("Unit.aspx?c={0}&vid={1}&uid={2}&cid={3}&ssd={4}&coi={5}", catalog.Code,
                                         catalog.VehicleId, unit.unitid, catalog.CategoryId, unit.ssd,
                                         sb.ToString().TrimEnd(','));
                    break;
                case "detail":
                    QuickDetailsList quickDetailsList = renderer as QuickDetailsList;
                    if (quickDetailsList == null)
                    {
                        throw new ArgumentException(String.Format("Expected type 'QuickDetailsList'. Actual type is '{0}'.",
                                                                  dataItem.GetType()));
                    }
                    DetailInfo unitDetail = dataItem as DetailInfo;
                    if (unitDetail == null)
                    {
                        throw new ArgumentException(String.Format("Expected type 'ListQuickDetailCategoryUnitDetail'. Actual type is '{0}'.",
                                                                                  dataItem.GetType()));
                    }
                    link = String.Format("Unit.aspx?c={0}&vid={1}&uid={2}&cid={3}&coi={4}&ssd={5}", catalog.Code,
                                         catalog.VehicleId, quickDetailsList.CurrentUnit.unitid,
                                         catalog.CategoryId, unitDetail.codeonimage, unitDetail.ssd);

                    break;
                default:
                    throw new ArgumentException(String.Format("Type '{0}' is not supported", type));
            }
            if (ItemId > 0)
            {
                link += string.Format("&ItemId={0}", ItemId);
            }

            return link;
        }
    }
}
