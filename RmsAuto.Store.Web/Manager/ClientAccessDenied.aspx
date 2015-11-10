<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientAccessDenied.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientAccessDenied" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h1>Access denied</h1>
    <div class="error">
        This section is inaccessible for the selected client
    </div>
</asp:Content>
