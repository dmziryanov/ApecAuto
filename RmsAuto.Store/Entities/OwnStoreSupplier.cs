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
    [MetadataType(typeof(OwnStoreSupplierMetadata))]
    public partial class OwnStoreSupplier
    {

    }

    [DisplayName("Собственные склады наличия")]
    [Sort("SupplierID")]
    public partial class OwnStoreSupplierMetadata
    {
        [DisplayName("ID поставщика")]
        [Required(ErrorMessage = "не задано ID поставщика")]
        public object SupplierID { get; set; }

        [UIHint("Custom/Html")]
        [DisplayName("Описание")]
        public object Description { get; set; }

        [DisplayName("Выдавать в результатах для поиска")]
        [Required(ErrorMessage = "не заданы настройки выдачи поиска")]
        public object IsTopSearchResults { get; set; }
    }
}
