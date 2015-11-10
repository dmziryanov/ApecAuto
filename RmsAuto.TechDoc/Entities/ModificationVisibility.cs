using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RmsAuto.TechDoc.Entities
{
    [ScaffoldTable(false)]
	public partial class InvisibleModification
	{
        public InvisibleModification(int modID)
		{
			this.ModificationID = modID;
		}
	}
}
