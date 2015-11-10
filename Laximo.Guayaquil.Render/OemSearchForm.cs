using System;
using System.Web.UI;

namespace Laximo.Guayaquil.Render
{
    public class OemSearchForm : GuayaquilTemplate
    {
        private string _previousOem;

        public const string OemRegularExpression = @"\^[A-z0-9\*-]{3,}\$";

        public OemSearchForm(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public string PreviousOEM
        {
            get { return _previousOem; }
            set { _previousOem = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            WriteOemCheckScript(writer);

            //<table border=0>
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            //<tr><td>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //</td><td>
            writer.Write(GetLocalizedString("SearchByOEM"));
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            WriteOEMForm(writer);
            //</td></tr></table>
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteOemCheckScript(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.WriteLine("function checkOemValue(value, submit_btn) {");
            writer.WriteLine(String.Format("    var expr = new RegExp('{0}', 'i');", OemRegularExpression));
            writer.WriteLine("    if (expr.test(value)) {");
            writer.WriteLine("        jQuery(submit_btn).attr('disabled', '1');");
            writer.WriteLine("        jQuery('#OemInput').attr('class','g_input');");
            writer.Write(String.Format(@"        window.location.href = '{0}'.replace('\$oem\$', value);",
                                       FormatLink("details", null)));
            writer.WriteLine("    }");
            writer.WriteLine("    else {");
            writer.WriteLine("        jQuery('#OemInput').attr('class','g_input_error');");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.RenderEndTag();
        }

        protected virtual void WriteOEMForm(HtmlTextWriter writer)
        {
            //<div id="OemInput" class="g_input">
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "OemInput");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_input");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<input name="Oem" type="text" id="Oem" size="17" style="width:200px;" value="'.$prevOem.'"/></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "Oem");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "Oem");
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "17");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:200px;");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, PreviousOEM);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();

            //<input type="button" onclick="checkVinValue(this.form.vin.value, this)" name="OemSubmit" value="'.$this->GetLocalizedString('Search').'" id="OemSubmit" /></form>
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "checkOemValue(this.form.vin.value, this)");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "OemSubmit");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "OemSubmit");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, GetLocalizedString("Search"));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }
}
