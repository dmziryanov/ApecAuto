<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsDiscountTopList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.News.NewsDiscountTopList" %>

<h3><asp:Literal ID="lHead" runat="server" Text="Действующие акции и предложения" 
		meta:resourcekey="lHeadResource1" /></h3>
<asp:Repeater ID="_repeater" runat="server">
	<HeaderTemplate>
	<div>&nbsp;</div>
	</HeaderTemplate>
	<ItemTemplate>
		<div class="item">
		<span class="date"><%#Eval( "NewsItemDate", "{0:dd.MM.yyyy}" )%></span>
		<a id="A1" href="<%=UrlManager.GetNewsDetailsUrl((int)Eval("NewsItemID"))%>">
			<span class="pict">
				<img width="157" height="117" src="<%#Eval("IconFileID")!=null ? GetFileUrl((int)Eval("IconFileID")) : ""%>" alt="">
			</span>
			<span class="name"><%#Server.HtmlEncode( (string)Eval( "NewsItemHeader" ) )%></span>
		</a>
		<%# Server.HtmlEncode((string)Eval("NewsItemAnnotation")) %>
	</div>
	</ItemTemplate>
	<FooterTemplate>
		<asp:Label ID="lblEmpty" 
			Text="На данный момент нет действующих акций и предложений." runat="server" ForeColor="Green"
			Visible='<%# bool.Parse((_repeater.Items.Count==0).ToString()) %>' 
			meta:resourcekey="lblEmptyResource1"></asp:Label>
	</FooterTemplate>
</asp:Repeater>