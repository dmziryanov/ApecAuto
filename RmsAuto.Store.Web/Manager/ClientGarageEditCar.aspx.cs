using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientGarageEditCar : ClientBoundPage
	{
		public static string GetUrl( int carId )
		{
			return string.Format( "~/Manager/ClientGarageEditCar.aspx?CarId={0}", carId );
        }

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.Garage; }
        }

		public static string GetNewCarUrl()
		{
			return GetUrl( 0 );
		}

		protected int CarId
		{
			get { return (int)ViewState[ "CarId" ]; }
			set { ViewState[ "CarId" ] = value; }
		}

		protected override void OnPreInit(EventArgs e)
		{
			throw new Exception("Page not found.");
			//base.OnPreInit(e);
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			if( !IsPostBack )
			{
				CarId = Convert.ToInt32( Request["CarId"] );
				if( CarId != 0 )
				{
					var car = ClientCarsDac.GetGarageCar( CarId );
					if( car == null )
					{
						throw new ArgumentException( "CarId" );
					}
					_garageCarEdit.SetFields( car );
				}
			}
		}

		protected void SaveButton_Click( object sender, EventArgs e )
        {

            if (IsValid)
            {
                if (CarId != 0)
                {
                    ClientCarsDac.UpdateGarageCar(
                                 this.CarId,
                                 _garageCarEdit.FillCarData<UserGarageCar>);
                }
                else
                {
                    using (var ctx = new DCWrappersFactory<StoreDataContext>())
                    {
                        var car = _garageCarEdit.GetNewCarData<UserGarageCar>();
                        car.AddedDate = DateTime.Now;
                        car.ClientId = ClientData.Profile.ClientId;

                        ctx.DataContext.UserGarageCars.InsertOnSubmit(car);
                        ctx.DataContext.SubmitChanges();
                    }

                    Response.Redirect(ClientGarage.GetUrl(), true);
                }
            }
		}
		protected void BackButton_Click( object sender, EventArgs e )
		{
			Response.Redirect( ClientGarage.GetUrl(), true );
		}
	}
}
