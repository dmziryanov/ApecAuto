<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportReportView.ascx.cs" Inherits="RmsAuto.Store.Adm.Controls.ImportReportView" %>
<%@ Import Namespace="RmsAuto.Store.Import" %>
<table cellspacing="5" cellpadding="5">
    <tr>
        <td>Файл</td>
        <td><asp:Literal ID="_ltrFilename" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td>Режим загрузки</td>
        <td><asp:Literal ID="_ltrMode" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td>Elapsed</td>
        <td><asp:Literal ID="_ltrElapsed" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td valign="top">Статистика</td>
        <td>
            <ul>
                <li>прочитано: <asp:Literal ID="_ltrFound" runat="server"></asp:Literal></li>
                <li>пропущено: <asp:Literal ID="_ltrSkipped" runat="server"></asp:Literal></li>
                <li>удалено  : <asp:Literal ID="_ltrRemoved" runat="server"></asp:Literal></li>
                <li>добавлено: <asp:Literal ID="_ltrAdded" runat="server"></asp:Literal></li>
                <li>обновлено: <asp:Literal ID="_ltrUpdated" runat="server"></asp:Literal></li>
            </ul>
        </td>
    </tr>
</table>
<hr />
<asp:Repeater ID="_rptValidationErrors" runat="server" 
    onitemdatabound="_rptValidationErrors_ItemDataBound">
    <HeaderTemplate>
        <h4>Информация об ошибках проверки:</h4>        
    </HeaderTemplate>
    <ItemTemplate>
        Исходная строка: <b><%# Eval("RawRecord") %></b><br />
        <asp:Repeater ID="_rptDetails" runat="server">
            <HeaderTemplate>
                <b>детали</b><br />
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("ColumnName") %> - <%# DisplayFailReason(((ValidationErrorDetail)Container.DataItem).FailReason) %>
            </ItemTemplate>
            <SeparatorTemplate><br /></SeparatorTemplate>
        </asp:Repeater>
     </ItemTemplate>
     <SeparatorTemplate>
        <hr />
     </SeparatorTemplate>
</asp:Repeater>
<hr />
<asp:Repeater ID="_rptDbErrors" runat="server">
    <HeaderTemplate>
        <h4>Информация об ошибках загрузки:</h4>        
    </HeaderTemplate>
    <ItemTemplate>
        <b><%# Eval("Message") %></b><br />
        <%# Eval("StackTrace") %><br />
        Source record: <%# ((Exception)Container.DataItem).Data["SourceRecord"] %>
    </ItemTemplate>
</asp:Repeater>

