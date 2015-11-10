using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web
{
    public partial class ActivateUserAccount : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected UserMaintEntryPurpose MaintEntryPurpose
		{
			get { return (UserMaintEntryPurpose)(int)ViewState[ "__entryPurpose" ]; }
			set { ViewState[ "__entryPurpose" ] = (int)value; }
		}

        protected string InternalFranchName
        {
            get { return (string)Request.QueryString[UrlKeys.Activation.FranchCode]; }
            set {  }
        }

		protected void Page_Load( object sender, EventArgs e )
		{
			if( Page.User.Identity.IsAuthenticated )
				Response.Redirect( "~/Default.aspx", true );

            //Если есть внутренее имя франча, то берем его из куки
          

			
            if( !IsPostBack )
			{
				
                _errorPanel.Visible = false;
				Guid maintUid;
				try
				{
					maintUid = ParseActivationCode();
					MaintEntryPurpose = ClientBO.GetUserMaintEntryPurpose( maintUid );
				}
				catch( BLException ex )
				{
					DisplayError( ex.Message );
					return;
				}

				switch( MaintEntryPurpose )
				{
					case UserMaintEntryPurpose.NewClientRegistration:
						{
							try
							{
								_maintTaskPanel.Visible = false;
								var clientId = ClientBO.CompleteOnlineRegistration( maintUid );
								try
								{
									var profile = ClientProfile.Load( clientId );
									if( profile.TradingVolume == TradingVolume.Retail )
										_newClientRegistration_TextItemControl.TextItemID = "UserAccountActivated.Retail.Text";
									else if( profile.TradingVolume == TradingVolume.Wholesale )
										_newClientRegistration_TextItemControl.TextItemID = "UserAccountActivated.Wholesale.Text";
								}
								catch
								{
								}
								_messagePanel.Visible = true;
							}
							catch( Acctg.AcctgException ex )
							{
								if( ex.ErrorCode == RmsAuto.Store.Acctg.AcctgError.ClientAlreadyExists )
									DisplayError( "Ошибка активации. Регистрационные данные уже используются другим клиентом интернет-магазина" );
								else
									throw ex;
							}
						}
						break;
                    case UserMaintEntryPurpose.ActivateClient:
					case UserMaintEntryPurpose.ExistingClientWebAccess:
					case UserMaintEntryPurpose.PasswordRecovery:
						_maintTaskPanel.Visible = true;
						_messagePanel.Visible = false;
						_editUser.Visible = MaintEntryPurpose == UserMaintEntryPurpose.ExistingClientWebAccess || MaintEntryPurpose == UserMaintEntryPurpose.ActivateClient;
						_setPassword.Visible = MaintEntryPurpose == UserMaintEntryPurpose.PasswordRecovery;
						break;
				}
			}
		}

		protected void _btnApply_Click( object sender, EventArgs e )
		{
			if( IsValid )
			{
				var maintUid = ParseActivationCode();
				if( MaintEntryPurpose == UserMaintEntryPurpose.ExistingClientWebAccess )
				{
					var clientId = ClientBO.AcceptOnlineAccessOffer( maintUid, _editUser.Username, _editUser.Password );
					try
					{
						var profile = ClientProfile.Load( clientId );
						if( profile.TradingVolume == TradingVolume.Retail )
							_existingClientWebAccess_TextItemControl.TextItemID = "UserAccountActivated.Retail.Text";
						else if( profile.TradingVolume == TradingVolume.Wholesale )
							_existingClientWebAccess_TextItemControl.TextItemID = "UserAccountActivated.Wholesale.Text";
					}
					catch
					{
					}
                }
                else if ( MaintEntryPurpose == UserMaintEntryPurpose.ActivateClient )
                {
                    var clientId = ClientBO.AcceptOnlineAccessOffer( maintUid, _editUser.Username, _editUser.Password );
                    try
                    {
                        var profile = ClientProfile.Load( clientId );
                        if ( profile.TradingVolume == TradingVolume.Retail )
                            _existingClientWebAccess_TextItemControl.TextItemID = "UserAccountActivated.Retail.Text";
                        else if ( profile.TradingVolume == TradingVolume.Wholesale )
                            _existingClientWebAccess_TextItemControl.TextItemID = "UserAccountActivated.Wholesale.Text";
                    }
                    catch
                    {
                        _existingClientWebAccess_TextItemControl.TextItemID = "UserAccountActivated.NotClientId";
                    }
                }
                else if ( MaintEntryPurpose == UserMaintEntryPurpose.PasswordRecovery )
                {
                    ClientBO.ResetPassword( maintUid, _setPassword.PasswordValue );
                }
				_maintTaskPanel.Visible = false;
				_messagePanel.Visible = true;
			}
		}

		private Guid ParseActivationCode()
		{
			var activationCode = Request[ UrlKeys.Activation.MaintUid ];
			if( string.IsNullOrEmpty( activationCode ) )
				throw new BLException( "Не найден код активации" );
			try
			{
				return new Guid( activationCode );
			}
			catch
			{
				throw new BLException( "Некорректный код активации" );
			}
		}

		private void DisplayError( string errorMessage )
		{
			_maintTaskPanel.Visible = false;
			_messagePanel.Visible = false;

			_errorMessage.InnerHtml = errorMessage.Replace( "\n", "<br/>" );
			_errorPanel.Visible = true;
		}

        

      
	}
}
