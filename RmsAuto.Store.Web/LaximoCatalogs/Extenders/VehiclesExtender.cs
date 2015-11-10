using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using System.Text;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class VehiclesExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            VehicleInfo vehicleInfo = dataItem as VehicleInfo;
            if (vehicleInfo == null)
            {
                throw new ArgumentException(String.Format("Expected type 'VehicleInfo'. Actual type is '{0}'.", dataItem.GetType()));
            }

            VehiclesList vehiclesList = renderer as VehiclesList;
            if (vehiclesList == null)
            {
                throw new ArgumentException(String.Format("Expected type 'VehiclesList'. Actual type is '{0}'.", renderer.GetType()));
            }

            if (string.IsNullOrEmpty(vehicleInfo.PathData))
            {
                throw new ArgumentNullException("VehicleInfo.PathData");
            }

            string catalogCode = string.IsNullOrEmpty(catalog.Code) ? vehicleInfo.catalog : catalog.Code;

            string pathData = (vehicleInfo.PathData.Length > 300)
                                  ? vehicleInfo.PathData.Substring(0, 300)
                                  : vehicleInfo.PathData;

            string link = String.Format("{0}.aspx?c={1}&vid={2}&ssd={3}&path_data={4}",
                                        vehiclesList.QuickGroupsEnable ? "QuickGroups" : "Vehicle",
                                        catalogCode,
                                        vehicleInfo.vehicleid,
                                        vehicleInfo.ssd,
                                        HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.Default.GetBytes(pathData))));

            if (ItemId > 0)
            {
                link += string.Format("&ItemId={0}", ItemId);
            }

            return link;
        }
    }
}
