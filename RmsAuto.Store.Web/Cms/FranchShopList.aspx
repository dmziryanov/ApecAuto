<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="ShopList.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.ShopList" Title="Untitled Page" %>
<%@ Register src="Shops/FranchShopList.ascx" tagname="ShopList" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

	<div style="margin-bottom:10px">
	    <b class="header3Style"><asp:Literal runat="server" ID="_pageTitleLiteral" /></b></br>
	</div>
	<uc1:ShopList ID="ShopList1" runat="server" />
</asp:Content>
