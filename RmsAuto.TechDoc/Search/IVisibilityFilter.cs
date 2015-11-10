using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.TechDoc.Search
{
	public interface IVisibilityFilter
	{
		void Apply<T>(List<T> items) where T : ITecdocItem;
	}
}
