<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Culture.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.Culture" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>

<% if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UseLocalization"]))
   { %>
<div class="btn-group-sm" dropdown="">
    <button class="btn btn-info dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
        <%=SiteContext.CurrentCulture.Split('-')[1] %>
        <span class="caret"></span>
    </button>

    <ul class="culture dropdown-menu">
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
</div>
<% } %>

