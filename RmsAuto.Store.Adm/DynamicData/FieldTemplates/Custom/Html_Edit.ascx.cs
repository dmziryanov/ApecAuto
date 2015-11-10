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
using RmsAuto.Store.Adm.scripts;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public partial class Text_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			_textBox.TextMode = TextBoxMode.MultiLine;
			_textBox.Columns = 80;
			_textBox.Rows = 10;
			_textBox.ToolTip = Column.Description;

			ScriptsManager.RegisterJQuery( Page );
			ScriptsManager.RegisterMsoFramework( Page );
			ScriptsManager.RegisterRmsTinyMce( Page );

			_editButton.OnClientClick = string.Format(
				"mso.textEditorPopup.editField($('#{0}'));return false;",
				_textBox.ClientID );

			/*

			Page.ClientScript.RegisterStartupScript(
				this.GetType(),
				"startup",
				@"
					tinyMCE.init({
						mode : 'exact',
						elements : '"+TextBox1.UniqueID+@"',
						theme : 'advanced',
						plugins : 'safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template',
						// Theme options
						theme_advanced_buttons1 : 'save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect',
						theme_advanced_buttons2 : 'cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor',
						theme_advanced_buttons3 : 'tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen',
						theme_advanced_buttons4 : 'insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak',
						theme_advanced_toolbar_location : 'top',
						theme_advanced_toolbar_align : 'left',
						theme_advanced_statusbar_location : 'bottom',
						theme_advanced_resizing : true,

						// Example content CSS (should be your site CSS)
						content_css : 'css/content.css'
						});
				",
				true );			 
			 */
			SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
			dictionary[ Column.Name ] = ConvertEditedValue( _textBox.Text );
        }

        public override Control DataControl
        {
            get
            {
				return _textBox;
            }
        }
    }
}

 