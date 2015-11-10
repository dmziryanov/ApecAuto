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

namespace RmsAuto.Store.Web.Manager
{
    public partial class Manager : System.Web.UI.MasterPage
    {
		string CurrentClientId
		{
			get { return (string)ViewState[ "CurrentClientId" ]; }
			set { ViewState[ "CurrentClientId" ] = value; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			if( HttpContext.Current.Session != null )
			{
				//Защита от изменения текущего клиента при выполнении сабмита формы.
				//переходы по ссылкам разрешены.
				string clientId = !SiteContext.Current.CurrentClient.IsGuest ? SiteContext.Current.CurrentClient.Profile.ClientId : string.Empty;
				if( CurrentClientId != null && clientId != CurrentClientId )
				{
					throw new Exception( "Состояние рабочего места менеджера было изменено в другом окне" );
				}
			}
        }

		protected void Page_PreRender( object sender, EventArgs e )
		{
			if( HttpContext.Current.Session != null )
			{
				CurrentClientId = !SiteContext.Current.CurrentClient.IsGuest ? SiteContext.Current.CurrentClient.Profile.ClientId : string.Empty;
			}
		}
	}
}
