using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using RmsAuto.Common.References;

namespace RmsAuto.Store.Acctg.Entities
{
    public class RefCodeEnvelope : Envelope
    {
        public string RefCode { get; set; }
    }

    public class RefItemsEnvelope : Envelope
    {
        [XmlElement("Record")]
        public ReferenceItem[] Items { get; set; }
    }

    public class RatesEnvelope : Envelope
    {
        [XmlElement("Record")]
        public CurrencyRate[] Rates { get; set; }
    }

    public class StoresEnvelope : Envelope
    {
        [XmlElement("Record")]
        public StoreInfo[] Stores { get; set; }
    }

    public class EmployeesEnvelope : Envelope
    {
        [XmlElement("Record")]
        public EmployeeInfo[] Employees { get; set; }

    }

    public class SuppliersEnvelope : Envelope
    {
        [XmlElement("Record")]
        public SupplierInfo[] Suppliers { get; set; }
    }
}
