<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecdocPartAddInfo.ascx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecdocAddInfo" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Repeater ID="_rpt" runat="server" DataSource="<%# this.AdditionalInfos %>">
<HeaderTemplate>
<table>
    <tbody>
</HeaderTemplate>
<ItemTemplate>
        <tr>
            <td><%# ((KeyValuePair<string, string>)Container.DataItem).Key %></td>
            <td><%# ((KeyValuePair<string, string>)Container.DataItem).Value%></td>
        </tr>
</ItemTemplate>
<FooterTemplate>
    </tbody>
</table>
</FooterTemplate>        
</asp:Repeater>
