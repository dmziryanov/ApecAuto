using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class SellerInfo
    {
		/// <summary>
		/// Наименование получателя
		/// </summary>
        public string CompanyName { get; set; }
        
		/// <summary>
		/// ИНН
		/// </summary>
        [XmlElement("INN")]
        public string Inn { get; set; }

		/// <summary>
		/// Расчетный счет
		/// </summary>
        public string BankAccount { get; set;}

		/// <summary>
		/// Наименование банка
		/// </summary>
        public string BankName { get; set; }

		/// <summary>
		/// БИК
		/// </summary>
        [XmlElement("BIK")]
        public string BankCode { get; set; }

		/// <summary>
		/// Корреспондентский счёт
		/// </summary>
        public string CorrAccount { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string KPP { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Issued { get; set; }
    }
}
