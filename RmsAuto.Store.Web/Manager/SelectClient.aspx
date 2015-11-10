<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="SelectClient.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.SelectClient" Title="Manager working place" %>
<%@ Register src="Controls/ClientPicker.ascx" tagname="ClientPicker" tagprefix="uc1" %>
<%@ Register src="Controls/ClientPickerLite.ascx" tagname="ClientPicker" tagprefix="uc2" %>
<%@ Register src="~/Manager/Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:HandyClientSetBar ID="_handyClientSet" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <uc1:ClientPicker ID="ClientPicker1" runat="server" />
    <uc2:ClientPicker ID="ClientPicker2" runat="server" />
    <asp:LinkButton ID="_lbRegisterClient" CssClass="button" runat="server" Text="<%$ Resources:Texts, RegisterNewClient %>" PostBackUrl="~/Manager/RegisterClient.aspx"></asp:LinkButton>
</asp:Content>