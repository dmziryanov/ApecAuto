using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg.Entities;

namespace RmsAuto.Store.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(RequestRepeatCountMetadata))]
    public partial class RequestRepeatCount
    {
        public static Int32 GetRepeatCount(AcctgDataContext dc, String requestName)
        {
            var el =
                   from o in dc.RequestRepeatCounts
                   where o.RequestName == requestName
                   select o;
            try
            {
                return el.First().RepeatCount;
            }
            catch
            {
                return 0;
            }
        }

        public static Int32 GetRepeatCount(String requestName)
        {
            using (var dc = new AcctgDataContext())
            {
                return GetRepeatCount(dc, requestName);
            }
        }
    }

    [DisplayName("Настройка запросов к Ханса")]
    public partial class RequestRepeatCountMetadata
    {
        [ScaffoldColumn(true)]
        [DisplayName("ID")]
        public object ID { get; set; }

        [ScaffoldColumn(true)]
        [DisplayName("Название")]
        public object RequestName { get; private set; }

        [ScaffoldColumn(true)]
        [DisplayName("Количество повторов")]
        public object RepeatCount { get; set; }
    }
}
