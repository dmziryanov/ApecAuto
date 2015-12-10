<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BreadCrumbs.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.BreadCrumbs" %>

<asp:Repeater runat="server" ID="_repeater">
	<HeaderTemplate>
		<div class="context">
    <ol class="breadcrumb">
	</HeaderTemplate>
<ItemTemplate>
    <li><asp:HyperLink runat="server" ID="_hl1" NavigateUrl='<%#Eval("Url")%>' Visible='<%#Container.ItemIndex!=_itemsCount-1%>'><%# Eval("Name")%></asp:HyperLink></li>
    <asp:Literal runat="server" ID="_li1" Visible='<%#Container.ItemIndex==_itemsCount-1%>' Text='<%# Eval("Name")%>' />
</ItemTemplate>
<%--<SeparatorTemplate>
&gt;
</SeparatorTemplate>--%>
	<FooterTemplate>
		</ol>
        </div>
	</FooterTemplate>
</asp:Repeater>
