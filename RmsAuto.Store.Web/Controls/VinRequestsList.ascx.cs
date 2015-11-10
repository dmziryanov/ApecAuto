using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms;
using RmsAuto.Common.Data;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.Controls
{
    public partial class VinRequestsList : System.Web.UI.UserControl
    {
        public IEnumerable<VinRequest> DataSource { get; set; }

        public override void DataBind()
        {
            rptRequests.DataSource = this.DataSource;

            base.DataBind();
        }
    }
}