using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web
{
    public partial class PageTwoColumns : System.Web.UI.MasterPage
    {
        /*
         * Необходимо избегать в разметке конструкции <%= ... %>, т.к. в данном случае будет падать
         * каталог Laximo в модуле Laximo.Guayaquil.Render в методе AppendCSS при вызове Page.Header.Controls.Add(cssLink);
        */
        
        protected override void OnInit(EventArgs e)
        {
			base.OnInit(e);

			#region === разная версия скрипта диалога, для работоспособности на страницах Laximo ===
			string scriptKey = "__prepareDialog";
			if (Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
			{
				//зачищаем имеющийся скрипт (по идее сюда вообще никогда попадать не будем, но на всяк)
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, "");
			}
			if (Page.MasterPageFile.Contains("Laximo"))
			{
				/*
				 * Для корректной работы данного скрипта на страницах каталогов Laximo
				 * в него необходимо добавлять следующую конструкцию:
				 * $.noConflict(true);
				 * т.к. в на страницах каталогов Laximo присутствует версия jQuery 1.9.1
				 * а на всех наших страницах также присутствует jQuery 1.3.1
				 * Собственно в данном случае даже не важна версия, а важно то, что на одной и той же 
				 * странице дважды регистрируется библиотека jQuery ( что вызывает конфликт: .dialog(...) )
				 */
				//Добавляем версию для Laximo
				string scriptText = @"
				    
                    prepareDialog = function() { 						
						$.noConflict(true);
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
			else
			{
				//Добавляем версию для всех остальных
				string scriptText = @"
				    $(document).ready(function(){
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
				        $('.ui-widget-header').removeClass('ui-widget-header');};
                   });";

				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, scriptText, true);

			}
			#endregion

			//if ((ConfigurationManager.AppSettings["CountersOn"] ?? "").ToLower() == "true")
			//{
			//    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GoogleAnalytics", "<script src='https://www.google-analytics.com/ga.js' type='text/javascript'></script>");
			//}
            
			lnkMapUrl.HRef = /*lnkMapUrl2.HRef =*/ UrlManager.GetMapUrl();
			lnkFeedbackUrl.HRef = /*lnkFeedbackUrl2.HRef =*/ UrlManager.GetFeedbackUrl();
            lnkHeadUrl.HRef = ResolveUrl("~");
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

            if (SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager)
            {
                lnkHeadUrl.HRef = ResolveUrl("~/Manager/Default.aspx");
            }
            else
            {
                ImgTel.Src = ResolveUrl(AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].ImgTel);
                HeadImg.Src = ResolveUrl(AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].lnkHeadUrl);
            }

            //lnkShopListUrl.HRef = UrlManager.GetShopListUrl();
            lnkSeoPartsCatalogRootUrl.HRef = UrlManager.GetSeoPartsCatalogRootUrl();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind(); //добавил в рамках переделок для Laximo
            try
            {
				//BannerControlPos0.CatalogItemID = CmsContext.Current.CatalogItem.CatalogItemID;
				//BannerControlPos0.Position = 0;
				//BannerControlPos0.ShowImage = true;

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
			if (!SiteContext/*.Current*/.CurrentCulture.Equals(commandArgument))
			{
				SiteContext/*.Current*/.CurrentCulture = commandArgument;
                LogonService.Logoff();
				//Response.Redirect(Request.RawUrl);
			}
		}

        protected void PartnersCombo_DataBound(object sender, EventArgs e)
        {
        }

        protected void PartnersCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
