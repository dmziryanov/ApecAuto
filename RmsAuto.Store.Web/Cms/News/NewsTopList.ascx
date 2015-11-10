<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsTopList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.News.NewsTopList" EnableViewState="false" %>

<h1><asp:Literal ID="lHead" runat="server" Text="Новости" 
		meta:resourcekey="lHeadResource1" /></h1>
<div class="news">
	<asp:Repeater ID="_repeaterCol1" runat="server">
		<HeaderTemplate>
			<div class="col1">
		</HeaderTemplate>
		<ItemTemplate>
			<div class="item">
				<span class="date"><%#Eval("NewsItemDate","{0:dd.MM.yyy}")%></span>
				<a href="<%# UrlManager.GetNewsDetailsUrl((int)Eval("NewsItemID")) %>">
					<span class="pict">
						<img width="157" height="117" alt="" src="<%# Eval("IconFileID")!=null ? GetFileUrl((int)Eval("IconFileID")) : "" %>" />
					</span>
					<span class="name"><%#Server.HtmlEncode((string)Eval("NewsItemHeader"))%></span>
				</a>
				<%# Server.HtmlEncode((string)Eval("NewsItemAnnotation")) %>
			</div>
		</ItemTemplate>
		<FooterTemplate>
			</div>
		</FooterTemplate>
	</asp:Repeater>
	<asp:Repeater ID="_repeaterCol2" runat="server">
		<HeaderTemplate>
			<div class="col2">
		</HeaderTemplate>
		<ItemTemplate>
			<div class="item">
				<span class="date"><%#Eval("NewsItemDate","{0:dd.MM.yyy}")%></span>
				<a href="<%# UrlManager.GetNewsDetailsUrl((int)Eval("NewsItemID")) %>">
					<span class="pict">
						<img width="157" height="117" alt="" src="<%# Eval("IconFileID")!=null ? GetFileUrl((int)Eval("IconFileID")) : "" %>" />
					</span>
					<span class="name"><%#Server.HtmlEncode((string)Eval("NewsItemHeader"))%></span>
				</a>
				<%# Server.HtmlEncode((string)Eval("NewsItemAnnotation")) %>
			</div>
		</ItemTemplate>
		<FooterTemplate>
			</div>
		</FooterTemplate>
	</asp:Repeater>
	<div class="clearfloat"></div>
	<asp:HyperLink runat="server" ID="_allNewsLink" CssClass="all"
		meta:resourcekey="_allNewsLinkResource1"><asp:Literal ID="lAllNews" 
		runat="server" Text="Все новости" meta:resourcekey="lAllNewsResource1" />
	</asp:HyperLink>
</div>