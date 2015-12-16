<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.MainMenu" EnableViewState="false" %>
<%@ Register Src="~/Cms/Catalog/LeftMenuCtl.ascx" TagName="LeftMenuCtl" TagPrefix="uc2" %>

<asp:SiteMapDataSource ID="_siteMapDataSource" runat="server"
    ShowStartingNode="false" />

<script type="text/javascript">

    function clickParent(id) {
        var div = document.getElementById('p_' + id);

        if (div == null) {
            return;
        }

        if (div.style.display == 'none') {
            div.style.display = '';
        }
        else {
            div.style.display = 'none';
        }
    }

    function redirect(path) {
        window.location = path;
    }

</script>
<div class="navbar-collapse collapse" id="navbar">
   
    <ul class="nav navbar-nav">
   
        <uc2:LeftMenuCtl ID="_leftMenuCtl"
            runat="server"
            DataSourceID="_siteMapDataSource"
            ChildsPlaceholderName="childsPlaceHolder"
            CatalogItemMenuType="CommonMenu">
            <RootElement>
                <li class="dropdown" style="float: left;">
                    <a class="dropdown-toggle" onclick="redirect('<%# Container.Node.Url %>');" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <%# Container.Node.Title %>
                    </a>

                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                        <asp:PlaceHolder ID="childsPlaceHolder" runat="server" />
                    </ul>
                </li>
            </RootElement>
            <RootElementSelected>
                <li class="dropdown active" style="float: left;">
                    <a class="dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"><%# Container.Node.Title %></a>
                    <ul role="menu" class="dropdown-menu">
                        <asp:PlaceHolder ID="childsPlaceHolder" runat="server" />
                    </ul>
                </li>
            </RootElementSelected>
            <ChildElement>
                <li><a class="message-author" href="<%# Container.Node.Url %>"><%# Container.Node.Title %></a></li>
            </ChildElement>
            <ChildElementSelected>
                <li><a class="m_active" href="<%# Container.Node.Url %>" <%# Container.NewWindow ? " target=\"_blank\"" : String.Empty %>><%# Container.Node.Title %></a></li>
            </ChildElementSelected>
        </uc2:LeftMenuCtl>

    </ul>
</div>
