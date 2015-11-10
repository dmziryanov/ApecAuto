<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="SearchSpareParts.aspx.cs" Inherits="RmsAuto.Store.Web.SearchSpareParts" %>
<%@ Register src="~/Controls/SpareParts.ascx" tagname="SpareParts" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc3" %>
<%@ Register TagPrefix="rms" Namespace="RmsAuto.Common.Web.UI" Assembly="RmsAuto.Common" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc3:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h1><%=global::Resources.Texts.SearchResults %></h1>

    <uc1:SpareParts ID="_spareParts" runat="server" />

    <rms:SortOptions ID="_partsSortOptions" runat="server" OptionListControlID="_sortOptionsBox">
        <rms:SortOption DisplayText="<%$ Resources:Texts, Costs %>" SortExpression="FinalSalePriceRUR" /> 
        <rms:SortOption DisplayText="<%$ Resources:Texts, CostsDESC %>" SortExpression="FinalSalePriceRUR desc" /> 
    </rms:SortOptions>

  </asp:Content>
