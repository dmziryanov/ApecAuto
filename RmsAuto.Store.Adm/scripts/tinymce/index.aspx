<%@ Page EnableEventValidation="False"  ValidateRequest="False" language="c#" AutoEventWireup="false" %>

<script runat="server" language="C#">

override protected void OnLoad( EventArgs e )
{
	base.OnLoad( e );
	Response.Cache.SetCacheability( HttpCacheability.NoCache );

}

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>DHTML Editor</title>

<script language="javascript" src="<%=ResolveUrl( "~/scripts/jquery-1.3.2.min.js" )%>"></script>
<script language="javascript" src="<%=ResolveUrl( "~/scripts/tinymce/jscripts/jquery.tinymce.js" )%>"></script>

<script language="javascript" type="text/javascript">

$(function(){
	try
	{
		_init();
	}
	catch( e )
	{
		if(__popupEditor)
			__popupEditor.onError( e );
		else
			alert(e.message);
	}
	$(window).resize( _onResize );
});

var __popupEditor = window.parent.mso.textEditorPopup._textEditor.__popupEditor;
function _init()
{
	document.getElementById('txt').value = __popupEditor.text;

	var args = __popupEditor.args;

	var wnd = $(window);

	
		$('#txt').tinymce({
			// Location of TinyMCE script
			script_url : '<%=ResolveUrl( "~/scripts/tinymce/jscripts/tiny_mce.js" )%>',
			// General options
			mode : "exact",
			elements : "txt",
			theme : "advanced",
			plugins : "safari,pagebreak,style,table,save,advhr,advimage,advlink,inlinepopups,insertdatetime,preview,searchreplace,print,contextmenu,paste,directionality,noneditable,visualchars,nonbreaking,rms_imagemanager",
			//plugins : "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

			// Layout
			width : wnd.width(),
			height : wnd.height(),
			
			language : "ru",
			
			// Theme options 
			theme_advanced_buttons1 : "save,newdocument,|,preview,print,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
			theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,|,forecolor,backcolor",
			theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,advhr,|,ltr,rtl,|,styleprops,|,visualchars,nonbreaking",
			//theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,visualchars,nonbreaking,pagebreak",
			//theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
			//theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
			//theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
			//theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",
			theme_advanced_toolbar_location : "top",
			theme_advanced_toolbar_align : "left",
			theme_advanced_statusbar_location : "bottom",
			theme_advanced_resizing : false,

			// Optimizer Arguments
			mso_args : __popupEditor.args,

			// Example content CSS (should be your site CSS)
			content_css : args.css ? args.css : "css/content.css",

			// Drop lists for link/image/media/template dialogs
			//template_external_list_url : "lists/template_list.js",
			//external_link_list_url : "lists/link_list.js",
			//external_image_list_url : "lists/image_list.aspx?ObjectId="+escape(args.objectId),
			//media_external_list_url : "lists/media_list.js",
			relative_urls : false,
			//document_base_url : "/",


			// Replace values for the template plugin
			template_replace_values : {
				username : "Some User",
				staffid : "991234"
			},
			
			save_onsavecallback : function(ed) {
				__popupEditor.onSave(ed.getContent());
			},

			init_instance_callback : function(inst) {
				//workaround: call onresize for non ie browsers
				_onResize();

				//show close button
				document.getElementById('_popupEditorCloseButton').style.visibility = "visible";

				//set focus to editor
				inst.focus();
			},

			//file_browser_callback : args.objectId ? 'myFileBrowser' : null,
			
			setup : function(ed) { 
				/*// Add a custom button 
				ed.addButton( 'mybutton', { 
					title : 'My button', 
					image : 'img/example.gif', 
					onclick : function() { 
					ed.selection.setContent('<STRONG>Hello world!</STRONG>'); 
					} 
				});*/
			}
		});
	
		$('#txt').show();
}



function _closeButtonClick()
{
	__popupEditor.onCancel(tinyMCE.get('txt').getContent());
}
function _onResize()
{
	var wnd = $(window);
	tinyMCE.get('txt').theme.resizeTo(wnd.width(), wnd.height());
}

</script>
</head>
<body style="margin: 0 0 0 0;padding: 0 0 0 0">

	<input id="_popupEditorCloseButton" style="position:absolute;top:0;right:0;width:16;height:16;visibility:hidden" type="image" src="close.gif" onclick="return _closeButtonClick();" />

	<textarea id="txt" style="visibility:hidden" cols="40" rows="10"></textarea>

</body>
</html>
