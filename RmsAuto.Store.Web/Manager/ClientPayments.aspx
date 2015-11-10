<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientPayments.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientPayments" %>
<%@ Register src="~/Manager/Controls/ClientPaymentsView.ascx" tagname="Payments" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:Payments id="_ClientPayments" runat="server" />
</asp:Content>