using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class SupplierInfo
    {
        [XmlElement("Key")]
        public int SupplierId { get; set; }

        [XmlElement("Value")]
        public string SupplierName { get; set; }
    }
}
