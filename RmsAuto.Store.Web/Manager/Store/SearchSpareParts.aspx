<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="SearchSpareParts.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.Store.SearchSpareParts" %>
<%@ Register src="~/Controls/SpareParts.ascx" tagname="SpareParts" tagprefix="uc1" %>
<%@ Register src="~/Manager/Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<%@ Register TagPrefix="rms" Namespace="RmsAuto.Common.Web.UI" Assembly="RmsAuto.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:HandyClientSetBar ID="_handyClientSet" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <h1>Search results</h1>
    <asp:Label runat="server" ID="lblMessage" Visible="false"></asp:Label>
    <uc1:SpareParts ID="_spareParts" runat="server" />
    <rms:SortOptions ID="_partsSortOptions" runat="server" OptionListControlID="_sortOptionsBox">
        <rms:SortOption DisplayText="Price + delivery term" SortExpression="FinalSalePriceRUR,DeliveryDaysMin" />
        <rms:SortOption DisplayText="Brand + partnumber" SortExpression="Manufacturer,PartNumber" />
        <rms:SortOption DisplayText="Partnumber" SortExpression="PartNumber" />
        <rms:SortOption DisplayText="Description" SortExpression="PartDescription" /> 
        <rms:SortOption DisplayText="Availability" SortExpression="QtyInStock" /> 
        <rms:SortOption DisplayText="Delivery term" SortExpression="DeliveryDaysMin" />       
    </rms:SortOptions>
 </asp:Content>
