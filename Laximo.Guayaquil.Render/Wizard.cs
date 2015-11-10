using System;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class Wizard : GuayaquilTemplate
    {
        private GetWizard _wizardInfo;

        public Wizard(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public GetWizard WizardInfo
        {
            get { return _wizardInfo; }
            set { _wizardInfo = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //<div class="guayaquil_WizardSelectionBox">
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_WizardSelectionBox");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write("function openWizard(valueid) { \n");
            writer.Write(String.Format("window.location = \'{0}\'.replace(\'\\$valueid\\$\', valueid); \n",
                                       FormatLink("wizard", WizardInfo)));
            writer.Write("} \n");
            writer.RenderEndTag();

            //<div name="findByParameterIdentifocation" id="findByParameterIdentifocation">
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "findByParameterIdentifocation");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "findByParameterIdentifocation");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //<table border="0" width="100%">
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            WriteHeader(writer);

            foreach (GetWizardRow wizardRow in WizardInfo.row)
            {
                WriteWizardRow(writer, wizardRow);
            }

            //</table></form>
            writer.RenderEndTag();
            writer.RenderEndTag();

            if(WizardInfo.row[0].allowlistvehicles)
            {
                WriteVehicleListLink(writer);
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteHeader(HtmlTextWriter writer)
        {
        }

        protected virtual void WriteWizardRow(HtmlTextWriter writer, GetWizardRow wizardRow)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "60%");
            if(!wizardRow.automatic)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_SelectedRow");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(GetWizardRowName(wizardRow));
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if(wizardRow.determined)
            {
                WriteDisabledSelector(writer, wizardRow);
            }
            else
            {
                WriteSelector(writer, wizardRow);
            }
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(GetWizardRowDescription(wizardRow));

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected virtual string GetWizardRowName(GetWizardRow wizardRow)
        {
            return wizardRow.name;
        }

        protected virtual string GetWizardRowDescription(GetWizardRow wizardRow)
        {
            return wizardRow.description;
        }

        protected virtual void WriteSelector(HtmlTextWriter writer, GetWizardRow wizardRow)
        {
            //<select style="width:250px" name="Select'.$condition['type'].'" onChange="openWizard(this.options[this.selectedIndex].value); return false;">
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:250px");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, String.Format("Select{0}", wizardRow.type));
            writer.AddAttribute(HtmlTextWriterAttribute.Onchange, "openWizard(this.options[this.selectedIndex].value); return false;");
            writer.RenderBeginTag(HtmlTextWriterTag.Select);

            //<option value="null">&nbsp;</option>
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "null");
            writer.RenderBeginTag(HtmlTextWriterTag.Option);
            writer.Write(HtmlTextWriter.SpaceChar.ToString());
            writer.RenderEndTag();

            foreach (GetWizardRowRow optionRow in wizardRow.options)
            {
                WriteSelectorOption(writer, optionRow);
            }

            //</select>
            writer.RenderEndTag();
        }

        protected virtual void WriteSelectorOption(HtmlTextWriter writer, GetWizardRowRow optionRow)
        {
            //<option value="'.$row['key'].'">'.$row['value'].'</option>
            writer.AddAttribute(HtmlTextWriterAttribute.Value, optionRow.key.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Option);
            writer.Write(optionRow.value);
            writer.RenderEndTag();
        }

        protected virtual void WriteDisabledSelector(HtmlTextWriter writer, GetWizardRow wizardRow)
        {
            //<select disabled style="width:250px" name="Select'.$condition['type'].'">
            writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:250px");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, String.Format("Select{0}", wizardRow.type));
            writer.RenderBeginTag(HtmlTextWriterTag.Select);

            WriteDisabledSelectorOption(writer, wizardRow);

            //</select>
            writer.RenderEndTag();
        }

        protected virtual void WriteDisabledSelectorOption(HtmlTextWriter writer, GetWizardRow wizardRow)
        {
            //<option disabled selected value="null">'.$condition['value'].'</option>
            writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            writer.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "null");
            writer.RenderBeginTag(HtmlTextWriterTag.Option);
            writer.Write(wizardRow.value);
            writer.RenderEndTag();
        }

        protected virtual void WriteVehicleListLink(HtmlTextWriter writer)
        {
            //<br>
            writer.WriteBreak();

            //<a class="gWizardVehicleLink" href="'.$this->FormatLink('vehicles', $wizard, $catalog).'">
            //$this->GetLocalizedString('List vehicles')
            //</a>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "gWizardVehicleLink");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, FormatLink("vehicles", WizardInfo));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(GetLocalizedString("ListVehicles"));
            writer.RenderEndTag();
        }
    }
}
