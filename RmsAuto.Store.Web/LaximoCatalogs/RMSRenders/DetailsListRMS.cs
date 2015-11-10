using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.RMSRenders
{
	public class DetailsListRMS : DetailsList
	{
		public DetailsListRMS(IGuayaquilExtender extender, ICatalog catalog)
            : base(extender, catalog)
        {
		}

		#region +++ GuayaquilTemplate +++
		public new string ClosedImageUrl
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(GuayaquilTemplate), "Laximo.Guayaquil.Render.Assets.images.details.images.closed.gif"); }
		}

		public new string CartImageUrl
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(GuayaquilTemplate), "Laximo.Guayaquil.Render.Assets.images.details.images.cart.gif"); }
		}

		public new string DetailInfoImageUrl
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(GuayaquilTemplate), "Laximo.Guayaquil.Render.Assets.images.details.images.info.gif"); }
		}

		public new string ZoomImageUrl
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(GuayaquilTemplate), "Laximo.Guayaquil.Render.Assets.images.common.zoom.png"); }
		}

		public new string NoImageUrl
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(GuayaquilTemplate), "Laximo.Guayaquil.Render.Assets.images.common.noimage.png"); }
		}
		#endregion

		#region +++ DetailsList +++
		public new string OpennedImageUrl
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(DetailsList), "Laximo.Guayaquil.Render.Assets.images.details.images.openned.gif"); }
		}

		public new string FullReplacementImage
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(DetailsList), "Laximo.Guayaquil.Render.Assets.images.details.images.replacement.gif"); }
		}

		public new string ForwardReplacementImage
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(DetailsList), "Laximo.Guayaquil.Render.Assets.images.details.images.replacement-forward.gif"); }
		}

		public new string BackwardReplacementImage
		{
			get { return Page.ClientScript.GetWebResourceUrl(typeof(DetailsList), "Laximo.Guayaquil.Render.Assets.images.details.images.replacement-backward.gif"); }
		}
		#endregion

		protected override void OnInit(EventArgs e)
		{
			_AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.js");
			_AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.tooltip.js");
			_AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.details.detailslist.js");
		}

		protected override void WriteDetailCellValue(HtmlTextWriter writer, DetailInfo detail, string column, int visibility)
        {
            if (column.Equals("oem"))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, RmsAuto.Store.Cms.Routing.UrlManager.GetSearchManufacturersUrl(detail.oem, true));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(detail.oem);
                writer.RenderEndTag();
            }
            else if (column.Equals("tooltip"))
            {
				//<img src="'.$this->detailinfoimage.'" width="22" height="22">
				writer.AddAttribute(HtmlTextWriterAttribute.Src, DetailInfoImageUrl);
				writer.AddAttribute(HtmlTextWriterAttribute.Width, "22");
				writer.AddAttribute(HtmlTextWriterAttribute.Height, "22");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
            }
            else
            {
                base.WriteDetailCellValue(writer, detail, column, visibility);
            }
        }

		protected override void WriteJavaScript(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.WriteLine(String.Format("var opennedimage = '{0}';", OpennedImageUrl));
			writer.WriteLine(String.Format("var closedimage = '{0}';", ClosedImageUrl));
			writer.WriteLine("jQuery(document).ready(function($){");
			writer.WriteLine(
				"jQuery('td.g_rowdatahint').tooltip({track: true, delay: 0, showURL: false, fade: 250, positionLeft: true, bodyHandler: g_getHint});");
			writer.WriteLine(
				"jQuery('img.g_addtocart').tooltip({track: true, delay: 0, showURL: false, fade: 250, bodyHandler: function() { return '" +
				GetLocalizedString("AddToCartHint") + "'; } });");
			writer.WriteLine(
				"jQuery('td[name=c_toggle] img').tooltip({track: true, delay: 0, showURL: false, fade: 250, bodyHandler: function() { return '" +
				GetLocalizedString("ToggleReplacements") + "'; } });");
			writer.WriteLine(
				"jQuery('img.c_rfull').tooltip({track: true, delay: 0, showURL: false, fade: 250, bodyHandler: function() { return '<h3>" +
				GetLocalizedString("ReplacementWay") + "</h3>" + GetLocalizedString("ReplacementWayFull") + "'; } });");
			writer.WriteLine(
				"jQuery('img.c_rforw').tooltip({track: true, delay: 0, showURL: false, fade: 250,	bodyHandler: function() { return '<h3>" +
				GetLocalizedString("ReplacementWay") + "</h3>" + GetLocalizedString("ReplacementWayForward") +
				"'; } });");
			writer.WriteLine(
				"jQuery('img.c_rbackw').tooltip({track: true, delay: 0, showURL: false, fade: 250, bodyHandler: function() { return '<h3>" +
				GetLocalizedString("ReplacementWay") + "</h3>" + GetLocalizedString("ReplacementWayBackward") +
				"'; } });");
			writer.WriteLine("});");
			writer.RenderEndTag();
		}

		private void _AppendJavaScript(string resName)
		{
			ClientScriptManager scriptManager = Page.ClientScript;
            //RegisterClientScriptResource prevent dublicate scripts
            scriptManager.RegisterClientScriptResource(typeof(GuayaquilTemplate), resName);
		}
	}
}
