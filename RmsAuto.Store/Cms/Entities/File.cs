using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
	[ScaffoldTable( true )]
	[MetadataType( typeof( FileMetadata ) )]
	public partial class File
	{
	}

	[DisplayName( "Файлы" )]
	public partial class FileMetadata
	{
	}
}
