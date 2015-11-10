using System;
using System.Collections.Generic;
using RmsAuto.TechDoc.Entities.TecdocBase;
using System.Data.Linq;

namespace RmsAuto.TechDoc.Cache
{
	public class TDCacheHelper
	{
		private static object _sync = new object();

        private static Dictionary<int, Country> _countries;

        public static Dictionary<int, Country> GetCountries()
		{
            if (_countries == null)
            {
                lock (_sync)
                {
                    if (_countries == null)
                    {
                        using (var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext())
                        {
                            var dlo = new DataLoadOptions();
                            dlo.LoadWith<Country>(co => co.Name);
                            ctx.LoadOptions = dlo;

                            _countries = ctx.ListCountries();
                        }
                    }
                }
            }
			return _countries;
		}
	}
}
