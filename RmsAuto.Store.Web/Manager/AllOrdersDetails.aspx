<%@ Page Language="C#"  MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="AllOrdersDetails.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.AllOrdersDetails"  Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="~/Controls/OrderLinesWholesale.ascx" tagname="OrderLinesWholesale" tagprefix="uc4" %>
<%@ Register src="~/Controls/OrderLinesWholesaleLite.ascx" tagname="OrderLinesWholesaleLite" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

	<div class="right_block">
	    <asp:HyperLink ID="_backLink" runat="server" CssClass="button" Text="Back to orders list"></asp:HyperLink>
	</div>

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>

	<uc4:OrderLinesWholesale ID="_orderLinesWholesale" runat="server" ViewMode="OrderDetailsMode" />
	<uc5:OrderLinesWholesaleLite ID="_orderLinesWholesaleLite" runat="server" ViewMode="OrderDetailsMode" />

</asp:Content>
