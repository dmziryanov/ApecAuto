<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Insert.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.SeoPartsCatalogItems.Insert" ValidateRequest="False" %>


<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2>[<%= table.DisplayName %>].добавление записи</h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1" Display="None" />

            <asp:DetailsView ID="DetailsView1" runat="server" 
				DataSourceID="DetailsDataSource" DefaultMode="Insert"
                AutoGenerateInsertButton="false" OnItemCommand="DetailsView1_ItemCommand" OnItemInserted="DetailsView1_ItemInserted"
                CssClass="detailstable" FieldHeaderStyle-CssClass="bold" >
                <Fields>
                    <asp:CommandField
                    ShowInsertButton="true"
                    ShowCancelButton="true"
                    ButtonType="Link" 
                    CancelText="Отмена" 
                    InsertText="Добавить" />
                </Fields>
            </asp:DetailsView>

            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableInsert="true">
            </asp:LinqDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
