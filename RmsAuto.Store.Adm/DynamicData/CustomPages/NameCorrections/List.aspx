<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.NameCorrections.List"
    MasterPageFile="~/Site.master" %>

<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <h2>
        <%= table.DisplayName%></h2>
    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1"
                Display="None" />
            <rmsauto:StateTracker ID="stateTracker" runat="server" GridViewID="GridView1" FilterRepeaterID="FilterRepeater" /><br /><br />
            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" AllowPaging="True"
                AllowSorting="True" CssClass="gridview" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="20">
                        <ItemTemplate>
                            <asp:HyperLink ID="EditHyperLink" runat="server" 
                                NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                            ToolTip="Изменить"><asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/page_edit.png" /></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Первоначальное название">
                        <ItemTemplate>
                            <span style='<%# Convert.ToBoolean(Eval("IsNew")) ? "color: red" : "" %>'><%# Eval("OriginalName") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CorrectedName" HeaderText="Исправленное название" ReadOnly="True"
                        SortExpression="CorrectedName" />
                </Columns>
                <PagerStyle CssClass="footer" />
                <PagerTemplate>
                    <asp:GridViewPager ID="GridViewPager" runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    В таблице нет данных.
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:LinqDataSource ID="GridDataSource" runat="server" ContextTypeName="RmsAuto.TechDoc.Entities.TecdocStoreDataContext"
                OrderBy="IsNew desc, OriginalName" TableName="NameCorrections">
                <WhereParameters>
                    <asp:Parameter />
                </WhereParameters>
            </asp:LinqDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
