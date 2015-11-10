using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Cms.Catalog
{
	public partial class LeftMenuCtl : System.Web.UI.UserControl
    {
        public string DataSourceID { get; set; }
        public string ChildsPlaceholderName { get; set; }
        public CatalogItemMenuType CatalogItemMenuType { get; set; }

        public override void DataBind()
        {
            if (CatalogItemMenuType == default(CatalogItemMenuType))
            {
                CatalogItemMenuType = CatalogItemMenuType.CommonMenu;
            }

            var dataSource = Parent.FindControl(DataSourceID) as SiteMapDataSource;
            if (dataSource == null)
            {
                throw new ArgumentNullException("DataSourceID");
            }

            var provider = dataSource.Provider as CatalogSiteMapProvider;

            bool flag = false;
            foreach (SiteMapNode _node in provider.RootNode.ChildNodes)
            {
                Control ctl;
                //  Отрисуем парентовый менюайтем
                //  Необходимо проверить факт выбранного парентового менюайтема

				if( !flag && provider.CurrentNode != null && provider.CurrentNode.Equals( provider.RootNode ) )
                {
                    InstantiateMenuItem(RootElementSelected, _node, contents, out ctl);
                    flag = true;
                }
                else
                {

                    InstantiateMenuItem(CheckIfIn(_node, provider.CurrentNode) && RootElementSelected != null ? RootElementSelected : RootElement,
                                        _node, contents, out ctl);
                }

                if (ChildElement != null && _node.HasChildNodes)
                {
                    //  Будем чаилдов рисовать
                    //  Если есть особый пласехолдер внутри парента - инстанцируем туда, иначе - в конец парент айтема
                    Control ctlToAppendChilds = null;
                    if (!String.IsNullOrEmpty(ChildsPlaceholderName))
                    {
                        ctlToAppendChilds = ctl.FindControl(ChildsPlaceholderName);
                    }
                    if (ctlToAppendChilds == null)
                    {
                        ctlToAppendChilds = new PlaceHolder();
                        ctl.Controls.Add(ctlToAppendChilds);
                    }

                    foreach (var __node in (_node as SiteMapNode).ChildNodes)
                    {
                        Control _ctl;
                        //  С чаилдами проверка на выбранный айтем попроще, т.к. у нас только один уровень вложенности
                        InstantiateMenuItem(__node.Equals(provider.CurrentNode) && ChildElementSelected != null ? ChildElementSelected : ChildElement,
                                            __node, ctlToAppendChilds, out _ctl);
                    }
                }

                //  contents.Controls.Add(ctl);
            }
            base.DataBind();
        }

        private bool CheckIfIn(SiteMapNode node, SiteMapNode selectedNode)
        {
            if (selectedNode == null || node == null)
            {
                return false;
            }

            if (selectedNode.Equals(node))
            {
                return true;
            }

            if (node.HasChildNodes)
            {
                foreach (SiteMapNode _node in node.ChildNodes)
                {
                    if (CheckIfIn(_node, selectedNode))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void InstantiateMenuItem(ITemplate template, object _node, Control contents, out Control ctl)
        {
            var _currentNode = _node as SiteMapNode;
            ctl = new MenuItemContainer(_currentNode, CatalogItemMenuType);
            template.InstantiateIn(ctl);
            contents.Controls.Add(ctl);
        }

        [TemplateContainer(typeof(MenuItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate RootElement { get; set; }

        [TemplateContainer(typeof(MenuItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ChildElement { get; set; }

        [TemplateContainer(typeof(MenuItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate RootElementSelected { get; set; }

        [TemplateContainer(typeof(MenuItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ChildElementSelected { get; set; }
	}

    public class MenuItemContainer : Control, INamingContainer
    {
        public SiteMapNode Node { get; set; }
        public CatalogItemMenuType CatalogItemMenuType { get; set; }

        public MenuItemContainer(SiteMapNode node, CatalogItemMenuType mType)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            Node = node;
            CatalogItemMenuType = mType;
        }

        public string ItemId
        {
            get
            {
                var provider = Node.Provider as CatalogSiteMapProvider;
                if (provider == null)
                {
                    return Node.Key;
                }
                return provider.GetCatalogItem(Node, CatalogItemMenuType).CatalogItemID.ToString();
            }
        }

        public string ItemImageUrl
        {
            get
            {
                var provider = Node.Provider as CatalogSiteMapProvider;
                if (provider == null)
                {
                    return String.Empty;
                }
                return provider.GetCatalogItem(Node, CatalogItemMenuType).CatalogItemImageUrl;
            }
        }

        public bool NewWindow
        {
            get
            {
                var provider = Node.Provider as CatalogSiteMapProvider;
                if (provider == null)
                {
                    return false;
                }
                return provider.GetCatalogItem(Node, CatalogItemMenuType).CatalogItemOpenNewWindow;
            }
        }
    }
}