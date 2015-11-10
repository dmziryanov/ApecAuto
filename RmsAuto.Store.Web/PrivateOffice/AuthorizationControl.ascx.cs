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
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using System.ComponentModel;
using System.Net;

namespace RmsAuto.Store.Web.PrivateOffice
{
	public partial class AuthorizationControl : System.Web.UI.UserControl
	{
		public SecurityRolesEnum Role { get; set; }

		[Flags]
		public enum SecurityRolesEnum
		{
			None = 0,
			Anonymous = 1,
			Client = 2,
			Manager = 4,
			ClientOrAnonymous = Client | Anonymous,
			ClientOrManager = Client | Manager,
			Any = Client | Manager | Anonymous
		}

		protected void Page_Init( object sender, EventArgs e )
		{

			CustomPrincipal principal = HttpContext.Current.User as CustomPrincipal;

			bool access = ( Role & SecurityRolesEnum.Anonymous ) != 0 && principal == null
				|| ( Role & SecurityRolesEnum.Client ) != 0 && principal != null && principal.Role == SecurityRole.Client
				|| ( Role & SecurityRolesEnum.Manager ) != 0 && principal != null && principal.Role == SecurityRole.Manager;

			if(!access)
			{
				if(!Page.User.Identity.IsAuthenticated)
				{
					FormsAuthentication.RedirectToLoginPage();
					Response.End();
				}
				else
				{
					throw new HttpException( (int)HttpStatusCode.Forbidden, "Нет доступа" );
				}
			}
		}
	}
}