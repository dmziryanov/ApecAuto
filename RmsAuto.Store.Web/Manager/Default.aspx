<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.Default" Title="Manager working place" %>
<%@ Register src="~/Manager/Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<%@ Register src="~/Manager/Controls/ClientProfileView.ascx" tagname="ClientProfileView" tagprefix="uc2" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc3" %>
<%@ Register src="Controls/ClientAccountInfo.ascx" tagname="ClientAccountInfo" tagprefix="uc4" %>

<asp:Content ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:HandyClientSetBar ID="_handyClientSet" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <h1><%= global::Resources.Texts.ManagerWorkingPlace %></h1>
</asp:Content>