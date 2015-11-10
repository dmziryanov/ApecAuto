using System;
using System.Collections.Generic;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class CategoriesList : GuayaquilTemplate
    {
        private ListCategories _categories;
        private bool _isNeedWriteToolbar = true;

        public CategoriesList(IGuayaquilExtender extender, ICatalog catalog)
            : base(extender, catalog)
        {
        }

        public ListCategories Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public bool IsNeedWriteToolbar
        {
            get { return _isNeedWriteToolbar; }
            set { _isNeedWriteToolbar = value; }
        }

        private Dictionary<int, CategoryRow> MakeHierarchy()
        {
            Dictionary<int, CategoryRow> categories = new Dictionary<int, CategoryRow>();
            foreach (ListCategoriesRow row in Categories.row)
            {
                if(row.parentid > 0)
                {
                    CategoryRow child = new CategoryRow();
                    child.Data = row;

                    if(categories.ContainsKey(row.parentid))
                    {
                        categories[row.parentid].Children.Add(child);
                    }
                    else
                    {
                        CategoryRow parent = new CategoryRow();
                        parent.Children.Add(child);
                        categories.Add(row.parentid, parent);
                    }
                }
                else
                {
                    if(categories.ContainsKey(row.categoryid))
                    {
                        categories[row.categoryid].Data = row;
                    }
                    else
                    {
                        CategoryRow parent = new CategoryRow();
                        parent.Data = row;
                        categories.Add(row.categoryid, parent);
                    }
                }
            }
            return categories;
        }

        protected class CategoryRow
        {
            public readonly List<CategoryRow> Children = new List<CategoryRow>();
            public ListCategoriesRow Data;
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            Dictionary<int, CategoryRow> categories = MakeHierarchy();

            Toolbar toolbar = (Catalog.Info != null && Catalog.Info.row.supportquickgroups)
                      ? new Toolbar(GetLocalizedString("QuickGroupsLink"),
                                    FormatLink("quickgroup", null))
                      : new Toolbar();
            if (IsNeedWriteToolbar)
                toolbar.RenderControl(writer);
            
            WriteBox(writer, categories);
        }

        protected virtual void WriteBox(HtmlTextWriter writer, Dictionary<int, CategoryRow> categories)
        {
            //<div class="guayaquil_categoryfloatbox">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_categoryfloatbox");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //<b class="xtop"><b class="xb4"></b><b class="xb3"></b><b class="xb2"></b><b class="xb1"></b></b>'
            GuayaquilHelper.WriteBoxBorder(writer, "xtop");

            //<div class="xboxcontent"><p class="block_header">'.$this->GetLocalizedString('Categories').'</p>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xboxcontent");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "block_header");
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write(GetLocalizedString("Categories"));
            writer.RenderEndTag();

            WriteItems(writer, categories);
            //</div>'
            writer.RenderEndTag();

            //<b class="xbottom"><b class="xb4"></b><b class="xb3"></b><b class="xb2"></b><b class="xb1"></b></b>'
            GuayaquilHelper.WriteBoxBorder(writer, "xbottom");
            //</div>
            writer.RenderEndTag();
            //<div class="guayaquil_categoryfloatbox" style="padding-top:5px;">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_categoryfloatbox");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-top:5px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<b class="xtop"><b class="xb4"></b><b class="xb3"></b><b class="xb2"></b><b class="xb1"></b></b>'
            GuayaquilHelper.WriteBoxBorder(writer, "xtop");
            //<div class="xboxcontent">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xboxcontent");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteSearchForm(writer);
            //</div>
            writer.RenderEndTag();
            //<b class="xbottom"><b class="xb4"></b><b class="xb3"></b><b class="xb2"></b><b class="xb1"></b></b>'
            GuayaquilHelper.WriteBoxBorder(writer, "xbottom");
            //</div>
            writer.RenderEndTag();
        }

        protected virtual void WriteSearchForm(HtmlTextWriter writer)
        {
            //<center><div>
            writer.RenderBeginTag(HtmlTextWriterTag.Center);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //<table border="0"><tr>
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //<td valign="center" align="right">'.$this->GetLocalizedString("Search by code").'</td>
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(GetLocalizedString("SearchByCode"));
            writer.RenderEndTag();
            //<td valign="center"><input name="code" size="7"></td>
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddAttribute(HtmlTextWriterAttribute.Name, "code");
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "7");
            writer.AddAttribute("onkeypress", "if(event.keyCode == 13){glow(this.value);return false;}");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            //</tr></table></div></center>
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual void WriteItems(HtmlTextWriter writer, Dictionary<int, CategoryRow> categories)
        {
            foreach (KeyValuePair<int, CategoryRow> pair in categories)
            {
                string link = FormatLink("unit", pair.Value.Data);
                WriteItem(writer, pair.Value, link, 0);
            }
        }

        protected virtual void WriteItem(HtmlTextWriter writer, CategoryRow row, string link, int level)
        {
            if (Catalog.CategoryId == row.Data.categoryid || (Catalog.CategoryId == -1 && row.Data.categoryid == 1))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_categoryitem_selected");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("margin-left:{0}px", level*20));
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location='{0}'", link));
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                WriteItemValue(writer, row.Data);
                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_categoryitem");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("margin-left:{0}px", level * 20));
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location='{0}'", link));
                writer.AddAttribute("onmouseout", "this.className='guayaquil_categoryitem';");
                writer.AddAttribute("onmouseover", "this.className='guayaquil_categoryitem_selected';");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                WriteItemValue(writer, row.Data);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }

            foreach (CategoryRow child in row.Children)
            {
                string childLink = FormatLink("unit", child.Data);
                WriteItem(writer, child, childLink, level + 1);
            }
        }

        protected virtual void WriteItemValue(HtmlTextWriter writer, ListCategoriesRow row)
        {
            writer.Write(row.name);
        }
    }
}
