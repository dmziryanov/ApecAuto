<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="UserSetting.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.UserSetting" %>

<%@ Register Src="~/Controls/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/NotificationConfig.ascx" TagName="NotificationConfig"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/PrcExcessPriceConfig.ascx" TagName="PrcExcessPriceConfig"
    TagPrefix="uc5" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
    <uc3:TextItemControl runat="server" ID="TextBlock" ShowHeader="true" TextItemID="AlertConfigHint" />
    <asp:MultiView ID="mvSetting" runat="server" ActiveViewIndex="0">
        <asp:View ID="vNotificationConfig" runat="server">
            <div class="tab_menu">
                <div class="on">
                    <asp:LinkButton runat="server" ID="LinkButton1" Text="<%$ Resources:Texts, NotificationManage %>" CommandName="SwitchViewByID"
                        CommandArgument="vNotificationConfig"></asp:LinkButton></div>
                <div>
                    <asp:LinkButton runat="server" ID="LinkButton2" Text="<%$ Resources:Texts, OrderImport %>" CommandName="SwitchViewByID"
                        CommandArgument="vPrcExcessPriceConfig"></asp:LinkButton></div>
            </div>
            <br />
            <uc4:NotificationConfig ID="_NotificationConfig" runat="server" />
        </asp:View>
        <asp:View ID="vPrcExcessPriceConfig" runat="server">
            <div class="tab_menu">
                <div>
                    <asp:LinkButton runat="server" ID="LinkButton3" Text="<%$ Resources:Texts, NotificationManage %>" CommandName="SwitchViewByID"
                        CommandArgument="vNotificationConfig"></asp:LinkButton></div>
                <div class="on">
                    <asp:LinkButton runat="server" ID="LinkButton4" Text="<%$ Resources:Texts, OrderImport %>" CommandName="SwitchViewByID"
                        CommandArgument="vPrcExcessPriceConfig"></asp:LinkButton></div>
            </div>
            <br />
            <uc5:PrcExcessPriceConfig ID="_PrcExcessPriceConfig" runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>
