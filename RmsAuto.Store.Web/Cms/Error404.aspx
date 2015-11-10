<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Error404.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.Error404" Title="Untitled Page" meta:resourcekey="PageResource1" %>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

<h1><asp:Literal ID="lHeader" runat="server" Text="Страница не найдена" 
		meta:resourcekey="lHeaderResource1" /></h1>

<asp:Literal ID="lPageNotFound" runat="server" 
		Text="Запрошенная Вами страница не найдена." 
		meta:resourcekey="lPageNotFoundResource1" />
</asp:Content>
