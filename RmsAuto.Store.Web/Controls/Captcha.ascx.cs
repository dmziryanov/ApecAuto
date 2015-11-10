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
using RmsAuto.Common.Web.UI;

namespace RmsAuto.Store.Web.Controls
{
    public partial class Captcha : System.Web.UI.UserControl
    {
        protected void _captureCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = CaptureImage.CheckCapture(args.Value);
        }
    }
}