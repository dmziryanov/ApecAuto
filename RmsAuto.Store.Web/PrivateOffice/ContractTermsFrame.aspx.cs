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
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Entities;
using System.Net;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class ContractTermsFrame : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
            TextItem item;
            
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
            {
                item = TextItemsDac.GetTextItem(SiteContext.Current.InternalFranchName+".RetailContractTerms.Text", SiteContext/*.Current*/.CurrentCulture);
            }
            else
            {
                item = TextItemsDac.GetTextItem("RetailContractTerms.Text", SiteContext/*.Current*/.CurrentCulture);
            }

			if (item == null)
				throw new HttpException((int)HttpStatusCode.NotFound, "Not found" );

			_contractTermsLiteral.Text = item.TextItemBody;
		}
	}
}
