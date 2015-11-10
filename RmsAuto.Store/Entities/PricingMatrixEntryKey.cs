using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    [Obsolete("PricingMatrixEntryKey was used for PricingMatrixEntry cache only")]
    class PricingMatrixEntryKey : IEquatable<PricingMatrixEntryKey>
    {
        private int _supplierId;
        private string _manufacturer;
        private string _partNumber;
        private string _rgCode;
        
        public PricingMatrixEntryKey(int supplierId, string manufacturer, string partNumber, string rgCode)
        {
            if (!string.IsNullOrEmpty(partNumber) && !string.IsNullOrEmpty(rgCode))
                throw new ArgumentException("Only one of 'crossPartNumber' and 'rgCode' can be specified");
            _supplierId = supplierId;
            _manufacturer = manufacturer;
            _partNumber = partNumber;
            _rgCode = rgCode;
        }


        #region IEquatable<PricingMatrixEntryKey> Members

        public bool Equals(PricingMatrixEntryKey other)
        {
            if (other == null)
                return false;
            return 
                other._supplierId == this._supplierId &&
                other._manufacturer == this._manufacturer &&
                other._partNumber == this._partNumber &&
                other._rgCode == this._rgCode;
        }

        #endregion

        public override int GetHashCode()
        {
            int hashCode = _supplierId;
            if (!string.IsNullOrEmpty(_manufacturer))
                hashCode ^= _manufacturer.GetHashCode();
            if (!string.IsNullOrEmpty(_partNumber))
                hashCode ^= _partNumber.GetHashCode();
            if (!string.IsNullOrEmpty(_rgCode))
                hashCode ^= _rgCode.GetHashCode();
            return hashCode;
        }
    }
}
