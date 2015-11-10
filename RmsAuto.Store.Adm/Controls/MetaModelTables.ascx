<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaModelTables.ascx.cs" Inherits="RmsAuto.Store.Adm.Controls.MetaModelTables" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />

<h4><% = HeaderText %></h4>

<asp:GridView ID="Menu1" runat="server" AutoGenerateColumns="false"
    CssClass="gridview" AlternatingRowStyle-CssClass="even">
    <Columns>
        <asp:TemplateField HeaderText="Название" SortExpression="TableName">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("ListActionPath") %>'><%#Eval("DisplayName") %></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
