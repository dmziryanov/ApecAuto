<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="VinRequestAccessDenied.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.VinRequestAccessDenied" Title="Error403VIN" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register TagPrefix="rms" Assembly="RmsAuto.Common" Namespace="RmsAuto.Common.Web.UI" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

	<uc1:TextItemControl runat="server" ID="TextBlock" ShowHeader="True" TextItemID="VINRequestRetail.Text" />
	<br />	
    <p><asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink> </p>

</asp:Content>