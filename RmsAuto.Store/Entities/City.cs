using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;


namespace RmsAuto.Store.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(CityMetadata))] 
    public partial class City
    {

    }

    [DisplayName("Города")]
    [Sort("Name")]
    public partial class CityMetadata
    {
        [DisplayName("CityID")]
        //[UIHint("Custom/AcctgRef", null, "BindingOptions", "Cities;Name;Name")]
        public object CityID { get; set; }

        [DisplayName("Видимость")]
        //[UIHint("Custom/AcctgRef", null, "BindingOptions", "Cities;Name;Name")]
        public object Visible { get; set; }

        [UIHint("Text")]
        [DisplayName("Название")]
        [Required(ErrorMessage = "не задан")]
        public object Name { get; set; }

        [UIHint("Custom/AcctgRef", null, "BindingOptions", "Region;RegionID;RegionName")]
        [DisplayName("Регион")]
        public object RegionID { get; set; }
    }
}
