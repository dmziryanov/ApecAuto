using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
	//dan 28.07.2011 task4770
	//добавление новго метода для отправки рекламаций в 1С
	[Obsolete]
	public class ReclamationEnvelope : Envelope
	{
		[XmlElement("ContactPerson")]
		public string ContactPerson { get; set; }

		[XmlElement( "ContactPhone" )]
		public string ContactPhone { get; set; }

		[XmlElement( "Qty" )]
		public int Qty { get; set; }

		[XmlElement( "ReclamationReason" )]
		public string ReclamationReason { get; set; }

		[XmlElement( "ReclamationDescription" )]
		public string ReclamationDescription { get; set; }

		[XmlElement( "Torg12Number" )]
		public string Torg12Number { get; set; }

		[XmlElement( "Torg12Price" )]
		public decimal? Torg12Price { get; set; }

		[XmlElement( "InvoiceNumber" )]
		public string InvoiceNumber { get; set; }

		[XmlElement( "ClientName" )]
		public string ClientName { get; set; }

		[XmlElement( "ClientID" )]
		public string ClientID { get; set; }

		[XmlElement( "ReclamationDate" )]
		public DateTime ReclamationDate { get; set; }

		[XmlElement( "OrderID" )]
		public int OrderID { get; set; }

		[XmlElement( "OrderDate" )]
		public DateTime OrderDate { get; set; }

		[XmlElement( "EstSupplyDate" )]
		public DateTime? EstSupplyDate { get; set; }

		[XmlElement( "ManagerName" )]
		public string ManagerName { get; set; }

		[XmlElement( "Manufacturer" )]
		public string Manufacturer { get; set; }

		[XmlElement( "PartNumber" )]
		public string PartNumber { get; set; }

		[XmlElement( "PartName" )]
		public string PartName { get; set; }

		[XmlElement( "UnitPrice" )]
		public decimal UnitPrice { get; set; }

		[XmlElement( "UnitQty" )]
		public int UnitQty { get; set; }

		[XmlElement( "SupplyDate" )]
		public DateTime? SupplyDate { get; set; }

		[XmlElement( "SupplierID" )]
		public int SupplierID { get; set; }

		[XmlElement( "CurrentStatus" )]
		public byte CurrentStatus { get; set; }

		[XmlElement( "CurrentStatusDate" )]
		public DateTime? CurrentStatusDate { get; set; }

		[XmlElement( "ReclamationType" )]
		public byte ReclamationType { get; set; }

		[XmlElement( "OrderLineID" )]
		public int OrderLineID { get; set; }
	}
}

