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
using System.Collections.Generic;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Controls
{
    public partial class OrderLineOptions : System.Web.UI.UserControl
    {
		/// <summary>
		/// Флаг предписывающий выставить флажок "не предлагать аналоги" и отключить возможность его снять
        /// Если значение null то реализуется функционал  -- поставщики только с аналогами
		/// </summary>
		public bool? IsAnalogsNotSupported
		{
			get { return (bool?)ViewState["__isAnalogsNotSupported"]; }
			set
			{
                if (value.HasValue)
                {
                    ViewState["__isAnalogsNotSupported"] = StrictlyThisNumber = value.Value;
                    _chkStrictlyThisNumber.Enabled = !value.Value;
                }
                else
                {
                    ViewState["__isAnalogsNotSupported"] = value;
                    _chkStrictlyThisNumber.Enabled = false;
                }
			}
		}
    

		public bool DisplayVinOptions { get; set; }

        public bool StrictlyThisNumber
        {
            get { return _chkStrictlyThisNumber.Checked; }
            set { if (IsAnalogsNotSupported.HasValue)_chkStrictlyThisNumber.Checked = value; }
        }

     

        public int? VinCheckupDataId
        {
            get
            {
				if( !_chkShowCars.Checked || _ddCarFromGarage.SelectedValue == String.Empty )
				{
					return null;
				}
				else
				{
					return Int32.Parse( _ddCarFromGarage.SelectedValue );
				}
            }
            set
            {
                BindGarageCars();
                if (value.HasValue)
                {
					_chkShowCars.Checked = true;
                    _ddCarFromGarage.SelectedValue = value.ToString();
                }
                else
                {
					_chkShowCars.Checked = false;
					_ddCarFromGarage.SelectedValue = String.Empty;
                }
            }
        }

        public void BindGarageCars()
        {
            var garageCars = ClientCarsDac.GetGarageCars(SiteContext.Current.CurrentClient.Profile.ClientId);
            if (garageCars.Count() > 0)
            {
                _litCarsNum.Text = garageCars.Count().ToString();
				foreach( var gc in garageCars )
				{
					_ddCarFromGarage.Items.Add( 
						new ListItem( gc.GetFullName(), gc.Id.ToString() )
						);
				}
            }
			_vinRow.Visible = DisplayVinOptions;
        }
    }
}