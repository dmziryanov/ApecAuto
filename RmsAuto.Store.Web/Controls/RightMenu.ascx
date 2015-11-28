<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.RightMenu" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%--<%@ Register src="~/Controls/LogonRight.ascx" tagname="logonright" tagprefix="uc1" %>--%>
<%@ Register src="~/Cms/Catalog/HelpMenu.ascx" tagname="HelpMenu" tagprefix="uc3" %>
<%--<%@ Register Src="~/Cms/Banners/BannerControl.ascx" TagName="BannerControl" TagPrefix="uc4" %>--%>

<%--<uc1:LogonRight runat="server" ID="rightLogonControl" />--%>
<%--<asp:PlaceHolder runat="server" ID="_cartPlaceHolder">
	<asp:HyperLink ID="_imgViewCartLink" runat="server"><img src="<%$ Resources:ImagesURL, basket_img %>" runat="server" alt="<%$ Resources:Texts, Cart %>" width="217" height="49" border="0"></asp:HyperLink><div class="basket">
	<asp:Label ID="_cartInfoLabel" runat="server">
		<%=global::Resources.Texts.AllParts%> <b><asp:Literal runat="server" ID="_cartCountLiteral" /></b><br /> <%=global::Resources.Texts.OnSumm%> <b><asp:Literal runat="server" ID="_cartSumLiteral" /></b> <%=global::Resources.Texts.RoubleShort%>
	</asp:Label>
	<asp:Label runat="server" ID="_cartEmptyLabel"><asp:Literal ID="lNoParts" runat="server" Text="<%$ Resources:Texts, NoParts %>" /></asp:Label>
	<br />
	<asp:HyperLink ID="_btnViewCartLink" runat="server"><asp:Literal ID="lPlaceOrder" runat="server" Text="<%$ Resources:Texts, Oform %>" /></asp:HyperLink>
	</div>
</asp:PlaceHolder>--%>

<%--<div class="bc_right">
    <uc4:BannerControl ID="BannerControlPos1" runat="server" />
</div>--%>

<%--<asp:PlaceHolder runat="server" ID="_finmarketPlaceHolder">
<div style="text-align:center">
<img style="margin-bottom:20px;margin-top:20px;" border="0"
src="http://extds.finmarket.ru/informersgif/cdusdfx1.gif" alt="Курс ЦБ РФ USD и Forex EUR/USD - ИА &laquo;Финмаркет&raquo;" title="Курс ЦБ РФ USD и Forex EUR/USD - ИА &laquo;Финмаркет&raquo;">
</div>
</asp:PlaceHolder>--%>

<uc3:HelpMenu ID="HelpMenu1" runat="server" />