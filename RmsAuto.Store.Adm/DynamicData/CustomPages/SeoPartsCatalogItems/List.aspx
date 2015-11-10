<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.SeoPartsCatalogItems.List" %>
<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Import Namespace="RmsAuto.Common.DataAnnotations" %>
<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>
<%@ Register src="~/DynamicData/Content/FilterUserControl.ascx" tagname="DynamicFilter" tagprefix="asp" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

   <h2><%= table.DisplayName%></h2>

	<div style="margin-bottom: 5px">
	<asp:Repeater runat="server" ID="_breadCrumbsRepeater">
	<ItemTemplate>
		<asp:HyperLink runat="server" NavigateUrl='<%#Eval("Url")%>' Text='<%#Server.HtmlEncode((string)Eval("Name"))%>' />
	</ItemTemplate>
	<SeparatorTemplate> &gt; </SeparatorTemplate>
	</asp:Repeater>
	&nbsp; 
	<asp:HyperLink runat="server" ID="_previewLink" Target="_blank">[предпросмотр]</asp:HyperLink>
	</div>
	
   <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource"
        AllowPaging="True" PageSize="50" AllowSorting="True" CssClass="gridview" AutoGenerateColumns="true">
        <Columns>
            <asp:TemplateField ItemStyle-Width="20">
                <ItemTemplate>
					<nobr>
                    <asp:HyperLink ID="EditHyperLink" runat="server" 
                        NavigateUrl='<%# GetEditUrl(GetDataItem()) %>'
                    ToolTip="Изменить"><asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/page_edit.png" /></asp:HyperLink>

                    <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                        CausesValidation="false" ToolTip="Удалить" 
                        OnClientClick='return confirm("Запись будет удалена. Вы уверены?");'
                        Visible='<%#(int?)Eval("ParentID")!=null%>'
                    ><asp:Image runat="server" ID="Image2" ImageUrl="~/DynamicData/Content/Images/cross.png" /></asp:LinkButton>

                    <asp:HyperLink ID="ListHyperLink" runat="server" 
                        NavigateUrl='<%# GetListUrl((int)Eval("ID")) %>'
                    ToolTip="Подразделы"><asp:Image runat="server" ID="Image3" ImageUrl="~/DynamicData/Content/Images/show_subcatalog.png" /></asp:HyperLink>
                    
					</nobr>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="Название">
				<ItemTemplate>
					<asp:HyperLink runat="server" ID="_link" NavigateUrl='<%#GetListUrl((int)Eval("ID"))%>' Text='<%#Server.HtmlEncode((string)Eval("Name"))%>' />
				</ItemTemplate>
            </asp:TemplateField>
            <asp:DynamicField DataField="UrlCode" />
            <asp:DynamicField DataField="Visible" />--%>
        </Columns>

        <PagerStyle CssClass="footer"/>        
        <PagerTemplate>
            <asp:GridViewPager runat="server" ID="GridViewPager"/>
        </PagerTemplate>
        <EmptyDataTemplate>
            В таблице нет данных.
        </EmptyDataTemplate>
    </asp:GridView>

    <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" OrderBy="NameRU">
    </asp:LinqDataSource>

    <br />

    <div class="bottomhyperlink">
        <asp:HyperLink ID="InsertHyperLink" runat="server"><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Добавить запись" />Добавить раздел</asp:HyperLink>
    </div>
</asp:Content>
