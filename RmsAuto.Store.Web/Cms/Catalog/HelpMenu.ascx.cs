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
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.Cms.Catalog
{
    public partial class HelpMenu : System.Web.UI.UserControl
    {
        protected bool IsCurrentNode( SiteMapNode node )
        {
            //TODO: определение текущего подраздела
            return string.Compare( node.Url, Request.RawUrl, true ) == 0;
        }
        protected void Page_Load( object sender, EventArgs e )
        {
            var menuCatalogItem = UrlManager.CatalogItems.GetCatalogItems( UrlManager.CatalogItems.RootCatalogItem.CatalogItemID, CatalogItemMenuType.HelpMenu ).FirstOrDefault();
            if ( menuCatalogItem != null )
            {
                var items =
                    from c in UrlManager.CatalogItems.GetCatalogItems( menuCatalogItem.CatalogItemID, CatalogItemMenuType.HelpMenu )
                    select new
                    {
                        ID = c.CatalogItemID,
                        Name = c.CatalogItemName,
                        Url = UrlManager.GetCatalogUrl( c.CatalogItemID ),
                        IsSelected = CmsContext.Current.CatalogPath.Where( p => p.CatalogItemID == c.CatalogItemID ).Count() != 0
                    };

                helpMenuRepeater.DataSource = items;
                helpMenuRepeater.DataBind();
            }
        }
    }
}