<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AcctgTransferErrors.aspx.cs" Inherits="RmsAuto.Store.Adm.AcctgTransferErrors" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h4>Ошибки синхронизации статусов обработки строк заказа (запись журнала от <% =TransferStartTime %>)</h4>
    <asp:GridView ID="_errorsView" runat="server" AllowPaging="true" PageSize="20"
    DataSourceID="_errorsDataSource" CssClass="gridview" AlternatingRowStyle-CssClass="even"
    AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="##">
                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>    
            </asp:TemplateField>
            <asp:BoundField DataField="ErrorMessage" HeaderText="Сообщение об ошибке" /> 
        </Columns>
    </asp:GridView>
    <asp:LinqDataSource 
    ID="_errorsDataSource" runat="server"
    ContextTypeName="RmsAuto.Store.Entities.StoreDataContext" TableName="TransferChangesErrors"
    AutoGenerateWhereClause="true" AutoPage="true" EnableInsert="false" 
    EnableUpdate="false" EnableDelete="false">
        <WhereParameters>
            <asp:QueryStringParameter Name="LogEntryID" QueryStringField="id" DbType="Int32" />
        </WhereParameters>
    </asp:LinqDataSource>
    <br />
    <a href='<% =string.Format("AcctgTransferChangesLog.aspx?page={0}", Request["page"])%>'>
        Назад к посмотру журнала
    </a>
</asp:Content>
