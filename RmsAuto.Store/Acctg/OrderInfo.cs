using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class OrderInfo
    {
        [XmlIgnore]
        public string ClientId { get; set; }

        [XmlElement("OrderNum")]
        public string OrderNo { get; set; }

        [XmlElement("OrderEntryDate")]
        public DateTime OrderDate { get; set; }

        public string EmployeeId { get; set; }
        
        [XmlElement("ClientDelivAddr")]
        public string DeliveryAddress { get; set; }

        public string PaymentType { get; set; }

        [XmlElement("Line")]
        public OrderLineInfo[] OrderLines { get; set; }

        [XmlElement("Comment")]
        public string OrderNotes { get; set; }

		/// <summary>
		/// Клиентский номера заказа (клиент при оформлении заказа может написать сюда всё что угодно)
		/// </summary>
		public string CustOrderNum { get; set; }
    }
}
