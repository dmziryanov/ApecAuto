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
    [MetadataType(typeof(OurCatalogdata))]
    public partial class OurCatalog
    {

    }

    [DisplayName("Наши каталоги")]
    [Sort("Name")]
    public partial class OurCatalogdata
    {
        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название")]
        public object Name { get; set; }

        [UIHint("Custom/FileForeignKey")]
        [DisplayName("Логотип")]
        public object File { get; set; }

        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название")]
        
        public object @ref { get; set; }

        [DisplayName("Описание")]
        [Required(ErrorMessage = "не задано название")]
        public object Description { get; set; }

        [DisplayName("Приоритет")]
        [Required(ErrorMessage = "не задано название")]
        public object Priority { get; set; }

        [DisplayName("Visible")]
        [Required(ErrorMessage = "не задано название")]
        public object Visible { get; set; }

    }
}
