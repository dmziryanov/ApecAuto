<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="VinRequestNew.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.VinRequestNew" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register src="~/Controls/VinRequestNew.ascx" tagname="VinRequestNew" tagprefix="uc3" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

<h1><asp:Literal ID="lVinReq" runat="server" Text="запросы по VIN" /></h1>
<span class="link_block"><a href="<%=UrlManager.GetVinRequestsUrl() %>"><%=global::Resources.Texts.VinRequests_BackToList %></a></span><br />
<br />
<!--
<div class="tabs">
<a href="<%=UrlManager.GetVinRequestsUrl() %>"><%=global::Resources.Texts.VinRequests_BackToList %></a>
<a href="<%=UrlManager.GetGarageUrl() %>"><%=global::Resources.Texts.VinRequests_Garage %></a>
</div>
-->

<uc3:VinRequestNew ID="vrNew" runat="server" OnSaveRequest="vrNewSaveRequest" />

<asp:Panel ID="_pValidResult" runat="server" Visible="false">

<%=global::Resources.Texts.YourRequestSentToManager %>

</asp:Panel>

</asp:Content>