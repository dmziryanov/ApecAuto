using System;
using System.Configuration;
using System.Web.UI;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public partial class LeftMenu : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager && Page.MasterPageFile == "/Manager/ManagerNEW.Master")
            {

                this.Visible = false;
            }
			//if (RmsAuto.Acctg.Diagnostics.Trace.Enabled)
			//    _pingControlPlaceHolder.Controls.Add(
			//        Page.LoadControl("~/Controls/AcctgPing.ascx"));
        }

        protected string liveInternetLabel, googleScript, yandexScript;
        protected void Page_Load(object sender, EventArgs e)
        {
            //phVisa.Visible = false;
            //try
            //{
            //    if (SiteContext.Current.InternalFranchName == "rmsauto")
            //    {
            //        phVisa.Visible = true;
            //    }
            //}
            //catch { }
/*<script type="text/javascript">
 (function() {
 document.write("<a href='http://www.liveinternet.ru/click' target=_blank> <img src='http://counter.yadro.ru/hit?t15.10;r" + escape(document.referrer) + ((typeof (screen) == "undefined") ? "" :
 ";s" + screen.width + "*" + screen.height + "*" + (screen.colorDepth ? screen.colorDepth : screen.pixelDepth)) + ";u" + escape(document.URL) +
 ";h" + escape(document.title.substring(0, 80)) + ";" + Math.random() + "' alt='' title='LiveInternet: показано число просмотров за 24" +
 " часа, посетителей за 24 часа и за сегодня' border='0' width='88' height='31'>")
 })();
 </script>*/
            bool fl = (ConfigurationManager.AppSettings["CountersOn"] ?? "").ToLower() == "true";
            liveInternetLabel = fl ?
                "<script type='text/javascript'>" +
                "(function() {" +
                "document.write("+new String((char)34, 1)+"<a href='http://www.liveinternet.ru/click' target=_blank> <img src='http://counter.yadro.ru/hit?t15.10;r + escape(document.referrer) + ((typeof (screen) == 'undefined') ? '' :" +
                "';s' + screen.width * screen.height * (screen.colorDepth ? screen.colorDepth : screen.pixelDepth)) + ;u + escape(document.URL)" +
                ";h + escape(document.title.substring(0, 80)) + ';' + Math.random() + ' alt='' title='LiveInternet: показано число просмотров за 24" +
                " часа, посетителей за 24 часа и за сегодня' border='0' width='88' height='31'>" + new String((char)34, 1) + ")" +
                "})();" +
                "</script>" : String.Empty;
         //   Page.ClientScript.RegisterClientScriptBlock(this.GetType(), liveInternetLabel, liveInternetLabel);

            fl = (ConfigurationManager.AppSettings["CountersOn"] ?? "").ToLower() == "true";
            googleScript = fl ? @"<script type='text/javascript>
                                var _gaq = _gaq || [];
                                gaq.push(['_setAccount', 'UA-22238226-1']);
                                gaq.push(['_trackPageview']);
                                (function() {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
            })();</script>" : String.Empty;
           // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), googleScript, googleScript);

            fl = (ConfigurationManager.AppSettings["CountersOn"] ?? "").ToLower() == "true";
            yandexScript = fl ? @"
            <script src='//mc.yandex.ru/metrika/watch.js' type='text/javascript'></script>
            <div style='display:none;'>
    
            <script type='text/javascript'>
            try {
                    var yaCounter810429 = new Ya.Metrika(810429);
                    yaCounter810429.clickmap(true);
                    yaCounter810429.trackLinks(true);
                } catch (e) { }
            </script></div
            <noscript><div><img src='//mc.yandex.ru/watch/810429' style='position:absolute; left:-9999px;' alt='' /></div></noscript>
                " : String.Empty;
          //  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), yandexScript, yandexScript);

			//Для Омска регитрируем скрипт для системы онлайн-консультант LiveTex
			if (SiteContext.Current.InternalFranchName == "omsk")
			{
				string script = @"
					var liveTex = true,
							liveTexID = 56164,
							liveTex_object = true;
						(function() {
							var lt = document.createElement('script');
							lt.type ='text/javascript';
							lt.async = true;
							lt.src = 'http://cs15.livetex.ru/js/client.js';
							var sc = document.getElementsByTagName('script')[0];
							sc.parentNode.insertBefore(lt, sc);
						})();";
				if (!Page.ClientScript.IsClientScriptBlockRegistered("LiveTex"))
				{
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LiveTex", script, true);
				}
			}
		}
    }
}