var mso = {
	
};

//mso.viewport = {
//	onresize : function() {
//	}
//};

mso.textEditorPopup = {

	_textEditor : null,
	
	setTextEditor : function(textEditor) {
		this._textEditor = textEditor;
	},

	editField : function( field, args, onSavingCallback, onSaveCallback, onCancelCallback ) {
		this.edit( 
			field.val(), args ? args : {}, 
			(function(newText){ 
				var flag = true;
				if( onSavingCallback ) {
					var res = onSavingCallback( newText ); 
					if( typeof(res)=="boolean" ) flag = res;
				}
				if( flag ) {
					field.val( newText );
					if( onSaveCallback ) { onSaveCallback(newText); }
					field.get(0).focus();
				}
			}),
			onCancelCallback
			);
	},

	edit : function( text, args, onSaveCallback, onCancelCallback ) {
		if( this._textEditor )
			this._textEditor.edit( text, args, onSaveCallback, onCancelCallback );
		else
			alert('Text editor not implemented');		
	}
};

mso.imageManagerPopup = {

	__popupWindow : null,
	
	select : function( selectType, onSaveCallback, onCancelCallback ) {

		if( this.__popupWindow != null ) { alert("Image manager already running"); return; }

		var t = this;
		this.__popupWindow = { 
				divBlocker : null,
				divPopup : null,
				onSave : function(fileID,fileName) {
					onSaveCallback(fileID,fileName);
					t.__closeWindow();
				},
				onCancel : function(newText) {
					if( onCancelCallback ) onCancelCallback();
					t.__closeWindow();
				},
				onError : function(e) {
					alert( e.message );
					t.__closeWindow();
				},
				onScrollOrResize : function() {
					var wnd = $(window);
					t.__popupWindow.divBlocker.css(
						{	'top' : 0,
							'left' : 0,
							'width' : $(document).width(),
							'height' : $(document).height() } );
					t.__popupWindow.divPopup.css(
						{	'top' : wnd.scrollTop()+50,
							'left' : 100/*,
							'width' : wnd.width()-200,
							'height' : wnd.height()-100*/ } );
				}
			};
	
		
		//Блокировка контента
		this.__popupWindow.divBlocker = $('<div style="position:absolute;left:0;top:0;width:100%;height:100%;display:block;background:#FFF;opacity:0.6;-ms-filter:\'alpha(opacity=60)\';filter:alpha(opacity=60);"></div>');
		this.__popupWindow.divBlocker.appendTo('body');

		//Всплывающее окно
		var url = '../Controls/FileManager/FileManagerPopup.aspx';
		url += '?select='+selectType;
		this.__popupWindow.divPopup = 
			$(	'<div style="position:absolute;left:100px;top:98px;right:100px;bottom:50px;width:898px;height:455px;">'+
				'<iframe src="'+url+'" style="width:100%;height:100%" frameborder="0" scrolling="no"></iframe>'+
				'</div>' );
		this.__popupWindow.divPopup.appendTo('body');

		$(window).bind('scroll',this.__popupWindow.onScrollOrResize);
		$(window).bind('resize',this.__popupWindow.onScrollOrResize);
		this.__popupWindow.onScrollOrResize();
	},
	
	__closeWindow : function()
	{
		if( this.__popupWindow == null ) return;
		
		$(window).unbind('scroll',this.__popupWindow.onScrollOrResize);
		$(window).unbind('resize',this.__popupWindow.onScrollOrResize);

		if( this.__popupWindow.divPopup ) this.__popupWindow.divPopup.remove();
		if( this.__popupWindow.divBlocker ) this.__popupWindow.divBlocker.remove();
		
		this.__popupWindow = null;
	}
	
};