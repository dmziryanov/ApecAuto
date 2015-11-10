using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public class FrameSearchExtender : CommonExtender
    {
        public override string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer)
        {
            return String.Format("Vehicles.aspx?ft=findByFrame&c={0}&frame=$frame$&frameNo=$frameno$", catalog.Code);
        }
    }
}
