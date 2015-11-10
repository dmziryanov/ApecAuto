<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecDocModifications.ascx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecDocModifications" EnableViewState="false" %>
<asp:Repeater runat="server" ID="_repeater">
	<HeaderTemplate>
		<table>
			<tr>
				<th><asp:Literal ID="lModification" runat="server" Text="Модификация" /></th>
				<th><asp:Literal ID="lEngVolume" runat="server" Text="Объем двигателя, см<sup>3</sup>." /></th>
				<th><asp:Literal ID="lEngPower" runat="server" Text="Мощность, л.с." /></th>
				<th><asp:Literal ID="lEngType" runat="server" Text="Тип двигателя" /></th>
				<th><asp:Literal ID="lCreateDates" runat="server" Text="Даты выпуска" /></th>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td>
				<a href="<%#GetModificationUrl((int)Eval("ID"))%>">
					<%#Server.HtmlEncode((string)Eval("Name.Text"))%>
				</a>
			</td>
			<td style="text-align: center">
				<%#Eval( "EngineVolume" )%>
			</td>
			<td style="text-align: center">
				<%#Eval( "TYP_HP_FROM" )%>
			</td>
			<td>
				<%#Eval( "EngineName.Text" )%>
				<br />
				<%#Eval( "FuelSupplyName.Text" )%>
			</td>
			<td>
				<%#Eval("DateFrom")%>
				по
				<%#Eval( "DateTo" )%>
			</td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:Repeater>
