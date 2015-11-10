using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public class AcctgRefBase : FieldTemplateUserControl
    {
        private const string _UIHintUsage = "UIHint usage example: [UIHint(Custom/AcctgRef\", null, \"BindingOptions\", \"<RefName>;[KeyField];[DisplayField]\")]";

        protected string _RefName
        {
            get { return (string)ViewState["refName"]; }
            set { ViewState["refName"] = value; }
        }

        protected string _KeyField
        {
            get { return (string)ViewState["keyField"] ?? "Key"; }
            set { ViewState["keyField"] = value; }
        }

        protected string _DisplayField
        {
            get { return (string)ViewState["displayField"] ?? "TextValue"; }
            set { ViewState["displayField"] = value; }
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(_RefName))
            {
                var uiHint = Column.Attributes.OfType<UIHintAttribute>().FirstOrDefault();
                if (uiHint == null)
                    throw new InvalidOperationException("RefName not specified: set RefName property or apply UIHintAttribute." + _UIHintUsage);

                var bindingOptionParam = (string)uiHint.ControlParameters["BindingOptions"];
                if (string.IsNullOrEmpty(bindingOptionParam))
                    throw new InvalidOperationException("BindingOptions not specified." + _UIHintUsage);
                ParseBindingOptionsParam(bindingOptionParam);                
            }
        }

        private void ParseBindingOptionsParam(string bindingOptionParam)
        {
            var bindingOptions = bindingOptionParam.Split(';');
            if (bindingOptions.Length == 1 || bindingOptions.Length == 3)
            {
                _RefName = bindingOptions[0];
                if (bindingOptions.Length == 3)
                {
                    _KeyField = bindingOptions[1];
                    _DisplayField = bindingOptions[2];
                }
            }
            else
                throw new InvalidOperationException("BindingOptions is incorrect." + _UIHintUsage);
        }
    }
}
