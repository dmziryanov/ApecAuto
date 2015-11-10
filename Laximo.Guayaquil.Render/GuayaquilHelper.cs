using System.Collections;
using System.Web.UI;

namespace Laximo.Guayaquil.Render
{
    internal static class GuayaquilHelper
    {
        private static readonly string[] _borders = new string[] { "xb1", "xb2", "xb3", "xb4" };
        
        public static void WriteGuayquilLabel(HtmlTextWriter writer)
        {
            //"<div style=\"visibility: visible; display: block; height: 20px; text-align: right;\">
            //<a href=\"http://dev.laximo.ru\" 
            //rel=\"follow\" style=\"visibility: visible; display: inline; font-size: 10px; font-weight: normal; text-decoration: none;\">guayaquil</a></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "visibility: visible; display: block; height: 20px; text-align: right;");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Href, "http://dev.laximo.ru");
            writer.AddAttribute(HtmlTextWriterAttribute.Rel, "follow");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "visibility: visible; display: inline; font-size: 10px; font-weight: normal; text-decoration: none;");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("guayaquil");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        public static void WriteBoxBorder(HtmlTextWriter writer, string classBorder)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, classBorder);
            writer.RenderBeginTag(HtmlTextWriterTag.B);

            ArrayList borders = new ArrayList(_borders);
            if(classBorder.Equals("xbottom"))
                borders.Reverse();

            foreach (string border in borders)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, border);
                writer.RenderBeginTag(HtmlTextWriterTag.B);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }
    }
}
