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

using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Configuration;
using RmsAuto.Common.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Common.Misc;
using System.Collections.Generic;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class MyGarageEdit : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected int CarId
        {
            get { return (int)ViewState["CarId"]; }
            set { ViewState["CarId"] = value; }
        }

		protected void Page_Load( object sender, EventArgs e )
		{
            if (!IsPostBack)
            {
				this.CarId = CmsContext.Current.PageParameters.TryGet<int>( UrlKeys.VinRequests.CarId );
                if (this.CarId  <= 0)
                {
                    throw new ArgumentException("CarId");
                }
                var car = ClientCarsDac.GetGarageCar(this.CarId);
                if (car == null)
                {
                    throw new ArgumentException("CarId");
                }
                _garageCarEdit.SetFields(car);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void Page_PreRender(object source, EventArgs e)
        {
            // Можем ли получить доступ к VIN запросам?
            new VinRequestUtil().CanVinRequest(this);
        }

        protected void btnEditCarClick(object sender, EventArgs e)
        {
			if( Page.IsValid )
			{
				ClientCarsDac.UpdateGarageCar(
										 this.CarId,
										 _garageCarEdit.FillCarData<UserGarageCar> );
				Response.Redirect( UrlManager.GetGarageUrl() );
			}
        }
    }
}
