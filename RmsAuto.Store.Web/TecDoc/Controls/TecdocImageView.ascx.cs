using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.TechDoc;
using RmsAuto.TechDoc.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
	public partial class TecdocImageView : System.Web.UI.UserControl
	{
		protected string GetImageUrl( int id )
		{
			return UrlManager.GetTecDocImageUrl( id );
		}

 		protected void Page_Load(object sender, EventArgs e)
		{
            (dsImages.SelectParameters[0] as QueryStringParameter).QueryStringField = UrlKeys.StoreAndTecdoc.ArticleId;
		}
    }
}