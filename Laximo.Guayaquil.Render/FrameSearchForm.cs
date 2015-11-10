using System;
using System.Web.UI;

namespace Laximo.Guayaquil.Render
{
    public class FrameSearchForm : GuayaquilTemplate
    {
        private string _previousFrame;
        private string _previousFrameNo;

        public const string FrameRegularExpression = @"\^[A-z0-9]{3,7}\$";
        public const string FramNoRegularExpression = @"\^[0-9]{3,7}\$";

        public FrameSearchForm(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public string PreviousFrame
        {
            get { return _previousFrame; }
            set { _previousFrame = value; }
        }

        public string PreviousFrameNo
        {
            get { return _previousFrameNo; }
            set { _previousFrameNo = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            WriteFrameCheckScript(writer);
            WriteFrameExample(writer);
            WriteFrameForm(writer);
        }

        protected virtual void WriteFrameCheckScript(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.WriteLine("function checkFrameValue(frame, frameno, submit_btn) {");
            writer.WriteLine(String.Format("    var frexpr = new RegExp(\'{0}\', \'i\');", FrameRegularExpression));
            writer.WriteLine("    var result = true;");
            writer.WriteLine("    if (frexpr.test(frame)) {");
            writer.WriteLine("        jQuery(\'#FrameInput\').attr(\'class\',\'g_input\');");
            writer.WriteLine("    }");
            writer.WriteLine("    else {");
            writer.WriteLine("        jQuery(\'#FrameNoInput\').attr(\'class\',\'g_input_error\');");
            writer.WriteLine("        result = false;");
            writer.WriteLine("    }");
            writer.WriteLine("    if (result) {");
            writer.WriteLine("        jQuery(submit_btn).attr(\'disabled\', \'1\');");
            writer.WriteLine(
                String.Format(
                    @"        window.location.href = '{0}'.replace('\$frame\$', frame).replace('\$frameno\$', frameno);",
                    FormatLink("vehicles", null)));
            writer.WriteLine("    }");
            writer.WriteLine("    return false;  // return result;");
            writer.WriteLine("}");
            writer.RenderEndTag();
        }

        private string GetFrameExample()
        {
            string result = String.Empty;
            if (Catalog == null || Catalog.Info == null || Catalog.Info.row == null ||
                string.IsNullOrEmpty(Catalog.Info.row.code))
            {
                return "XZU423-0001026";
            }

            if (!string.IsNullOrEmpty(Catalog.Info.row.frameexample))
            {
                return Catalog.Info.row.frameexample;
            }

            string twoSymbolsCode = Catalog.Info.row.code.Substring(0, 2);
            switch (twoSymbolsCode)
            {
                case "hj":
                    result = "RN1-1000002";
                    break;
                case "tj":
                    result = "XZU423-0001026";
                    break;
                case "nj":
                    result = "FNN15-502358";
                    break;
                case "ij":
                    result = "PS13-016173";
                    break;
                case "mj":
                    result = "NB8C-400140";
                    break;
            }
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            string sixSymbolsCode = Catalog.Info.row.code.Substring(0, 6);
            if (sixSymbolsCode.EndsWith("MMCM00"))
            {
                result = "CK2A-0403374";
            }

            return result;
        }

        protected virtual void WriteFrameExample(HtmlTextWriter writer)
        {
            //$this->GetLocalizedString('InputFrame', array($this->GetFrameExample($cataloginfo))).'<br>';
            writer.Write(GetLocalizedString("InputFrame", GetFrameExample()));
            writer.WriteBreak();
        }

        protected virtual void WriteFrameForm(HtmlTextWriter writer)
        {
            //<div id="FrameInput" class="g_input">
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "FrameInput");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_input");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<input name="frame" type="text" id="frame" size="17" width="90" value="'.$prevframe.'"/></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "frame");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "frame");
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "17");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "90");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, PreviousFrame);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.Write("-");

            //<div id="FrameNoInput" class="g_input">
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "FrameNoInput");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_input");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<input name="frameNo" type="text" id="frameNo" size="17" width="120" value="'.$prevframeno.'"/></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "frameNo");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "frameNo");
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "17");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "120");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, PreviousFrameNo);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();

            //<input type="button" onclick="checkFrameValue(this.form.frame.value, this.form.frameNo.value, this)"
            // name="frameSubmit" value="'.$this->GetLocalizedString('Search').'" id="frameSubmit" /></form>
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "checkFrameValue(this.form.frame.value, this.form.frameNo.value, this)");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "frameSubmit");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "frameSubmit");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, GetLocalizedString("Search"));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }
    }
}
