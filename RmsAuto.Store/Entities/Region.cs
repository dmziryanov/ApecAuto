using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace RmsAuto.Store.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(RegionMetadata))]
    public partial class Region
    {
    }

    [DisplayName("Регионы")]
    [Sort("RegionName")]
    [ReadOnly(true)]
    public partial class RegionMetadata
    {
        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название региона")]

        public object RegionName { get; set; }

        /*[UIHint("Custom/AcctgRef", null, "BindingOptions", "Stores;StoreId;StoreName")]
        [DisplayName("Склад (Hansa)")]
        public object StoreId { get; set; }*/
    }
}
