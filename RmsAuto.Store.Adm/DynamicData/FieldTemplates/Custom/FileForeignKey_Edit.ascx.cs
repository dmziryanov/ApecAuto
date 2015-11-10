using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Adm.scripts;
using RmsAuto.Store.Adm.Controls.FileManager;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
    public partial class FileForeignKey_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
		protected string GetFileUrl( int fileID )
		{
			return FileManagerControl.GetFileUrl( fileID );
		}
		protected string GetThumbnailUrl( int fileID )
		{
			return FileManagerControl.GetThumbnailUrl( fileID );
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			ScriptsManager.RegisterJQuery( Page );
			ScriptsManager.RegisterMsoFramework( Page );
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit)
            {
				string foreignkey = ForeignKeyColumn.GetForeignKeyString( Row );
				FileIDHidden.Value = foreignkey;
            }
        }

		protected string GetFileJS()
		{
			if( Row != null )
			{
				string foreignkey = ForeignKeyColumn.GetForeignKeyString( Row );
				if( !string.IsNullOrEmpty( foreignkey ) )
				{
                    using (var dc = new DCWrappersFactory<CmsDataContext>())
					{
						File file = dc.DataContext.Files.Where( f => f.FileID == Convert.ToInt32( foreignkey ) ).FirstOrDefault();
						if( file != null )
						{
							return string.Format(
							"{{type:'{0}',isFolder:false,isImage:{1},fileID:{2},fileName:'{3}',fileNote:'{4}',fileUrl:'{5}',imageWidth:{6},imageHeight:{7}}}",
								file.IsImage ? "image" : "file",
								file.IsImage ? "true" : "false",
								file.FileID,
								file.FileName.Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" ),
								( file.FileNote ?? "" ).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" ),
								GetFileUrl( file.FileID ),
								file.ImageWidth ?? 0,
								file.ImageHeight ?? 0
								);
						}
					}
				}
			}
			return "null";
		
		}

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            // If it's an empty string, change it to null
			string val = FileIDHidden.Value;
            if (val == String.Empty)
                val = null;

			ExtractForeignKey( dictionary, val );
        }
		

        public override Control DataControl
        {
            get
            {
                return FileIDHidden;
            }
        }
    }
}
