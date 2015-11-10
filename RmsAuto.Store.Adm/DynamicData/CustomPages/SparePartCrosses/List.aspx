<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.SparePartCrosses.List" Title="Кроссы и переходы" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

	<h2>Кроссы и переходы</h2>

	<table class="detailstable">
	<tr>
	<th>Кроссы (старые)</th>
	<td><asp:Label runat="server" ID="_crossCountLabel"></asp:Label></td>
	<td><asp:LinkButton runat="server" ID="_deleteCrossesButton" Text="Удалить" onclick="_deleteButton_Click" OnClientClick="return confirm('Удалить кроссы и переходы?')" /></td>
	</tr>
	<tr>
	<th>Тиражируемые бренды</th>
	<td><asp:Label runat="server" ID="_crossBrandsLabel"></asp:Label></td>
	<td><asp:LinkButton runat="server" ID="_deleteBrandsButton" Text="Удалить" onclick="_deleteBrandsButton_Click" OnClientClick="return confirm('Удалить тиражируемые бренды?')" /></td>
	</tr>
	<tr>
	<th>Группы запчастей</th>
	<td><asp:Label runat="server" ID="_crossGroupsLabel"></asp:Label></td>
	<td><asp:LinkButton runat="server" ID="_deleteGroupsButton" Text="Удалить" onclick="_deleteGroupsButton_Click" OnClientClick="return confirm('Удалить группы запчастей?')" /></td>
	</tr>
	<tr>
	<th>Связи групп</th>
	<td><asp:Label runat="server" ID="_crossLinksLabel"></asp:Label></td>
	<td><asp:LinkButton runat="server" ID="_deleteLinksButton" Text="Удалить" onclick="_deleteLinksButton_Click" OnClientClick="return confirm('Удалить связи групп?')" /></td>
	</tr>
	<tr>
	</table>

</asp:Content>
