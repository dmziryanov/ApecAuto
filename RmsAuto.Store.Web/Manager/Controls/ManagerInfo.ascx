<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagerInfo.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ManagerInfo" %>
<div class="links">
    <%= global::Resources.Texts.Manager %>
    <span class="sep"></span>
    <asp:Label runat="server" ID="_managerNameLabel" Font-Bold="true"></asp:Label>
    <span class="sep"></span>
    <asp:LinkButton runat="server" ID="_logoffButton" onclick="_logoffButton_Click" CausesValidation="false"><%= global::Resources.Texts.Logoff %></asp:LinkButton>
</div>