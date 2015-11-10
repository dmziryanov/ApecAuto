using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    public class CreateClientEnvelope : Envelope
    {
        public int ClientType; 
        public string ClientName; 
        public int ClientOptTag; 
        public string ClientDelivAddr;
        public string ShopId; 
        public string ClientEmail; 
        public string ClientMainContact; 
        public string ClientMainPh;
        public string ClientMobPh;
        public string ClientHomePh;
        public string Direction;
        public string KartNum;
        public string EmployeeId;
        public int IsGuest;
    }

    // deas 15.03.2011 task2626
    // добавленны дополнительные поля баланса и просроченной задолженности
    public class GetClientEnvelope : Envelope
    {
        public string ClientCode;
        public string ClientName;
        public int ClientType; 
        public string ClientKPP;
        public string ClientINN;
        public string ClientInvAddr;
        public int ClientOptTag;
        public string ClientMainContact;
        public string ClientMainPh;
        public string ClientMobPh;
        public string ClientHomePh;
        public string Direction;
        public string KartNum;

        [XmlElement("ClientDelivAddr")]
        public string[] DelivAddrList;

        public string ShopId; 
        public string ClientEmail; 
        public string EmployeeId;
        public string EmployeeDept;
        public int CheckStatus;
        public int PriceGroup;
        public decimal PrepaymentPercent;
		public int IsGuest;
		public decimal PersonalMarkup;
        public decimal Balance;
        public decimal DelayCredit;
	}

    public class ClientIdEnvelope : Envelope
    {
        [XmlElement("ClientCode")]
        public string ClientId { get; set; }
    }

    public class ClientSearchArgsEnvelope : Envelope
    {
        public int SubStr { get; set; }

        public string ClientName { get; set; }

        [XmlElement("ClientMainPh")]
        public string MainPhone { get; set; }

        public string ClientEmail { get; set; }
    }

    public class ClientSearchResultsEnvelope : Envelope
    {
        [XmlElement("Record")]
        public BriefClientInfo[] Clients { get; set; }
    }


    public class ClientEmailEnvelope : Envelope
    {
        [XmlElement("ClientCode")]
        public string ClientId { get; set; }

        [XmlElement("ClientEmail")]
        public string Email { get; set; }
    }

    //deas 17.03.2011 task3308
    //добавление класса для обмена с 1С для отправки детального баланса клиента
    public class SendClientBalanceEnvelope : Envelope
    {
        [XmlElement( "ClientCode" )]
        public string ClientId { get; set; }

        [XmlElement( "DateStart" )]
        public string DateStart { get; set; }

        [XmlElement( "DateEnd" )]
        public string DateEnd { get; set; }
    }

	/// <summary>
	/// Класс для обмена с 1С для отправки запроса о непоставке
	/// </summary>
	public class SendClientNonDeliveryRequestEnvelope : Envelope
	{
		[XmlElement("ClientId")]
		public string ClientId { get; set; }
		[XmlElement("OrderId")]
		public string OrderId { get; set; }
		[XmlElement("Data")]
		public string Data { get; set; }
	}

    //deas 06.04.2011 task3696
    //добавление метода обмена с 1С для получения текущего баланса клиента

    public class GetCurrentClientBalance
    {
        /// <summary>
        /// Колонка прайса клиента
        /// </summary>
        public int PriceGroup;
        /// <summary>
        /// Текущий баланс клиента
        /// </summary>
        public string Balance;
        /// <summary>
        /// Просроченная дебиторская задолженность
        /// </summary>
        public string DelayCredit;
    }

}
