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
using RmsAuto.Store.Adm.Controls.FileManager;

namespace RmsAuto.Store.Adm
{
    public partial class FileForeignKeyField : System.Web.DynamicData.FieldTemplateUserControl
    {
        private bool _allowNavigation = true;
        private string _navigateUrl;

        public string NavigateUrl
        {
            get
            {
                return _navigateUrl;
            }
            set
            {
                _navigateUrl = value;
            }
        }

        public bool AllowNavigation
        {
            get
            {
                return _allowNavigation;
            }
            set
            {
                _allowNavigation = value;
            }
        }

        protected string GetDisplayString()
        {
			return FormatFieldValue( ForeignKeyColumn.ParentTable.GetDisplayString( FieldValue ) );
        }

        protected string GetNavigateUrl()
        {
			return FieldValue != null ? FileManagerControl.GetFileUrl( Convert.ToInt32( ForeignKeyColumn.GetForeignKeyString( Row ) ) ) : "";
        }

        public override Control DataControl
        {
            get
            {
                return HyperLink1;
            }
        }
    }
}
