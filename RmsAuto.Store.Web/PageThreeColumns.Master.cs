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
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web
{
    public partial class PageThreeColumns : System.Web.UI.MasterPage
    {
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
            
			string scriptKey = "__prepareDialog";
			if (Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
			{
				//зачищаем имеющийся скрипт (по идее сюда вообще никогда попадать не будем, но на всяк)
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, "");
			}
			string scriptText = @";
				    prepareDialog = function() { 						
						$('#RegionList').dialog({ title: 'Ваш город',
				            position: [260, 210],
				            autoOpen: false,
				            resizable: false,
				            height: 500,
				            width: '678px',
				            modal: true
				           });
				        $('.ui-dialog').addClass('dialogWithDropShadow');
				        $('.ui-dialog').addClass('overlay');
				        $('.ui-widget-header').removeClass('ui-widget-header');}";
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, scriptText, true);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
         
            //CmsContext.Current.CatalogItem = UrlManager.CatalogItems.RootCatalogItem;
            if (SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager)
            {
                lnkHeadUrl.HRef = ResolveUrl("~/Manager/Default.aspx");
            }

			//if ((ConfigurationManager.AppSettings["CountersOn"] ?? "").ToLower() == "true")
			//{
			//    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GoogleAnalytics", "<script src='https://www.google-analytics.com/ga.js' type='text/javascript'></script>");
			//}
            //lnkShopListUrl.HRef = UrlManager.GetShopListUrl();


			//if (SiteContext.Current.InternalFranchName == "rmsauto")
			//{
			//    RmsOfficeUrl.Visible = true;
			//    lnkShopListUrl.Visible = false;
			//}
			//else
			//{
			//    lnkShopListUrl.Visible = true;
			//    RmsOfficeUrl.Visible = false;
			//}


            try
            {
                //var cI = UrlManager.CatalogItems.GetCatalogItem(CmsContext.Current.CatalogItem.CatalogItemID);
                BannerControlPos0.CatalogItemID = CmsContext.Current.CatalogItem.CatalogItemID;
                BannerControlPos0.Position = 0;
                BannerControlPos0.ShowImage = true;
				//BannerControlPos1.CatalogItemID = CmsContext.Current.CatalogItem.CatalogItemID;
				//BannerControlPos1.Position = 1;
				//BannerControlPos1.ShowImage = true;
                ImgTel.Src = ResolveUrl(AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].ImgTel);
                HeadImg.Src = ResolveUrl(AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].lnkHeadUrl);
				// Если флага в настройках нет то переключатель локали просто не показываем (по дефолту Visible=false) Daniil 29.04.2011
				phFlags.Visible = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLocalization"]);
            }
            catch (Exception)
            {
                // если баннер отобразить не получилось, не значит что надо падать
            }

         
        }

		protected void ibLang_Click(object sender, ImageClickEventArgs e)
		{
			// Переключаем текущую локаль
			string commandArgument = ((ImageButton)sender).CommandArgument;
			if (!SiteContext.Current/*.CurrentCulture*/.Equals(commandArgument))
			{
				SiteContext/*.Current*/.CurrentCulture = commandArgument;
                LogonService.Logoff();
				//Response.Redirect(Request.RawUrl);
			}
		}

        protected void PartnersCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Redirect(UrlManager.MakeAbsoluteUrl("/FranchPartners.aspx?Region=" + PartnersCombo.Text), false);
         
        }

        protected void PartnersCombo_DataBound(object sender, EventArgs e)
        {
           
        }
    }
}
