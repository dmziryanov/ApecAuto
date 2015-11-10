<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.OrderLineStatuses.List" %>
<%@ Import Namespace="RmsAuto.Store.Entities"%>
<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Import Namespace="RmsAuto.Common.DataAnnotations" %>
<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>
<%@ Register src="~/DynamicData/Content/FilterUserControl.ascx" tagname="DynamicFilter" tagprefix="asp" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2><%= table.DisplayName%></h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
      
            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource"
                AllowPaging="False" AllowSorting="True" CssClass="gridview" >
                <Columns>
                    <asp:TemplateField ItemStyle-Width="20">
                        <ItemTemplate>
							<nobr>    
							
                            <asp:HyperLink ID="EditHyperLink" runat="server" 
                            ToolTip="Применить изменения" NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'>
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/page_edit.png" />
                            </asp:HyperLink>
                                                                      							
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField datafield="OrderLineStatusID" SortExpression="OrderLineStatusID" HeaderText="ID" ItemStyle-Width="30" />
                    <asp:BoundField datafield="NameRMS" SortExpression="NameRMS" HeaderText="ID в RMS" ItemStyle-Width="120" />
                    <asp:BoundField datafield="NameHansa" SortExpression="NameHansa" HeaderText="ID в Хансе" ItemStyle-Width="120" />                    
                    <asp:BoundField datafield="IsFinal" SortExpression="IsFinal" HeaderText="Терминальный" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField datafield="RequiresClientReaction" SortExpression="RequiresClientReaction" HeaderText="Требует реакции" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField datafield="ExcludeFromTotalSum" SortExpression="ExcludeFromTotalSum" HeaderText="Исключен из суммы" ItemStyle-Width="140" ItemStyle-HorizontalAlign="Center" />
                </Columns>

                <EmptyDataTemplate>
                    В таблице нет данных.
                </EmptyDataTemplate>
            </asp:GridView>
            
            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableUpdate="true">
            </asp:LinqDataSource>

            <br />
            
            <rmsauto:CrudTracker ID="crudTracker" runat="server" DynamicDataSourceID="GridDataSource">
            </rmsauto:CrudTracker>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
