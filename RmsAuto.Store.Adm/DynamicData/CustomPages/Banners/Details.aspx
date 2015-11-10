<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="BannerToCatalogItems.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.Banners.Details" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>

<%@ Import Namespace="RmsAuto.Store.Adm" %>
<%@ Import Namespace="RmsAuto.Common.DataAnnotations" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" runat="Server">
	<h2> Привязка баннера к разделам </h2>
	<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
	<asp:Label runat="server" ID="_errorLabel" Font-Bold="true" ForeColor="Red" EnableViewState="false" />
	<asp:Repeater runat="server" ID="_repeater">
		<HeaderTemplate>
			<table class="gridview">
				<tr>
				    <th>Видимость</th>
				    <th>
						<%#table.GetColumn("CatalogItemID").DisplayName + " раздела, Позиция и " + table.GetColumn("CatalogItemName").DisplayName%>
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
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
			    <td align="left" width="110">
			        <span style="padding-left:35px">
			        <asp:CheckBox runat="server" ID="CheckBoxVisibility"
			            ToolTip="Изменить видимость" AutoPostBack="true"
			            ForeColor="White"
			            Text='<%#Server.HtmlEncode( "[" + Convert.ToString( Eval( "CatalogItemLink.CatalogItemID" ) ) + "] {" + Convert.ToString( Eval( "CatalogItemLink.Position" ) ) + "}")%>'
			            Checked='<%#Eval("CatalogItemLink.Visible")%>'
			            OnCheckedChanged="CheckBoxVisibilityChecked" />
			        </span>
			    </td>
				<td>
			        <span style="padding-left:<%#25*(int)Eval("Level")%>px">
			            <asp:CheckBox runat="server" ID="CheckBoxLink"
			            Text='<%#Server.HtmlEncode( "[" + Convert.ToString( Eval( "CatalogItemLink.CatalogItemID" ) ) + "] {" + Convert.ToString( Eval( "CatalogItemLink.Position" ) ) + "}")%>'
			            ToolTip="Изменить привязку баннеров"  AutoPostBack="true"
			            Checked='<%#Eval("CatalogItemLink.Banded")%>'
			            OnCheckedChanged="CheckBoxLinkChecked" />
				    </span>
					<%#Server.HtmlEncode(" " + Convert.ToString(Eval("CatalogItemLink.CatalogItemName")))%>
				</td>
				<td>
					<%#Server.HtmlEncode(Convert.ToString(Eval("CatalogItemLink.CatalogItemCode")))%>
				</td>
				<td>
					<%#((Enum)Eval("CatalogItemLink.CatalogItemMenuType")).ToTextOrName()%>
				</td>
				<td>
					<%#Server.HtmlEncode(Convert.ToString(Eval("CatalogItemLink.CatalogItemPath")))%>
					<%#Eval("CatalogItemLink.CatalogItemQueryString") != null ? "? " + Server.HtmlEncode(Convert.ToString(Eval("CatalogItemLink.CatalogItemQueryString"))) : null%>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	
	<br />
	<td>
        <div class="bottomhyperlink">
            <asp:HyperLink ID="GoToEditLink" runat="server">
                <img id="Img3" runat="server" src="~/DynamicData/Content/Images/down_arc_small.png" alt="Вернуться в карточку баннера" />
                в карточку баннера
            </asp:HyperLink>
            <asp:HyperLink ID="GoToListLink" runat="server">
                <img id="Img1" runat="server" src="~/DynamicData/Content/Images/down_arc_small.png" alt="Вернуться в список баннеров" />
                в список баннеров
            </asp:HyperLink>
        </div>
    </td>          
</asp:Content>
