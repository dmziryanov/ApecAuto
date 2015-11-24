<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartTop.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.CartTop" %>

<div class="block cart">
	<div class="title"> <span class="icon"><img src="/images/cart.png" width="19" height="15" alt="/"></span><%=global::Resources.Texts.Cart %></div>
	<div class="in">
		<%=global::Resources.Texts.AllParts %> <strong class="blue goods-quantity"><asp:Literal ID="lCartQty" runat="server" /></strong> <br />
		<%=global::Resources.Texts.OnSumm %> <strong class="blue goods-amount"><asp:Literal ID="lCartTotal" runat="server" /></strong> руб <br />
		<a class="button" href="<%=UrlManager.GetCartUrl() %>"><%=global::Resources.Texts.Validate %></a>
	</div>
</div>