using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Configuration;
using RmsAuto.TechDoc.Entities.TecdocBase;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.TechDoc.Search
{
	public class CountryVisibilityFilter : IVisibilityFilter
	{
        List<CountryDelivery> _deliveriesHelper;
        const string CacheKey = "__VisibleCountries";
		List<CountryDelivery> Deliveries
		{
            get
            {
                try
                {
                    return (List<CountryDelivery>)HttpContext.Current.Cache[CacheKey];
                }
                catch (NullReferenceException)
                {
                    //  Запуск из тестов
                    return this._deliveriesHelper;
                }
            }
            set
            {
                try
                {
                    HttpContext.Current.Cache.Insert(
                                             CacheKey,
                                             value,
                                             null,
                                             System.Web.Caching.Cache.NoAbsoluteExpiration,
                                             new TimeSpan(30, 0, 0, 0),
                                             System.Web.Caching.CacheItemPriority.Normal,
                                             null);
                }
                catch (NullReferenceException)
                {
                    //  Запуск из тестов
                    this._deliveriesHelper = value;
                }
            }
		}

		public CountryVisibilityFilter()
		{
            InitFilter();
		}

        protected virtual void InitFilter()
        {
            using (var ctx = new TecdocStoreDataContext())
            {
                var countries = ctx.CountryVisibilities.Select(cv => cv.CountryID).ToArray();
                using (var ctxBase = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext())
                {
                    //тяжелый запрос, кидаем в кэш. 
                    //При удалении/добавлении видимой страны ("Видимость стран/регионов") кэш очищается в OnValidate класса TechDoc.Entities.CountryVisibility
                    this.Deliveries = ctxBase.CountryDeliveries.Where(cd => countries.Contains(cd.CountryID)).ToList();
                }
            }
        }

		public void Apply<T>(List<T> items) where T : RmsAuto.TechDoc.Entities.ITecdocItem
		{
			Type t = typeof (T);
			if ( t == typeof(Manufacturer))
				items.RemoveAll(i => !Deliveries.Exists(d => d.ManufacturerID == i.ID));
			else if (t == typeof(Model))
				items.RemoveAll(i => !Deliveries.Exists(d => d.ModelID == i.ID));
			else
				items.RemoveAll(i => !Deliveries.Exists(d => d.TypeID == i.ID));
		}
	}
}
