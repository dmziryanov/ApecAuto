<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RmsAuto.Store.Web.Login1" %>
<%@ Register src="~/Controls/Logon.ascx" tagname="Logon1" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h1><%=global::Resources.Texts.SystemEntrance %></h1>
    <uc1:Logon1 ID="_logonPanel" runat="server" />
</asp:Content>