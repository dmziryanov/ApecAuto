using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.TechDoc.Search
{
	public class VisibilityFilterProvider
	{
		readonly List<IVisibilityFilter> _filters = new List<IVisibilityFilter>();

		public void Clear()
		{
			_filters.Clear();
		}

		public void AddFilter(VisibilityFilterType filterType)
		{
			if ((filterType & VisibilityFilterType.Country) == VisibilityFilterType.Country)
				_filters.Add(new CountryVisibilityFilter());
			if ((filterType & VisibilityFilterType.Manufacturer) == VisibilityFilterType.Manufacturer)
				_filters.Add(new ManufacturerVisibilityFilter());
			if ((filterType & VisibilityFilterType.Model) == VisibilityFilterType.Model)
				_filters.Add(new CountryVisibilityFilter());
			if ((filterType & VisibilityFilterType.Modification) == VisibilityFilterType.Modification)
				_filters.Add(new ModificationVisibilityFilter());
		}

		public void ApplyFilters<T>(List<T> items) where T: ITecdocItem
		{
			foreach (IVisibilityFilter filter in _filters)
			{
				filter.Apply(items);
			}
		}

		public void ApplyFilter<T>(List<T> items, VisibilityFilterType filterType) where T: ITecdocItem
		{
			switch (filterType)
			{
				case VisibilityFilterType.Country:
					new CountryVisibilityFilter().Apply(items);
					break;
				case VisibilityFilterType.Manufacturer:
					new ManufacturerVisibilityFilter().Apply(items);
					break;
				case VisibilityFilterType.Model:
					new ModelVisibilityFilter().Apply(items);
					break;
			}
		}
	}
}
