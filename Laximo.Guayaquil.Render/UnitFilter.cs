using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class UnitFilter : GuayaquilTemplate
    {
        private GetFilterByUnit _filterByUnit;
        private GetUnitInfo _unit;
        private bool _isEmpty;

        public UnitFilter(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public GetFilterByUnit FilterByUnit
        {
            get { return _filterByUnit; }
            set { _filterByUnit = value; }
        }

        public GetUnitInfo Unit
        {
            get {
                return _unit;
            }
            set {
                _unit = value;
            }
        }

        protected bool IsEmpty
        {
            get
            {
                return _isEmpty;
            }
            set
            {
                _isEmpty = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            WriteJavaScript(writer);
            WriteHeader(writer);
            WriteBody(writer);
            WriteFooter(writer);

            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteJavaScript(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.WriteLine("function ProcessFilters(skip) {");
            writer.WriteLine(String.Format("    var url = '{0}';", FormatLink("vehicle", null)));
            writer.WriteLine(String.Format("    var ssd = '{0}';", Catalog.Ssd));
            writer.WriteLine("    var col = jQuery('#guayaquilFilterForm .g_filter');");
            writer.WriteLine("    var hasErrors = false;");
            writer.WriteLine("    col.each(function() {");
            writer.WriteLine("        var name = this.nodeName;");
            writer.WriteLine("        var ssdmod = null;");
            writer.WriteLine("        if (name == 'SELECT')");
            writer.WriteLine("            ssdmod = this.value;");
            writer.WriteLine("        else if (name == 'INPUT' && jQuery(this).attr('type') == 'text' && this.value.length > 0) {");
            writer.WriteLine("            var s = jQuery(this).attr('ssd');");
            writer.WriteLine("            if (s != null && s.length > 0) {");
            writer.WriteLine("                var expr = new RegExp(jQuery(this).attr('regexp'), 'i');");
            writer.WriteLine("                if ((expr.test(value))) {");
            writer.WriteLine(@"                   ssdmod = s.replace('\$', this.value);");
            writer.WriteLine("                    jQuery(this).removeClass('g_error');");
            writer.WriteLine("                }");
            writer.WriteLine("                else {");
            writer.WriteLine("                    jQuery(this).addClass('g_error');");
            writer.WriteLine("                    hasErrors = true;");
            writer.WriteLine("                }");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("        else if (name == 'INPUT' && jQuery(this).attr('type') == 'radio' && this.checked)");
            writer.WriteLine("            var ssdmod = jQuery(this).attr('ssd');");
            writer.WriteLine("        if (ssdmod != null && ssdmod.length > 0)");
            writer.WriteLine("            ssd += ssdmod;");
            writer.WriteLine("        })");
            writer.WriteLine("    if (!hasErrors)");
            writer.WriteLine(@"       window.location = url.replace('\$ssd\$', ssd);");
            writer.WriteLine("}");
            writer.RenderEndTag();
        }

        protected virtual void WriteHeader(HtmlTextWriter writer)
        {
            //<form onsubmit="ProcessFilters(false); return false;" id="guayaquilFilterForm"><table class="g_filter_table">';
            writer.Write("Выберите из списка значение условия:");
            writer.WriteBreak();
            writer.WriteBreak();
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "guayaquilFilterForm");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_table");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
        }

        protected virtual void WriteBody(HtmlTextWriter writer)
        {
            if(FilterByUnit.row.Length > 0)
            {
                foreach (FilterRow row in FilterByUnit.row)
                {
                    WriteFilter(writer, row);
                }
            }
            else
            {
                WriteEmpty(writer);
            }
        }

        protected virtual void WriteFilter(HtmlTextWriter writer, FilterRow row)
        {
            //'<tr><td valign="top">'.$this->DrawFilterName($filter).'</td><td>'.$this->DrawFilterBox($filter).'</td></tr>'
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            WriteFilterName(writer, row);

            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            WriteFilterBox(writer, row);

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteFilterName(HtmlTextWriter writer, FilterRow row)
        {
            writer.Write(row.name);
        }

        protected virtual void WriteFilterBox(HtmlTextWriter writer, FilterRow row)
        {
            if(row.type.Equals("list"))
            {
                WriteFilterBoxList(writer, row);
            }
            else
            {
                WriteFilterBoxInput(writer, row);
            }
        }

        protected virtual void WriteFilterBoxList(HtmlTextWriter writer, FilterRow row)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_box g_filter");
            //writer.RenderBeginTag(HtmlTextWriterTag.Select);

            //writer.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
            //writer.RenderBeginTag(HtmlTextWriterTag.Option);
            //writer.Write("-- Не указано --");
            //writer.RenderEndTag();

            int count = 0;
            foreach (ValuesRow valuesRow in row.values)
            {
                foreach (ValueRow valueRow in valuesRow.row)
                {
                    if(!string.IsNullOrEmpty(valueRow.Text))
                    {
                        WriteFilterBoxInputList(writer, row, count);
                        return;
                    }
                    count++;

                    writer.AddAttribute(HtmlTextWriterAttribute.Value, valueRow.ssdmodification.Replace("\"", "\"\""));
                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    writer.Write(valueRow.name);
                    writer.RenderEndTag();
                }
            }
            if(count == 0)
                WriteEmpty(writer);

            writer.RenderEndTag();
        }

        protected virtual void WriteFilterBoxInputList(HtmlTextWriter writer, FilterRow row, int id)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (ValuesRow valuesRow in row.values)
            {
                if(valuesRow.row.Length > 5)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_scroller");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }

                foreach (ValueRow valueRow in valuesRow.row)
                {
                    //<div class="g_filter_label"><label>
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_label");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderBeginTag(HtmlTextWriterTag.Label);
                    //<input type="radio" name="filter_'.$id.'" class="g_filter g_filter_radio" ssd="'.str_replace('"', '""', $value['ssdmodification']).'">
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, string.Concat("filter_", id));
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter g_filter_radio");
                    writer.AddAttribute("ssd", valueRow.ssdmodification.Replace("\"", "\"\""));
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    //<span class="g_filter_name">'.$value['name'].'</span><br>
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_name");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(valueRow.name);
                    writer.RenderEndTag();
                    writer.WriteBreak();
                    //<div class="g_filter_note">'.str_replace("\n", '<br>', (string)$value).'</div>
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter_note");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write(valueRow.Text.Replace("\n", "<br>"));
                    writer.RenderEndTag();
                    //</label></div>
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

                if (valuesRow.row.Length > 5)
                {
                    writer.RenderEndTag();
                }
            }
            writer.RenderEndTag();
        }

        protected virtual void WriteFilterBoxInput(HtmlTextWriter writer, FilterRow row)
        {
            //<input type=text class="g_filter g_filter_box" regexp="'.str_replace('"', '""', $filter['regexp']).'" ssd="'.str_replace('"', '""', $filter['ssdmodification']).'">
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_filter g_filter_box");
            writer.AddAttribute("regexp", row.regexp.Replace("\"", "\"\""));
            writer.AddAttribute("ssd", row.ssdmodification.Replace("\"", "\"\""));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        protected virtual void WriteEmpty(HtmlTextWriter writer)
        {
            IsEmpty = true;
            writer.Write(
                "Не найдено ни одного варианта условий, нажмите кнопку \"Пропустить выбор\" и перейдите на иллюстрацию");
        }

        protected virtual void WriteFooter(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "Пропустить выбор");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location='{0}'; return false;", FormatLink("skip", null)));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "Подтвердить");
            if(IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ProcessFilters(false)");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}
