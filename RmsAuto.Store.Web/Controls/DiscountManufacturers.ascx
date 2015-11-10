<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscountManufacturers.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.DiscountManufacturers" %>

<style type="text/css">
<!--

.row div {
    float:left;
    width:20%;
    padding-bottom: 2px;
    padding-top: 5px;
    height:20px;
}

.row div:hover {
    background-color: #c9e2f1;
}

.row div a {
    color: #1A5694;
    font-family: 'Trebuchet MS';
    font-size: 12px;
    font-weight: bold;
    text-decoration: underline;
    margin-left:6px;
}

.row div a:hover {
    text-decoration: none;
}
-->
</style>

<asp:Label runat="server" ID="_emptyLabel" Font-Bold="true" ForeColor="Green" />

<asp:Repeater ID="_rptDiscountBrands" runat="server">
	<HeaderTemplate>
		<div class="row" style="width:100%;">
	</HeaderTemplate>
	<ItemTemplate>
		<div>
			<asp:HyperLink runat="server" NavigateUrl="<%# GetSearchDiscountSparePartsUrl((string)Container.DataItem) %>" Text="<%# Container.DataItem %>" />
		</div>
	</ItemTemplate>
	<FooterTemplate>
		</div>
	</FooterTemplate>
</asp:Repeater>
