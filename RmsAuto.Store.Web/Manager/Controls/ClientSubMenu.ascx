<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientSubMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientSubMenu" %>
<h2><%= RmsAuto.Store.Web.SiteContext.Current.CurrentClient.Profile.ClientName %></h2>

<div class="tab_menu">
    <asp:Repeater ID="_menuItems" runat="server">
        <ItemTemplate>
            <div<%# (bool)Eval("Selected") ? " class=\"on\"" : "" %>>
                <asp:HyperLink ID="_dataSectionLink" runat="server" 
                    NavigateUrl='<%# Eval("SectionUrl") %>'
                    Enabled='<%# Eval("Enabled") %>'>
                        <%# Eval("DisplayText") %>
                </asp:HyperLink>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="tab-bottom"></div>