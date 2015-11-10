using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class ArticleInfo
    {
        [XmlElement("ItemNumb")]
        public string PartNumber { get; set; }

        [XmlElement("ItemIntNumb")]
        public string InternalPartNumber { get; set; }

        [XmlElement("ProducedBy")]
        public string Manufacturer { get; set; }

        [XmlElement("ItemDescr")]
        public string Description { get; set; }

        [XmlElement("ItemDescrOrig")]
        public string DescriptionOrig { get; set; }

        [XmlElement("SupplierPriceWithMarkup")]
		public decimal SupplierPriceWithMarkup { get; set; }

        [XmlElement("SupplierMarkup")]
		public decimal SupplierMarkup { get; set; }
        
        public int SupplierId { get; set; }

        [XmlElement("ReferenceID")]
        public string ReferenceID { get; set; }

        [XmlElement("MinDelivDays")]
        public int DeliveryDaysMin { get; set; }

        [XmlElement("MaxDelivDays")]
        public int DeliveryDaysMax { get; set; }

        [XmlElement("Weight")]
        public decimal WeightPhysical { get; set; }

        [XmlElement("WeightVol")]
        public decimal WeightVolume { get; set; }

        [XmlElement("DiscGroup")]
        public string DiscountGroup { get; set; }
    }
}
