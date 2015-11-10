using System;
using System.Web;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class UnitsList : GuayaquilTemplate
    {
        private int _imageSize;
        private ListUnits _units;

        private const int DefaultImageSize = 175;

        public UnitsList(IGuayaquilExtender extender, ICatalog catalog)
            : base(extender, catalog)
        {
        }

        #region properties

        public int ImageSize
        {
            get
            {
                if(_imageSize <= 0)
                {
                    _imageSize = DefaultImageSize;
                }
                return _imageSize;
            }
            set { _imageSize = value; }
        }

        public ListUnits Units
        {
            get { return _units; }
            set { _units = value; }
        }

        public string FilterImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.filter.png"); }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.vehicle.unitlist-floated.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.colorbox.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.tooltip.js");
            AppendCSS("Laximo.Guayaquil.Render.Assets.css.common.colorbox.css");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            int noteId = 0;

            if (Units != null && Units.row != null && Units.row.Length > 0)
            {
                foreach (ListUnitsRow row in Units.row)
                {
                    string link = FormatLink(string.IsNullOrEmpty(row.filter) ? "unit" : "filter", row);
                    WriteItem(writer, row, link, row.filter, ++noteId);
                }
            }
        }

        protected virtual void WriteItem(HtmlTextWriter writer, ListUnitsRow row, string link, string filter, int noteId)
        {
            //<a name="_'.$row['code'].'"></a>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, String.Format("_{0}", row.code));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderEndTag();
            //<div class="guayaquil_floatunitlist_box guayaquil_floatunitlist_'.$this->imagesize.'" note="'.$noteid.'">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, String.Format("guayaquil_floatunitlist_box guayaquil_floatunitlist_{0}", ImageSize));
            writer.AddAttribute("note", noteId.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //<div class="guayaquil_unit_icons">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_unit_icons");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!string.IsNullOrEmpty(filter))
            {
                //<div class="guayaquil_unit_filter"><img src="'.$this->filter_image.'"></div>
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_unit_filter");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, FilterImageUrl);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            //<div class="guayaquil_zoom" full="'.str_replace('%size%', 'source', $row['imageurl']).'" title="'.$row['code'].': '.$row['name'].'">
            //<img src="'.$this->zoom_image.'"></div></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_zoom");
            if(!string.IsNullOrEmpty(row.imageurl))
                writer.AddAttribute("full", row.imageurl.Replace("%size%", "source"));
            writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format("{0}: {1}", row.code, row.name));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, ZoomImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<div name="'.trim($row['code']).'" class="g_highlight">
            writer.AddAttribute(HtmlTextWriterAttribute.Name, row.code);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_highlight");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //<div name="'.$row['code'].'" class="g_highlight" onclick="window.location=\''.$link.'\'">
            writer.AddAttribute(HtmlTextWriterAttribute.Name, row.code.Trim());
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_highlight");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location='{0}'", link));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //<table class="guayaquil_floatunitlist" border="0">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_floatunitlist");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            //<tr><td valign="center" class="guayaquil_floatunitlist_image_'.$this->imagesize.'">
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, String.Concat("guayaquil_floatunitlist_image_", ImageSize));
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            WriteImage(writer, row, link);

            //</td></tr>
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<tr><td class="guayaquil_floatunitlist_title" id="unm'.$noteid.'">
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_floatunitlist_title");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Concat("unm", noteId));
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            
            WriteUnitName(writer, row, link, filter);
            
            //</td></tr></table></div></div>
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            string note = GetUnitNote(row.note);
            if(!string.IsNullOrEmpty(note))
            {
                //<span id="utt'.$noteid.'" style="display:none">'.htmlspecialchars($note).'</span>
                writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Concat("utt", noteId));
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(HttpUtility.HtmlEncode(note));
                writer.RenderEndTag();
            }
        }

        protected virtual void WriteUnitName(HtmlTextWriter writer, ListUnitsRow row, string link, string filter)
        {
            //<center><a href="'.$link.'"'.(strlen($filter) > 0 ? ' class="g_filter_unit"' : '').'>'.$this->DrawUnitNameValue($row).'</a></center>
            writer.RenderBeginTag(HtmlTextWriterTag.Center);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            if(!string.IsNullOrEmpty(filter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_unit");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            WriteUnitNameValue(writer, row);

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteUnitNameValue(HtmlTextWriter writer, ListUnitsRow row)
        {
            //'<b>'.$row['code'].':</b> '.$row['name'];
            writer.RenderBeginTag(HtmlTextWriterTag.B);
            writer.Write(String.Format("{0}:", row.code));
            writer.RenderEndTag();
            writer.Write(row.name);
        }

        protected virtual void WriteImage(HtmlTextWriter writer, ListUnitsRow row, string link)
        {
            string img = string.IsNullOrEmpty(row.imageurl) ? NoImageUrl : row.imageurl;
            writer.RenderBeginTag(HtmlTextWriterTag.Center);
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, img.Replace("%size%", ImageSize.ToString()));
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual string GetUnitNote(string note)
        {
            return note;
        }
    }
}
