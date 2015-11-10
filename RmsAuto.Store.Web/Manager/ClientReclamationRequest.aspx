<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="ClientReclamationRequest.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientReclamationRequest" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<%@ Register Src="~/Controls/ReclamationRequest.ascx" TagName="ReclamationRequest" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />
	<uc2:ReclamationRequest ID="ReclamationRequest1" runat="server" />
</asp:Content>
