<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageHeader.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.PageHeader" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<%@ Register src="~/Cms/Catalog/BreadCrumbs.ascx" tagname="BreadCrumbs" tagprefix="uc1" %>
<table width="100%" cellpadding=0 cellspacing=0 border=0>
<tr>
    <td class="basket_top" valign=top>
        <asp:PlaceHolder runat="server" id="_cartPlaceHolder">
        <span><asp:HyperLink ID="_btnViewCartLink" runat="server"><asp:Literal ID="lPlaceOrder" runat="server" Text="<%$ Resources:Texts, Oform %>" /></asp:HyperLink></span>
        <asp:Label ID="_btnViewCartInfo" runat="server">
			<asp:Literal ID="lCountDetails" runat="server" Text="<%$ Resources:Texts, AllParts %>" />&nbsp;
			<b><asp:Literal runat="server" ID="_cartCountLiteral" /></b>&nbsp;
			<asp:Literal ID="lOnSumm" runat="server" Text="<%$ Resources:Texts, OnSumm %>" />&nbsp;
			<b><asp:Literal runat="server" ID="_cartSumLiteral" /></b> <%=global::Resources.Texts.DollarShort%><br />
			<asp:Literal ID="_balanceInfoLiteral" runat="server" />
        </asp:Label>
        <img runat="server" id="img3" src="~/images/1pix.gif" alt="" width="1" height="47" border="0">
		<asp:HyperLink ID="_imgViewCartLink" runat="server">
			<asp:Image ID="_imgViewCart" runat="server" ImageUrl="<%$ Resources:ImagesURL, basket_img_top3 %>" width="120" height="47" />
		</asp:HyperLink>
        </asp:PlaceHolder>
    </td>
    <td class="cabinet_top">
        <asp:HyperLink ID="_btnViewCabinetLink" runat="server"><img id="Img1" runat=server src="~/images/logoff_img.gif" alt="<%$ Resources:Texts, PrivateOffice %>" style="margin-right:0px;" width=53 height=47 align=left border=0 /></asp:HyperLink>
        <% if (Page.User.Identity.IsAuthenticated) { %>
        <span class="right_block" style="margin:13px 10px 0px 0px;"> 
        <asp:LinkButton ID="_btnLogoff" Text="<%$ Resources:Texts, Logoff %>" runat="server" OnClick="_btnLogoff_Click" CausesValidation="false" />
        </span>
        <div style="margin:13px 0px 10px 50px;width:132px;"><center>
        <% =Server.HtmlEncode( CurrentClientName ) %>
        <br />
        <asp:Literal ID="lClientID" runat="server" Text="<%$ Resources:Texts, ClientID %>" />: <% =Server.HtmlEncode( CurrentClientID ) %>
        </center></div>
        <% } else { %>
        <asp:ImageButton ID="_btnLogon" runat="server" ImageUrl="<%$ Resources:ImagesURL, logoff_title %>" CssClass="cabinet_title" onclick="_btnLogon_Click" CausesValidation="false"/>
        <% } %>
    </td>
</tr>
</table>
<div class="context2">
<uc1:BreadCrumbs ID="_breadCrumbs" runat="server" />
</div>
