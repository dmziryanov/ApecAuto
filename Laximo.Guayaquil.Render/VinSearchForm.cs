using System;
using System.Web.UI;

namespace Laximo.Guayaquil.Render
{
    public class VinSearchForm : GuayaquilTemplate
    {
        private string _previousVin;

        public const string VinRegularExpression = @"\^[A-z0-9]{12}[0-9]{5}\$";

        public VinSearchForm(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public string PreviousVin
        {
            get { return _previousVin; }
            set { _previousVin = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            WriteVinCheckScript(writer);

            WriteVinExample(writer);

            WriteVinForm(writer);
        }

        protected virtual void WriteVinCheckScript(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.WriteLine("function checkVinValue(value, submit_btn) {");
            writer.WriteLine(String.Format("    var expr = new RegExp('{0}', 'i');", VinRegularExpression));
            writer.WriteLine("    if (expr.test(value)) {");
            writer.WriteLine("        jQuery(submit_btn).attr('disabled', '1');");
            writer.WriteLine("        jQuery('#VINInput').attr('class','g_input');");
            writer.WriteLine(String.Format(@"        window.location.href = '{0}'.replace('\$vin\$', value); ",
                                           FormatLink("vehicles", null)));
            writer.WriteLine("    }");
            writer.WriteLine("    else {");
            writer.WriteLine("        jQuery('#VINInput').attr('class','g_input_error');");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.RenderEndTag();
        }

        protected virtual void WriteVinExample(HtmlTextWriter writer)
        {
            writer.Write(GetLocalizedString("InputVIN", GetVinExample()));
            writer.WriteBreak();
        }

        private string GetVinExample()
        {
            if (Catalog == null || Catalog.Info == null || Catalog.Info.row == null ||
                string.IsNullOrEmpty(Catalog.Info.row.code))
            {
                return "WAUBH54B11N111054";
            }

            if (!string.IsNullOrEmpty(Catalog.Info.row.vinexample))
            {
                return Catalog.Info.row.vinexample;
            }

            string result = string.Empty;

            string twoSymbolsCode = Catalog.Info.row.code.Substring(0, 2);
            switch (twoSymbolsCode)
            {
                case "he":
                    result = "1HGCB7543LA000002";
                    break;
                case "ki":
                    result = "KNEBA24428T522301";
                    break;
                case "hy":
                    result = "KMHVD34N8VU263043";
                    break;
                case "te":
                    result = "SB164SBK10E032155";
                    break;
                case "tg":
                    result = "JT111PJA508001249";
                    break;
                case "tu":
                    result = "1NXBR32E53Z094412";
                    break;
                case "ne":
                    result = "3N1BC13E07L364030";
                    break;
                case "ie":
                    result = "5N3AA08C04N811146";
                    break;
                case "mg":
                    result = "JM7GG32F141127052";
                    break;
                case "me":
                    result = "JM7BK326081421458";
                    break;
                case "AU":
                    result = "WAUBH54B11N111054";
                    break;
                case "VW":
                    result = "WVWZZZ7MZ7V006700";
                    break;
                case "SK":
                    result = "TMBCA21Z962131685";
                    break;
                case "SE":
                    result = "VSSZZZ9KZ1R003158";
                    break;
                case "MB":
                    result = "WDBUF65J14A605034";
                    break;
            }
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            string sixSymbolsCode = Catalog.Info.row.code.Substring(0, 6);
            switch (sixSymbolsCode)
            {
                case "MMCM60":
                    result = "XMCLNDA1A3F016543";
                    break;
                case "MMCM50":
                    result = "4A3AC54L3YE163458";
                    break;
                case "MMCM80":
                    result = "MMCJNKB409D008733";
                    break;
            }

            return result;
        }

        protected virtual void WriteVinForm(HtmlTextWriter writer)
        {
            //<div id="VINInput" class="g_input">
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "VINInput");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_input");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<input name="vin" type="text" id="vin" size="17" style="width:200px;" value="'.$prevvin.'"/></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "vin");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "vin");
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "17");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:200px;");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, PreviousVin);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();

            //<input type="button" onclick="checkVinValue(this.form.vin.value, this)" name="vinSubmit" value="'.$this->GetLocalizedString('Search').'" id="vinSubmit" /></form>
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "checkVinValue(this.form.vin.value, this)");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "vinSubmit");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "vinSubmit");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, GetLocalizedString("Search"));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }
}
