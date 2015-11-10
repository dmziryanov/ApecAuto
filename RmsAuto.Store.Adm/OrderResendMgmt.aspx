<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OrderResendMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.OrderResendMgmt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	
	<h4>Повторная отправка заказа в учетную систему</h4>
	
	<table>
		<tr>
			<td>Номер заказа:</td>
			<td><asp:TextBox ID="_txtOrderId" runat="server"></asp:TextBox></td>
		</tr>
		<tr>
			<td colspan="2"><asp:Button ID="_btnResendOrder" runat="server" Text="Отправить заказ" OnClick="_btnResendOrder_Click" OnClientClick="return confirm('Вы уверены что хотите переотправить данный заказ?')"/></td>
		</tr>
		<tr>
			<td colspan="2"><asp:Label ID="_lblInfoText" runat="server"></asp:Label></td>
		</tr>
	</table>
</asp:Content>
