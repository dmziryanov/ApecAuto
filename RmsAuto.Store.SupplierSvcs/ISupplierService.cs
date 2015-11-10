using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RmsAuto.Store.SupplierSvcs
{
	[ServiceContract]
	public interface ISupplierService
	{
		#region === samples ===
		//[OperationContract]
		//string GetData(int value);

		//[OperationContract]
		//CompositeType GetDataUsingDataContract(CompositeType composite);
		#endregion

		[OperationContract]
		SearchResult GetPriceAndQuantity(string PriceLogo, string DetailNum, string MakeNum);

	}

	[DataContract]
	public class SearchResult
	{
		[DataMember]
		public int Quantity { get; set; }

		[DataMember]
		public Decimal Price { get; set; }

		[DataMember]
		public int LotQuantity { get; set; }
	}

	#region === sample of complex type ===
	// Use a data contract as illustrated in the sample below to add composite types to service operations
	//[DataContract]
	//public class CompositeType
	//{
	//    bool boolValue = true;
	//    string stringValue = "Hello ";

	//    [DataMember]
	//    public bool BoolValue
	//    {
	//        get { return boolValue; }
	//        set { boolValue = value; }
	//    }

	//    [DataMember]
	//    public string StringValue
	//    {
	//        get { return stringValue; }
	//        set { stringValue = value; }
	//    }
	//}
	#endregion
}
