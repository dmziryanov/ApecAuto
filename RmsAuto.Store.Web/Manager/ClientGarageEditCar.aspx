<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="ClientGarageEditCar.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientGarageEditCar" Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="../Controls/GarageCarEdit.ascx" tagname="GarageCarEdit" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />
	<asp:Label runat="server" ID="_errorLabel" CssClass="error" EnableViewState="false" />
	<uc3:GarageCarEdit ID="_garageCarEdit" runat="server" />
	<asp:LinkButton runat="server" ID="SaveButton" Text="Сохранить" 
		onclick="SaveButton_Click" ValidationGroup="CarEditGroup" />
	<asp:LinkButton runat="server" ID="BackButton" Text="Вернуться" 
		onclick="BackButton_Click" />
</asp:Content>
