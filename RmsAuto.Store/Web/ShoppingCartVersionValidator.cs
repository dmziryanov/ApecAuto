using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using RmsAuto.Store.Cms.Routing;


namespace RmsAuto.Store.Web
{
    public class ShoppingCartVersionValidator : Control, IValidator 
    {
        public enum TrackMode
        {
            ChangeCartContent = 0, Checkout = 1
        }

        public TrackMode Mode
        {
            get { return (TrackMode)Convert.ToInt32(ViewState["__mode"]); }
            set { ViewState["__mode"] = (int)value; }
        }

        public string ShoppingCartUrl
        {
            get { return (string)ViewState["__cartUrl"]; }
            set { ViewState["__cartUrl"] = value; }
        }

        private string _CartVersion
        {
            get { return (string)ViewState["__cartVersion"]; }
            set { ViewState["__cartVersion"] = value; }
        }

        public void PinVersion()
        {
            Page.Session["__cartVersion"] = _CartVersion;
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.Validators.Add(this);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (Mode == TrackMode.Checkout)
                {
                    _CartVersion = (string)Page.Session["__cartVersion"];
                    Validate();
                }
                else
                {
                    _CartVersion = SiteContext.Current.CurrentClient.Cart.GetVersion();
                    IsValid = true;
                }
            }
            else
            {
                if (Mode == TrackMode.ChangeCartContent)
                    SiteContext.Current.CurrentClient.Cart.ContentChanged += new EventHandler<EventArgs>(Cart_ContentChanged);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            if (Page != null)
                Page.Validators.Remove(this);

			if( Mode == TrackMode.ChangeCartContent && !SiteContext.Current.CurrentClient.IsGuest)
                SiteContext.Current.CurrentClient.Cart.ContentChanged -= new EventHandler<EventArgs>(Cart_ContentChanged);
            base.OnUnload(e);
        }

        void Cart_ContentChanged(object sender, EventArgs e)
        {
            _CartVersion = SiteContext.Current.CurrentClient.Cart.GetVersion();
        }

        #region IValidator Members

        public string ErrorMessage
        {
            get { return Convert.ToString(ViewState["__errorMessage"]); }
            set { ViewState["__errorMessage"] = value; }
        }

        public bool IsValid
        {
            get;
            set;
        }

        private bool _validateCalled;
        
        public void Validate()
        {
            _validateCalled = true;
            IsValid = _CartVersion == SiteContext.Current.CurrentClient.Cart.GetVersion();
        }

        #endregion
                
 
        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (_validateCalled && !IsValid)
            {
                Controls.Add(new Label() { 
                    Text = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : "CartVersion validation failed",
                    ForeColor = Color.Red 
                });
                Controls.Add(new HtmlAnchor()
                {
                    InnerText = Mode == TrackMode.ChangeCartContent ? "update" : "cart",
                    HRef = !string.IsNullOrEmpty(ShoppingCartUrl) ? 
                        ShoppingCartUrl : UrlManager.GetCartUrl()
                });
            }
            ChildControlsCreated = true;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            this.RenderChildren(writer);
        }
    }
}
