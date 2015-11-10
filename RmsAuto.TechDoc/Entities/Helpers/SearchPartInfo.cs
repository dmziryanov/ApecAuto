using System;
using System.Text.RegularExpressions;

namespace RmsAuto.TechDoc.Entities.Helpers
{
    [Obsolete()]
	public class SearchPartInfo : IComparable<SearchPartInfo>
	{
		public string SearchPartNumber
		{
			get; set;
		}

        private string _partNumber;
        public string PartNumber
		{
			get { return string.IsNullOrEmpty(_partNumber) ? String.Empty : Regex.Replace(_partNumber, @"([^\w]|\s)", string.Empty).ToUpper(); }
			set { _partNumber = value; }
		}

		public string Brand
		{
			get; set;
		}
		
		public SearchPartInfo()
		{
			
		}

        public SearchPartInfo(string searchPartNumber, string partNumber, string brand)
		{
			this.SearchPartNumber = searchPartNumber;
			this.PartNumber = partNumber;
			this.Brand = brand;
		}

        public override string ToString()
        {
            return String.Format("{0} => {1} ({2})", this.SearchPartNumber, this.PartNumber, this.Brand);
        }

        public int CompareTo(SearchPartInfo other)
        {
            var brandCmp = this.Brand.CompareTo(other.Brand);
            if (brandCmp != 0)
            {
                return brandCmp;
            }
            var pnCmp = this.PartNumber.CompareTo(other.PartNumber);
            if (pnCmp != 0)
            {
                return pnCmp;
            }
            var snCmp = this.SearchPartNumber.CompareTo(other.SearchPartNumber);
            if (snCmp != 0)
            {
                return snCmp;
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            var spOther = (SearchPartInfo)obj;
            return this.PartNumber.Equals(spOther.PartNumber) &&
                   this.SearchPartNumber.Equals(spOther.SearchPartNumber) &&
                   this.Brand.Equals(spOther.Brand);
        }
    }
}
