<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.PricingMatrixEntries.List" %>
<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Import Namespace="RmsAuto.Common.DataAnnotations" %>
<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2><%= table.DisplayName%></h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" />
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Literal ID="LiteralSp" runat="server"></asp:Literal>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="DropDownSp" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Literal ID="LiteralMn" runat="server"></asp:Literal>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TextBoxMn" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegexValidatorMn" runat="server" ControlToValidate="TextBoxMn" Display="Dynamic" Enabled="False"></asp:RegularExpressionValidator>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Literal ID="LiteralRg" runat="server"></asp:Literal>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TextBoxRg" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegexValidatorRg" runat="server" ControlToValidate="TextBoxRg" Display="Dynamic" Enabled="False"></asp:RegularExpressionValidator>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Literal ID="LiteralPn" runat="server"></asp:Literal>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TextBoxPn" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegexValidatorPn" runat="server" ControlToValidate="TextBoxPn" Display="Dynamic" Enabled="False"></asp:RegularExpressionValidator>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Button ID="ButtonApplyFilter" runat="server" Text="Применить фильтр" 
                onclick="ButtonApplyFilter_Click" />
            <br />
            <br />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource"
                AllowPaging="True" AllowSorting="True" CssClass="gridview" OnDataBound="GridView1_DataBound" >
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
                <PagerSettings Position="Bottom" />       
                <PagerTemplate>
                    <asp:GridViewPager runat="server" ID="GridViewPager" OnPageSizeChanged="GridViewPager_PageSizeChanged" />
                </PagerTemplate>        
                
                <EmptyDataTemplate>
                    В таблице нет данных, соответствующих условиям фильтра.
                </EmptyDataTemplate>
            </asp:GridView>

            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true"
                     Where="( (SupplierID == @Sp || @Sp == 0) && (Manufacturer.StartsWith(@Mn) || @Mn == string.Empty ) && (RgCodeSpec.StartsWith(@Rg) || @Rg == string.Empty ) && (PartNumber.StartsWith(@Pn) || @Pn == string.Empty ) )">
                <WhereParameters>
                    <asp:ControlParameter
                        Name="Sp"
                        Type = "Int32"
                        ControlId = "DropDownSp"
                        DefaultValue = "0"
                     />    
                    <asp:ControlParameter
                        Name="Mn"
                        Type = "String"
                        ControlId = "TextBoxMn"
                        DefaultValue = ""
                        ConvertEmptyStringToNull ="false" 
                     />     
                     <asp:ControlParameter
                        Name="Rg"
                        Type = "String"
                        ControlId = "TextBoxRg"
                        DefaultValue = ""
                        ConvertEmptyStringToNull ="false" 
                     />     
                     <asp:ControlParameter
                        Name="Pn"
                        Type = "String"
                        ControlId = "TextBoxPn"
                        DefaultValue = ""
                        ConvertEmptyStringToNull ="false" 
                     />     
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
