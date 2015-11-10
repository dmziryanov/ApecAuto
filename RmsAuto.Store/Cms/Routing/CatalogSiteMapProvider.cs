using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using RmsAuto.Common.Caching;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Cms.Routing
{
	public class CatalogSiteMapProvider : SiteMapProvider
	{
		public CatalogSiteMapProvider Default
		{
			get { return _cache.CachedObject; }
		}

		SingleObjectCache<CatalogSiteMapProvider> _cache = new SingleObjectCache<CatalogSiteMapProvider>(
			new string[] { CatalogItemsCache.CacheKey }, null );

		SiteMapNode _root;
        Dictionary<CatalogItemMenuType, OneMenuTypeHashHelper> _menuHashes = new Dictionary<CatalogItemMenuType, OneMenuTypeHashHelper>();

        public CatalogSiteMapProvider()
		{
			_root = new SiteMapNode( this, HttpRuntime.AppDomainAppVirtualPath, HttpRuntime.AppDomainAppVirtualPath, UrlManager.CatalogItems.RootCatalogItem.CatalogItemName );
			string rootUrl = _root.Url.TrimEnd( '/' );
            foreach (CatalogItemMenuType _mit in Enum.GetValues(typeof(CatalogItemMenuType)))
            {
                FillOneMenuType(rootUrl, _mit);
            }
		}

        private void FillOneMenuType(string rootUrl, CatalogItemMenuType _mit)
        {
            var curMenuTypeHelper = new OneMenuTypeHashHelper(_mit);

            curMenuTypeHelper.HashUrl.Add(rootUrl.ToLower(), _root);
            curMenuTypeHelper.HashUrl.Add(rootUrl.ToLower() + "/", _root);
            curMenuTypeHelper.HashUrl.Add((rootUrl + "/default.aspx").ToLower(), _root);
            curMenuTypeHelper.HashParent.Add(_root, null);

            Stack<KeyValuePair<SiteMapNode, int?>> stack = new Stack<KeyValuePair<SiteMapNode, int?>>();
            stack.Push(new KeyValuePair<SiteMapNode, int?>(_root, null));
            while (stack.Count != 0)
            {
                KeyValuePair<SiteMapNode, int?> pair = stack.Pop();

                SiteMapNodeCollection childs = new SiteMapNodeCollection();
                curMenuTypeHelper.HashChilds[pair.Key] = childs;

                IEnumerable<CatalogItem> catalogItems = UrlManager.CatalogItems.GetCatalogItems(pair.Value??UrlManager.CatalogItems.RootCatalogItem.CatalogItemID, _mit);
				foreach (CatalogItem catalogItem in catalogItems)
                {
                    string url = UrlManager.GetCatalogUrl(catalogItem.CatalogItemID);

                    SiteMapNode node = new SiteMapNode(
                        this,
                        url,
                        url,
                        catalogItem.CatalogItemName, string.IsNullOrEmpty(catalogItem.CssClass) ? "" : catalogItem.CssClass /*прокидываем название css-ника через св-во узла Description*/);

                    try { curMenuTypeHelper.HashParent.Add(node, pair.Key); } catch { continue; }
                    childs.Add(node);
                    curMenuTypeHelper.HashUrl.Add(node.Url.ToLower(), node);
                    curMenuTypeHelper.CatalogItemHash.Add(node, catalogItem);
                    curMenuTypeHelper.CatalogItemRevHash.Add(catalogItem, node);

                    stack.Push(new KeyValuePair<SiteMapNode, int?>(node, catalogItem.CatalogItemID));
                }
            }

            _menuHashes.Add(_mit, curMenuTypeHelper);
        }

        public CatalogItem GetCatalogItem(SiteMapNode node, CatalogItemMenuType mType)
		{
			CatalogItem res;
			_menuHashes[mType].CatalogItemHash.TryGetValue( node, out res );
			return res;
		}

        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            return FindSiteMapNode(rawUrl, CatalogItemMenuType.CommonMenu);
        }

		public SiteMapNode FindSiteMapNode( string rawUrl, CatalogItemMenuType mType )
		{
			SiteMapNode res;
            if (!_menuHashes[mType].HashUrl.TryGetValue(rawUrl.ToLower(), out res))
			{
				if( rawUrl == HttpContext.Current.Request.RawUrl && CmsContext.Current.CatalogItem != null )
				{
					_menuHashes[ mType ].CatalogItemRevHash.TryGetValue( CmsContext.Current.CatalogItem, out res );
				}
			}
			return res;
		}

		public override SiteMapNodeCollection GetChildNodes( SiteMapNode node )
		{

            return GetChildNodes(node, CatalogItemMenuType.CommonMenu);
		}

        public SiteMapNodeCollection GetChildNodes( SiteMapNode node, CatalogItemMenuType mType )
        {
            return _menuHashes[mType].HashChilds[node];
        }

		public override SiteMapNode GetParentNode( SiteMapNode node )
		{
			return GetParentNode(node, CatalogItemMenuType.CommonMenu);
		}

        public SiteMapNode GetParentNode(SiteMapNode node, CatalogItemMenuType mType)
        {
            return _menuHashes[mType].HashParent[node];
        }

		protected override SiteMapNode GetRootNodeCore()
		{
			return _root;
		}
	}

    public class OneMenuTypeHashHelper
    {
        public Dictionary<string, SiteMapNode> HashUrl { get; protected set; }
        public Dictionary<SiteMapNode, SiteMapNodeCollection> HashChilds { get; protected set; }
        public Dictionary<SiteMapNode, SiteMapNode> HashParent { get; protected set; }
        public Dictionary<SiteMapNode, CatalogItem> CatalogItemHash { get; protected set; }
        public Dictionary<CatalogItem, SiteMapNode> CatalogItemRevHash { get; protected set; }
        public CatalogItemMenuType MenuType { get; protected set; }

        public OneMenuTypeHashHelper(CatalogItemMenuType mType)
        {
            HashUrl = new Dictionary<string, SiteMapNode>();
            HashChilds = new Dictionary<SiteMapNode, SiteMapNodeCollection>();
            HashParent = new Dictionary<SiteMapNode, SiteMapNode>();
            CatalogItemHash = new Dictionary<SiteMapNode, CatalogItem>();
            CatalogItemRevHash = new Dictionary<CatalogItem, SiteMapNode>();

            MenuType = mType;
        }
    }
}
