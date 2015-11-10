<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.PasswordRecovery" %>
<%@ Register src="../Controls/WebUserMaintRequest.ascx" tagname="WebUserMaintRequest" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc1:WebUserMaintRequest ID="_pwdRecoveryRequest" runat="server" />
</asp:Content>