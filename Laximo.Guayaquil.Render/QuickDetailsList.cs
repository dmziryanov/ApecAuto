using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class QuickDetailsList : GuayaquilTemplate
    {
        private ListQuickDetailCategoryUnit _currentUnit;
        private ListQuickDetail _quickDetailList;
        private int _imageSize;
        private DetailsList _detailsList;
        private bool _isNeedWriteToolbar = true;

        private const int DefaultImageSize = 175;

        public QuickDetailsList(IGuayaquilExtender extender, ICatalog catalog)
            : base(extender, catalog)
        {
        }

        #region properties

        public ListQuickDetailCategoryUnit CurrentUnit
        {
            get {
                return _currentUnit;
            }
            private set {
                _currentUnit = value;
            }
        }

        public ListQuickDetail QuickDetailList
        {
            get { return _quickDetailList; }
            set { _quickDetailList = value; }
        }

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

        public DetailsList DetailsList
        {
            get { return _detailsList; }
            set { _detailsList = value; }
        }

        public bool IsNeedWriteToolbar
        {
            get { return _isNeedWriteToolbar; }
            set { _isNeedWriteToolbar = value; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.colorbox.js");
            AppendCSS("Laximo.Guayaquil.Render.Assets.css.common.colorbox.css");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            WriteJavaScript(writer);

            Toolbar toolbar = new Toolbar(GetLocalizedString("VehicleLink"), FormatLink("vehicle", null));
            if(IsNeedWriteToolbar)
                toolbar.RenderControl(writer);

            if(QuickDetailList.Category == null || QuickDetailList.Category.Length == 0)
            {
                WriteEmptySet(writer);
            }
            else
            {
                foreach (ListQuickDetailCategory category in QuickDetailList.Category)
                {
                    WriteCategory(writer, category);
                }
            }

            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteCategory(HtmlTextWriter writer, ListQuickDetailCategory category)
        {
            Catalog.CategoryId = category.categoryid;
            
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "gdCategory");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteCategoryContent(writer, category);

            foreach (ListQuickDetailCategoryUnit unit in category.Unit)
            {
                WriteUnit(writer, unit);
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteUnit(HtmlTextWriter writer, ListQuickDetailCategoryUnit unit)
        {
            CurrentUnit = unit;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "gdUnit");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "gdImageCol");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, (ImageSize + 4).ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            
            WriteUnitImage(writer, unit);
            
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "gdDetailCol");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            
            WriteUnitDetails(writer, unit);
            
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteUnitDetails(HtmlTextWriter writer, ListQuickDetailCategoryUnit unit)
        {
            //$this->detaillistrenderer->Draw($this->catalog, $unit->Detail);
            DetailsList.Details = unit.Detail;
            RenderChildren(writer);
        }

        protected virtual void WriteUnitImage(HtmlTextWriter writer, ListQuickDetailCategoryUnit unit)
        {
            //<div class="guayaquil_unit_icons">
            //<div class="guayaquil_zoom" full="'.str_replace('%size%', 'source', $unit['imageurl']).'" title="'.$unit['code'].': '.$unit['name'].'">
            //<img src="'.$this->zoom_image.'"></div></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_unit_icons");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_zoom");
            writer.AddAttribute("full",
                                string.IsNullOrEmpty(unit.imageurl)
                                    ? string.Empty
                                    : unit.imageurl.Replace("%size%", "source"));
            writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format("{0}: {1}", unit.code, unit.name));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Src, ZoomImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            //<div class="gdImage'.(!strlen($img) ? ' gdNoImage' : '').'" style="width:'.(int)$this->size.'px; height:'.(int)$this->size.'px;">
            writer.AddAttribute(HtmlTextWriterAttribute.Class,
                                String.Format("gdImage{0}", string.IsNullOrEmpty(unit.imageurl) ? " gdNoImage" : null));
            writer.AddAttribute(HtmlTextWriterAttribute.Style, String.Format("width:{0}px; height:{0}px;", ImageSize));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!string.IsNullOrEmpty(unit.imageurl))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, unit.imageurl.Replace("%size%", ImageSize.ToString()));
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();

            string link = FormatLink("unit", unit);
            //<a href="'.$link.'"><b>'.$unit['code'].':</b> '.$unit['name'].'</a>
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderBeginTag(HtmlTextWriterTag.B);
            writer.Write(String.Format("{0}: ", unit.code));
            writer.RenderEndTag();
            writer.Write(unit.name);
            writer.RenderEndTag();
        }

        protected virtual void WriteCategoryContent(HtmlTextWriter writer, ListQuickDetailCategory category)
        {
            string link = FormatLink("category", category);
            writer.RenderBeginTag(HtmlTextWriterTag.H3);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(category.name);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteEmptySet(HtmlTextWriter writer)
        {
            string link = FormatLink("vehicle", null);
            writer.Write("Ќичего не найдено, воспользуйтесь ");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("иллюстрированным каталогом");
            writer.RenderEndTag();
            writer.Write(" дл€ поиска требуемой детали");
        }

        protected virtual void WriteJavaScript(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.WriteLine("jQuery(document).ready(function($){");
            writer.WriteLine("    jQuery('.guayaquil_zoom').colorbox({'href':");
            writer.WriteLine("        function () {");
            writer.WriteLine("            var url = jQuery(this).attr('full');");
            writer.WriteLine("            return url;");
            writer.WriteLine("        },");
            writer.WriteLine("        'photo':true,");
            writer.WriteLine("        'opacity': 0.3,");
            writer.WriteLine("        'title' : function () {");
            writer.WriteLine("            var title = jQuery(this).attr('title');");
            writer.WriteLine("            return title;");
            writer.WriteLine("        },");
            writer.WriteLine("        'maxWidth' : '98%',");
            writer.WriteLine("        'maxHeight' : '98%'");
            writer.WriteLine("        }");
            writer.WriteLine("    )");
            writer.WriteLine("});");
            writer.RenderEndTag();
        }
    }
}
