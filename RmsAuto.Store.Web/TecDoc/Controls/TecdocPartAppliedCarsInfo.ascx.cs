using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.TechDoc;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.TechDoc.Entities.Helpers;
using System.Collections.Generic;
using RmsAuto.TechDoc.Entities.TecdocBase;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
    public partial class TecdocPartAppliedCarsInfo : System.Web.UI.UserControl
    {
        public int ArticleId { get; set; }
        public IEnumerable<CarType> Cars { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.Cars = Facade.GetAppliedCars( this.ArticleId );
            DataBind();
        }

        protected string GetEngineVolume(CarType carType)
        {
            if (carType.EngineVolume.HasValue)
            {
                return Math.Round((decimal)carType.EngineVolume.Value / 1000m, 1).ToString("0.0");
            }
            else
            {
                return String.Empty;
            }
        }
    }
}