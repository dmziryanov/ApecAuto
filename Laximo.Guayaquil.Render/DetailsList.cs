using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class DetailsList : GuayaquilTemplate
    {
        private bool _filterByGroup;
        private DetailInfo[] _details;

        private readonly Dictionary<string, int> _defaultColumns;
        private string _rowClass;
        private Dictionary<string, int> _columns;
        private static int _tableNo;

        public DetailsList(IGuayaquilExtender extender, ICatalog catalog)
            : base(extender, catalog)
        {
            _defaultColumns = new Dictionary<string, int>();
            _defaultColumns.Add("Toggle", 1);
            _defaultColumns.Add("OEM", 3);
            _defaultColumns.Add("Name", 3);
            _defaultColumns.Add("Cart", 1);
            _defaultColumns.Add("Price", 3);
            _defaultColumns.Add("Note", 3);
            _defaultColumns.Add("Tooltip", 1);
        }

        #region properties

        public bool FilterByGroup
        {
            get { return _filterByGroup; }
            set { _filterByGroup = value; }
        }

        public DetailInfo[] Details
        {
            get {
                return _details;
            }
            set {
                _details = value;
            }
        }

        public string OpennedImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.openned.gif"); }
        }

        public string FullReplacementImage
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.replacement.gif"); }
        }

        public string ForwardReplacementImage
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.replacement-forward.gif"); }
        }

        public string BackwardReplacementImage
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.replacement-backward.gif"); }
        }

        public string RowClass
        {
            get
            {
                return _rowClass;
            }
            private set
            {
                _rowClass = value;
            }
        }

        public Dictionary<string, int> Columns
        {
            get
            {
                if (_columns == null || _columns.Count == 0)
                {
                    _columns = _defaultColumns;
                }
                return _columns;
            }
            set { _columns = value; }
        }

        private static int TableNo
        {
            get { return _tableNo; }
            set { _tableNo = value; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.tooltip.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.details.detailslist.js");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Details == null || Details.Length == 0)
            {
                WriteEmptySet(writer);
                return;
            }

            WriteJavaScript(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_table");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "96%");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Concat("g_DetailTable", ++TableNo));
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            WriteHeader(writer);
            RowClass = string.Empty;

            int rowNo = 0;
            if(FilterByGroup)
            {
                List<DetailInfo> searchedDetails = new List<DetailInfo>();
                List<DetailInfo> additionalDetails = new List<DetailInfo>();

                foreach (DetailInfo detail in Details)
                {
                    if(!string.IsNullOrEmpty(detail.match))
                    {
                        searchedDetails.Add(detail);
                    }
                    else
                    {
                        additionalDetails.Add(detail);
                    }
                }

                foreach (DetailInfo detail in searchedDetails)
                {
                    WriteItem(writer, detail, ++rowNo);
                }

                WriteAdditionalItemSplitter(writer);
                RowClass += "g_addgr g_addgr_collapsed ";

                foreach (DetailInfo detail in additionalDetails)
                {
                    WriteItem(writer, detail, ++rowNo);
                }
            }
            else
            {
                foreach (DetailInfo detail in Details)
                {
                    WriteItem(writer, detail, ++rowNo);
                }
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteEmptySet(HtmlTextWriter writer)
        {
            writer.Write(GetLocalizedString("nothingfound"));
        }

        protected virtual void WriteAdditionalItemSplitter(HtmlTextWriter writer)
        {
            /*'<tr>
            <td colspan="'.count($this->columns).'">
                <img class="g_additional_toggler g_addcollapsed" class="g_addcollapsed" src="'.$this->closedimage.'" width="16" height="16" onclick="g_toggleAdditional(\'g_DetailTable'.GuayaquilDetailsList::$table_no.'\', opennedimage, closedimage);">
                <a href="#" onClick="g_toggleAdditional(\'g_DetailTable'.GuayaquilDetailsList::$table_no.'\', opennedimage, closedimage); return false;"> Остальные детали узла</a>
            </td>
            </tr>';
             */
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, Columns.Count.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            //writer.AddAttribute(HtmlTextWriterAttribute.Id, "g_additional_toggler");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_additional_toggler g_addcollapsed");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, ClosedImageUrl);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "16");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, "16");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                String.Format("g_toggleAdditional('g_DetailTable{0}', opennedimage, closedimage);",
                                              TableNo));
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                String.Format(
                                    "g_toggleAdditional('g_DetailTable{0}', opennedimage, closedimage); return false;",
                                    TableNo));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(" Остальные детали узла");
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteItem(HtmlTextWriter writer, DetailInfo detail, int rowNo)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class,
                                String.Format("{0}g_collapsed g_highlight{1}{2}", RowClass,
                                              string.IsNullOrEmpty(detail.filter) ? string.Empty : " g_filter_row",
                                              string.IsNullOrEmpty(detail.flag) ? string.Empty : " g_nonstandarddetail"));
            writer.AddAttribute(HtmlTextWriterAttribute.Name,
                                !string.IsNullOrEmpty(detail.codeonimage)
                                    ? detail.codeonimage
                                    : String.Concat("d_", rowNo));
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Concat("d_", rowNo));
            writer.AddAttribute("onmouseout", "hl(this, 'out');");
            writer.AddAttribute("onmouseover", "hl(this, 'in');");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (KeyValuePair<string, int> pair in Columns)
            {
                WriteDetailCell(writer, detail, pair.Key.ToLower(), pair.Value);
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteDetailCell(HtmlTextWriter writer, DetailInfo detail, string column, int visibility)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, string.Concat("c_", column));
            if((visibility & 1) == 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none;");
            }
            if(column.Equals("tooltip"))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_rowdatahint");
            }
            else if((visibility & 2) > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_ttd");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            WriteDetailCellValue(writer, detail, column, visibility);

            writer.RenderEndTag();
        }

        protected virtual void WriteDetailCellValue(HtmlTextWriter writer, DetailInfo detail, string column, int visibility)
        {
            switch (column)
            {
                case "toggle":
                    break;
                case "pnc":
                    writer.Write(detail.codeonimage);
                    break;
                case "oem":
                    writer.Write(detail.oem);
                    break;
                case "amount":
                    writer.Write(detail.amount);
                    break;
                case "name":
                    writer.Write(string.Concat(" ",
                                               string.IsNullOrEmpty(detail.name)
                                                   ? "Наименование не указано"
                                                   : detail.name));
                    break;
                case "cart":
                    //<img class="g_addtocart" src="'.$this->cartimage.'" width="22" height="22">
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_addtocart");
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, CartImageUrl);
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "22");
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "22");
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                    break;
                case "price":
                    writer.Write(!string.IsNullOrEmpty(detail.price) ? detail.price : "-");
                    break;
                case "note":
                    if(!string.IsNullOrEmpty(detail.note))
                    {
                        writer.Write(detail.note.Replace("\n", "<br>"));
                    }
                    break;
                case "tooltip":
                    //<img src="'.$this->detailinfoimage.'" width="22" height="22">
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, DetailInfoImageUrl);
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "22");
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "22");
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                    break;
                case "flag":
                    if (!string.IsNullOrEmpty(detail.flag))
                    {
                        int bits = Convert.ToInt32(detail.flag);
                        if ((bits & 1) > 0)
                        {
                            writer.Write("Нестандартная деталь");
                        }
                    }
                    break;
                case "availability":
                    break;
                default:
                    string value = GetDetailProperyValue(detail, column);
                    if(!string.IsNullOrEmpty(value))
                    {
                        writer.Write(value.Replace("\n", "<br>"));
                    }
                    break;
            }
        }

        protected virtual void WriteJavaScript(HtmlTextWriter writer)
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

        protected virtual void WriteHeader(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (KeyValuePair<string, int> pair in Columns)
            {
                WriteHeaderCell(writer, pair.Key.ToLower(), pair.Value);
            }
            writer.RenderEndTag();
        }

        protected virtual void WriteHeaderCell(HtmlTextWriter writer, string column, int visibility)
        {
            //<th id="c_'.$column.'"'.(($visibility & 1) == 0?' style="display:none;"':'').'>'.$this->DrawHeaderCellValue($column, $visibility).'</th>
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("c_{0}", column));
            if((visibility & 1) == 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none;");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            WriteHeaderCellValue(writer, column, visibility);
            writer.RenderEndTag();
        }

        protected virtual void WriteHeaderCellValue(HtmlTextWriter writer, string column, int visibility)
        {
            switch (column)
            {
                case "toggle":
                case "cart":
                case "tooltip":
                    writer.Write(HtmlTextWriter.SpaceChar.ToString());
                    break;
                case "pnc":
                    writer.Write(GetLocalizedString("ColumnDetailCodeOnImage"));
                    break;
                case "oem":
                    writer.Write(GetLocalizedString("ColumnDetailOEM"));
                    break;
                case "amount":
                    writer.Write(GetLocalizedString("ColumnDetailAmount"));
                    break;
                case "name":
                    writer.Write(GetLocalizedString("ColumnDetailName"));
                    break;
                case "price":
                    writer.Write(GetLocalizedString("ColumnDetailPrice"));
                    break;
                case "note":
                    writer.Write(GetLocalizedString("ColumnDetailNote"));
                    break;
                case "availability":
                    writer.Write(GetLocalizedString("WhereToBuy"));
                    break;
                default:
                    writer.Write(GetLocalizedString(String.Format("ColumnDetail{0}", column)));
                    break;
            }
        }
        
        private static string GetDetailProperyValue(DetailInfo detail, string name)
        {
            PropertyInfo propertyInfo = detail.GetType().GetProperty(name);
            if(propertyInfo != null)
            {
                object value = propertyInfo.GetValue(detail, null);
                if (value != null)
                    return value.ToString();
            }
            return string.Empty;
        }
    }
}
