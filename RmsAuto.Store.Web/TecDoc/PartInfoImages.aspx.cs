using System;
using System.Collections.Specialized;
using System.Web.UI;
using RmsAuto.TechDoc;
using RmsAuto.TechDoc.Entities;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web
{
    public partial class PartInfoImages : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            _tdPartInfo.ArticleId = Request.QueryString.Get<int>(UrlKeys.StoreAndTecdoc.ArticleId);
        }
	}
}
