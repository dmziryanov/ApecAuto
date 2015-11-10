using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class ShoppingCartKey : IEquatable<ShoppingCartKey>
    {
        private string _mfr;
        private string _pn;
        private int _supplierId;
        private string _referenceID;

        public static ShoppingCartKey Parse( string s )
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("ShoopingCartKey string cannot be empty", "s");
            string[] parts = s.Split(',');

            return new ShoppingCartKey()
            {
                _mfr = parts[0],
                _pn = parts[1],
                _supplierId = int.Parse(parts[2]),
                _referenceID = parts[3]
            };
        }

        private ShoppingCartKey() { }

        public ShoppingCartKey( string mfr, string pn, int supplierId, string referenceID )
        {
            if (string.IsNullOrEmpty(mfr))
                throw new ArgumentException("Manufacturer cannot be empty", "mfr");
            if (string.IsNullOrEmpty(pn))
                throw new ArgumentException("PartNumber cannot be empty", "pn");
            _mfr = mfr;
            _pn = pn;
            _supplierId = supplierId;
            _referenceID = referenceID;
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

        public string ReferenceID
        {
            get { return _referenceID; }
        }

        public override string ToString()
        {
            return string.Format( "{0},{1},{2},{3}", _mfr, _pn, _supplierId, _referenceID );
        }

        #region IEquatable<ShoppingCartKey> Members

        public bool Equals( ShoppingCartKey other )
        {
            if (other != null)
                return 
                    this.Mfr == other.Mfr &&
                    this.PN == other.PN &&
                    this.SupplierId == other.SupplierId &&
                    this.ReferenceID == other.ReferenceID;
            else
                return false;
        }

        #endregion

        public override int GetHashCode()
        {
            return Mfr.GetHashCode() ^ PN.GetHashCode() ^ SupplierId ^ ReferenceID.GetHashCode();
        }
    }
}
