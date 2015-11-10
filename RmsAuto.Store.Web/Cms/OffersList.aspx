<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="OffersList.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.OffersList" %>

<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="~/Cms/News/NewsDiscountTopList.ascx" tagname="NewsDiscountTopList" tagprefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl" TagPrefix="uc3" %>


<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>
	<uc3:TextItemControl ID="TextItem1" runat="server" ShowHeader="false" TextItemID="Offers.Text" />
	<br />
	<uc2:NewsDiscountTopList ID="NewsDiscountTopList1" runat="server" />
</asp:Content>
