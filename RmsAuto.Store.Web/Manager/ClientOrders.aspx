<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientOrders.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientOrders" Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="~/Controls/OrderList.ascx" tagname="OrderList" tagprefix="uc3" %>
<%@ Register src="~/Controls/OrderLinesRequiresReaction.ascx" tagname="OrderLinesRequiresReaction" tagprefix="uc4" %>
<%@ Register src="~/Controls/OrderLinesWholesale.ascx" tagname="OrderLinesWholesale" tagprefix="uc5" %>
<%@ Register src="~/Controls/OrderListLite.ascx" tagname="OrderListLite" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <script type="text/javascript">
	$(document).ready(function() {
		$(".date").datepicker();
	});
</script>
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />

	<asp:MultiView ID="_multiView" ActiveViewIndex="2" runat="server" onactiveviewchanged="_multiView_ActiveViewChanged">
	<asp:View runat="server" ID="_requiresReactionView" >
		<div class="tab_menu">
		    <div class="on"><asp:LinkButton runat="server" ID="LinkButton1" Text="Confirmation is required" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton2" Text="List of orders" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton3" Text="Order archive" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton4" Text="Details" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></div>
		</div>
		<uc4:OrderLinesRequiresReaction ID="OrderLinesRequiresReaction1" runat="server" />
	</asp:View>
	
	<asp:View runat="server" ID="_activeOrdersView" >
		<div class="tab_menu">
			<div><asp:LinkButton runat="server" ID="LinkButton5" Text="Confirmation is required" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div class="on"><asp:LinkButton runat="server" ID="LinkButton6" Text="List of orders" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton7" Text="Order archive" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton8" Text="Details" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></div>
		</div>
		<uc3:OrderList runat="server" ID="OrderList1" OrderStatusFilter="ActiveOrders" />
		<uc6:OrderListLite runat="server" ID="OrderList3" OrderStatusFilter="ActiveOrders" />
	</asp:View>
	
	<asp:View runat="server" ID="_archiveOrdersView" >
		<div class="tab_menu">
			<div><asp:LinkButton runat="server" ID="LinkButton9" Text="Confirmation is required" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton10" Text="List of orders" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div class="on"><asp:LinkButton runat="server" ID="LinkButton11" Text="Order archive" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton12" Text="Details" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></div>
		</div>
		
		<uc3:OrderList runat="server" ID="OrderList2" OrderStatusFilter="ArchiveOrders" />
		<uc6:OrderListLite runat="server" ID="OrderList4" OrderStatusFilter="ArchiveOrders" />
	</asp:View>
	
	<asp:View runat="server" ID="_wholesaleView">
		<div class="tab_menu">
			<div><asp:LinkButton runat="server" ID="LinkButton15" Text="Confirmation is required" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton16" Text="List of orders" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton17" Text="Order archive" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div class="on"><asp:LinkButton runat="server" ID="LinkButton13" CssClass="selected" Text="Details" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></div>
		</div>

		<uc5:OrderLinesWholesale ID="OrderLinesWholesale1" runat="server" ViewMode="WholesaleMode" />		
		
	</asp:View>
	</asp:MultiView>

</asp:Content>


