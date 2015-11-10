<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="VacancyDetails.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.VacancyDetails" 
	Title="Untitled page" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="Vacancies/VacancyDetails.ascx" tagname="VacancyDetails" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader" runat="server" RenderCurrentNodeAsLink="True" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc2:VacancyDetails ID="_vacancyDetails" runat="server" />
</asp:Content>
