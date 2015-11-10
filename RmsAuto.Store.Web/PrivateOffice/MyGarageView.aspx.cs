using System;
using System.Collections;
using System.Collections.Specialized;
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
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Configuration;
using RmsAuto.Common.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Common.Misc;
using System.Collections.Generic;

using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class MyGarageView : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected UserGarageCar Car { get; set; }

		protected void Page_Load( object sender, EventArgs e )
		{
			var carId = CmsContext.Current.PageParameters.TryGet<int>( UrlKeys.VinRequests.CarId );
            if (carId <= 0)
            {
                throw new ArgumentException("CarId");
            }
            this.Car = ClientCarsDac.GetGarageCar(carId);
            if (this.Car == null)
            {
                throw new ArgumentException("CarId");
            }
            //_garageCarEdit.SetFields(car);
            _garageCarDetails.CarParameters = this.Car;
        }

        protected void Page_PreRender(object source, EventArgs e)
        {
            // Можем ли получить доступ к VIN запросам?
            new VinRequestUtil().CanVinRequest(this);
        }
    }
}
