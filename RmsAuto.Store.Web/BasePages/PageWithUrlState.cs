using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using RmsAuto.Common.Web.UrlState;

namespace RmsAuto.Store.Web.BasePages
{
	public class PageWithUrlState : LocalizablePage, IPageUrlState 
	{

		public UrlStateContainer UrlStateContainer
		{
			get { return _urlStateContainer; }
		}
		UrlStateContainer _urlStateContainer;

		protected override void OnPreInit( EventArgs e )
		{
			_urlStateContainer = new UrlStateContainer( Request.Url );

			base.OnPreInit( e );
		}

		protected override void Render( HtmlTextWriter writer )
		{
			Form.Action = UrlStateContainer.GetBasePageUrl();

			base.Render( writer );
		}
	}
}
