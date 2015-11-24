using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ClientPicker : UserControl
	{
        public bool NoClientsFound { get; set; }

        public bool TooManyClientsFound { get; set; }

        protected void Page_Load( object sender, EventArgs e )
		{
			_txtClientName.Focus();
		}

		protected void _btnSearchClient_Click( object sender, EventArgs e )
		{
			try
			{
				var searchResults = ClientSearch.Search(
                    _txtClientName.Text.Trim(),
                    _txtPhone.Text.Trim(), 
                    ClientSearchMatching.Fuzzy );

				_rptSearchResults.DataSource = searchResults;
				_rptSearchResults.DataBind();
				NoClientsFound = false;
				TooManyClientsFound = false;
			}
			catch( AcctgException ex )
			{
                _rptSearchResults.DataSource = null;
                _rptSearchResults.DataBind();

                if( ex.ErrorCode == AcctgError.TooManyClientsFound )
					TooManyClientsFound = true;
				else if( ex.ErrorCode == AcctgError.NoDataToRespond )
					NoClientsFound = true;
				else
					throw ex;
			}
		}

        protected void _rptSearchResults_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataBound())
            {
                var info = (BriefClientInfo)e.Item.DataItem;

                if (!string.IsNullOrEmpty(info.ClientID))
                {
                    if (((ManagerSiteContext)SiteContext.Current).ClientSet.Contains(info.ClientID))
                    {
                        e.Item.FindControl("_lbAddToHandySet").Visible = false;
                        ((Label)e.Item.FindControl("_lblWarning")).Text = "Already in work";
                    }
                }
                else
                {
                    e.Item.FindControl("_lbAddToHandySet").Visible = false;
                    ((Label)e.Item.FindControl("_lblWarning")).Visible = false;
                    e.Item.FindControl("_lblNotActivated").Visible = string.IsNullOrEmpty(info.ClientID);
                    
                }
            }
        }

		protected void _rptSearchResults_ItemCommand( object source, RepeaterCommandEventArgs e )
		{
			var clientId = (string)e.CommandArgument;
			if( e.CommandName == "AddToHandySet" )
			{
                if (!string.IsNullOrEmpty(clientId))
                {
                    var context = (ManagerSiteContext)SiteContext.Current;
                    context.ClientSet.AddClient(clientId);
                    context.ClientSet.SetDefaultClient(clientId);
                    Response.Redirect(ClientProfile.GetUrl(), true);
                }
			}
	    }

        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }
	}
}