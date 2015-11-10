<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="ClientVinRequests.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientVinRequests" Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="Controls/VinRequestsList.ascx" tagname="VinRequestsList" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />
	<uc3:VinRequestsList ID="VinRequestsList1" runat="server" />
</asp:Content>
