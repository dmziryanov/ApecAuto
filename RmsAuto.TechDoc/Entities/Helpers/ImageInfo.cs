using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.TechDoc.Entities.Helpers
{
	public class ImageInfo
	{
		//  [DBFieldMapping("GRD_ID")]
		public int ID
		{
			get; set;
		}


		//  [DBFieldMapping("GRD_GRAPHIC")]
		public byte[] Content
		{
			get; set;
		}

	    //  [DBFieldMapping("DOC_EXTENSION")]
		public string Extension
		{
			get; set;
		}

		//  [DBFieldMapping("LGA_SORT")]
		public int SortIndex
		{ 
			get; set;
		}

        public ImageInfo(int id, byte[] content, string ext, int sortIndex)
        {
            this.ID = id;
            this.Content = content;
            this.Extension = ext;
            this.SortIndex = sortIndex;
        }

        public ImageInfo(int id, System.Data.Linq.Binary content, string ext, int sortIndex)
            : this(id, content.ToArray(), ext, sortIndex) { }

        public ImageInfo()
        {

        }
	}
}
