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
using System.Collections.Generic;
using RmsAuto.Store.Adm.scripts;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
	public partial class FolderForeignKey_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			ScriptsManager.RegisterJQuery( Page );
			ScriptsManager.RegisterMsoFramework( Page );
        }

		protected override void OnDataBinding( EventArgs e )
		{
			base.OnDataBinding( e );

			if( Mode == DataBoundControlMode.Edit )
			{
				string foreignkey = ForeignKeyColumn.GetForeignKeyString( Row );
				FolderIDHidden.Value = foreignkey;
			}
		}

		protected override void ExtractValues( IOrderedDictionary dictionary )
		{
			// If it's an empty string, change it to null
			string val = FolderIDHidden.Value;
			if( val == String.Empty )
				val = null;

			ExtractForeignKey( dictionary, val );
		}

		protected string GetFolderJS()
		{
			if( Row != null )
			{
				string foreignkey = ForeignKeyColumn.GetForeignKeyString( Row );
				if( !string.IsNullOrEmpty( foreignkey ) )
				{
                    using (var dc = new DCWrappersFactory<CmsDataContext>())
					{
						Folder folder = dc.DataContext.Folders.Where( f => f.FolderID == Convert.ToInt32( foreignkey ) ).FirstOrDefault();
						if( folder != null )
						{
							string folderName = FilesDac.GetFolderPath( folder.FolderID );
							return string.Format(
							"{{type:'folder',isFolder:true,folderID:{0},folderName:'{1}'}}",
								folder.FolderID,
								folderName.Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )
								);
						}
					}
				}
			}
			return "null";
		}

		public override Control DataControl
		{
			get
			{
				return FolderIDHidden;
			}
		}
    }
}
