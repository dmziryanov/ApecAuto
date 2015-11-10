var ImageManagerDialog = {
	preInit : function() {
		var url;


		if (url = tinyMCEPopup.getParam("external_image_list_url"))
			document.write('<script language="javascript" type="text/javascript" src="' + tinyMCEPopup.editor.documentBaseURI.toAbsolute(url) + '"></script>');
	},

	init : function(ed) {
		var f = document.forms[0], nl = f.elements, ed = tinyMCEPopup.editor, dom = ed.dom;

		tinyMCEPopup.resizeToInnerSize();
//		TinyMCE_EditableSelects.init();

	},

	selectImage : function( imageUrl, altText ) {
		//call this function only after page has loaded
		//otherwise tinyMCEPopup.close will close the
		//"Insert/Edit Image" or "Insert/Edit Link" window instead

		// Let TinyMCE convert the URLs
		if (typeof(TinyMCE_convertURL) != "undefined")
			imageUrl = TinyMCE_convertURL(imageUrl, null, true);
		else if (tinyMCE.convertURL)
			imageUrl = tinyMCE.convertURL(imageUrl, null, true);
		else
			imageUrl = tinyMCE.activeEditor.convertURL(imageUrl, null, true);

		var win = tinyMCEPopup.getWindowArg("window");

		// insert information now
		var n = win.document.getElementById(tinyMCEPopup.getWindowArg("input"));
		
		n.value = imageUrl;

		try {
			n.onchange();
		} catch (e) {
			// Skip it
		}
		
		// Set alt and title info
		if( altText ) {
			var f = win.document.forms[0];
			na = ['alt', 'title', 'linktitle'];
			for(var i = 0; i < na.length; i++) {
				if (f.elements[na[i]])
					f.elements[na[i]].value = altText;
			}
		}
				
		// for image browsers: update image dimensions
		if(win.getImageData) win.getImageData();

		// close popup window
		tinyMCEPopup.close();
	}

	
			
};

ImageManagerDialog.preInit();
tinyMCEPopup.onInit.add(ImageManagerDialog.init, ImageManagerDialog);
