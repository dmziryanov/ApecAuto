<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopVacancyBriefList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Vacancies.ShopVacancyBriefList" %>
<ul>
<asp:Repeater runat="server" ID="_itemRepeater">
<ItemTemplate>
<li>
<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%# UrlManager.GetVacancyDetailsUrl((int)Eval("VacancyID"))%>'><%# Server.HtmlEncode( (string)Eval("VacancyName") ) %></asp:HyperLink>
</li>
</ItemTemplate>
</asp:Repeater>
</ul>