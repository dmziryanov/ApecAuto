using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Cms.Entities
{
    public enum CatalogItemMenuType
    {
        [Text("Основное меню")]
        CommonMenu = 0,
        [Text("Меню помощи")]
        HelpMenu = 1
    }
}