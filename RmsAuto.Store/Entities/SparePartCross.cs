using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RmsAuto.Store.Entities
{
	[ScaffoldTable(true)]
	[MetadataType( typeof( SparePartCrossMetadata ) )]
	public partial class SparePartCross
	{

	}

	[DisplayName( "Кроссы и переходы" )]
	public partial class SparePartCrossMetadata
	{
	}
}
