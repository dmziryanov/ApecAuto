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
using System.Collections.Generic;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
    public partial class TecdocAddInfo : System.Web.UI.UserControl
    {
        public int ArticleId { get; set; }
        protected IEnumerable<KeyValuePair<string, string>> AdditionalInfos { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.AdditionalInfos = Facade.GetPartAddInfo(this.ArticleId);
            DataBind();
        }
    }
}