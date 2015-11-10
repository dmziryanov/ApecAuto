<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Edit.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.SeoPartsCatalogItems.Edit" ValidateRequest="False" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2>[<%= table.DisplayName %>].редактирование записи</h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1" Display="None" />

            <asp:DetailsView ID="DetailsView1" runat="server" 
				DataSourceID="DetailsDataSource" DefaultMode="Edit"
                AutoGenerateEditButton="false" OnItemCommand="DetailsView1_ItemCommand" OnItemUpdated="DetailsView1_ItemUpdated"
                CssClass="detailstable" FieldHeaderStyle-CssClass="bold" 
				oniteminserting="DetailsView1_ItemInserting" 
				>
                <Fields>
                    <asp:CommandField
                    ShowEditButton="true" 
                    ShowCancelButton="true"
                    ButtonType="Link" 
                    CancelText="Отмена" 
                    UpdateText="Применить" />
                </Fields>
            </asp:DetailsView>

            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableUpdate="true">
                <WhereParameters>
                    <asp:DynamicQueryStringParameter />
                </WhereParameters>
            </asp:LinqDataSource>
            <rmsauto:StateTracker ID="stateTracker" runat="server" DetailsViewID="DetailsView1" FilterRepeaterID="FilterRepeater" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
