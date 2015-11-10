using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class FloatBox : GuayaquilTemplate
    {
        private ListCatalogs _catalogs;

        private const string ImageResourceTemplate = "Laximo.Guayaquil.Render.Assets.images.catalogs.{0}";

        public FloatBox(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public ListCatalogs Catalogs
        {
            get { return _catalogs; }
            set { _catalogs = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            foreach (ListCatalogsRow row in Catalogs.row)
            {
                string link = FormatLink("catalog", row);
                WriteRow(writer, row, link);
            }

            //<br style="clear:both;">
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "clear:both;");
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            writer.RenderEndTag();
            
            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteRow(HtmlTextWriter writer, ListCatalogsRow row, string link)
        {
            //<div class="g_catalog_float_box" onclick="window.location=\''.$link.'\'">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_catalog_float_box");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location=\'{0}\'", link));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<div class="g_catalog_float_icon">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_catalog_float_icon");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<a class="guayaquil_tablecatalog" href="'.$link.'">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_tablecatalog");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            //<img border="0" width="40" height="40" src="'.$this->iconsFolder.$catalog['icon'].'">
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "40");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, "40");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, GetResourceUrl(string.Format(ImageResourceTemplate, row.icon)));
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();

            //</a></div>
            writer.RenderEndTag();
            writer.RenderEndTag();

            //<div class="g_catalog_float_name">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_catalog_float_name");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<a class="guayaquil_tablecatalog" href="'.$link.'">'.$catalog['name'].'</a>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_tablecatalog");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(row.name);
            writer.RenderEndTag();

            //</div></div>
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}
