using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class QuickGroupsList : GuayaquilTemplate
    {
        private ListQuickGroups _groupses;
        private bool _isNeedWriteToolbar = true;
        private const int RootQuickGroupId = 10001;

        public QuickGroupsList(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public ListQuickGroups Groupses
        {
            get { return _groupses; }
            set { _groupses = value; }
        }

        public bool IsNeedWriteToolbar
        {
            get { return _isNeedWriteToolbar; }
            set { _isNeedWriteToolbar = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AppendCSS("Laximo.Guayaquil.Render.Assets.css.qgroups.groups.css");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.qgroups.groups.js");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            Toolbar toolbar = new Toolbar(GetLocalizedString("VehicleLink"), FormatLink("vehicle", null));
            if (IsNeedWriteToolbar)
                toolbar.RenderControl(writer);

            WriteSearchPanel(writer);

            //<div id="qgTree" onclick="tree_toggle(arguments[0])"><ul class="qgContainer">
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "qgTree");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "tree_toggle(arguments[0])");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "qgContainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            t_row[] qgroups = Groupses.row;
            if (qgroups != null && qgroups.Length == 1)
            {
                //skip root group if quickgroupid == 10001 ("Ћегковые автомобили")
                t_row rootGroup = qgroups[0];
                if(rootGroup != null && rootGroup.quickgroupid == RootQuickGroupId)
                {
                    qgroups = rootGroup.row;
                }
            }

            if (qgroups != null)
            {
                for (int i = 0; i < qgroups.Length; i++)
                {
                    WriteTreeNode(writer, qgroups[i], 1, (i + 1) == qgroups.Length);
                }
            }

            //</ul></div>
            writer.RenderEndTag();
            writer.RenderEndTag();

            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteTreeNode(HtmlTextWriter writer, t_row row, int level, bool isLast)
        {
            //<li class="qgNode '.($childrens == 0 ? 'qgExpandLeaf' : 'qgExpandClosed').($last ? ' qgIsLast' : '').'">
            //<div class="qgExpand"></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Class,
                                String.Format("qgNode {0}{1}",
                                              (row.row != null && row.row.Length > 0) ? "qgExpandClosed" : "qgExpandLeaf",
                                              isLast ? " qgIsLast" : null));
            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "qgExpand");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            WriteItem(writer, row, level);

            if (row.row != null && row.row.Length > 0)
            {
                //<ul class="qgContainer">'.$subhtml.'</ul>
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "qgContainer");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                for (int i = 0; i < row.row.Length; i++)
                {
                    WriteTreeNode(writer, row.row[i], level + 1, (i + 1) == row.row.Length);
                }

                writer.RenderEndTag();
            }

            //</li>
            writer.RenderEndTag();
        }

        protected virtual void WriteItem(HtmlTextWriter writer, t_row row, int level)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "qgContent");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if(row.link)
            {
                string link = FormatLink("quickgroup", row);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(row.name);
                writer.RenderEndTag();
            }
            else
            {
                writer.Write(row.name);
            }
            writer.RenderEndTag();
        }

        protected virtual void WriteSearchPanel(HtmlTextWriter writer)
        {
            //<input type="text" maxlength="20" size="50" id="qgsearchinput" value="¬ведите им€ группы запчастей"
            // title="¬ведите им€ группы запчастей" onfocus=";if(this.value==\'¬ведите им€ группы запчастей\')this.value=\'\';"
            // onblur="if(this.value.replace(\' \', \'\')==\'\')this.value=\'¬ведите им€ группы запчастей\';" onkeyup="QuickGroups.Search(this.value);">
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "20");
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "50");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "qgsearchinput");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "¬ведите им€ группы запчастей");
            writer.AddAttribute(HtmlTextWriterAttribute.Title, "¬ведите им€ группы запчастей");
            writer.AddAttribute("onfocus", ";if(this.value=='¬ведите им€ группы запчастей')this.value='';");
            writer.AddAttribute("onblur", "if(this.value.replace(' ', '')=='')this.value='¬ведите им€ группы запчастей';");
            writer.AddAttribute("onkeyup", "QuickGroups.Search(this.value);");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            //<input type="button" value="—бросить" onclick="jQuery(\'#qgsearchinput\').attr(\'value\', \'¬ведите им€ группы запчастей\');
            //QuickGroups.Search(\'\');">
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "—бросить");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                "jQuery('#qgsearchinput').attr('value', '¬ведите им€ группы запчастей'); QuickGroups.Search('');");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            //<div id="qgFilteredGroups"></div>
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "qgFilteredGroups");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }
    }
}
