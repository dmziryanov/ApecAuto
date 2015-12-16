<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartTop.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.CartTop" %>
<div class="col-lg-4">
    <div class="panel panel-primary" style="display: inline-block; width: 180px">
        <div class="panel-heading"><%=global::Resources.Texts.Cart %></div>
        <div class="panel-body">

            <%=global::Resources.Texts.AllParts %> <strong class="blue goods-quantity">
                <asp:Literal ID="lCartQty" runat="server" /></strong>&nbsp;<%=global::Resources.Texts.OnSumm %> <strong class="blue goods-amount">
                    <asp:Literal ID="lCartTotal" runat="server" /></strong> <%=global::Resources.Texts.RoubleShort %>&nbsp;<a class="glyphicon glyphicon-shopping-cart" title="<%=global::Resources.Texts.Validate %>" href="<%=UrlManager.GetCartUrl() %>"></a>

        </div>
    </div>
</div>

