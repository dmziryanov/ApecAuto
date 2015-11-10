<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientProfile.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientProfile" Title="Manager working place" %>
<%@ Register src="~/Manager/Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<%@ Register src="~/Manager/Controls/ClientProfileView.ascx" tagname="ClientProfileView" tagprefix="uc2" %>
<%@ Register src="~/Manager/Controls/ClientProfileViewLite.ascx" tagname="clientprofileviewLite" tagprefix="uc5" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc3" %>
<%@ Register src="Controls/ClientAccountInfo.ascx" tagname="ClientAccountInfo" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:HandyClientSetBar ID="_handyClientSet" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc3:ClientSubMenu     ID="_clientSubMenu" runat="server" />
	<uc2:ClientProfileView ID="_clientProfileView" runat="server" />
	<uc5:clientprofileviewLite ID="_clientProfileViewLite" runat="server" />
	<uc4:ClientAccountInfo ID="_clientAccountInfo" runat="server" />
</asp:Content>
