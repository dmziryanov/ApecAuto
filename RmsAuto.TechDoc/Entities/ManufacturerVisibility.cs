using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RmsAuto.TechDoc.Entities
{
	[ScaffoldTable(true)]
    [MetadataType(typeof(InvisibleManufacturerMetadata))]
	public partial class InvisibleManufacturer
	{
        public InvisibleManufacturer(int manufacturerID)
		{
			this.ManufacturerID = manufacturerID;
		}
	}

	[DisplayName("TecDocs - исключенные модели и модификации")]
    public class InvisibleManufacturerMetadata
	{
		[ScaffoldColumn(false)]
		public object ManufacturerID { get; set; }

        
	}
}
