using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class VinRequestModification
    {
        public int Id { get; protected set; }
        public VinRequestModel Model { get; set; }
        public string Name { get; set; }

        public VinRequestModification(int id)
        {
            this.Id = id;
        }

        public VinRequestModification(int id, string name, VinRequestModel mdl)
            : this(id)
        {
            this.Name = name;
            this.Model = mdl;
        }
    }
}
