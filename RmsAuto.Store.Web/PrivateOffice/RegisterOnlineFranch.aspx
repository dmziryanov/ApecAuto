<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true"
    CodeBehind="RegisterOnlineFranch.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.RegisterOnlineFranch" %>

<%@ Register Src="~/Controls/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/FillFranchProfileExt.ascx" TagName="FillFranchProfileExt" TagPrefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <uc2:FillFranchProfileExt ID="_fillFranchProfileExt" runat="server"
    Title="<%$ Resources:Texts, OnlineRegistration %>" />
        
    <asp:ImageButton ID="completeButton" runat="server" ImageUrl="~/images/send_btn.gif" onclick="FillFranchProfileCompletedExt"/>
       
    <div id="_messagePane" runat="server" visible="false">

        <uc3:TextItemControl ID="saveOKTIC" runat="server" TextItemID="RegistrationFranch.SendOK" ShowHeader="false" />
        <br/><br/>
        <asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink>
    </div>
</asp:Content>
    <%-- if ( Page.User.Identity.IsAuthenticated )
       { %>
    <h2>
        <%=global::Resources.Texts.YouLoggedOn %></h2>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink>
    <% } >
</asp:Content--%>
