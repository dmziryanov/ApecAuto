using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.Mail;
using RmsAuto.Store.Cms.Mail.Messages;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;
using System.Security.Cryptography;
using System.Configuration;


namespace RmsAuto.Store.BL
{
	public static class ClientBO
	{

        private static string ComputeHash(MD5 hasher, string input)
        {
            var hash = hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
        
        public static string GetMD5Hash(this string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            using (MD5 hasher = MD5.Create())
            {
                
                    return ComputeHash(hasher, ComputeHash(hasher, input));
                
            }
        }
        
        private static Boolean expiredAllowDefault = false;

        ///Это вроде не надо
        //public static void RegisterClient(ClientRegistrationData regData, Func<Guid, string> makeActivationUrl)
        //{
        //    if( regData == null )
        //        throw new ArgumentNullException( "regData" );
        //    if( makeActivationUrl == null )
        //        throw new ArgumentNullException( "makeActivationUrl" );

        //    var maintUid = Guid.NewGuid();

        //    UserMaintDac.AddEntry(
        //        new UserMaintEntry
        //        {
        //            EntryUid = maintUid,
        //            EntryPurpose = UserMaintEntryPurpose.NewClientRegistration,
        //            RegistrationData = regData,
        //            Username = regData.Login,
        //        } );

        //    MailEngine.SendMail(
        //        new MailAddress( regData.Email, regData.ClientCategory == ClientCategory.Legal ? regData.ContactName : regData.ClientName ),
        //        new ClientActivationAlert
        //        {
        //            ActivationUrl = makeActivationUrl( maintUid ),
        //            DaysToLive = GetDaysToLive( UserMaintEntryPurpose.NewClientRegistration ).GetValueOrDefault()
        //        } );
        //}

        //deas 29.03.2011 task3563
        // регистрация клиента с расширенным профилем
        public static void RegisterClient( ClientRegistrationDataExt regDataExt, Func<Guid, string> makeActivationUrl )
        {
            if ( regDataExt == null )
                throw new ArgumentNullException( "regData" );
            if ( makeActivationUrl == null )
                throw new ArgumentNullException( "makeActivationUrl" );

            var maintUid = Guid.NewGuid();

            UserMaintDac.AddEntry(
                new UserMaintEntry
                {
                    EntryUid = maintUid,
                    EntryPurpose = UserMaintEntryPurpose.NewClientRegistration,
                    RegistrationDataExt = regDataExt,
                    Username = regDataExt.Login,
                } );

            //deas 18.04.2011 task3846
            // рассылка писем в зависимости от объемов закупок клиента
            switch ( regDataExt.TradingVolume )
            {
                case TradingVolume.Wholesale:
                    MailEngine.SendMail(
                        new MailAddress( regDataExt.Email, regDataExt.ClientName ),
                        new ClientActivationAlertWhSl
                        {
                            ActivationUrl = makeActivationUrl( maintUid ),
                            DaysToLive = GetDaysToLive( UserMaintEntryPurpose.NewClientRegistration ).GetValueOrDefault()
                        } );
                    break;
                case TradingVolume.Retail:
                    MailEngine.SendMail(
                        new MailAddress( regDataExt.Email, regDataExt.ClientName ),
                        new ClientActivationAlertRet
                        {
                            ActivationUrl = makeActivationUrl( maintUid ),
                            DaysToLive = GetDaysToLive( UserMaintEntryPurpose.NewClientRegistration ).GetValueOrDefault(),
                            RetailUrl = UrlManager.MakeAbsoluteUrl( "/Roz.aspx" )
                        } );
                    break;
                default:
                    //старый код
                    MailEngine.SendMail(
                        new MailAddress( regDataExt.Email, regDataExt.ClientName ),
                        new ClientActivationAlertWhSl
                        {
                            ActivationUrl = makeActivationUrl( maintUid ),
                            DaysToLive = GetDaysToLive( UserMaintEntryPurpose.NewClientRegistration ).GetValueOrDefault()
                        } );
                    break;
            }
        }

		public static void ResendActivationEmailForNewClient(
			Guid maintUid,
			Func<Guid, string> makeActivationUrl )
		{
			if( makeActivationUrl == null )
				throw new ArgumentNullException( "makeActivationUrl" );

			var entry = UserMaintDac.GetEntry( maintUid );

			if( entry == null )
				throw new InvalidOperationException( "User maint entry not found" );

			if( entry.EntryPurpose != UserMaintEntryPurpose.NewClientRegistration )
				throw new InvalidOperationException( "Can send activation more than once for NewClientRegistration purpose only" );

			UserMaintDac.SetEntryTime( maintUid, DateTime.Now );
			MailEngine.SendMail(
				new MailAddress( entry.RegistrationData.Email, entry.RegistrationData.ClientCategory == ClientCategory.Legal ? entry.RegistrationData.ContactPersonSurname : entry.RegistrationData.ClientName ),
                new ClientActivationAlertWhSl
				{
					ActivationUrl = makeActivationUrl( maintUid ),
					DaysToLive = GetDaysToLive( UserMaintEntryPurpose.NewClientRegistration ).GetValueOrDefault()
				} );
		}

        //public static string CreateClient( ClientRegistrationData regData, string managerId, bool isRestricted )
        //{
        //    if( regData == null )
        //        throw new ArgumentNullException( "regData" );
        //    return ClientProfile.Create(
        //                regData.ClientName,
        //                regData.ClientCategory,
        //                regData.TradingVolume,
        //                regData.ShippingAddress,
        //                regData.RmsStoreId,
        //                regData.Email,
        //                regData.ContactName,
        //                regData.MainPhone,
        //                regData.AuxPhone1,
        //                regData.AuxPhone2,
        //                regData.FieldOfAction,
        //                regData.DiscountCardNumber,
        //                managerId,
        //                isRestricted ).ClientId;
        //}

        //deas 29.03.2011 task3563
        //новая процедура регистрации клиента с измененным профилем
        /// <summary>
        /// Регистрация клиента с расширенным профилем
        /// </summary>
        /// <param name="regDataExt">Профиль клиента</param>
        /// <param name="managerId">Код менеджера</param>
        /// <param name="isRestricted">Ограниченный доступ</param>
        /// <returns>Код 1С клиента</returns>
        public static int CreateClient( ClientRegistrationDataExt regDataExt, string managerId, bool isRestricted, string InternalFranchName)
        {
            dcCommonDataContext DC = new dcCommonDataContext();
            if ( regDataExt == null )
                throw new ArgumentNullException( "regDataExt" );
            return ClientProfile.Create(
                "","",
                regDataExt.ClientName,
                regDataExt.Email,
                regDataExt.TradingVolume,
                regDataExt.ClientCategory,
                regDataExt.CountryID,
                regDataExt.Locality,
				regDataExt.RmsStoreID,
				regDataExt.ScopeType,
				regDataExt.DiscountCardNumber,
				regDataExt.HowKnow,
				managerId,
                isRestricted,
                InternalFranchName,
                
				regDataExt.ContactPersonPosition,
				regDataExt.ContactPersonName,
				regDataExt.ContactPersonSurname,
                regDataExt.ContactPersonPhone,
                regDataExt.ContactPersonExtPhone,
                regDataExt.ContactPersonFax,
				regDataExt.ContactPersonEmail,
                regDataExt.DeliveryAddress,
                
				regDataExt.CompanyName,
				regDataExt.CompanyRegistrationID,
				regDataExt.CompanyAddress,

                regDataExt.BankName,
				regDataExt.IBAN,
				regDataExt.SWIFT,
				regDataExt.BankAddress,

                regDataExt.DirectorName,
				regDataExt.DirectorSurname,
                
				regDataExt.CorrespondentBankName,
				regDataExt.CorrespondentIBAN,
				regDataExt.CorrespondentSWIFT,
				regDataExt.CorrespondentBankAddress,
                regDataExt.RegisterAs).UserId;
        }

        public static void OfferOnlineAccess( string clientId, string email, Func<Guid, string> makeActivationUrl )
        {
            if ( string.IsNullOrEmpty( clientId ) )
                throw new ArgumentException( "ClientId cannot be empty", "clientId" );
            if ( string.IsNullOrEmpty( email ) )
                throw new ArgumentException( "Email cannot be empty", "email" );
            if ( makeActivationUrl == null )
                throw new ArgumentNullException( "makeActivationUrl" );

            var maintUid = Guid.NewGuid();

            var profile = ClientProfile.Load( clientId );
            UserMaintDac.AddEntry(
                    new UserMaintEntry
                    {
                        EntryUid = maintUid,
                        EntryPurpose = UserMaintEntryPurpose.ExistingClientWebAccess,
                        ClientID = clientId,
						UserID = profile.UserId,
                        RegistrationData = new ClientRegistrationDataExt { Email = email }
                    } );
            MailEngine.SendMail(
            new MailAddress( email, profile.Category == ClientCategory.Legal ? profile.ContactPerson : profile.ClientName ),
            new ClientActivationAlertWhSl
            {
                ActivationUrl = makeActivationUrl( maintUid ),
                DaysToLive = GetDaysToLive( UserMaintEntryPurpose.ExistingClientWebAccess ).GetValueOrDefault()
            } );
        }

        // deas 06.04.2011 task3696
        // добавление для нового профиля регистрации
        public static void OfferOnlineAccessExt( int userID, string clientName, string email, Func<Guid, string> makeActivationUrl )
        {
            if ( string.IsNullOrEmpty( email ) )
                throw new ArgumentException( "Email cannot be empty", "email" );
            if ( makeActivationUrl == null )
                throw new ArgumentNullException( "makeActivationUrl" );

            var maintUid = Guid.NewGuid();
            UserMaintDac.AddEntry(
                    new UserMaintEntry
                    {
                        EntryUid = maintUid,
                        EntryPurpose = UserMaintEntryPurpose.ActivateClient,
                        UserID = userID,
                        RegistrationDataExt = new ClientRegistrationDataExt { Email = email }
                    } );
            MailEngine.SendMail(
            new MailAddress( email, clientName ),
            new ClientActivationAlertWhSl
            {
                ActivationUrl = makeActivationUrl( maintUid ),
                DaysToLive = GetDaysToLive( UserMaintEntryPurpose.ExistingClientWebAccess ).GetValueOrDefault()
            } );
        }

        public static UserMaintEntryPurpose GetUserMaintEntryPurpose(Guid maintUid, Boolean expiredAllow)
		{
			var maintEntry = GetEntry( maintUid, expiredAllow );
            //deas 05.05.2011 task4038
            // изменение определения наличия записи клиента на сайте
            if ( maintEntry.EntryPurpose == UserMaintEntryPurpose.ExistingClientWebAccess &&
                !string.IsNullOrEmpty( UserDac.GetUserByClientId( maintEntry.ClientID ).Username ) )
                throw new BLException( "Клиент уже имеет учетную запись пользователя сайта" );
			return maintEntry.EntryPurpose;
		}

        public static UserMaintEntryPurpose GetUserMaintEntryPurpose(Guid maintUid)
        {
            return GetUserMaintEntryPurpose(maintUid, expiredAllowDefault);
        }


        public static string CompleteOnlineRegistration(Guid maintUid, Boolean expiredAllow, string InternalFranchName)
		{
			var maintEntry = GetEntry(maintUid, expiredAllow);

                if ( maintEntry.EntryPurpose != UserMaintEntryPurpose.NewClientRegistration ||
                    maintEntry.RegistrationDataExt == null )
                    throw new BLException( "Некорректное состояние регистрационных данных" );

                var rd = maintEntry.RegistrationDataExt;

				var clientId = ClientProfile.Create(
				rd.Login,
				rd.Password.GetMD5Hash(),
				rd.ClientName,
				rd.Email,
				rd.TradingVolume,
				rd.ClientCategory,
				rd.CountryID,
				rd.Locality,
				rd.RmsStoreID,
				rd.ScopeType,
				rd.DiscountCardNumber,
				rd.HowKnow,
				string.Empty, //managerID
				false, //isRestricted
				InternalFranchName,

				rd.ContactPersonPosition,
				rd.ContactPersonName,
				rd.ContactPersonSurname,
				rd.ContactPersonPhone,
				rd.ContactPersonExtPhone,
				rd.ContactPersonFax,
				rd.ContactPersonEmail,
				rd.DeliveryAddress,

				rd.CompanyName,
				rd.CompanyRegistrationID,
				rd.CompanyAddress,

				rd.BankName,
				rd.IBAN,
				rd.SWIFT,
				rd.BankAddress,

				rd.DirectorName,
				rd.DirectorSurname,

				rd.CorrespondentBankName,
				rd.CorrespondentIBAN,
				rd.CorrespondentSWIFT,
				rd.CorrespondentBankAddress,
                rd.RegisterAs
				).ClientId;
                
                using (var mtx = new DCFactory<StoreDataContext>())
                {
                    UserMaintDac.DeleteEntry(maintEntry.EntryUid, mtx.DataContext);
                }

                return clientId;
		}

        public static string CompleteOnlineRegistration(Guid maintUid)
        {
            return CompleteOnlineRegistration(maintUid, expiredAllowDefault, SiteContext.Current.InternalFranchName);
        }

        public static string AcceptOnlineAccessOffer(Guid maintUid, string username, string password, Boolean expiredAllow)
		{
            //var maintEntry = GetEntry(maintUid, expiredAllow);
            //if (maintEntry.EntryPurpose != UserMaintEntryPurpose.ActivateClient && maintEntry.EntryPurpose != UserMaintEntryPurpose.ExistingClientWebAccess ||
            //    string.IsNullOrEmpty(maintEntry.ClientID))
            //    throw new BLException("Некорректное состояние регистрационных данных");

            //var regData = maintEntry.RegistrationData;
            //if(regData == null || string.IsNullOrEmpty(regData.Email))
            //    throw new BLException("Некорректное состояние регистрационных данных");

            //if (UserDac.GetUserByClientId(maintEntry.ClientID) != null && !string.IsNullOrEmpty(UserDac.GetUserByClientId(maintEntry.ClientID).Username))
            //    throw new BLException("Учетная запись сайта уже существует для данного клиента");

            var maintEntry = GetEntry(maintUid, expiredAllow);

            if (maintEntry.EntryPurpose != UserMaintEntryPurpose.ExistingClientWebAccess && !maintEntry.UserID.HasValue)
                throw new BLException("Некорректное состояние регистрационных данных");

            var regData = maintEntry.RegistrationDataExt;
            if (regData == null || string.IsNullOrEmpty(regData.Email))
                throw new BLException("Некорректное состояние регистрационных данных");

            if (!string.IsNullOrEmpty(UserDac.GetUserByUserId(maintEntry.UserID.Value).Username))
                throw new BLException("Учетная запись сайта уже существует для данного клиента");

            string clientId = UserDac.GetUserByUserId(maintEntry.UserID.Value).AcctgID;
            
			using(var mtx = new DCFactory<StoreDataContext>(System.Data.IsolationLevel.ReadCommitted,false, null, true))
			{
                
                mtx.DataContext.spUpdUsersNamePswd( /*maintEntry.ClientID*/clientId, username, password.GetMD5Hash());
                //UserDac.AddUser(
                //    new User
                //    {
                //        Username = username,
                //        Password = password.GetMD5Hash(),
                //        Role = SecurityRole.Client,
                //        AcctgID = maintEntry.ClientID
                //    },
                //    mtx.DataContext );

				UserMaintDac.DeleteEntry(maintEntry.EntryUid, mtx.DataContext);

                mtx.DataContext.spUpdUserEmail(maintEntry.ClientID, maintEntry.RegistrationData.Email);
				mtx.SetCommit();
			}
            //Здесь возращаем не  maintEntry.ClientId, а clientId, так как он еще не заполнен в UserMaintEntries
            return clientId;
		}

        public static string AcceptOnlineAccessOffer(Guid maintUid, string username, string password)
        {
            return AcceptOnlineAccessOffer(maintUid, username, password, expiredAllowDefault);
        }

	    public static void ResetPassword( Guid maintUid, string password, Boolean expiredAllow )
		{
			if( string.IsNullOrEmpty( password ) )
				throw new ArgumentException( "Password cannot be empty", "password" );

			var maintEntry = GetEntry( maintUid, expiredAllow );
			if( maintEntry.EntryPurpose != UserMaintEntryPurpose.PasswordRecovery ||
				string.IsNullOrEmpty( maintEntry.ClientID ) )
				throw new BLException( "Некорректное состояние регистрационных данных" );

			var user = UserDac.GetUserByClientId( maintEntry.ClientID );
			if( user == null )
				throw new BLException( "Учетная запись пользователя сайта не найдена" );

            using (var mtx = new DCFactory<StoreDataContext>())
			{
				UserDac.UpdatePassword(
					user.UserID, password.GetMD5Hash(),
					mtx.DataContext );
				UserMaintDac.DeleteEntry(
					maintEntry.EntryUid,
					mtx.DataContext );
			}
		}

        public static void ResetPassword( Guid maintUid, string password )
        {
            ResetPassword(maintUid, password, expiredAllowDefault);
        }

	    public static ClientStatus GetClientStatus( string clientId )
		{
            //deas 20.04.2011 task3843
            // изменение определения статуса по наличию e-mail
            //deas 05.05.2011 task4038
            // изменение определения статуса по наличию Username
            //return UserDac.GetUserByClientId( clientId ) != null ? ClientStatus.Online : ClientStatus.Offline;
            User cUser = UserDac.GetUserByClientId( clientId );
            if ( cUser != null )
            {
                return string.IsNullOrEmpty( cUser.Username ) ? ClientStatus.Offline : ClientStatus.Online;
            }
            else
            {
                return ClientStatus.Offline;
            }
		}

		public static bool LoginInUse( string login )
		{
			if( string.IsNullOrEmpty( login ) )
				throw new ArgumentException( "Login cannot be empty", "login" );

			var entries = UserMaintDac.GetEntries( login );
			return
				UserDac.GetUserByUsername( login ) != null ||
				entries.Any( e => !EntryExpired( e ) );
		}

		public static bool EmailInUse( string email )
		{
			if( string.IsNullOrEmpty( email ) )
				throw new ArgumentException( "email cannot be empty", "email" );
			try
			{
				return GetClientIdsByEmail( email ).Length > 0;
			}
			catch( AcctgException ex )
			{
				if( ex.ErrorCode == AcctgError.TooManyClientsFound )
					return true;
				throw ex;
			}
		}

		private static string[] GetClientIdsByEmail( string email )
		{
			try
			{
				return ClientSearch.Search( email ).Select( bci => bci.ClientID ).ToArray();
			}
			catch( AcctgException ex )
			{
				if( ex.ErrorCode == AcctgError.NoDataToRespond )
					return new string[ 0 ];
				else
					throw ex;
			}
		}

		public static void SubmitPasswordRecoveryRequest( string email, Func<Guid, string> makeActivationUrl )
		{
			if( string.IsNullOrEmpty( email ) )
				throw new ArgumentException( "Email cannot be empty", "email" );
			if( makeActivationUrl == null )
				throw new ArgumentNullException( "makeActivationUrl" );
			string[] clientIds = null;
			bool tooManyClients = false;
			try
			{
				clientIds = GetClientIdsByEmail( email );
			}
			catch( AcctgException ex )
			{
				if( ex.ErrorCode == AcctgError.TooManyClientsFound )
					tooManyClients = true;
				else
					throw ex;
			}

			if( clientIds.Length == 0 )
				throw new BLException( "Клиент с указанным электронным адресом не найден" );
			if( clientIds.Length > 1 || tooManyClients )
				throw new BLException(
					"С указанным электронным адресом зарегистрировано несколько клиентов. " +
					"Отправка запроса на восcтановление пароля невозможна." );

			var clientId = clientIds[ 0 ];

			var user = UserDac.GetUserByClientId( clientId );
			if( user == null )
				throw new BLException( "Учетная запись пользователя с указанным электронным адресом не найдена" );

			var profile = ClientProfile.Load( clientId );
			var maintUid = Guid.NewGuid();

			UserMaintDac.AddEntry(
					new UserMaintEntry
					{
						EntryUid = maintUid,
						EntryPurpose = UserMaintEntryPurpose.PasswordRecovery,
						ClientID = clientIds[ 0 ]
					} );

			MailEngine.SendMail(
                new MailAddress( email, profile.Category == ClientCategory.Legal ? profile.ContactPerson : profile.ClientName ),
				new ResetPasswordAlert
				{
					MaintUrl = makeActivationUrl( maintUid ),
					ClientLogin = user.Username,
                    ClientFullName = profile.Category == ClientCategory.Physical ?
					profile.ClientName :
					profile.ContactPerson,
					DaysToLive = GetDaysToLive( UserMaintEntryPurpose.PasswordRecovery ).GetValueOrDefault()
				} );
		}

		public static ClientProfile GuestProfile
		{
			get
			{
				if( _guestProfile == null )
				{
					_guestProfile = ClientProfile.CreateGuestProfile( DiscountGroupsConfiguration.Current.DiscountGroup3, 0 );
				}
				return _guestProfile;
			}
		}
		private static ClientProfile _guestProfile = null;

		private static UserMaintEntry GetEntry( Guid entryUid, Boolean expiredAllow )
		{
			var entry = UserMaintDac.GetEntry( entryUid );
			if( entry == null )
				throw new BLException( "Activation code is not found" );
            if (expiredAllow == false)
			    if( EntryExpired( entry ) )
				    throw new BLException( "Activation code is expired" );
			return entry;
		}

		private static bool EntryExpired( UserMaintEntry entry )
		{
			var settings = ActivationConfiguration.Current.PurposeSettings[ entry.EntryPurpose ];
			return settings != null && ( DateTime.Now - entry.EntryTime ).TotalDays > settings.DaysToLive;
		}

		private static int? GetDaysToLive( UserMaintEntryPurpose purpose )
		{
			var settings = ActivationConfiguration.Current.PurposeSettings[ purpose ];
			return settings != null ? settings.DaysToLive : default( int? );
		}

		public static string ToVinOrderLineComment( this UserGarageCar car )
		{
			if( car == null )
				throw new ArgumentNullException( "car" );

			var comment = new StringBuilder();

			comment.Append( car.GetFullName() );

			return comment.ToString();
		}

		public static bool CanManageClient( EmployeeInfo manager, ClientProfile client )
		{
			if( manager == null )
				throw new ArgumentNullException( "manager" );
			if( client == null )
				throw new ArgumentNullException( "client" );
			return manager.EmployeeId == client.ManagerId || manager.StoreId == client.RmsStoreId;
		}

		public static bool CanUseClientCart( EmployeeInfo manager, ClientProfile client )
		{
			if( manager == null )
				throw new ArgumentNullException( "manager" );
			if( client == null )
				throw new ArgumentNullException( "client" );
			return CanManageClient( manager, client ) && client.IsChecked && !client.IsRestricted;
		}

        public static bool CanVinRequest(ClientProfile client)
        {
            return (client.TradingVolume != Acctg.TradingVolume.Retail);
        }
	}
}
