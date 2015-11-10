using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms;
using RmsAuto.Common.Data;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.Controls
{
    public partial class GarageCars : System.Web.UI.UserControl
    {
        protected bool IsManagerMode
        {
            get
            {
                return SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager;
            }
        }

		protected string GetEditUrl( int carId )
		{
            if (IsManagerMode)
			{
                return null; // Manager.ClientGarageEditCar.GetUrl( carId );
			}
			else
			{
				return UrlManager.GetGarageCarEditUrl( carId );
			}
		}

        protected string GetViewUrl(int carId)
        {
            if (IsManagerMode)
            {
                return RmsAuto.Store.Web.Manager.ClientGarageEditCar.GetUrl(carId);
            }
            else
            {
                return UrlManager.GetGarageCarViewUrl(carId);
            }
        }

        protected string GetCreateVinUrl(int carId)
		{
            return UrlManager.GetVinRequestNewUrl(carId);
        }

		protected void Page_PreRender( object sender, EventArgs e )
		{
			rptGarageCars.DataSource = ClientCarsDac.GetGarageCars( SiteContext.Current.CurrentClient.Profile.ClientId );
			rptGarageCars.DataBind();
		}

		protected void rptGarageCarsCommand( object sender, RepeaterCommandEventArgs e )
		{
			switch( e.CommandName )
			{
				case UrlKeys.VinRequests.Operations.Delete:
					{
						ClientCarsDac.DeleteGarageCar( Convert.ToInt32( e.CommandArgument ) );
						break;
					}
			}
		}

    }
}