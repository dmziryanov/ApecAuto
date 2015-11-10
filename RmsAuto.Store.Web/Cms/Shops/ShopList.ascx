<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Shops.ShopList" %>

<asp:ListView ID="_listView" runat="server" >
<LayoutTemplate>
	<table cellspacing="0" cellpadding="0" border="0" width="100%">
    <tr runat="server" id="itemPlaceholder" >
    </tr>
    </table>
</LayoutTemplate>
<ItemTemplate>
	<tr id="Li1" runat="server">
	    <td width="35%" runat="server" style="height:20px;padding-bottom:5px" >
           <asp:HyperLink runat="server" ID="_itemLink" style="" class="GrayTextStyle" NavigateUrl='<%# UrlManager.GetShopDetailsUrl((int)Eval("ShopID")) %>'><%#Server.HtmlEncode((string)Eval( "ShopString" ))%></asp:HyperLink>
	    </td>
	</tr>
</ItemTemplate>
</asp:ListView>

<asp:LinqDataSource ID="_linqDataSource" runat="server"
	ContextTypeName="RmsAuto.Store.Cms.Entities.CmsDataContext" 
    TableName="Shops" 
    Where="ShopVisible"
    OrderBy="ShopPriority" >
</asp:LinqDataSource>

