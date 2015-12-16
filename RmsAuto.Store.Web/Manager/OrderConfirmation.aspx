<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.OrderConfirmation" Title="Manager working place" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h1>Order is successfully placed</h1>
	Customer: <% =SiteContext.Current.CurrentClient.Profile.ClientName %><br />
	Order's number: <% =OrderNumber %><br />
	<asp:LinkButton ID="_btnContinue" runat="server" Class="btn btn-primary btn-sm" Text="Continue" />	
</asp:Content>
