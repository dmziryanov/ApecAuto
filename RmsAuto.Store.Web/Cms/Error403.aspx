<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Error403.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.Error403" Title="Untitled Page" meta:resourcekey="PageResource1" %>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register TagPrefix="rms" Assembly="RmsAuto.Common" Namespace="RmsAuto.Common.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

<h1><asp:Literal ID="lErrorHead" runat="server" Text="Ошибка" 
		meta:resourcekey="lErrorHeadResource1" /></h1>
<b><asp:Literal ID="lNotAcces" runat="server" Text="Нет доступа" 
		meta:resourcekey="lNotAccesResource1" /></b>
<br />

<p><asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" 
		Text="Перейти на главную страницу" meta:resourcekey="_btnDefaultResource1"></asp:HyperLink> </p>
</asp:Content>
