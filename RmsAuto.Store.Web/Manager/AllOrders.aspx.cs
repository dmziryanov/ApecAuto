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
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using System.Net;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class AllOrders : RMMPage, IPageUrlState
    {
        public static string GetUrl()
        {
            return "~/Manager/AllOrders.aspx";
        }

   

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!((ManagerSiteContext)SiteContext.Current).ClientDataSectionEnabled(ClientDataSection.AllOrders))
                throw new HttpException((int)HttpStatusCode.Forbidden, "Access Denied");

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["view"]))
                {
                    RmsAuto.Store.Web.PrivateOffice.OrderList.Views view = (RmsAuto.Store.Web.PrivateOffice.OrderList.Views)int.Parse(Request["view"]);
                }
                else
                {


                }

            }

            //UrlStateContainer["view"] = Convert.ToString(_multiView.ActiveViewIndex);
        }

        protected void _multiView_ActiveViewChanged(object sender, EventArgs e)
        {
         //   UrlStateContainer["view"] = Convert.ToString(_multiView.ActiveViewIndex);
        }


        #region IPageUrlState Members

        public UrlStateContainer UrlStateContainer
        {
            get { return _urlStateContainer; }
        }
        UrlStateContainer _urlStateContainer;

        protected override void OnPreInit(EventArgs e)
        {
            _urlStateContainer = new UrlStateContainer(Request.Url);

            base.OnPreInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            Form.Action = UrlStateContainer.GetBasePageUrl();

            base.Render(writer);
        }

        #endregion
    }
}
