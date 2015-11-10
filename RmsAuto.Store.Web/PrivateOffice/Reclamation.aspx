<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Reclamation.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.Reclamation" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register Src="~/Controls/Reclamation.ascx" TagName="Reclamation" TagPrefix="uc1" %>

<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
	<h1><asp:Literal ID="_pageTitleLiteral" runat="server" Text="Рекламации" /></h1>
	<uc1:Reclamation ID="Reclamation1" runat="server" />
</asp:Content>
