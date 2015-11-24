using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class OrderLineInfo
    {
        [XmlElement("OrderLineWeb")]
        public int WebOrderLineId { get; set; }

        [XmlElement("OrderLineHW")]
        public int AcctgOrderLineId { get; set; }

        [XmlElement("OldOrderLineHW")]
        public int? ParentAcctgOrderLineId { get; set; }

        public ArticleInfo Article { get; set; }

        [XmlElement("Price")]
        public decimal FinalSalePrice { get; set; }

        [XmlElement("LineQuan")]
        public int Quantity { get; set; }

        [XmlElement("ThisNumberOnly")]
        public int StrictlyThisNumber { get; set; }

        [XmlElement("VINInfo")]
        public string VinCheckupData { get; set; }

        [XmlElement("CommentLine")]
        public string OrderLineNotes { get; set; }

        [XmlElement("LineStatus")]
        public string OrderLineStatus { get; set; }

        [XmlElement("NewSupplyDate")]
        public DateTime? EstSupplyDate { get; set; }
        
        [XmlElement("LastDateTimeChange")]
        public DateTime? StatusChangeTime { get; set; }

        [XmlElement("InternalFranchName")]
        public string InternalFranchName { get; set; }
    }
}
