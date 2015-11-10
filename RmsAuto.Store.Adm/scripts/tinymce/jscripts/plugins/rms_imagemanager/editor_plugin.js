/**
 * $Id: editor_plugin_src.js 677 2008-03-07 13:52:41Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2008, Moxiecode Systems AB, All rights reserved.
 */
(function() {

	tinymce.create('tinymce.plugins.ImageManagerPlugin', {
		editor : null,
		url : null,
		
		init : function(ed, url) {
		
			this.editor = ed;
			this.url = url;
			
			var args = this.editor.getParam('mso_args',{});
			
			//Register image browser callback only if objectId has value
			//if( args.objectId )
			//{
				//ed.settings.theme_advanced_image_image_browser_callback = function(field_name, url, type, win) { 
				//		return t.filebrowserCallBack(field_name, url, type, win); 
				//	};
				
				if( ed.settings.file_browser_callback )
					ed.settings.mso_imagemanager_prev_file_browser_callback = ed.settings.file_browser_callback;
					
				var t = this;
				ed.settings.file_browser_callback = function(field_name, url, type, win) { 
						if( type=='image' || type=='file' )
							return t.filebrowserCallBack(field_name, url, type, win); 
						else if( t.editor.settings.mso_imagemanager_prev_file_browser_callback )
							return t.editor.execCallback('mso_imagemanager_prev_file_browser_callback', field_name, url, type, win);
					};
				
			//}

		},
		
		filebrowserCallBack : function(field_name, url, type, win) {
			var args = this.editor.getParam('mso_args',{});

		    tinyMCE.activeEditor.windowManager.open({
				file : this.url+'/ImageManager.aspx',
				//title : 'My Image Browser',
				width : 700,  // Your dimensions may differ - toy around with them!
				height : 400,
				resizable : "no",
				inline : "yes",  // This parameter only has an effect if you use the inlinepopups plugin!
				close_previous : "no"
			}, {
				window : win,
				input : field_name,
				plugin_url : this.url
			});
		},

		getInfo : function() {
			return {
				longname : 'RMSAUTO Image Manager',
				author : 'Garant-Park-Internet',
				authorurl : 'http://metric.ru',
				infourl : '',
				version : '1.0.0.0'
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('rms_imagemanager', tinymce.plugins.ImageManagerPlugin);
})();