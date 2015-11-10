<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HandyClientSetBar.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.HandyClientSetBar" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<asp:Repeater ID="_rptHandyClients" runat="server" 
    onitemcommand="_rptHandyClients_ItemCommand">
    <ItemTemplate>
		<div  class="person<%#((ManagerSiteContext)SiteContext.Current).ClientSet.IsClientDefault((string)Eval("Profile.ClientId"))?"_on":""%>">
			<asp:LinkButton ID="_lbRemoveClient" runat="server" CommandName="Remove" CommandArgument='<%# Eval("Profile.ClientId") %>' OnClientClick='return confirm("Close client profile?");' ToolTip='Close client profile'><img id="img7" src="~/images/del.png" alt="" runat="server" /></asp:LinkButton>
			<img id="Img3" runat="server" src="~/images/acc_on.png" alt="" Visible='<%#IsClientOnline((RmsAuto.Store.Web.ClientData)Container.DataItem)%>'/>
<%--			<img id="Img4" runat="server" src="~/images/acc_off.png" alt="Нет учётной записи на сайте" Visible='<%#!IsClientOnline((RmsAuto.Store.Web.ClientData)Container.DataItem)%>' />--%>
			<asp:LinkButton ID="_cartURL" runat="server" ToolTip="There are goods in the cart" CommandName="Cart" CommandArgument='<%# Eval("Profile.ClientId") %>' Visible='<%# GetCartDisplay((RmsAuto.Store.Web.ClientData)Container.DataItem) == CartDisplay.Full %>'><img id="Img1" runat="server" src="~/images/cart_on.png" alt="" /></asp:LinkButton>
			<img id="Img2" runat="server" src="~/images/cart_empty.png" alt="" title="The cart is empty" Visible='<%# GetCartDisplay((RmsAuto.Store.Web.ClientData)Container.DataItem) == CartDisplay.Empty %>' />
			<img id="Img5" runat="server" src="~/images/cart_off.png" alt="" title="Cart is inaccessible" Visible='<%# GetCartDisplay((RmsAuto.Store.Web.ClientData)Container.DataItem) == CartDisplay.Disabled %>' />
			<asp:LinkButton tooltip='<%# Server.HtmlEncode((string)Eval("Profile.ClientName")) %>' ID="_lbManageClient" runat="server" CommandName="Manage"  CommandArgument='<%# Eval("Profile.ClientId") %>' >
				<%# Server.HtmlEncode((string)Eval("Profile.ClientName")) %>
			</asp:LinkButton>
		</div>
    </ItemTemplate>
</asp:Repeater>
<div class="person select">
    <asp:HyperLink ID="_lnkSelectClient" runat="server" Text="<%$ Resources:Texts, AddCustomer %>" NavigateUrl="~/Manager/SelectClient.aspx"></asp:HyperLink>
</div>

