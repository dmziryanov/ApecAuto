using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using RmsAuto.Store.Entities.Helpers;

namespace RmsAuto.Store.Web.Controls
{
    public partial class SparePartInformationExt : System.Web.UI.UserControl
    {
		public AdditionalInfoExt AdditionalInfoExt { get; set; }
		
		public string AdditionalInfoKey
		{
			get
			{
				if (this.AdditionalInfoExt != null)
				{
					return AdditionalInfoExt.Key.Manufacturer + "," + AdditionalInfoExt.Key.PartNumber + "," + AdditionalInfoExt.Key.SupplierID;
				}
				return string.Empty;
			}
		}

		protected override void DataBindChildren()
		{
			if (AdditionalInfoExt != null)
			{
				_placeHolder.Visible = true;
				base.DataBindChildren();
			}
			else
			{
				_placeHolder.Visible = false;
			}
		}

    }
}