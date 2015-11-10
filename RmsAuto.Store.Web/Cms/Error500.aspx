<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Error500.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.Error500" Title="Untitled Page" meta:resourcekey="PageResource1" %>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

<h1><asp:Literal ID="lHeadError" runat="server" Text="Ошибка" 
		meta:resourcekey="lHeadErrorResource1" /></h1>
<%--<b>К сожалению, система в настоящее время недоступна, повторите свой запрос позднее.</b>--%>
<b><%=global::Resources.Exceptions.CommonException%></b>
<br />

<p><asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" 
		Text="Перейти на главную страницу" meta:resourcekey="_btnDefaultResource1"></asp:HyperLink> </p>
</asp:Content>
