using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Controls
{
    public partial class PrcExcessPriceConfig : System.Web.UI.UserControl
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                var DC = new DCWrappersFactory<StoreDataContext>();
                var currSet = DC.DataContext.spSelUserSetting( SiteContext.Current.User.UserId ).FirstOrDefault();
                if ( currSet != null )
                {
                    txtPrcExcessPriceConfig.Text = currSet.PrcExcessPrice.ToString();
                }
                else
                {
                    txtPrcExcessPriceConfig.Text = "0";
                }
            }
            phSaveOK.Visible = false;
            phSaveError.Visible = false;
        }

        protected void btnSave_Click( object sender, EventArgs e )
        {
			using (var DC = new DCWrappersFactory<StoreDataContext>())
			{
				try
				{
					byte newPrc = Convert.ToByte(txtPrcExcessPriceConfig.Text);
					DC.DataContext.spUpdUSPrcExcessPrice(SiteContext.Current.User.UserId, newPrc);
					phSaveOK.Visible = true;

					DC.SetCommit();
				}
				catch
				{
					phSaveError.Visible = true;
				}
			}
        }
    }
}