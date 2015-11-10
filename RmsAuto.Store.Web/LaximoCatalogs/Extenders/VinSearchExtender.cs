using System;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class VinSearchExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            return String.Format("Vehicles.aspx?ft=findByVIN&c={0}&vin=$vin$&ssd=", catalog.Code);
        }
    }
}
