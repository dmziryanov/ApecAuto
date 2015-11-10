<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManufacturerList.ascx.cs"
	Inherits="RmsAuto.Store.Web.Cms.Manufacturers.ManufacturerList" %>
<h3><asp:Literal ID="lHead" runat="server" Text="Производители автозапчастей" 
		meta:resourcekey="lHeadResource1" /></h3>
<asp:DataList ID="_dataList" runat="server" RepeatColumns="6" 
	ItemStyle-Width="17%" Width="100%" CssClass="catalog_list" 
	ItemStyle-CssClass="item">
	<ItemTemplate>
		<asp:HyperLink runat="server" ID="HyperLink1" 
			NavigateUrl='<%# GetManufacturerDetailsUrl((string)Eval("UrlCode")) %>'><%# Server.HtmlEncode((string)Eval("Name")) %></asp:HyperLink>
	</ItemTemplate>

<ItemStyle CssClass="item" Width="17%"></ItemStyle>
</asp:DataList>
