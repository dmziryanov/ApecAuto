using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Routing;
using System.Text;

namespace RmsAuto.Store.Web.Controls
{
    public partial class PageHeader : System.Web.UI.UserControl
    {
        public bool RenderCurrentNodeAsLink
		{
			get { return _breadCrumbs.RenderCurrentNodeAsLink; }
			set { _breadCrumbs.RenderCurrentNodeAsLink = value; }
		}
        
		protected string CurrentClientName
		{
			get 
			{
				try
				{
					return SiteContext.Current.CurrentClient.Profile.ClientName;
				}
				catch
				{
					return SiteContext.Current.User.Identity.Name;
				}
			}
		}

		protected string CurrentClientID
		{
			get
			{
				try
				{
					return /*"№ Клиента " +*/ SiteContext.Current.CurrentClient.Profile.ClientId;
				}
				catch
				{
					return /*"№ Клиента " +*/ SiteContext.Current.User.AcctgID;
				}
			}
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            RefreshHeaderControls();
        }

		protected void Page_PreRender( object sender, EventArgs e )
		{
			try
			{
				_cartCountLiteral.Text = SiteContext.Current.CurrentClientTotals.PartsCount.ToString();
				_cartSumLiteral.Text = string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClientTotals.Total );
                // deas 15.03.2011 task2626
                // добавлено заполнение текста о балансе и просроченной задолженности
                StringBuilder sbBalance = new StringBuilder("");
                if ( !SiteContext.Current.CurrentClient.IsGuest && SiteContext.Current.CurrentClient.Profile.Balance != 0 )
                {
                    sbBalance.Append("Ваша <b>");
                    if ( SiteContext.Current.CurrentClient.Profile.Balance > 0 )
                    {
                        sbBalance.Append( "задолженность: <b>" );
                        sbBalance.AppendFormat( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Profile.Balance );
                    }
                    else
                    {
                        sbBalance.Append( "предоплата: <b>" );
                        sbBalance.AppendFormat( "{0:### ### ##0.00}", 0M - SiteContext.Current.CurrentClient.Profile.Balance );
                    }
                    if ( SiteContext.Current.CurrentClient.Profile.DelayCredit > 0 )
                    {
                        sbBalance.Append( "</b> р. просрочено: <b>" );
                        sbBalance.AppendFormat( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Profile.DelayCredit );
                    }
                    sbBalance.Append( "</b> р." );
					sbBalance.Append( string.Format( "<br /> <font size='1'>(Данные на: {0})</font>",
						SiteContext.Current.CurrentClient.Profile.BalanceDate.ToString( "dd.MM.yyyy hh:mm" ) ) );

                   
                }

                if (SiteContext.Current.InternalFranchName != "rmsauto")
                {
                    if (!string.IsNullOrEmpty(SiteContext.Current.CurrentClient.Profile.ManagerName))
                    { sbBalance.Append(string.Format("<br /> <font size='1'>(Ваш менеджер: {0})</font>", SiteContext.Current.CurrentClient.Profile.ManagerName)); }
                    else
                    { sbBalance.Append(string.Format("<br /> <font size='1'>(Ваш менеджер: {0})</font>", "не назначен")); }
                }

                _balanceInfoLiteral.Text = sbBalance.ToString();

				_cartPlaceHolder.Visible = true;
			}
			catch(Exception ex)
			{
				Logger.WriteException(ex);
				_cartPlaceHolder.Visible = false;
			}
		}
        protected void _btnLogoff_Click(object sender, EventArgs e)
        {
            LogonService.Logoff();
        }

        protected void _btnLogon_Click(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        private void RefreshHeaderControls()
        {
            bool isAuthenticated = Context.User.Identity.IsAuthenticated;
            
            _btnLogon.Visible = !isAuthenticated;
            _btnLogoff.Visible = isAuthenticated;
            
            _btnViewCartLink.Visible = !Context.User.IsInRole("Manager");
            _btnViewCartInfo.Visible = !Context.User.IsInRole("Manager");
            _imgViewCart.Visible = !Context.User.IsInRole("Manager");
            			
            _btnViewCartLink.NavigateUrl = UrlManager.GetCartUrl();
            _imgViewCartLink.NavigateUrl = UrlManager.GetCartUrl();
            _btnViewCabinetLink.NavigateUrl = UrlManager.GetPrivateOfficeUrl();
        }
    }
}