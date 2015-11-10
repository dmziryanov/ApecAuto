<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="ClientGarage.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientGarage" Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="~/Controls/GarageCars.ascx" tagname="MyGarageCars" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />
	<uc3:MyGarageCars ID="_garageCars" runat="server" />
	
<span class="right_block" style="float:left;"><asp:HyperLink runat="server" id="_carCreateLink" Text="Создать машину" /></span><br />
</asp:Content>
