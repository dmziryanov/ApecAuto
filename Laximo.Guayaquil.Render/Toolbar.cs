using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Laximo.Guayaquil.Render
{
    public class Toolbar : WebControl
    {
        //if need store buttons - make '_buttons' is static
        private readonly List<ToolBarButton> _buttons = new List<ToolBarButton>();
        private readonly ToolBarButton _currentButton;
        
        public Toolbar(){}

        public Toolbar(string title, string url)
        {
            _currentButton = new ToolBarButton(title, url);

            if(!_buttons.Contains(_currentButton))
                _buttons.Add(_currentButton);
        }

        public Toolbar(string title, string url, string onclick, string alt, string img, string id):this(title, url)
        {
            _currentButton.Onclick = onclick;
            _currentButton.Alt = alt;
            _currentButton.Img = img;
            _currentButton.Id = id;
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (_buttons.Count == 0) return;
            
            GuayaquilHelper.WriteBoxBorder(writer, "xtop");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xboxcontent");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "guayaquil_toolbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (ToolBarButton button in _buttons)
            {
                WriteButton(writer, button);
            }

            writer.RenderEndTag();
            GuayaquilHelper.WriteBoxBorder(writer, "xbottom");
            writer.WriteBreak();
        }

        private static void WriteButton(HtmlTextWriter writer, ToolBarButton button)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "g_ToolbarButton");
            if(!string.IsNullOrEmpty(button.Id))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, button.Id);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, button.Url);
            if(!string.IsNullOrEmpty(button.Onclick))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, button.Onclick);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            if(!string.IsNullOrEmpty(button.Img))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, button.Img);
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, button.Alt);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            writer.Write(button.Title);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private class ToolBarButton
        {
            private string _title;
            private string _url;
            private string _onclick;
            private string _alt;
            private string _img;
            private string _id;

            public ToolBarButton(string title, string url)
            {
                _title = title;
                _url = url;
            }

            public ToolBarButton(string title, string url, string onclick, string alt, string img, string id): this(title, url)
            {
                _onclick = onclick;
                _alt = alt;
                _img = img;
                _id = id;
            }

            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }

            public string Url
            {
                get { return _url; }
                set { _url = value; }
            }

            public string Onclick
            {
                get { return _onclick; }
                set { _onclick = value; }
            }

            public string Alt
            {
                get { return _alt; }
                set { _alt = value; }
            }

            public string Img
            {
                get { return _img; }
                set { _img = value; }
            }

            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == typeof (ToolBarButton) && Equals((ToolBarButton) obj);
            }

            private bool Equals(ToolBarButton other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(other._title, _title) && Equals(other._url, _url);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_title != null ? _title.GetHashCode() : 0)*397) ^ (_url != null ? _url.GetHashCode() : 0);
                }
            }
        }
    }
}
