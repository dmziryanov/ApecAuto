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
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Configuration;
using RmsAuto.Common.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Common.Misc;
using System.Collections.Generic;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class MyGarageNew : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected void btnAddCarClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                using (var ctx = new DCFactory<StoreDataContext>())
                {
                    var car = _garageCarEdit.GetNewCarData<UserGarageCar>();
                    car.AddedDate = DateTime.Now;
                    car.ClientId = SiteContext.Current.CurrentClient.Profile.ClientId;

                    ctx.DataContext.UserGarageCars.InsertOnSubmit(car);
                    ctx.DataContext.SubmitChanges();
                }

                Response.Redirect(UrlManager.GetGarageUrl());
            }
        }

        protected void Page_PreRender(object source, EventArgs e)
        {
            // Можем ли получить доступ к VIN запросам?
            new VinRequestUtil().CanVinRequest(this);
        }
    }
}
