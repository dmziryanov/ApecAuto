using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace RmsAuto.Store.Cms.Entities
{
    public partial class Disc
    {
        public decimal Price { get; set; }
        public string Ref { get; set; }
        public object SparePart { get; set; }

        partial void OnValidate(ChangeAction action)
        {
            using (CmsDataContext dc = new CmsDataContext())
            {
                if (this.ManufaturerId == null)
                    this.ManufaturerId = dc.DiscBrands.SingleOrDefault(m => m.Name == this.Manufacturer).Id;
                if (this.ManufaturerId != null)
                    this.Manufacturer = dc.DiscBrands.SingleOrDefault(m => m.Id == this.ManufaturerId).Name;
            }
        }
    }
}
