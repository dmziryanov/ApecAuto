<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="CartImport.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.CartImport" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="~/Controls/CartImportExt.ascx" tagname="CartImportExt" tagprefix="uc1" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Store.Web" Assembly="RmsAuto.Store" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">	
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
    <h1><%=global::Resources.Texts.OrderImport %></h1>
   
    <uc1:CartImportExt ID="_cartImport" runat="server" />
</asp:Content>
