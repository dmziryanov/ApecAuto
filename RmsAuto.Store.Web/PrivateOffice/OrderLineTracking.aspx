<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="OrderLineTracking.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.OrderLineTracking" Title="Untitled Page" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register src="../Controls/OrderLineTracking.ascx" tagname="OrderLineTracking" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
   	<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>

	<span class="right_block">
	<asp:HyperLink ID="_backLink" runat="server" Text="<%$ Resources:Texts, BackToOrderList %>" />
	</span>

	<uc3:OrderLineTracking ID="OrderLineTracking1" runat="server" />


</asp:Content>
