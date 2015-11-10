<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CartMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.CartMgmt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	
	<h4>Управление корзиной клиента</h4>
	
	<table>
		<tr>
			<td>ID клиента:</td>
			<td><asp:TextBox ID="_txtClientID" runat="server"></asp:TextBox></td>
			<td><asp:Button ID="_btnGetCart" runat="server" Text="показать содержимое корзины" OnClick="_btnGetCart_Click" /></td>
		</tr>
	</table>
	
	<br />
	
	<asp:DataGrid ID="_gvToExcel" runat="server" AutoGenerateColumns="false" CssClass="gridview" AlternatingItemStyle-CssClass="even">
		<Columns>
			<asp:BoundColumn DataField="Manufacturer" HeaderText="Производитель" />
			<asp:BoundColumn DataField="PartName" HeaderText="Название детали" />
			<asp:BoundColumn DataField="PartNumber" HeaderText="Номер детали" />
			<asp:BoundColumn DataField="Qty" HeaderText="Количество" />
		</Columns>
	</asp:DataGrid>
	
	<br />
	
	<asp:ImageButton ID="_btnExcel" runat="server" ImageUrl="~/images/ico-excel.png" ToolTip="выгрузить содержимое в Excel" onclick="_btnExcel_Click" />
	<asp:ImageButton ID="_btnClean" runat="server" ImageUrl="~/images/ico-basket.png" ToolTip="очистить корзину клиента" onclick="_btnClean_Click" OnClientClick="return confirm('Вы уверены что хотите очистить корзину клиента?')" />
	
</asp:Content>
