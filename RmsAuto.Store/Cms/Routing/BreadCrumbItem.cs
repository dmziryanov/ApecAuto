using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Cms.Routing
{
	public class BreadCrumbItem
	{
		public string Name { get; private set; }
		public string Url { get; private set; }

		public BreadCrumbItem( string name, string url )
		{
			Name = name;
			Url = url;
		}
	}
}
