<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.News.NewsList" %>


<%--<asp:DropDownList   runat="server"
                    ID="_ddSelectYear"
                    AutoPostBack="true" />--%>

<asp:ListView ID="_listView" runat="server">
<LayoutTemplate>
	<div class="news">
	    <div runat="server" id="itemPlaceholder" class="item"></div>
	</div>
    <div class="pages">
	    <asp:DataPager ID="_dataPager" runat="server" PageSize="20">
		    <Fields>
			    <asp:NextPreviousPagerField ShowFirstPageButton="False" 
				    ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="" ButtonCssClass="prev" />
			    <asp:NumericPagerField />
			    <asp:NextPreviousPagerField ShowLastPageButton="False" 
				    ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="" ButtonCssClass="next" />
		    </Fields>
	    </asp:DataPager>
    </div>
</LayoutTemplate>
<ItemTemplate>
    <div class="item">
		<span class="date"><%#Eval( "NewsItemDate", "{0:dd.MM.yyyy}" )%></span>
		<a href="<%# UrlManager.GetNewsDetailsUrl((int)Eval("NewsItemID"))%>">
			<span class="pict">
				<img width="157" height="117" src="<%#Eval("IconFileID")!=null ? GetFileUrl((int)Eval("IconFileID")) : ""%>" alt="">
			</span>
			<span class="name"><%#Server.HtmlEncode( (string)Eval( "NewsItemHeader" ) )%></span>
		</a>
		<%# Server.HtmlEncode((string)Eval("NewsItemAnnotation")) %>
	</div>
</ItemTemplate>
</asp:ListView>