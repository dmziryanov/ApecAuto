using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.TechDoc.Search
{
	public class ModelVisibilityFilter :IVisibilityFilter
	{
        public void Apply<T>(List<T> items) where T : ITecdocItem
        {
            using (var ctx = new TecdocStoreDataContext())
            {
                var iItems = items.Select(i => i.ID);
                var excludes = ctx.InvisibleModels.Where(m => iItems.Contains(m.ModelID)).Select(m => m.ModelID).ToList();
                items.RemoveAll(i => excludes.Contains(i.ID));
            }
        }
	}
}
