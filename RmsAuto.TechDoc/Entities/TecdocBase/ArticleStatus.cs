using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public enum ArticleStatus : byte
    {
        [Text("В подготовке")]
        InProcess = 0,
        [Text("Нормальный")]
        Normal = 1,
        [Text("Не поставляется")]
        NotSupplied = 2,
        [Text("Поставляется только по спецификации")]
        SuppliedOnlyBySpec = 5,
        [Text("Псевдо-изделие")]
        PseudoArticle = 7,
        [Text("Изделие снятое с производства")]
        Discontinued = 8,
        [Text("Больше не поставляется")]
        NotSuppliesMore = 9,
        [Text("По запросу")]
        ByRequest = 11,
        [Text("Не может быть получен отдельно")]
        CantBeGotAlone = 12
    }
}
