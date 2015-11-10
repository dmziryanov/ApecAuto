<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeoShortLinkList.ascx.cs"
    Inherits="RmsAuto.Store.Web.Cms.SeoShortLinks.SeoShortLinkList" %>
<%@ Register Src="SeoShortLink.ascx" TagName="SeoShortLink" TagPrefix="uc1" %>
<div class="right_menu">
    <h3 style="padding-left: 58px">
        <asp:Literal ID="lSectionName" runat="server" 
            meta:resourcekey="lSectionNameResource1" />
    </h3>
    <ul class="help">
        <asp:ListView ID="lvShortLinks" runat="server">
            <LayoutTemplate>
                <li runat="server" id="itemPlaceholder" />
            </LayoutTemplate>
            <ItemTemplate>
                <uc1:SeoShortLink ID="SeoShortLink1" runat="server" SeoLinksID='<%# Container.DataItem %>' />
            </ItemTemplate>
        </asp:ListView>
    </ul>
</div>
