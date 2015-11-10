<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="NewsDetails.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.NewsDetails" 
	Title="" %>
<%@ Register src="News/NewsDetails.ascx" tagname="NewsDetails" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<uc1:NewsDetails ID="_newsDetails" runat="server" />
</asp:Content>