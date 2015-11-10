<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.CatalogItems.List" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>

<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Import Namespace="RmsAuto.Common.DataAnnotations" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" runat="Server">
	<h2>
		<%= table.DisplayName%></h2>
	<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
	<asp:Label runat="server" ID="_errorLabel" Font-Bold="true" ForeColor="Red" EnableViewState="false" />
	<asp:Repeater runat="server" ID="_repeater" 
		onitemcommand="_repeater_ItemCommand">
		<HeaderTemplate>
			<table class="gridview">
				<tr>
					<th>
						<%#table.GetColumn("CatalogItemName").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("CatalogItemCode").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("CatalogItemMenuType").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("CatalogItemPath").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("CatalogItemVisible").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("IsServicePage").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("CatalogItemOpenNewWindow").DisplayName%>
					</th>
					<th>
						<%#table.GetColumn("CatalogItemPriority").DisplayName%>
					</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td nowrap>
					<span style="padding-left:<%#25*(int)Eval("Level")%>px">
					<asp:HyperLink ID="EditHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Edit, Eval("CatalogItem")) %>'
						ToolTip="Изменить">
						<asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/page_edit.png" /></asp:HyperLink>
					<asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CommandArgument='<%#Eval("CatalogItem.CatalogItemID")%>' CausesValidation="false"
						ToolTip="Удалить" OnClientClick='return confirm("Запись будет удалена. Вы уверены?");'>
						<asp:Image runat="server" ID="Image2" ImageUrl="~/DynamicData/Content/Images/cross.png" /></asp:LinkButton>
					<asp:HyperLink ID="DetailsHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Details, Eval("CatalogItem")) %>'
						ToolTip="Просмотреть детали">
						<asp:Image runat="server" ID="Image3" ImageUrl="~/DynamicData/Content/Images/zoom.png" />
					</asp:HyperLink>
					<%#Server.HtmlEncode( Convert.ToString( Eval( "CatalogItem.CatalogItemName" ) ) )%>
					</span>
				</td>
				<td>
					<%#Server.HtmlEncode( Convert.ToString( Eval( "CatalogItem.CatalogItemCode" ) ) )%>
				</td>
				<td>
					<%#((Enum)Eval( "CatalogItem.CatalogItemMenuType" ) ).ToTextOrName()%>
				</td>
				<td>
					<%#Server.HtmlEncode( Convert.ToString( Eval( "CatalogItem.CatalogItemPath" ) ) )%>
					<%#Eval( "CatalogItem.CatalogItemQueryString" )!=null ? "? "+Server.HtmlEncode( Convert.ToString( Eval( "CatalogItem.CatalogItemQueryString" ) ) ) : null%>
				</td>
				<td align="center">
					<%# (bool)Eval( "CatalogItem.CatalogItemVisible" ) ? "&#x25A0;" : "" %>
				</td>
				<td align="center">
					<%# (bool)Eval( "CatalogItem.IsServicePage" ) ? "&#x25A0;" : "" %>
				</td>
				<td align="center">
					<%# (bool)Eval( "CatalogItem.CatalogItemOpenNewWindow" ) ? "&#x25A0;" : "" %>
				</td>
				<td>
					<%# Eval( "CatalogItem.CatalogItemPriority" ) %>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	
	<br />
	<div class="bottomhyperlink">
		<asp:HyperLink ID="InsertHyperLink" runat="server"><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Добавить запись" />Добавить запись</asp:HyperLink>
	</div>
</asp:Content>
