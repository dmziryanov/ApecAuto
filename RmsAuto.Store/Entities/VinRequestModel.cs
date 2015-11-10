using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class VinRequestModel
    {
        public int Id { get; protected set; }
        public VinRequestManufacturer Manufacturer { get; set; }
        public string Name { get; set; }

        public VinRequestModel(int id)
        {
            this.Id = id;
        }

        public VinRequestModel(int id, string name, VinRequestManufacturer mfr) : this(id)
        {
            this.Name = name;
            this.Manufacturer = mfr;
        }
    }
}
