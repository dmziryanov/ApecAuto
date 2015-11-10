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
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class VinRequestDetails : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
		protected void Page_PreRender( object source, EventArgs e )
		{
            // Можем ли получить доступ к VIN запросам?
            new VinRequestUtil().CanVinRequest(this);

            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<VinRequest>( r => r.VinRequestItems );
				dc.DataContext.LoadOptions = dlo;
				dc.DataContext.DeferredLoadingEnabled = false;

				int rid = Int32.Parse( CmsContext.Current.PageParameters[ UrlKeys.VinRequests.RequestId ] );
				vrDetails.VinRequest = dc.DataContext.VinRequests.Where( r => r.Id == rid && r.ClientId == SiteContext.Current.CurrentClient.Profile.ClientId ).Single();

				//  Все гуд - сохраним как увиденное
				if( vrDetails.VinRequest.HasNotBeenSeen )
				{
					vrDetails.VinRequest.HasNotBeenSeen = false;
					dc.DataContext.SubmitChanges();
				}
			}
		}
    }
}
