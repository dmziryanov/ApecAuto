<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.NewsList" 
	Title="Untitled page" %>	
<%@ Register src="News/NewsList.ascx" tagname="NewsList" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>

	<uc1:NewsList ID="NewsList1" runat="server" />
</asp:Content>
