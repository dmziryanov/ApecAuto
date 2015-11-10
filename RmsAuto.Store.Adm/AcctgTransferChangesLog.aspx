<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AcctgTransferChangesLog.aspx.cs" Inherits="RmsAuto.Store.Adm.AcctgTransferChangesLog" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h4>Журнал синхронизации статусов обработки заказов</h4>
    
    <table> 
        <tr>
            <td>Время последнего изменения статусов строк:</td>
            <td><% =LastStatusChangeTime.HasValue ? LastStatusChangeTime.Value.To_ddMMyyyy_HHmmss_String() : "NA" %></td>
        </tr>   
        <tr>
            <td>Время "точки отсчета" новой синхронизации:</td>
            <td><% =NextCheckpointTime.To_ddMMyyyy_HHmmss_String() %></td>
        </tr>
        <tr>
            <td>Явно указать "точку отсчета" (dd.MM.yyyy HH:mm:ss)</td>
            <td><asp:TextBox ID="_txtCheckpointTime" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <%--<asp:Button ID="_getChanges" runat="server" Text="GetChanges" onclick="_getChanges_Click" />--%>
    <%--<br />--%>
    <br />
    <asp:GridView ID="_logEntriesView" runat="server" 
    AutoGenerateColumns="false"
    DataSourceID="_logEntriesDataSource"
    DataKeyNames="LogEntryID"
    CssClass="gridview" AlternatingRowStyle-CssClass="even" AllowPaging="true" PageSize="20">
        <Columns>
            <asp:BoundField DataField="CheckpointTime" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderText="Точка отсчета" />
            <asp:BoundField DataField="ChangesReceived" HeaderText="Получено изменений" />
            <asp:BoundField DataField="MinChangeTime" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderText="Мин. время изменений" />
            <asp:BoundField DataField="MaxChangeTime" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderText="Макс. время изменений" />
            <asp:BoundField DataField="OrderLinesAdded" HeaderText="Добавлено новых строк" />
            <asp:BoundField DataField="StatusChangesAdded" HeaderText="Добавлено новых статусов" />
            <asp:BoundField DataField="TransferStartTime" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderText="Время начала синхронизации" />
            <asp:BoundField DataField="TransferDuration" DataFormatString="{0:HH:mm:ss}" HeaderText="Время выполнения" />
            <asp:TemplateField HeaderText="## ошибок">
                <ItemTemplate>
                    <asp:LinkButton ID="_btnViewErrors" runat="server" 
                    Text='<%# Eval("TransferChangesErrors.Count") %>' 
                    PostBackUrl='<%# string.Format("~/AcctgTransferErrors.aspx?id={0}&page={1}", Eval("LogEntryID"), _logEntriesView.PageIndex)%>'
                    Enabled='<%# (int)Eval("TransferChangesErrors.Count") > 0 %>'>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            журнал синхронизации не содержит записей
        </EmptyDataTemplate>
    </asp:GridView>
    <br />
    <asp:Button ID="_clearLogEntries" runat="server" Text="Очистить журнал" onclick="_clearLogEntries_Click" />
    <asp:LinqDataSource ID="_logEntriesDataSource" runat="server"
    ContextTypeName="RmsAuto.Store.Entities.StoreDataContext"
    TableName="TransferChangesLogEntries"
    EnableInsert="false"
    EnableDelete="false"
    EnableUpdate="false">
    </asp:LinqDataSource>
</asp:Content>
