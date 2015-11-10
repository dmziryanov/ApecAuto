using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Laximo.Guayaquil.Render
{
    public abstract class GuayaquilTemplate : WebControl
    {
        protected readonly IGuayaquilExtender _extender;
        private readonly ICatalog _catalog;

        protected GuayaquilTemplate(IGuayaquilExtender extender, ICatalog catalog)
        {
            _extender = extender;
            _catalog = catalog;
        }

        #region common properties

        public string ClosedImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.closed.gif"); }
        }

        public string CartImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.cart.gif"); }
        }

        public string DetailInfoImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.details.images.info.gif"); }
        }
        
        public string ZoomImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.zoom.png"); }
        }

        public string NoImageUrl
        {
            get { return GetResourceUrl("Laximo.Guayaquil.Render.Assets.images.common.noimage.png"); }
        }

        #endregion

        public ICatalog Catalog
        {
            get { return _catalog; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (_extender == null)
            {
                throw new ArgumentNullException("extender");
            }

            AppendCSS("Laximo.Guayaquil.Render.Assets.css.common.guayaquil.css");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.js");
            AppendJavaScript("Laximo.Guayaquil.Render.Assets.scripts.common.jquery.migrate.js");
        }

        public string GetLocalizedString(string name, params string[] p)
        {
            return _extender == null ? name : _extender.GetLocalizedString(name, this, p);
        }

        public string GetLocalizedString(string name)
        {
            return GetLocalizedString(name, null);
        }

        public string FormatLink(string type, object dataItem)
        {
            if(_extender == null)
            {
                throw new ArgumentNullException("extender");
            }
            return _extender.FormatLink(type, dataItem, Catalog, this);
        }

        public void AppendJavaScript(string resName)
        {
            ClientScriptManager scriptManager = Page.ClientScript;
            //RegisterClientScriptResource prevent dublicate scripts
            scriptManager.RegisterClientScriptResource(typeof(GuayaquilTemplate), resName);
        }

        public void AppendCSS(string resName)
        {
            HtmlControl css = Page.Header.FindControl(resName) as HtmlControl;
            if (css == null)
            {
                //load the style sheet
                HtmlLink cssLink = new HtmlLink();
                cssLink.ID = resName;
                cssLink.Href = GetResourceUrl(resName);
                cssLink.Attributes.Add("rel", "stylesheet");
                cssLink.Attributes.Add("type", "text/css");

                // Add the HtmlLink to the Head section of the page.
                Page.Header.Controls.Add(cssLink);
            }
        }

        public string GetResourceUrl(string resName)
        {
            return Page.ClientScript.GetWebResourceUrl(typeof(GuayaquilTemplate), resName);
        }
    }
}
