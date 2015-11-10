mso.textEditorPopup.setTextEditor({

	__popupEditor : null,
	
	edit : function( srcText, args, onSaveCallback, onCancelCallback ) {

		if( this.__popupEditor != null ) { alert("Text editor already running"); return; }

		var t = this;
		this.__popupEditor = { 
				text : srcText, 
				args : args,
				divBlocker : null,
				divPopup : null,
				onSave : function(newText) {
					onSaveCallback(newText);
					setTimeout(	(function(){ t.__closeEditor() }), 50 );
				},
				onCancel : function(newText) {
					if( newText==srcText || confirm( 'Закрыть окно без сохранения?' ) )
					{
						setTimeout(	(function(){ t.__closeEditor() }), 50 );
						if( onCancelCallback ) onCancelCallback();
					}
				},
				onError : function(e) {
					alert( e.message );
					setTimeout(	(function(){ t.__closeEditor() }), 50 );
				},
				onScrollOrResize : function() {
					var wnd = $(window);
					t.__popupEditor.divBlocker.css(
						{	'top' : 0,
							'left' : 0,
							'width' : $(document).width(),
							'height' : $(document).height() } );
					t.__popupEditor.divPopup.css(
						{	'top' : wnd.scrollTop()+50,
							'left' : 100,
							'width' : wnd.width()-200,
							'height' : wnd.height()-100 } );
				}
			};
	
		
		//Блокировка контента
		this.__popupEditor.divBlocker = $('<div style="position:absolute;left:0;top:0;width:100%;height:100%;display:block;background:#FFF;opacity:0.6;-ms-filter:\'alpha(opacity=60)\';filter:alpha(opacity=60);"></div>');
		this.__popupEditor.divBlocker.appendTo('body');

		//Всплывающее окно
		this.__popupEditor.divPopup = 
			$(	'<div style="position:absolute;left:100px;top:98px;right:100px;bottom:50px;width:expression(document.documentElement.clientWidth-200);height:expression(document.documentElement.clientHeight-150);">'+
				'<iframe src="../scripts/tinymce/index.aspx" style="width:100%;height:100%" frameborder="0" scrolling="no"></iframe>'+
				'</div>' );
		this.__popupEditor.divPopup.appendTo('body');
		
		$(window).bind('scroll',this.__popupEditor.onScrollOrResize);
		$(window).bind('resize',this.__popupEditor.onScrollOrResize);
		this.__popupEditor.onScrollOrResize();

	},
	
	__closeEditor : function()
	{
		if( this.__popupEditor == null ) return;
		
		$(window).unbind('scroll',this.__popupEditor.onScrollOrResize);
		$(window).unbind('resize',this.__popupEditor.onScrollOrResize);
		
		if( this.__popupEditor.divPopup ) this.__popupEditor.divPopup.remove();
		if( this.__popupEditor.divBlocker ) this.__popupEditor.divBlocker.remove();
		
		this.__popupEditor = null;
	}
	
});
