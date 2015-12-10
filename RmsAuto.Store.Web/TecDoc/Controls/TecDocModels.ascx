<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecDocModels.ascx.cs" Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecDocModels" EnableViewState="false" %>
<asp:Repeater runat="server" ID="_repeater">
	<HeaderTemplate>
		<table>
			<tr>
				<th><asp:Literal ID="lModel" runat="server" Text="Модель" /></th>
				<th><asp:Literal ID="lCreateDate" runat="server" Text="Даты выпуска" /></th>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td>
				<a href="<%#GetModelUrl((int)Eval("ID"))%>">
					<%#Server.HtmlEncode((string)Eval("Name.Tex_Text"))%>
				</a>
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
