<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.OrderDetails" Title="Untitled Page" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/Controls/OrderLinesWholesale.ascx" tagname="OrderLinesWholesale" tagprefix="uc4" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>
    <div class="text-right">
	    <asp:HyperLink ID="_backLink" runat="server" Text="<%$ Resources:Texts, BackToOrderList %>"></asp:HyperLink>
	</div>
	<uc4:OrderLinesWholesale ID="_orderLinesWholesale" runat="server" ViewMode="OrderDetailsMode" />

</asp:Content>
