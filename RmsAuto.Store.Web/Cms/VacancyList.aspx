<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="VacancyList.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.VacancyList" Title="Untitled Page" %>
<%@ Register src="Vacancies/VacancyList.ascx" tagname="VacancyList" tagprefix="uc1" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>
	
	<uc2:TextItemControl runat="server" ID="TextBlock" ShowHeader="false" TextItemID="SuperjobInfo" />

	<uc1:VacancyList ID="VacancyList1" runat="server" />
</asp:Content>
