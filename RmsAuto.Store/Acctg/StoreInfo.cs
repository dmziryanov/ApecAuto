using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class StoreInfo
    {
        [XmlElement("ShopId")]
        public string StoreId { get; set; }

        [XmlElement("ShopNumber")]
        public string StoreNumber { get; set; }
                	
		[XmlElement( "ShopName" )]
		public string StoreName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [XmlElement("RetailShop")]
        public bool IsRetail { get; set; }

        [XmlElement("WholesaleShop")]
        public bool IsWholesale { get; set; }
        
        [XmlIgnore]
        public string FullInfo
        {
            get
            {
                var info = new StringBuilder();
                if (!string.IsNullOrEmpty(StoreName))
                    info.Append("\"" + StoreName + "\"");
                if (!string.IsNullOrEmpty(Address))
                    info.Append(" " + Address);
                if (!string.IsNullOrEmpty(Phone))
                    info.Append(" т." + Phone);

                var s = info.ToString(); 
                return !string.IsNullOrEmpty(s) ? s : StoreId;
            }
			private set { ;} //добавлено для того чтобы можно было заполнять этот объект прямым запросом dc.ExecuteQuery<StoreInfo>( "запрос" )
        }
    }

	/// <summary>
	/// Собственный склад наличия магазина
	/// </summary>
	public class ShopStoreInfo
	{
		public string RmsStoreID { get; set; }
		public int SupplierID { get; set; }
	}
}
