using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public abstract class CatalogsList : GuayaquilTemplate
    {
        private ListCatalogs _catalogs;
        private string[] _columns = new string[] { "icon", "name", "version" };

        private const string ImageResourceTemplate = "Laximo.Guayaquil.Render.Assets.images.catalogs.{0}";
        
        protected CatalogsList(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        protected abstract bool IsNeedWriteHeader(int indexRow);

        protected abstract bool IsNeedWriteFinishedRow(int indexRow);

        public ListCatalogs Catalogs
        {
            get { return _catalogs; }
            set { _catalogs = value; }
        }

        public string[] Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //'<table border=\"0\" width=\"100%\"><tr><td>'
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            for (int i = 1; i <= Catalogs.row.Length; i++)
            {
                ListCatalogsRow row = Catalogs.row[i - 1];
                string link = FormatLink("catalog", row);

                if (IsNeedWriteHeader(i))
                {
                    //<table class=\"guayaquil_tablecatalog\" border=\"0\">
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_tablecatalog");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    WriteHeader(writer);
                }
                WriteRow(writer, row, link);

                if (IsNeedWriteFinishedRow(i))
                {
                    //</table></td><td valign=top>
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                }
                if (i == Catalogs.row.Length)
                {
                    //</table>
                    writer.RenderEndTag();
                }
            }

            //</td></tr></table>
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteRow(HtmlTextWriter writer, ListCatalogsRow row, string link)
        {
            //'<tr onmouseout="this.className=\'\';" onmouseover="this.className=\'over\';" onclick="window.location=\''.$link.'\'">'
            writer.AddAttribute("onmouseout", "this.className=\'\';");
            writer.AddAttribute("onmouseover", "this.className=\'over\';");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location=\'{0}\'", link));
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (string column in Columns)
            {
                WriteRowCell(writer, row, link, column.ToLower());
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteRowCell(HtmlTextWriter writer, ListCatalogsRow row, string link, string column)
        {
            //<td valign="center">
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            //<a class="guayaquil_tablecatalog" href="'.$link.'">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_tablecatalog");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            WriteRowCellValue(writer, row, column);

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteRowCellValue(HtmlTextWriter writer, ListCatalogsRow row, string column)
        {
            switch (column)
            {
                case "icon":
                    //<img border="0" width="40" height="40" src="'.$this->iconsFolder.$catalog['icon'].'">
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "40");
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "40");
                    writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                        GetResourceUrl(String.Format(ImageResourceTemplate, row.icon)));
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                    break;
                case "name":
                    //'.$catalog['name'].'
                    writer.Write(row.name);
                    break;
                case "version":
                    //'.$catalog['version'].'
                    writer.Write(row.version);
                    break;
            }
        }

        protected virtual void WriteHeader(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (string column in Columns)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(WriteHeaderCellValue(column.ToLower()));
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        protected virtual string WriteHeaderCellValue(string column)
        {
            switch (column)
            {
                case "icon":
                    return HtmlTextWriter.SpaceChar.ToString();//'&nbsp;'
                case "name":
                    return GetLocalizedString("CatalogTitle");
                case "version":
                    return GetLocalizedString("CatalogDate");
                default:
                    return string.Empty;
            }
        }
    
    }
}