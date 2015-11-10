﻿<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientCartImport.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientCartImport" Title="Manager working place" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="~/Controls/CartImportExt.ascx" tagname="CartImportExt" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc3:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Manager" />
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />
    <uc2:CartImportExt ID="_cartImport" runat="server" />
</asp:Content>