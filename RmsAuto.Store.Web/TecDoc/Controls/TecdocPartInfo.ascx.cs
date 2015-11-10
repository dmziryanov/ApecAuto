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
using RmsAuto.TechDoc;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
    public partial class TecdocPartInfo : System.Web.UI.UserControl
    {
        public int ArticleId { get; set; }
        protected PartInfo PartInfo { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.PartInfo = Facade.GetPartInfo(this.ArticleId);
            DataBind();
        }
    }
}