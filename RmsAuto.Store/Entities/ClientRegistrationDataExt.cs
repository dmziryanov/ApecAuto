using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Entities
{
    /// <summary>
    /// Регистрационная информация о клиенте
    /// </summary>
    [Serializable]
    public class ClientRegistrationDataExt
	{
		#region === Общая информация ======================
		/// <summary>
        /// Итоговое имя клиента
        /// </summary>
		public string ClientName
		{
			get
			{
				switch (ClientCategory)
				{
					//case ClientCategory.Physical:
					//    return ContactPersonSurname + " " + ContactPersonName + " " + ContactMiddleName;
					//case ClientCategory.PhysicalIP:
					//    if (!string.IsNullOrEmpty(IPName)) { return IPName; }
					//    else { return DirectorSurname + " " + DirectorName + " " + DirectorMiddleName; }
					case ClientCategory.Legal:
						if (!string.IsNullOrEmpty(CompanyName)) { return CompanyName; }
						else { return DirectorSurname + " " + DirectorName; }
					default:
						return ContactPersonSurname + " " + ContactPersonName;
				}
			}
			set
			{
				switch (ClientCategory)
				{
					//case ClientCategory.Physical:
					//    {
					//        ContactPersonSurname = value;
					//    } break;
					//case ClientCategory.PhysicalIP:
					//    {
					//        DirectorSurname = value;
					//    } break;
					case ClientCategory.Legal:
						{ CompanyName = value; } break;
					default:
						{
							ContactPersonSurname = value;
						} break;
				}
			}
		}
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
        public ClientCategory ClientCategory { get; set; }
        /// <summary>
        /// Код страны клиента
        /// </summary>
        public int CountryID { get; set; }
        /// <summary>
        /// Город (округ) клиента
        /// </summary>
        public string Locality { get; set; }
        /// <summary>
        /// Код склада RMS для получения заказов
        /// </summary>
        public string RmsStoreID { get; set; }
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
        #endregion

		#region === Контактная информация =================
		/// <summary>
		/// Должность контактного лица
		/// </summary>
		public string ContactPersonPosition { get; set; }
		/// <summary>
		/// Имя контактного лица
		/// </summary>
		public string ContactPersonName { get; set; }
		/// <summary>
		/// Фамилия контактного лица
		/// </summary>
		public string ContactPersonSurname { get; set; }
		/// <summary>
		/// Телефон контактного лица
		/// </summary>
		public string ContactPersonPhone { get; set; }
		/// <summary>
		/// Дополнительный телефон контактного лица
		/// </summary>
		public string ContactPersonExtPhone { get; set; }
        public int RegisterAs { get; set; }
        
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
        /// БИК Банка
        /// </summary>
        public string BankBIC { get; set; }

        /// <summary>
        /// ИНН Банка
        /// </summary>
        public string BankINN { get; set; }

        /// <summary>
        /// Фамилия контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManLastName { get; set; }

        /// <summary>
        /// Имя контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManFirstName { get; set; }

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
        /// E-mail контактного лица по сверке взаиморасчетов
        /// </summary>
        public string BalanceManEmail { get; set; }

   
        #region Legal

        /// <summary>
        /// Наименование организации для ЮрЛица
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Наименование организации для ИП
        /// </summary>
        public string IPName { get; set; }

        /// <summary>
        /// КПП организации
        /// </summary>
        public string KPP { get; set; }

        /// <summary>
        /// ОГРН организации
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Должность руководителя
        /// </summary>
        public string DirectorPosition { get; set; }


		#region === Корреспондентский банк (если есть) ====

		public string CorrespondentBankName { get; set; }
		public string CorrespondentIBAN { get; set; }
		public string CorrespondentSWIFT { get; set; }
		public string CorrespondentBankAddress { get; set; }

		#endregion

        #endregion

        /// <summary>
        /// Дополнительный телефон контактного лица
        /// </summary>
        public string ContactExtPhone { get; set; }

        /// <summary>
        /// Распорядок работы склада
        /// </summary>
        public string ScheduleStock { get; set; }

        /// <summary>
        /// Распорядок работы офиса
        /// </summary>
        public string ScheduleOfice { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ОГРНИП
        /// </summary>
        public string OGRNIP { get; set; }

        /// <summary>
        /// Факс контактного лица
        /// </summary>
        public string ContactFax { get; set; }


        public string ShippingAddress { get; set; }

        public string AuxPhone1 { get; set; }

        public string AuxPhone2 { get; set; }

        public string FieldOfAction { get; set; }

        public string MainPhone { get; set; }
        public string RmsStoreId { get; set; }

        /// <summary>
        /// Должность контактного лица
        /// </summary>
        public string ContactPosition { get; set; }

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
        /// Фактический адрес
        /// </summary>
        public string RealAddress { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string OficialAddress { get; set; }


        public bool NDSAggent { get; set; }


        /// <summary>
        /// Телефон контактного лица
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Код региона клиента
        /// </summary>
        public int RegionID { get; set; }
    }
}
