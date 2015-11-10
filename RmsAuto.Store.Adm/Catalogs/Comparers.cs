
using System;
using System.Collections.Generic;
using System.Linq;

namespace RmsAuto.Store.Adm
{
    public class BatteryComparer : IComparer<RmsAuto.Store.Cms.Entities.Battery>, IEqualityComparer<RmsAuto.Store.Cms.Entities.Battery>
    {
        #region IComparer<Tires> Members

        public int Compare(RmsAuto.Store.Cms.Entities.Battery x, RmsAuto.Store.Cms.Entities.Battery y)
        {
            if (x.PartNumber == y.PartNumber) return 0;
            var tmp = new List<string>();
            tmp.Add(x.PartNumber);
            tmp.Add(y.PartNumber);
            tmp.Sort();
            if (tmp.IndexOf(x.PartNumber) > tmp.IndexOf(y.PartNumber)) return -1;
            if (tmp.IndexOf(x.PartNumber) < tmp.IndexOf(y.PartNumber)) return 1;
            return 0;
        }

        bool IEqualityComparer<RmsAuto.Store.Cms.Entities.Battery>.Equals(RmsAuto.Store.Cms.Entities.Battery x, RmsAuto.Store.Cms.Entities.Battery y)
        {
            return (x.PartNumber == y.PartNumber);
        }

        int IEqualityComparer<RmsAuto.Store.Cms.Entities.Battery>.GetHashCode(RmsAuto.Store.Cms.Entities.Battery obj)
        {
            return obj.PartNumber.GetHashCode();
        }

        #endregion
    }

    public class DiscComparer : IComparer<RmsAuto.Store.Cms.Entities.Disc>, IEqualityComparer<RmsAuto.Store.Cms.Entities.Disc>
    {
        #region IComparer<Tires> Members

        public int Compare(RmsAuto.Store.Cms.Entities.Disc x, RmsAuto.Store.Cms.Entities.Disc y)
        {
            if (x.PartNumber == y.PartNumber) return 0;
            var tmp = new List<string>();
            tmp.Add(x.PartNumber);
            tmp.Add(y.PartNumber);
            tmp.Sort();
            if (tmp.IndexOf(x.PartNumber) > tmp.IndexOf(y.PartNumber)) return -1;
            if (tmp.IndexOf(x.PartNumber) < tmp.IndexOf(y.PartNumber)) return 1;
            return 0;
        }

        bool IEqualityComparer<RmsAuto.Store.Cms.Entities.Disc>.Equals(RmsAuto.Store.Cms.Entities.Disc x, RmsAuto.Store.Cms.Entities.Disc y)
        {
            return (x.PartNumber == y.PartNumber);
        }

        int IEqualityComparer<RmsAuto.Store.Cms.Entities.Disc>.GetHashCode(RmsAuto.Store.Cms.Entities.Disc obj)
        {
            return obj.PartNumber.GetHashCode();
        }

        #endregion
    }

    public class MyComparer : IComparer<RmsAuto.Store.Cms.Entities.Tires>, IEqualityComparer<RmsAuto.Store.Cms.Entities.Tires>
    {


        public int Compare(RmsAuto.Store.Cms.Entities.Tires x, RmsAuto.Store.Cms.Entities.Tires y)
        {
            if (x.TireNumber == y.TireNumber) return 0;
            var tmp = new List<string>();
            tmp.Add(x.TireNumber);
            tmp.Add(y.TireNumber);
            
            tmp.Sort(); //Сравниваем что больше таким образом: помещаем обе строки в массив сортируем и смотрим кто выше
            if (tmp.IndexOf(x.TireNumber) > tmp.IndexOf(y.TireNumber)) return -1;
            if (tmp.IndexOf(x.TireNumber) < tmp.IndexOf(y.TireNumber)) return 1;
            //Равны во всех остальных случаях
            return 0;
        }

        bool IEqualityComparer<RmsAuto.Store.Cms.Entities.Tires>.Equals(RmsAuto.Store.Cms.Entities.Tires x, RmsAuto.Store.Cms.Entities.Tires y)
        {
            return (x.TireNumber == y.TireNumber);
        }

        int IEqualityComparer<RmsAuto.Store.Cms.Entities.Tires>.GetHashCode(RmsAuto.Store.Cms.Entities.Tires obj)
        {
            return obj.TireNumber.GetHashCode();
        }
    }

}
