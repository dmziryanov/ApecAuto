<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BreadCrumbs.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.BreadCrumbs" %>

<asp:Repeater runat="server" ID="_repeater">
	<HeaderTemplate>
		<div class="context">
	</HeaderTemplate>
<ItemTemplate>
<asp:HyperLink runat="server" ID="_hl1" NavigateUrl='<%#Eval("Url")%>' Visible='<%#Container.ItemIndex!=_itemsCount-1%>'><%# //Server.HtmlEncode((string)Eval("Name"))
                                                                                                                         	Eval("Name")%></asp:HyperLink>
<%--<asp:Literal runat="server" ID="_li1" Visible='<%#Container.ItemIndex==_itemsCount-1%>' Text='<%# Server.HtmlEncode((string)Eval("Name"))%>' />--%>
<span><asp:Literal runat="server" ID="_li1" Visible='<%#Container.ItemIndex==_itemsCount-1%>' Text='<%# Eval("Name")%>' /></span>
</ItemTemplate>
<%--<SeparatorTemplate>
&gt;
</SeparatorTemplate>--%>
	<FooterTemplate>
		</div>
	</FooterTemplate>
</asp:Repeater>
