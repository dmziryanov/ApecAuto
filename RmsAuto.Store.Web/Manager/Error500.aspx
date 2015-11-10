<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="Error500.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.Error500" Title="Manager working place" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

<h1>Error</h1>

<asp:Label runat="server" ID="_errorLabel" ForeColor="Red" Font-Bold="true"></asp:Label>
<br />
<asp:Label runat="server" ID="_dateTimeLabel" Font-Bold="true"></asp:Label>
</asp:Content>
