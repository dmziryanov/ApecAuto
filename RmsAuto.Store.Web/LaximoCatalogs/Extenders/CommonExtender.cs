using System;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs.Extenders
{
    public abstract class CommonExtender : IGuayaquilExtender
    {
        private int _itemId;
        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public string GetLocalizedString(string name, GuayaquilTemplate renderer, params string[] p)
        {
            if (p != null && p.Length > 0)
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return String.Format(Resources.Laximo.ResourceManager.GetString(name), p);
                // ReSharper restore AssignNullToNotNullAttribute
            }
            return Resources.Laximo.ResourceManager.GetString(name);
        }

        public abstract string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer);
    }
}
