<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsDetails.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.News.NewsDetails" %>
<h1><asp:Literal ID="lHead" runat="server" Text="Новости" 
		meta:resourcekey="lHeadResource1" /></h1>

<div class="news-list">
    <div class="item">
		<span class="date"><%= _item.NewsItemDate.ToString("dd.MM.yyyy") %></span>
		<span class="name"><%= Server.HtmlEncode(_item.NewsItemHeader) %></span>
		<div class="text"><%= _item.NewsItemText %></div>
	</div>
</div>

<div class="link_block">
	<asp:HyperLink runat="server" ID="_allNewsLink"><asp:Literal ID="lReturnToList" runat="server" Text="Вернуться к списку новостей" meta:resourcekey="lReturnToListResource1" /></asp:HyperLink>
</div>