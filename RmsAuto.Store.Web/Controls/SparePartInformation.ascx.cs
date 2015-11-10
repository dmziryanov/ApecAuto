using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Text;
using RmsAuto.TechDoc.Entities;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Common.Linq;
using System.Reflection;
using RmsAuto.Store.Cms.BL;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.Store.Web.Controls
{
    public partial class SparePartInformation : System.Web.UI.UserControl
    {
        public AdditionalInfo AdditionalInfo { get; set; }

		protected override void DataBindChildren()
		{
			if( AdditionalInfo != null )
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