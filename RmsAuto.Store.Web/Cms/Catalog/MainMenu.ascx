<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.MainMenu" EnableViewState="false" %>
<%@ Register src="~/Cms/Catalog/LeftMenuCtl.ascx" tagname="LeftMenuCtl" tagprefix="uc2" %>

<asp:SiteMapDataSource ID="_siteMapDataSource" runat="server" 
	ShowStartingNode="false" />

<script type="text/javascript">

    function clickParent(id)
    {
        var div = document.getElementById('p_' + id);
        
        if (div == null)
        {
            return;
        }
        
        if (div.style.display == 'none')
        {
            div.style.display = '';
        }
        else
        {
            div.style.display = 'none';
        }
    }

    function redirect(path) {
        window.location = path;
    }

</script>

<uc2:LeftMenuCtl ID="_leftMenuCtl"
                 runat="server"
                 DataSourceID="_siteMapDataSource"
                 ChildsPlaceholderName="childsPlaceHolder"
                 CatalogItemMenuType="CommonMenu">
    <RootElement>
           <div class="dropdown" style="float: left;">
            <button class="m_active btn btn-default dropdown-toggle" onclick="redirect('<%# Container.Node.Url %>');" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <%# Container.Node.Title %>
            </button>
            
            <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                <asp:PlaceHolder ID="childsPlaceHolder" runat="server" />
            </ul>
            </div>
    </RootElement>
    <RootElementSelected>
            <div class="dropdown" style="float: left;">
            <button class="m_active btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <%# Container.Node.Title %>
            </button>
            
            <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                <asp:PlaceHolder ID="childsPlaceHolder" runat="server" />
            </ul>
            </div>
    </RootElementSelected>
    <ChildElement>
        <li><a href="<%# Container.Node.Url %>"><%# Container.Node.Title %></a></li>
    </ChildElement>
    <ChildElementSelected>
        <li><a class="m_active" href="<%# Container.Node.Url %>"<%# Container.NewWindow ? " target=\"_blank\"" : String.Empty %>><%# Container.Node.Title %></a></li>
    </ChildElementSelected>
</uc2:LeftMenuCtl>
