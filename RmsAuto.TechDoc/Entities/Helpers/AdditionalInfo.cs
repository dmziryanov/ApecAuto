using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Data;

namespace RmsAuto.TechDoc.Entities.Helpers
{
	public class AdditionalInfo
	{
		public bool HasPics { get; set; }
		public bool HasAppliedCars { get; set; }
		public bool HasDescription { get; set; }

		public int TecDocArticulId { get; set; }

		public PartKey Key { get; set; }

		public string PartDescription { get; set; }
	}
}
