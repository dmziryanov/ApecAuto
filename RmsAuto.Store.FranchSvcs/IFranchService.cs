using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.FranchSvcs
{
	[ServiceContract]
	public interface IFranchService
	{
		[OperationContract]
		object TestMethod();

		[OperationContract]
		List<SupplierResult> SupplierTimes();

		[OperationContract]
		List<SparePartResult> GetPrices(string pn, string brand, bool st);

		[OperationContract]
		List<OrderLineStatusResult> GetOrderStatuses(int[] orderids);

		[OperationContract]
		OrderResult SendOrder(OrderLineItemSource[] orderlines, string custOrderNum);

		[OperationContract]
		Dictionary<byte, string> GetStatuses();
	}

	[DataContract]
	public class SupplierResult
	{
		[DataMember]
		public int SupplierID { get; set; }
		[DataMember]
		public int DeliveryDaysMin { get; set; }
		[DataMember]
		public int DeliveryDaysMax { get; set; }
	}

	[DataContract]
	public class SparePartResult
	{
		[DataMember]
		public string PartNumber;
		[DataMember]
		public string PartDescription;
		[DataMember]
		public decimal Price;
		[DataMember]
		public int SupplierID;
		[DataMember]
		public int DeliveryDaysMin;
		[DataMember]
		public int DeliveryDaysMax;
		[DataMember]
		public string Manufacturer;
		[DataMember]
		public SparePartItemType SparePartType;
		[DataMember]
		public int? MinOrderQty;
		[DataMember]
		public int? QtyInStock;
		[DataMember]
		public DateTime PriceDate;
	}

	[DataContract]
	public class OrderLineStatusResult
	{
		[DataMember]
		public int OrderID;
		[DataMember]
		public byte CurrentStatus;
		[DataMember]
		public string PartNumber;
		[DataMember]
		public string Manufacturer;
		[DataMember]
		public int SupplierID;
		[DataMember]
		public int Qty;
		[DataMember]
		public decimal UnitPrice;
		[DataMember]
		public string ReferenceID;
		[DataMember]
		public bool StrictlyThisNumber;
		[DataMember]
		public bool StrictlyThisBrand;
		[DataMember]
		public bool StrictlyThisQty;
	}

	[DataContract]
	public class OrderLineItemSource
	{
		[DataMember]
		public int ItemID;
		[DataMember]
		public string PartNumber;
		[DataMember]
		public string Manufacturer;
		[DataMember]
		public int SupplierID;
		[DataMember]
		public int Qty;
		[DataMember]
		public decimal UnitPrice;
		[DataMember]
		public string ReferenceID;
		[DataMember]
		public bool StrictlyThisNumber;
		[DataMember]
		public bool StrictlyThisBrand;
		[DataMember]
		public bool StrictlyThisQty;
	}

	[DataContract]
	public class OrderResult
	{
		[DataMember]
		public int? OrderID;
		[DataMember]
		public OrderItemResult[] ItemResults;
	}

	[DataContract]
	public class OrderItemResult
	{
		[DataMember]
		public int ItemID;
		[DataMember]
		public string ResultInfo;
		[DataMember]
		public int ResultCode;
	}
}
