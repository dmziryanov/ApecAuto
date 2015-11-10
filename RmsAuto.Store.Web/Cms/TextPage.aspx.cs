using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.BL;
using System.Net;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Entities;
//using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Web.Cms
{
    public partial class TextPage : RmsAuto.Store.Web.BasePages.LocalizablePage
	{

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager)
            {
                Page.MasterPageFile = "~/Manager/ManagerNEW.Master";
            }
        }
        
        protected void Page_Load( object sender, EventArgs e )
		{
			
            var textItemId = CmsContext.Current.PageParameters[ "ID" ];
			if( !string.IsNullOrEmpty( textItemId ) )
			{
                var item = TextItemsDac.GetTextItem( textItemId, SiteContext/*.Current*/.CurrentCulture );
				_titleLiteral.Text = Server.HtmlEncode( item.TextItemHeader );
				_bodyLiteral.Text = item.TextItemBody;
			}
			else if( CmsContext.Current.CatalogItem != null )
			{
				_titleLiteral.Text = Server.HtmlEncode( CmsContext.Current.CatalogItem.CatalogItemName );
				_bodyLiteral.Text = CmsContext.Current.CatalogItem.PageBody;
			}
			else
			{
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );
			}
		}

	}
}
