using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;


namespace RmsAuto.Store.Cms.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(TireBranddata))]
    public partial class TireBrand
    {
        
    }

    [DisplayName("Бренды шин")]
    [Sort("Name")]
    public partial class TireBranddata
    {
        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название")]
        public object Name { get; set; }

        [UIHint("Custom/FileForeignKey")]
        [DisplayName("Логотип")]
        public object File { get; set; }

        [DisplayName("Отображать в каталоге")]
        [Required(ErrorMessage = "не задано название")]
        public object IsVisible { get; set; }

    }


    [ScaffoldTable(true)]
    [MetadataType(typeof(BatteryBranddata))]
    public partial class BatteryBrand
    {

    }

    [DisplayName("Бренды аккумуляторов")]
    [Sort("Name")]
    public partial class BatteryBranddata
    {
        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название")]
        public object Name { get; set; }

        [UIHint("Custom/FileForeignKey")]
        [DisplayName("Логотип")]
        public object File { get; set; }

        [DisplayName("Отображать в каталоге")]
        [Required(ErrorMessage = "не задано название")]
        public object IsVisible { get; set; }

    }


    [ScaffoldTable(true)]
    [MetadataType(typeof(DiscBranddata))]
    public partial class DiscBrand
    {

    }

    [DisplayName("Бренды дисков")]
    [Sort("Name")]
    public partial class DiscBranddata
    {
        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название")]
        public object Name { get; set; }

        [UIHint("Custom/FileForeignKey")]
        [DisplayName("Логотип")]
        public object File { get; set; }

        [DisplayName("Отображать в каталоге")]
        [Required(ErrorMessage = "не задано название")]
        public object IsVisible { get; set; }

    } 
}
