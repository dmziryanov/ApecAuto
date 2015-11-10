using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
    [ScaffoldTable( true )]
    [MetadataType( typeof( SeoShortLinkMetadata ) )]
    public partial class SeoShortLink
    {
    }

    [DisplayName( "Быстрые ссылки" )]
    [Sort( "UpdateDate" )]
    public partial class SeoShortLinkMetadata
    {
        [DisplayName( "ID" )]
        public object SeoLinksID { get; set; }
        
        [DisplayName( "Наименование" )]
        [Required( ErrorMessage = "не задано наименование" )]
        public object NameLink { get; set; }

        [DisplayName( "Ссылка" )]
        [Required( ErrorMessage = "не задана ссылка" )]
        public object Url { get; set; }

        [DisplayName( "Дата изменения" )]
        public object UpdateDate { get; private set; }

        [DisplayName( "Язык" )]
        public object Localization1 { get; set; }
    }
}
