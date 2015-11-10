<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="AllOrders.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.AllOrders" Title="Manager working place" %>

<%@ Register src="~/Controls/AllOrders.ascx" tagname="AllOrders" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
		<uc3:AllOrders ID="OrderLinesWholesale1" runat="server" 
            ViewMode="WholesaleMode" />		
</asp:Content>


