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

</script>

<uc2:LeftMenuCtl ID="_leftMenuCtl"
                 runat="server"
                 DataSourceID="_siteMapDataSource"
                 ChildsPlaceholderName="childsPlaceHolder"
                 CatalogItemMenuType="CommonMenu">
    <RootElement>
        <table class="menu-off">
            <tr>
                <td class="icon" onclick="javascript:clickParent(<%# Container.ItemId %>);"><span><img id="img1" runat="server" src='<%# "~/images/menu-off.png" %>' width="9" height="9" alt="" style='<%# Container.Node.HasChildNodes?"cursor:pointer;": "" %>' /></span></td>
                <td class="<%# Container.Node.Description %>"><a href="<%# Container.Node.Url %>"<%# Container.NewWindow ? " target=\"_blank\"" : String.Empty %>><%# Container.Node.Title %></a></td>
            </tr>
        </table>
        <asp:PlaceHolder runat="server" Visible="<%# Container.Node.HasChildNodes %>">
            <div style="display:none" id="p_<%# Container.ItemId %>" class="submenu"><ul>
                <asp:PlaceHolder ID="childsPlaceHolder" runat="server" />
            </ul></div>
        </asp:PlaceHolder>
    </RootElement>
    <RootElementSelected>
        <table class="menu-on active">
            <tr>
                <td class="icon" onclick="javascript:clickParent(<%# Container.ItemId %>);"><span class="tl"><img id="img3" runat="server" src='<%# "~/images/menu-on.png" %>' width="9" height="6" alt="" style='<%# Container.Node.HasChildNodes?"cursor:pointer;":"" %>'/></span></td>
                <td class="<%# Container.Node.Description %>"><a href="<%# Container.Node.Url %>"<%# Container.NewWindow ? " target=\"_blank\"" : String.Empty %>><%# Container.Node.Title %></a></td>
            </tr>
        </table>
        <asp:PlaceHolder runat="server" Visible="<%# Container.Node.HasChildNodes %>">
            <div id="p_<%# Container.ItemId %>" class="submenu"><ul>
                <asp:PlaceHolder ID="childsPlaceHolder" runat="server" />
            </ul></div>
        </asp:PlaceHolder>
    </RootElementSelected>
    <ChildElement>
        <li><a href="<%# Container.Node.Url %>"<%# Container.NewWindow ? " target=\"_blank\"" : String.Empty %>><%# Container.Node.Title %></a></li>
    </ChildElement>
    <ChildElementSelected>
        <li><span class="active"><%# Container.Node.Title %></span></li>
    </ChildElementSelected>
</uc2:LeftMenuCtl>
