<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VinRequestsList.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.VinRequestsList" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<asp:Repeater ID="_requestsRepeater" runat="server">
	<HeaderTemplate>
		<table width="100%" cellpadding="0" cellspacing="0" class="list" style="margin-top:0px;">
		<tr>
			<th>Дата запроса</th>
			<th>Запрос</th>
			<th>Обработан</th>
			<th></th>
		</tr>
	</HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td><%# Eval("RequestDate","{0:dd.MM.yyyy HH:mm:ss}") %></td>
                <td>
					<b><%#(Container.DataItem as VinRequest).GetFullName()%></b>
                </td>
                <td>
					<%#(bool)Eval("Proceeded")?Eval("AnswerDate","{0:dd.MM.yyyy HH:mm:ss}"):"нет"%>
                </td>
                <td><asp:HyperLink runat="server" ID="_detailsLink" NavigateUrl='<%#GetDetailsUrl((int)Eval("Id"))%>'>Подробнее</asp:HyperLink></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate>
		</table>
    </FooterTemplate>
</asp:Repeater>
