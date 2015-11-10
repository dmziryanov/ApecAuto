using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class VinRequestManufacturer
    {
        public int Id { get; protected set; }
        public string Name { get; set; }

        public VinRequestManufacturer(int id)
        {
            this.Id = id;
        }

        public VinRequestManufacturer(int id, string name) : this(id)
        {
            this.Name = name;
        }
    }
}
