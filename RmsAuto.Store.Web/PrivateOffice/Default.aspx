<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.Default" Title="" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>

	<asp:DataList ID="_dataList" runat="server" RepeatColumns="1">
	<ItemTemplate>
		<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%#UrlManager.GetCatalogUrl((int)Eval("CatalogItemID"))%>' Target='<%#(bool)Eval("CatalogItemOpenNewWindow")==true?"_blank":""%>'>
			<%# //Server.HtmlEncode((string)Eval("CatalogItemName"))
				Eval("CatalogItemName")%>
		</asp:HyperLink>
	</ItemTemplate>
	</asp:DataList>
	
</asp:Content>


