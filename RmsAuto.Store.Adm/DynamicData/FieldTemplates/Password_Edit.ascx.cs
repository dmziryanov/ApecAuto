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
using RmsAuto.Common.Misc;
using RmsAuto.Store.Adm.scripts;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public partial class Password_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			ScriptsManager.RegisterJQuery( Page );

			Page.ClientScript.RegisterStartupScript(
				this.GetType(),
				"key",
				@"$(function(){
	
					if( $('#_keepPasswordBox').attr('checked') ) $('#_passwordBox').val('****').attr('disabled','disabled');
					$('#_keepPasswordBox').live( 'click', function() {
						if( this.checked )
							$('#_passwordBox').val('****').attr('disabled','disabled');
						else
							$('#_passwordBox').val('').removeAttr('disabled').focus();
					});
				});".Replace( "_keepPasswordBox", _keepPasswordBox.ClientID ).Replace( "_passwordBox", _passwordBox.ClientID ),
				   true );

            _passwordBox.MaxLength = Column.MaxLength;
            if (Column.MaxLength < 20)
				_passwordBox.Columns = Column.MaxLength;
			_passwordBox.ToolTip = Column.Description;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
        }

		public string CurrentPassword
		{
			get { return (string)ViewState[ "CurrentPassword" ]; }
			set { ViewState[ "CurrentPassword" ] = value; }
		}

		protected override void OnDataBinding( EventArgs e )
		{
			CurrentPassword = (string)DataBinder.Eval( Row, Column.Name );

			if( CurrentPassword == null )
			{
				_passwordBox.Text = "";
				_keepPasswordBox.Visible = false;
				_keepPasswordBox.Checked = false;
			}
			else
			{
				_passwordBox.Text = "****";
				_keepPasswordBox.Visible = true;
				_keepPasswordBox.Checked = true;
			}
		}


        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
			if( _keepPasswordBox.Checked )
			{
				dictionary[ Column.Name ] = CurrentPassword;
			}
			else
			{
				dictionary[ Column.Name ] = ConvertEditedValue( _passwordBox.Text.GetMD5Hash() );
			}
        }

        public override Control DataControl
        {
            get
            {
				return _passwordBox;
            }
        }
    }
}
