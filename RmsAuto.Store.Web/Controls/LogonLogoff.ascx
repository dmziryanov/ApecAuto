<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogonLogoff.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.LogonLogoff" %>

<% if (!Page.User.Identity.IsAuthenticated) { %>
<a id="login" href="#"><%=global::Resources.Texts.Login %></a>
<% } else { %>
<asp:LinkButton ID="LinkButton1" Text="<%$ Resources:Texts, Logoff %>" runat="server" OnClick="OnLogOff" CausesValidation="false" />
<% } %>