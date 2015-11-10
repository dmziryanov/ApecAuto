using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace RmsAuto.TechDoc.Entities
{
	[ScaffoldTable(false)]
	public partial class InvisibleModel
	{
        public InvisibleModel(int modelID)
		{
			this.ModelID = modelID;
		}
	}
}
