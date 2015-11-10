<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="RmsAuto.Store.Adm.Controls.TopMenu" %>
<%@ Import Namespace="System.Configuration" %>


<asp:menu id="_menu" disappearafter="2000" staticdisplaylevels="2" staticsubmenuindent="10" 
        orientation="Horizontal" font-names="Arial" runat="server" DataSourceID="RmsAutoWebSitemap" >
        <staticmenuitemstyle backcolor="LightSteelBlue" forecolor="Black"/>
        <statichoverstyle backcolor="LightSkyBlue"/>
        <dynamicmenuitemstyle backcolor="Black" forecolor="Silver"/>
        <dynamichoverstyle backcolor="LightSkyBlue" forecolor="Black"/>
</asp:menu>
<asp:SiteMapDataSource ID="RmsAutoWebSitemap" SiteMapProvider="WebSitemap" runat="server"/>

     <%--<asp:menuitem navigateurl="~/Import.aspx" text="Импорт данных" />--%>
     <%--<asp:menuitem navigateurl="~/AcctgRefMgmt.aspx" text="Кэш данных Hansa" />--%>
     <%--<asp:menuitem navigateurl="~/AcctgHansaMgmt.aspx" text="Управление Hansa" />--%>
     <%--<asp:menuitem navigateurl="~/AcctgTransferChangesLog.aspx" text="Статусы строк Hansa - WEB" />--%>
     <%--<asp:MenuItem NavigateUrl="~/OrderResendMgmt.aspx" Text="Повторная отправка заказа"></asp:MenuItem>--%>

