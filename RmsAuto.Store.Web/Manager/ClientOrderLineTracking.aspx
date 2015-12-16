<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientOrderLineTracking.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientOrderLineTracking" Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="~/Controls/OrderLineTracking.ascx" tagname="OrderLineTracking" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />

	<span class="right_block">
	    <asp:HyperLink ID="_backLink" Class="btn btn-primary btn-sm" runat="server" Text="Вернуться в список заказов" />
	</span>

	<uc3:OrderLineTracking ID="OrderLineTracking1" runat="server" />

</asp:Content>
