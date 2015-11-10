using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities.Helpers
{
	public class AdditionalInfoExt
	{
		public bool HasDefectPics { get; set; }
		public SparePartKeyExt Key { get; set; }
	}

	public class SparePartKeyExt : IEquatable<SparePartKeyExt>//, IComparable<SparePartKeyExt>
	{
		public string Manufacturer { get; set; }
		public string PartNumber { get; set; }
		public int SupplierID { get; set; }

		public SparePartKeyExt(string mfr, string pn, int supplierID)
		{
			this.Manufacturer = mfr;
			this.PartNumber = pn;
			this.SupplierID = supplierID;
		}

		//public override bool Equals(object obj)
		//{
		//    //return this.Equals((SparePartKeyExt)obj);
		//}

		public override int GetHashCode()
		{
			//return base.GetHashCode();
			return this.Manufacturer.ToLower().GetHashCode() ^ this.PartNumber.ToLower().GetHashCode() ^ this.SupplierID.GetHashCode();
		}

		#region IEquatable<SparePartKeyExt> Members

		public bool Equals(SparePartKeyExt other)
		{
			return other != null &&
				this.Manufacturer.Equals(other.Manufacturer, StringComparison.CurrentCultureIgnoreCase) &&
				this.PartNumber.Equals(other.PartNumber, StringComparison.CurrentCultureIgnoreCase) &&
				this.SupplierID.Equals(other.SupplierID);
		}

		#endregion

		//#region IComparable<SparePartKeyExt> Members

		//public int CompareTo(SparePartKeyExt other)
		//{
		//    if (other != null)
		//    {
		//        int res = String.Compare(this.Manufacturer, other.Manufacturer, true);
		//        if (res == 0)
		//            res = String.Compare(this.PartNumber, other.PartNumber, true);
		//        if (res == 0)
		//            if (this.SupplierID == other.SupplierID)
		//                res = 0;
		//        return res;
		//    }
		//    return 1;
		//}

		//#endregion
	}
}
