<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.Banners.List" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>
<%@ Import Namespace="RmsAuto.Store.Entities"%>
<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Import Namespace="RmsAuto.Common.DataAnnotations" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2><%=table.DisplayName%></h2>

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
                            ToolTip="Карточка баннера" NavigateUrl='<%#GetEditURL(GetDataItem())%>'>
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/page_edit.png" />
                            </asp:HyperLink>
                            			
							<asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CommandArgument='<%#Eval("BannerID")%>'
                                CausesValidation="false" ToolTip="Удалить" 
                                OnClientClick='return confirm("Запись будет удалена. Вы уверены?");'
                                OnCommand="OnDeleteClick">
                            <asp:Image runat="server" ID="Image2" ImageUrl="~/DynamicData/Content/Images/cross.png" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    В таблице нет данных.
                </EmptyDataTemplate>
            </asp:GridView>
            
            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableInsert="true" EnableUpdate="true">
            </asp:LinqDataSource>

            <br />
            <div class="bottomhyperlink">
                <asp:HyperLink ID="InsertHyperLink" runat="server">
                    <img id="Img1" runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Добавить запись" />
                    Добавить запись
                </asp:HyperLink>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>