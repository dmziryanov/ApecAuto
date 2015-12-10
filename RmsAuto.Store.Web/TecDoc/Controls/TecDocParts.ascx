<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecDocParts.ascx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecDocParts" EnableViewState="false" %>
<div class="legenda">
	<img src="<%=ResolveUrl("~/images/search_desc.png")%>" alt="описание" width="14"
		height="15" border="0" />
	- <asp:Literal ID="lNote" runat="server" Text="описание" />;
	<img src="<%=ResolveUrl("~/images/search_photo.png")%>" alt="фотография" width="14"
		height="15" border="0" />
	- <asp:Literal ID="lPhoto" runat="server" Text="фотография" />;
	<img src="<%=ResolveUrl("~/images/search_auto.png")%>" alt="применяемость на авто"
		width="14" height="15" border="0" />
	- <asp:Literal ID="lUseOnCar" runat="server" Text="применяемость на авто;" />
</div>
<asp:Repeater runat="server" ID="_repeater">
	<HeaderTemplate>
		<table>
			<tr>
				<th>
				</th>
				<th>
					<asp:Literal ID="lPartNumber" runat="server" Text="Номер детали" />
				</th>
				<th>
					<asp:Literal ID="lPartNote" runat="server" Text="Описание" />
				</th>
				<th>
					<asp:Literal ID="lPrices" runat="server" Text="Цены" />
				</th>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td colspan="4" class="title_name">
				
			</td>
		</tr>
		<asp:Repeater runat="server" ID="_subRepeater" DataSource='<%#Container.DataItem%>'>
			<ItemTemplate>
				<tr>
					<td>
						<nobr>
							<a runat="server" id="a1" visible='<%#Eval("HasDescription")%>' href='<%# UrlManager.GetTecDocInfoUrl((int)Eval("Article.ID"))%>' onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
							<img src="<%#ResolveUrl("~/images/search_desc.png")%>" alt="описание" width="14" height="15" border="0"/></a>
							<a runat="server" id="a2" visible='<%#Eval("HasPics")%>' href='<%# UrlManager.GetTecDocInfoImagesUrl((int)Eval("Article.ID"))%>' onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
							<img src="<%#ResolveUrl("~/images/search_photo.png")%>" alt="фотография" width="14" height="15" border="0"/></a>
							<a runat="server" id="a3" visible='<%#Eval("HasAppliedCars")%>' href='<%# UrlManager.GetTecDocInfoCarsUrl((int)Eval("Article.ID"))%>' onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
							<img src="<%#ResolveUrl("~/images/search_auto.png")%>" alt="применяемость на авто" width="14" height="15" border="0"/></a>
						</nobr>
					</td>
					<td>
						<%#Server.HtmlEncode( (string)Eval( "Article.ArticleNumber" ) )%>
					</td>
					<td>
						<%#Server.HtmlEncode( (string)Eval( "Article.CompleteName.Tex_Text" ) )%>
					</td>
					<td>
						<a href="<%#GetSearchResultsUrl((string)Eval("Article.Supplier.Name"), (string)Eval("Article.ArticleNumber") )%>">
							<asp:Literal ID="lSearch" runat="server" Text="Найти" /></a>
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:Repeater>
