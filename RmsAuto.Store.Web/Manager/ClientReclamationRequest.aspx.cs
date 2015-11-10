using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientReclamationRequest : ClientBoundPage
	{
		public override ClientDataSection DataSection
		{
			get { return ClientDataSection.Reclamations; }
		}

		protected void Page_Load( object sender, EventArgs e )
		{

		}
	}
}
