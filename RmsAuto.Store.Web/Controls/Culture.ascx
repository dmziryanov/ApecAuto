<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Culture.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.Culture" %>

<% if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UseLocalization"])) { %>
<ul class="culture">
<li>
    <asp:LinkButton ID="lnkCultureEN" runat="server" OnClick="Culture_Click" CommandArgument="en-US">
        EN
        <b></b>
    </asp:LinkButton>
</li>
    <li>
    <asp:LinkButton ID="lnkCultureDE" runat="server" OnClick="Culture_Click" CommandArgument="de-DE">
        DE
        <b></b>
    </asp:LinkButton>
</li>
<li>
    <asp:LinkButton ID="lnkCultureRU" runat="server" OnClick="Culture_Click" CommandArgument="ru-RU">
        RU
        <b></b>
    </asp:LinkButton>
</li>
</ul>
<% } %>