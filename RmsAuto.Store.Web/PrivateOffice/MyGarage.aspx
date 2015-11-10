<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="MyGarage.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.MyGarage" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register src="~/Controls/GarageCars.ascx" tagname="GarageCars" tagprefix="uc3" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

<uc2:AuthorizationControl runat="server" Role="Client" />

<h1><asp:Literal ID="lMyGarage" runat="server" Text="Мой гараж" /></h1>
<span class="right_block" style="float:left;"><a href="<%=UrlManager.GetNewGarageCarUrl() %>"><%=global::Resources.Texts.VinRequests_NewGarageCar %></a></span><br /><br />

<uc3:GarageCars ID="mgcCarsList" runat="server" />



</asp:Content>