using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class UnitImage : GuayaquilTemplate
    {
        private int _containerWidth;
        private int _containerHeight;
        private GetUnitInfo _unitInfo;
        private ListImageMapByUnit _imageMap;

        private const int DefaultContainerWidth = 740;
        private const int DefaultContainerHeight = 600;

        public UnitImage(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        #region properties

        public int ContainerWidth
        {
            get
            {
                if (_containerWidth <= 0)
                {
                    _containerWidth = DefaultContainerWidth;
                }
                return _containerWidth;
            }
            set { _containerWidth = value; }
        }

        public int ContainerHeight
        {
            get
            {
                if (_containerHeight <= 0)
                {
                    _containerHeight = DefaultContainerHeight;
                }
                return _containerHeight;
            }
            set { _containerHeight = value; }
        }

        public GetUnitInfo UnitInfo
        {
            get {
                return _unitInfo;
            }
            set {
                _unitInfo = value;
            }
        }

        public ListImageMapByUnit ImageMap
        {
            get {
                return _imageMap;
            }
            set {
                _imageMap = value;
            }
        }

        public string SpacerImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.unit.spacer.gif"); }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.dimensions.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.dragscrollable.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.mousewheel.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.colorbox.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.unit.unit.js");
            AppendCSS("Laximo.Guayaquil.Render.Assets.css.unit.unit.css");
            AppendCSS("Laximo.Guayaquil.Render.Assets.css.common.colorbox.css");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            WriteUnitImage(writer);
        }

        protected virtual void WriteUnitImage(HtmlTextWriter writer)
        {
            WriteMagnifier(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "viewport");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "inline_block");
            writer.AddAttribute(HtmlTextWriterAttribute.Style,
                                String.Format(
                                    "position:absolute; border: 1px solid #777; background: white; width:{0}px; height:{1}px; overflow: auto;",
                                    ContainerWidth/2, ContainerHeight));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteImageMap(writer);
            WriteImage(writer);

            writer.RenderEndTag();
        }

        protected virtual void WriteImage(HtmlTextWriter writer)
        {
            if(UnitInfo == null || UnitInfo.FirstRow == null)
            {
                return;
            }
            
            string img = string.IsNullOrEmpty(UnitInfo.FirstRow.largeimageurl) ? NoImageUrl : UnitInfo.FirstRow.largeimageurl;
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "dragger");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, img.Replace("%size%", "source"));
            writer.AddAttribute("onLoad", "rescaleImage(-100);");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

        protected virtual void WriteImageMap(HtmlTextWriter writer)
        {
            if(ImageMap != null && ImageMap.row != null)
            {
                foreach (ListImageMapByUnitRow row in ImageMap.row)
                {
                    WriteImageMapElement(writer, row);
                }
            }
        }

        protected virtual void WriteImageMapElement(HtmlTextWriter writer, ListImageMapByUnitRow row)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, row.code);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "dragger g_highlight");
            writer.AddAttribute(HtmlTextWriterAttribute.Style,
                                String.Format(
                                    "position:absolute; width:{0}px; height:{1}px; margin-top:{2}px; margin-left:{3}px; overflow:hidden;",
                                    row.x2 - row.x1, row.y2 - row.y1, row.y1, row.x1));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            WriteImageMapElementClickableArea(writer);
            writer.RenderEndTag();
        }

        protected virtual void WriteImageMapElementClickableArea(HtmlTextWriter writer)
        {
            //'<img src="'.$this->spacer.'" width="200" height="200"/>';
            writer.AddAttribute(HtmlTextWriterAttribute.Src, SpacerImageUrl);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "200");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, "200");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

        protected virtual void WriteMagnifier(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_unit_icons");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_zoom");
            if (!string.IsNullOrEmpty(UnitInfo.FirstRow.largeimageurl))
                writer.AddAttribute("full", UnitInfo.FirstRow.largeimageurl.Replace("%size%", "source"));
            writer.AddAttribute(HtmlTextWriterAttribute.Title, string.Format("{0}: {1}", UnitInfo.FirstRow.code, UnitInfo.FirstRow.name));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Src, ZoomImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}
