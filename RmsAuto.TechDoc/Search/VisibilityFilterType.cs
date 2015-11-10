using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.TechDoc.Search
{
	[Flags]
	public enum VisibilityFilterType
	{
		Country,
		Manufacturer,
		Model,
		Modification
	}
}
