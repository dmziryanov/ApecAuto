<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmationMessage.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ConfirmationMessage" %>
<% =MessageText %>
<br />
<asp:HyperLink ID="_btnDefault" runat="server" 
NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>">
</asp:HyperLink>     

