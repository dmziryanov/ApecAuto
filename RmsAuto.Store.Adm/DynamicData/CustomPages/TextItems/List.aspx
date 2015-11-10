<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.List" %>
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

            <asp:FilterRepeater ID="FilterRepeater" runat="server">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("DisplayName") %>' AssociatedControlID="DynamicFilter$DropDownList1" />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
                </ItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:FilterRepeater>
            <rmsauto:StateTracker ID="stateTracker" runat="server" GridViewID="GridView1" FilterRepeaterID="FilterRepeater" /><br /><br />

            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource"
                AllowPaging="True" AllowSorting="True" CssClass="gridview">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="20">
                        <ItemTemplate>
							<nobr>
                            <asp:HyperLink ID="EditHyperLink" runat="server" 
                                NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                            ToolTip="Изменить"><asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/page_edit.png" /></asp:HyperLink>
                            
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                CausesValidation="false" ToolTip="Удалить" 
                                OnClientClick='return confirm("Запись будет удалена. Вы уверены?");'
                                Visible='<%# !(bool)Eval("TextItemFixed") %>'
                            ><asp:Image runat="server" ID="Image2" ImageUrl="~/DynamicData/Content/Images/cross.png" /></asp:LinkButton>
                            
                            <asp:HyperLink ID="DetailsHyperLink" runat="server"
                                NavigateUrl='<%# table.GetActionPath(PageAction.Details, GetDataItem()) %>'
                                ToolTip="Просмотреть детали">
                                <asp:Image runat="server" ID="Image3" ImageUrl="~/DynamicData/Content/Images/zoom.png" />
                               </asp:HyperLink>
							</nobr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="footer"/>        
                <PagerTemplate>
                    <asp:GridViewPager runat="server" ID="GridViewPager"/>
                </PagerTemplate>
                <EmptyDataTemplate>
                    В таблице нет данных.
                </EmptyDataTemplate>
            </asp:GridView>

            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true">
                <WhereParameters>
                    <asp:DynamicControlParameter ControlID="FilterRepeater" />
                </WhereParameters>
            </asp:LinqDataSource>

            <br />

            <div class="bottomhyperlink">
                <asp:HyperLink ID="InsertHyperLink" runat="server"><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Добавить запись" />Добавить запись</asp:HyperLink>
            </div>
            <rmsauto:CrudTracker ID="crudTracker" runat="server" DynamicDataSourceID="GridDataSource" EditControlID="EditHyperLink"
            CreateControlID="InsertHyperLink" DeleteControlID="DeleteLinkButton"></rmsauto:CrudTracker>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
