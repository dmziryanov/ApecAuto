using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Text;
using RmsAuto.Acctg;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Acctg
{
    [Serializable()]
    public class ClientProfile
    {
        public static Acknowledgement SetEmail( string clientId, string email )
        {
            if ( string.IsNullOrEmpty( clientId ) )
                throw new ArgumentException( "ClientId cannot be empty", "clientId" );
            if ( string.IsNullOrEmpty( email ) )
                throw new ArgumentException( "Email cannot be empty", "email" );

                var DC = new DCFactory<StoreDataContext>();
                DC.DataContext.spUpdUserEmail(clientId, email);
				return Acknowledgement.OK;
        }

        public static Acknowledgement SendClientBalance(string clientId, DateTime dateStart, DateTime dateEnd )
        {
            if ( string.IsNullOrEmpty( clientId ) )
                throw new ArgumentException( "ClientId cannot be empty", "clientId" );
            return ServiceProxy.Default.SendClientBalance( clientId, dateStart, dateEnd ) == "OK" ?
                Acknowledgement.OK :
                Acknowledgement.Unknown;
        }

        private ClientProfile()
        {
        }

        public static ClientProfile CreateGuestProfile(RmsAuto.Acctg.ClientGroup clientGroup,decimal personalMarkup)
        {
            return new ClientProfile() { ClientGroup = clientGroup, PersonalMarkup = personalMarkup };
        }

        public static ClientProfile Create(
            string login,
            string password,
            string clientName,
            string email,
            TradingVolume tradingVolume,
            ClientCategory clientCategory,
            int countryID,
            string locality,
			string rmsStoreID,
			string scopeType,
			string discountCardNumber,
			string howKnow,
			string managerId,
            bool isRestricted,
            string internalFranchName,

			string contactPersonPosition,
			string contactPersonName,
			string contactPersonSurname,
            string contactPersonPhone,
            string contactPersonExtPhone,
            string contactPersonFax,
			string contactPersonEmail,
            string deliveryAddress,

			string companyName,
			string companyRegistrationID,
			string companyAddress,

			string bankName,
			string IBAN,
			string SWIFT,
			string bankAddress,

            string directorName,
			string directorSurname,
            
			string correspondentBankName,
			string correspondentIBAN,
			string correspondentSWIFT,
			string correspondentBankAddress,
            int RegisterAs
            )
        {
            if ( string.IsNullOrEmpty( clientName ) )
                throw new ArgumentException( "Client name cannot be empty", "clientName" );
        //    if ( string.IsNullOrEmpty( contactPersonPhone ) )
      //          throw new ArgumentException( "Main phone number must be specified" );

            var profile = new ClientProfile()
            {
                ClientId = "",
                ClientName = clientName,
                Email = email,
                TradingVolume = tradingVolume,
                Category = clientCategory,
                CountryID = countryID,
                Locality = locality,
				RmsStoreId = rmsStoreID,
				ScopeType = scopeType,
				DiscountCardNumber = discountCardNumber,
				HowKnow = howKnow,
				IsRestricted = isRestricted,
                PrepaymentPercent = 0M,
                PersonalMarkup = 0M,
                Balance = 0M,
                DelayCredit = 0M,
                InternalFranchName = internalFranchName,

				ContactPersonPosition = contactPersonPosition,
				ContactPersonName = contactPersonName,
                ContactPersonSurname = contactPersonSurname,
                ContactPersonPhone = contactPersonPhone,
                ContactPersonExtPhone = contactPersonExtPhone,
                ContactPersonFax = contactPersonFax,
				ContactPersonEmail = contactPersonEmail,
                DeliveryAddress = deliveryAddress,
                
                BankName = bankName,
				IBAN = IBAN,
				SWIFT = SWIFT,
				BankAddress = bankAddress,

				DirectorName = directorName,
				DirectorSurname = directorSurname,
                
				CorrespondentBankName = correspondentBankName,
				CorrespondentIBAN = correspondentIBAN,
				CorrespondentSWIFT = correspondentSWIFT,
				CorrespondentBankAddress = correspondentBankAddress,
                RegisterAs = RegisterAs
            };

            RmsAuto.Acctg.ClientGroup clientGroup = tradingVolume == TradingVolume.Retail ? RmsAuto.Acctg.ClientGroup.DefaultRetail : RmsAuto.Acctg.ClientGroup.DefaultWholesale;

            using (var DC = new DCFactory<StoreDataContext>())
            {
              
                var rezIns = DC.DataContext.spInsUsers( login, password, clientName, email, (byte)tradingVolume, (byte)clientCategory,
                countryID, /*regionID,*/ locality, /*contactLastName, contactFirstName, contactMiddleName,
                contactPhone, contactExtPhone,*/ scopeType, howKnow, managerId, "", false, isRestricted,
                (int)clientGroup, 100M, 0M, /*contactFax, scheduleOfice, scheduleStock, shippingAddress,*/
                rmsStoreID, /*discountCardNumber, contactPosition, legalName, IPName, iNN, oGRNIP, kPP, oGRN,
                nDSAggent, oficialAddress, realAddress, account, bankBIC, bankINN, directorPosition,
                directorLastName, directorFirstName, directorMiddleName, balanceManPosition,
                balanceManLastName, balanceManFirstName, balanceManMiddleName,
                balanceManPhone, balanceManEmail,*/ internalFranchName,
				contactPersonPosition, contactPersonName, contactPersonSurname, contactPersonPhone, contactPersonExtPhone,
				contactPersonFax, contactPersonEmail, deliveryAddress, companyName, companyRegistrationID, companyAddress,
				bankName, IBAN, SWIFT, bankAddress, directorName, directorSurname, correspondentBankName,
				correspondentIBAN, correspondentSWIFT, correspondentBankAddress, RegisterAs);
                profile.UserId = rezIns.ToList().FirstOrDefault().UserID.Value;
            }

            return profile;
        }

        public int RegisterAs { get; set; }

        public static ClientProfile Load(string clientId)
		{
			if (string.IsNullOrEmpty(clientId))
				throw new ArgumentException("ClientId cannot be empty", "clientId");


			spSelUsersByAcctgIDResult gte = null;
			using (var dc = new DCFactory<StoreDataContext>())
			{
				gte = dc.DataContext.spSelUsersByAcctgID(clientId).FirstOrDefault();
			}



			if (gte == null)
				throw new ArgumentException("Client not find", "client");

			string ManagerData = null;
			if (AcctgRefCatalog.RmsEmployees[gte.ManagerId] != null)
				ManagerData = AcctgRefCatalog.RmsEmployees[gte.ManagerId].FullName;

			ClientProfile cProfile = new ClientProfile()
			{
				AcctgId = clientId,
				UserId = gte.UserID,
				ClientId = clientId,
				ClientName = gte.ClientName,
				Category = (ClientCategory)(byte)gte.ClientCategory,
				TradingVolume = (TradingVolume)(byte)gte.TradingVolume,
				Email = gte.Email,
				CountryID = gte.CountryID ?? 0,
				Locality = gte.Locality,
				ContactPersonPosition = gte.ContactPersonPosition,
				ContactPersonName = gte.ContactPersonName,
				ContactPersonSurname = gte.ContactPersonSurname,
				ContactPersonPhone = gte.ContactPersonPhone,
				ContactPersonExtPhone = gte.ContactPersonExtPhone,
				ContactPersonFax = gte.ContactPersonFax,
				ContactPersonEmail = gte.ContactPersonEmail,
				RmsStoreId = gte.RmsStoreID,
				DeliveryAddress = gte.DeliveryAddress,
				ScopeType = gte.ScopeType,
				HowKnow = gte.HowKnow,
				CompanyName = gte.CompanyName,
				CompanyRegistrationID = gte.CompanyRegistrationID,
				CompanyAddress = gte.CompanyAddress,
				BankName = gte.BankName,
				IBAN = gte.IBAN,
				SWIFT = gte.SWIFT,
				BankAddress = gte.BankAddress,
				DirectorSurname = gte.DirectorSurname,
				DirectorName = gte.DirectorName,
				CorrespondentBankName = gte.CorrespondentBankName,
				CorrespondentIBAN = gte.CorrespondentIBAN,
				CorrespondentSWIFT = gte.CorrespondentSWIFT,
				CorrespondentBankAddress = gte.CorrespondentBankAddress,
				ManagerId = gte.ManagerId,
				ManagerDeptId = gte.ManagerDeptId,
				ClientGroup = gte.ClientGroup == 0
					? ((TradingVolume)(byte)gte.TradingVolume == TradingVolume.
					Retail ? RmsAuto.Acctg.ClientGroup.DefaultRetail : RmsAuto.Acctg.ClientGroup.DefaultWholesale)
					: (RmsAuto.Acctg.ClientGroup)gte.ClientGroup,
				IsChecked = gte.IsChecked,
				PrepaymentPercent = gte.PrepaymentPercent,
				IsRestricted = gte.IsRestricted,
				PersonalMarkup = gte.PersonalMarkup,
				ReclamationPeriod = gte.ReclamationPeriod,
				Balance = gte.Balance,
				DelayCredit = gte.DelayCredit,
				BalanceDate = gte.BalanceDate,
				InternalFranchName = gte.InternalFranchName,
				ManagerName = ManagerData != null ? ManagerData : ""
			};

			return cProfile;
		}

        public string OGRNIP { get; set; }

        /// <summary>
        /// Код региона клиента
        /// </summary>
        public int RegionID { get; set; }

        public string AcctgId { get; set; }

        /// <summary>
        /// Ид клиента в БД сайта
        /// </summary>
        public int UserId { get; private set; }
		/// <summary>
		/// Ид клиента в Hansa
		/// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// КПП организации
        /// </summary>
        public string KPP { get; set; }

        /// <summary>
        /// Является плательщиком НДС
        /// </summary>
        public bool NDSAggent { get; set; }

        /// <summary>
        /// Фамилия директора
        /// </summary>
        public string DirectorLastName { get; set; }

        /// <summary>
        /// Имя директора
        /// </summary>
        public string DirectorFirstName { get; set; }

        /// <summary>
        /// Отчество директора
        /// </summary>
        public string DirectorMiddleName { get; set; }

        /// <summary>
        /// Счет клиента
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Имя контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManFirstName { get; set; }

        /// <summary>
        /// БИК Банка
        /// </summary>
        public string BankBIC { get; set; }

        /// <summary>
        /// ИНН Банка
        /// </summary>
        public string BankINN { get; set; }

        /// <summary>
        /// Отчество контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManMiddleName { get; set; }

        /// <summary>
        /// Телефон контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManPhone { get; set; }

        /// <summary>
        /// Должность контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManPosition { get; set; }

		/// <summary>
        /// Итоговое имя клиента
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Контактная персона
        /// </summary>
        public string ContactPerson
        {
            get
            {
				return ContactPersonSurname + " " + ContactPersonName;
            }
        }

        /// <summary>
        /// Фамилия контактного лица
        /// </summary>
        public string ContactLastName { get; set; }


        /// <summary>
        /// Имя контактного лица
        /// </summary>
        public string ContactFirstName { get; set; }

        /// <summary>
        /// Отчество контактного лица
        /// </summary>
        public string ContactMiddleName { get; set; }

        /// <summary>
        /// Распорядок работы склада
        /// </summary>
        public string ScheduleStock { get; set; }
        
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Email клиента
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Объем закупок (опт, розн)
        /// </summary>
        public TradingVolume TradingVolume { get; set; }
        /// <summary>
        /// Категория клиента (юр,физ,ИП)
        /// </summary>
        public ClientCategory Category { get; set; }
        /// <summary>
        /// Код страны клиента
        /// </summary>
        public int CountryID { get; set; }

        /// <summary>
        /// Телефон контактного лица
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Дополнительный телефон контактного лица
        /// </summary>
        public string ContactExtPhone { get; set; }

        /// <summary>
        /// Факс контактного лица
        /// </summary>
        public string ContactFax { get; set; }

        /// <summary>
        /// Распорядок работы офиса
        /// </summary>
        public string ScheduleOfice { get; set; }


        /// <summary>
        /// Должность контактного лица
        /// </summary>
        public string ContactPosition { get; set; }
      
     
        /// <summary>
        /// Адрес доставки
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }
       

        public string[] ShippingAddressList
        {
            get
            {
                return new string[] { ShippingAddress };
            }
        }

        /// <summary>
        /// Город (округ) клиента
        /// </summary>
        public string Locality { get; set; }
		/// <summary>
		/// Сфера деятельности клиента (маг,сервис и т.д.)
		/// </summary>
		public string ScopeType { get; set; }
		/// <summary>
		/// Номер дисконтной карты
		/// </summary>
		public string DiscountCardNumber { get; set; }
		/// <summary>
		/// Откуда вы о нас узнали
		/// </summary>
		public string HowKnow { get; set; }
		/// <summary>
		/// Код склада RMS для получения заказов
		/// </summary>
		public string RmsStoreId { get; set; }

		#region === Контактная информация =================
		/// <summary>
		/// Должность контактного лица
		/// </summary>
		public string ContactPersonPosition { get; set; }
		/// <summary>
        /// Фамилия контактного лица
        /// </summary>
        public string ContactPersonSurname { get; set; }
        /// <summary>
        /// Имя контактного лица
        /// </summary>
        public string ContactPersonName { get; set; }
        /// <summary>
        /// Телефон контактного лица
        /// </summary>
        public string ContactPersonPhone { get; set; }
        /// <summary>
        /// Дополнительный телефон контактного лица
        /// </summary>
        public string ContactPersonExtPhone { get; set; }
        /// <summary>
        /// Факс контактного лица
        /// </summary>
        public string ContactPersonFax { get; set; }
		/// <summary>
		/// Email контактного лица
		/// </summary>
		public string ContactPersonEmail { get; set; }
        /// <summary>
        /// Адрес доставки
        /// </summary>
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string OficialAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public string RealAddress { get; set; }

        /// <summary>
        /// Фамилия контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManLastName { get; set; }


        /// <summary>
        /// Наименование организации
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// ОГРН организации
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Должность руководителя
        /// </summary>
        public string DirectorPosition { get; set; }
       
        /// <summary>
        /// E-mail контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManEmail { get; set; }

   
     
		#endregion

		#region === Информация о компании =================

		/// <summary>
		/// Наименование организации для ЮрЛица
		/// </summary>
		public string CompanyName { get; set; }
		/// <summary>
		/// Регистрационный код компании (ИНН для России, VAT для Европы)
		/// </summary>
		public string CompanyRegistrationID { get; set; }
		/// <summary>
		/// Адрес регистрации компании
		/// </summary>
		public string CompanyAddress { get; set; }

		#endregion

		#region === Банковские реквизиты ==================
		/// <summary>
		/// Наименование банка компании
		/// </summary>
		public string BankName { get; set; }
		/// <summary>
		/// Номер счета в банке
		/// </summary>
		public string IBAN { get; set; }
		/// <summary>
		/// БИК
		/// </summary>
		public string SWIFT { get; set; }
		/// <summary>
		/// Адрес регистрации банка
		/// </summary>
		public string BankAddress { get; set; }
		#endregion

		#region === Генеральный директор ==================
		/// <summary>
		/// Имя директора
		/// </summary>
		public string DirectorName { get; set; }
		/// <summary>
		/// Фамилия директора
		/// </summary>
		public string DirectorSurname { get; set; }
		#endregion

		#region === Корреспондентский банк (если есть) ====

		public string CorrespondentBankName { get; set; }
		public string CorrespondentIBAN { get; set; }
		public string CorrespondentSWIFT { get; set; }
		public string CorrespondentBankAddress { get; set; }

		#endregion

		/// <summary>
		/// Ид менеджера в учетной системе
		/// </summary>
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        /// <summary>
		/// Ид отдела, в котором работает менеджер
		/// </summary>
        public string ManagerDeptId { get; private set; }
		/// <summary>
		/// Статус проверки учётной записи менеджером в учетной системе
		/// </summary>
        public bool IsChecked { get; /*private*/ set; }
		/// <summary>
		/// Профиль с ограниченными возможностями (только поиск цен)
		/// </summary>
        public bool IsRestricted { get; private set; }
        //Регион в базу  которого ломился датаконтекст за данными
        public string InternalFranchName { get; private set; }
        /// <summary>
        /// Клиентская группа скидки
        /// </summary>
        /// <remarks>
        ///	Для вычисления стоимости товара для клиента необходимо определить скидочный коэффициент Скi, зависящий от номера группы
        /// </remarks>
        public RmsAuto.Acctg.ClientGroup ClientGroup { get; set; }
        public string ClientGroupName
        {
            get
            {
                using (dcCommonDataContext DC = new dcCommonDataContext())
                {
                    var CG = DC.ClientGroups.FirstOrDefault(t => t.ClientGroupID == (int)ClientGroup);
                    if (CG == null)
                    {
                        return ((int)ClientGroup).ToString();
                    }
                    else
                    {
                        return CG.ClientGroupName;
                    }
                }
            }
        }
		/// <summary>
		/// Процент предоплаты
		/// </summary>
		public decimal PrepaymentPercent { get; set; }
		/// <summary>
		/// Персональная наценка, стоимость детали домножается на коэффициент (100 + PersonalMarkup)/100
		/// </summary>
		public decimal PersonalMarkup { get; private set; }
		/// <summary>
		/// Юрлицо-опт
		/// </summary>
        public bool IsLegalWholesale
        {
            get { return TradingVolume == TradingVolume.Wholesale && Category == ClientCategory.Legal; }
        }
		/// <summary>
		/// Баланс клиента из учетной системы
		/// </summary>
		public decimal Balance { get; private set; }
  		/// <summary>
		/// Просроченная дебиторская задолженность
		/// </summary>
		public decimal DelayCredit { get; private set; }
		/// <summary>
		/// Дата на которую актуальны Balance и DelayCredit
		/// </summary>
		public DateTime BalanceDate { get; private set; }
		/// <summary>
		/// Период в течении которого может подаваться рекламация
		/// (по умолчанию = 10)
		/// </summary>
		public int ReclamationPeriod { get; private set; }

   

     }
}
