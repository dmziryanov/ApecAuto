using System;
using System.Web.UI;

namespace Laximo.Guayaquil.Render
{
    public class Unit : GuayaquilTemplate
    {
        private DetailsList _detailsList;
        private UnitImage _unitImage;
        private int _containerWidth;
        private int _containerHeight;
        private bool _isNeedWriteLegend = true;
        private bool _isNeedWriteToolbar= true;

        private const int DefaultContainerWidth = 740;
        private const int DefaultContainerHeight = 600;

        public Unit(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        #region properties

        public bool IsNeedWriteLegend
        {
            get { return _isNeedWriteLegend; }
            set { _isNeedWriteLegend = value; }
        }

        public int ContainerWidth
        {
            get
            {
                if(_containerWidth <= 0)
                {
                    _containerWidth = DefaultContainerWidth;
                }
                return _containerWidth;
            }
            set { _containerWidth = value; }
        }

        public int ContainerHeight
        {
            get
            {
                if (_containerHeight <= 0)
                {
                    _containerHeight = DefaultContainerHeight;
                }
                return _containerHeight;
            }
            set { _containerHeight = value; }
        }

        public DetailsList DetailsList
        {
            get { return _detailsList; }
            set { _detailsList = value; }
        }

        public UnitImage UnitImage
        {
            get { return _unitImage; }
            set { _unitImage = value; }
        }

        public string MouseWheelImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.mouse_wheel.png"); }
        }

        public string MouseImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.mouse.png"); }
        }

        public string LmbInfoImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.lmb.png"); }
        }

        public string MoveImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.move.png"); }
        }

        public string ArrowImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.pointer.png"); }
        }

        public bool IsNeedWriteToolbar
        {
            get { return _isNeedWriteToolbar; }
            set { _isNeedWriteToolbar = value; }
        }

        #endregion

        protected override void RenderContents(HtmlTextWriter writer)
        {
            Toolbar toolbar = (Catalog.Info != null && Catalog.Info.row.supportquickgroups)
                                  ? new Toolbar(GetLocalizedString("QuickGroupsLink"),
                                                FormatLink("quickgroup", null))
                                  : new Toolbar();
            if (IsNeedWriteToolbar)
                toolbar.RenderControl(writer);
            
            WriteContainer(writer);

            if(IsNeedWriteLegend)
            {
                WriteLegend(writer);
            }

            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteContainer(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "g_container");
            writer.AddAttribute(HtmlTextWriterAttribute.Style,
                                String.Format("vertical-align:top;height:{0}px;width:100%; overflow:hidden;",
                                              ContainerHeight + 2));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteUnitImage(writer);

            WriteDetailList(writer);

            writer.RenderEndTag();
        }

        protected virtual void WriteUnitImage(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:49%; height:100%;");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "inline_block");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            UnitImage.RenderControl(writer);

            writer.RenderEndTag();
        }

        protected virtual void WriteDetailList(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "overflow:auto; width:49%; height:100%;");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "inline_block");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "viewtable");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DetailsList.RenderControl(writer);

            writer.RenderEndTag();
        }

        protected virtual void WriteLegend(HtmlTextWriter writer)
        {
            //<table width="100%" border=0 style="margin-top:5px">
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin-top:5px");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            //<tr><th align=center colspan=4>'.$this->GetLocalizedString('UNIT_LEGEND').'</th></tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(GetLocalizedString("UNIT_LEGEND"));
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<tr><td align=center><img src="'.$this->mouse_wheel.'"></td>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, MouseWheelImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<td> - '.$this->GetLocalizedString('UNIT_LEGEND_IMAGE_RESIZING').'</td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(String.Format(" - {0}", GetLocalizedString("UNIT_LEGEND_IMAGE_RESIZING")));
            writer.RenderEndTag();
            //<td align=right><img src="'.$this->lmb.'"> <img src="'.$this->closedimage.'"></td>
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, LmbInfoImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Src, ClosedImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<td> - '.$this->GetLocalizedString('UNIT_LEGEND_SHOW_REPLACEMENT_PARTS').'</td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(String.Format(" - {0}", GetLocalizedString("UNIT_LEGEND_SHOW_REPLACEMENT_PARTS")));
            writer.RenderEndTag();
            //</tr><tr>
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //<td align=center><img src="'.$this->lmb.'"> <img src="'.$this->move.'"></td>
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, LmbInfoImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Src, MoveImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<td> - '.$this->GetLocalizedString('UNIT_LEGEND_MOUSE_SCROLL_IMAGE').'</td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(String.Format(" - {0}", GetLocalizedString("UNIT_LEGEND_MOUSE_SCROLL_IMAGE")));
            writer.RenderEndTag();
            //<td align=right><img src="'.$this->lmb.'"> <img src="'.$this->cartimage.'"></td>
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, LmbInfoImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Src, CartImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<td> - '.$this->GetLocalizedString('UNIT_LEGEND_ADD_TO_CART').'</td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(String.Format(" - {0}", GetLocalizedString("UNIT_LEGEND_ADD_TO_CART")));
            writer.RenderEndTag();
            //</tr><tr>
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //<td align=center><img src="'.$this->mouse.'"> <img src="'.$this->arrow.'"></td>
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, MouseImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Src, ArrowImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<td> - '.$this->GetLocalizedString('UNIT_LEGEND_HIGHLIGHT_PARTS').'</td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(String.Format(" - {0}", GetLocalizedString("UNIT_LEGEND_HIGHLIGHT_PARTS")));
            writer.RenderEndTag();
            //<td align=right><img src="'.$this->lmb.'"> <img src="'.$this->detailinfoimage.'"></td>
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, LmbInfoImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Src, DetailInfoImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //<td> - '.$this->GetLocalizedString('UNIT_LEGEND_SHOW_HIND').'</td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(String.Format(" - {0}", GetLocalizedString("UNIT_LEGEND_SHOW_HIND")));
            writer.RenderEndTag();
            //</tr></table>
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}
