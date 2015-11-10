<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.NameCorrections.Edit"
    MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="false" />
    <h2>
        [<%= table.DisplayName %>].редактирование записи</h2>
    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1"
                Display="None" />
            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" OnItemCommand="DetailsView1_ItemCommand"
                DataKeyNames="NameCorrectionID" DataSourceID="DetailsDataSource" DefaultMode="Edit"
                OnItemUpdated="DetailsView1_ItemUpdated" CssClass="detailstable" OnItemUpdating="DetailsView1_ItemUpdating">
                <Fields>
                    <asp:BoundField DataField="OriginalName" HeaderText="Первоначальное название" SortExpression="OriginalName"
                        ControlStyle-CssClass="droplist" HeaderStyle-CssClass="bold" ReadOnly="true" />
                    <asp:BoundField DataField="CorrectedName" HeaderText="Исправленное название" SortExpression="CorrectedName"
                        ControlStyle-CssClass="droplist" HeaderStyle-CssClass="bold" />
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ButtonType="Link"
                        CancelText="Отмена" UpdateText="Применить" />
                </Fields>
            </asp:DetailsView>
            <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="RmsAuto.TechDoc.Entities.TecdocStoreDataContext"
                EnableUpdate="true" TableName="NameCorrections" Where="NameCorrectionID == @NameCorrectionID">
                <WhereParameters>
                    <asp:QueryStringParameter Name="NameCorrectionID" QueryStringField="NameCorrectionID"
                        Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
