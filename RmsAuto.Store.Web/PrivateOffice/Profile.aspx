<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.Profile" Title="Untitled Page" %>
<%--<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>--%>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register src="../Controls/ClientProfile.ascx" tagname="ClientProfile" tagprefix="uc3" %>
<%@ Register src="../Controls/ClientAlerts.ascx" tagname="ClientAlerts" tagprefix="uc4" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
   	<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>
	<uc3:ClientProfile ID="ClientProfile1" runat="server" />
	<uc4:ClientAlerts ID="ClientAlerts1" runat="server" />
</asp:Content>
