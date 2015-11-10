using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RmsAuto.Store.Entities
{
    public class SparePartPriceKey : IEquatable<SparePartPriceKey>
    {
        private string _mfr;
        private string _pn;
        private int _supplierId;
        
        public static SparePartPriceKey Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("SparePartKey string cannot be empty", "s");
            string[] parts = s.Split(',');

            return new SparePartPriceKey()
            {
                _mfr = parts[0],
                _pn = parts[1],
                _supplierId = int.Parse(parts[2])
            };
        }

        private SparePartPriceKey() { }

        public SparePartPriceKey(string mfr, string pn, int supplierId)
        {
            if (string.IsNullOrEmpty(mfr))
                throw new ArgumentException("Manufacturer cannot be empty", "mfr");
            if (string.IsNullOrEmpty(pn))
                throw new ArgumentException("PartNumber cannot be empty", "pn");
            _mfr = mfr;
            _pn = pn;
            _supplierId = supplierId;            
        }
               
        public string Mfr
        {
            get { return _mfr; }
        }

        public string PN
        {
            get { return _pn; }
        }

        public int SupplierId
        {
            get { return _supplierId; }
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", _mfr, _pn, _supplierId);
        }

        public string ToUrlString()
        {
            return string.Format("{0},{1},{2}", HttpUtility.UrlEncode(_mfr), HttpUtility.UrlEncode(_pn), _supplierId);
        }

        #region IEquatable<SparePartPriceKey> Members

        public bool Equals(SparePartPriceKey other)
        {
            if (other != null)
                return 
                    this.Mfr == other.Mfr &&
                    this.PN == other.PN &&
                    this.SupplierId == other.SupplierId;
            else
                return false;
        }

        #endregion

        public override int GetHashCode()
        {
            return Mfr.GetHashCode() ^ PN.GetHashCode() ^ SupplierId;
        }
    }
}
