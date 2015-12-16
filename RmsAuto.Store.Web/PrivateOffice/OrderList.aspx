<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.OrderList" Title="Untitled Page" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register src="~/Controls/OrderList.ascx" tagname="OrderList" tagprefix="uc3" %>
<%@ Register Src="~/Controls/OrderLineList.ascx" TagName="OrderLineList" TagPrefix="uc8" %>
<%@ Register src="../Controls/OrderLinesRequiresReaction.ascx" tagname="OrderLinesRequiresReaction" tagprefix="uc4" %>
<%@ Register src="../Controls/OrderLinesWholesale.ascx" tagname="OrderLinesWholesale" tagprefix="uc5" %>
<%@ Register src="../Controls/OrderLinesReadyForDeliveryInfo.ascx" tagname="OrderLinesReadyForDeliveryInfo" tagprefix="uc6" %>
<%@ Register src="../Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc7" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
   	<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

    <uc7:TextItemControl runat="server" ID="_ftpPriceHintTextItem" TextItemID="OrderList.WholesaleFtpPriceHint" ShowHeader="False" />

	<uc6:OrderLinesReadyForDeliveryInfo ID="OrderLinesReadyForDeliveryInfo1" runat="server" />
	<h1> <asp:Literal runat="server" ID="_pageTitleLiteral" />	</h1>
	
    <div class="orders">
	<asp:MultiView ID="_multiView" ActiveViewIndex="0" runat="server" 
		onactiveviewchanged="_multiView_ActiveViewChanged">
	<asp:View runat="server" ID="_requiresReactionView" >
		<%--<span class="right_block"><asp:LinkButton runat="server" ID="LinkButton4" Text="<%$ Resources:Texts, ForWholesale %>" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></span>--%>
		<div class="tab_menu">
		    <div class="on"><asp:LinkButton runat="server" ID="LinkButton1" Text="<%$ Resources:Texts, RequiredConfirm %>" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton2" Text="<%$ Resources:Texts, OrderList %>" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton3" Text="<%$ Resources:Texts, ArchiveOrders %>" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton15" Text="<%$ Resources:Texts, AnalysisOrders %>" CommandName="SwitchViewByID" CommandArgument="_analysisOrdersView"></asp:LinkButton></div>
		</div>
        
		<uc4:OrderLinesRequiresReaction ID="OrderLinesRequiresReaction1" runat="server" />
	</asp:View>
	
	<asp:View runat="server" ID="_activeOrdersView" >
		<%--<span class="right_block"><asp:LinkButton runat="server" ID="LinkButton8" Text="<%$ Resources:Texts, ForWholesale %>" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></span>--%>
		<div>
			<asp:LinkButton  class="btn btn-default sm" runat="server" ID="LinkButton5" Text="<%$ Resources:Texts, RequiredConfirm %>" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton>
			<asp:LinkButton  class="btn btn-primary sm" runat="server" ID="LinkButton6" Text="<%$ Resources:Texts, OrderList %>" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton>
			<asp:LinkButton  class="btn btn-default sm" runat="server" ID="LinkButton7" Text="<%$ Resources:Texts, ArchiveOrders %>" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton>
			<asp:LinkButton  class="btn btn-default sm"  runat="server" ID="LinkButton16" Text="<%$ Resources:Texts, AnalysisOrders %>" CommandName="SwitchViewByID" CommandArgument="_analysisOrdersView"></asp:LinkButton>
		</div>
		<uc3:OrderList runat="server" ID="OrderList1" OrderStatusFilter="ActiveOrders" />
	</asp:View>
	
	<asp:View runat="server" ID="_archiveOrdersView" >
		<%--<span class="right_block"><asp:LinkButton runat="server" ID="LinkButton12" Text="<%$ Resources:Texts, ForWholesale %>" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></span>--%>
		<div class="tab_menu">
			<div><asp:LinkButton runat="server" ID="LinkButton9" Text="<%$ Resources:Texts, RequiredConfirm %>" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton10" Text="<%$ Resources:Texts, OrderList %>" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div class="on"><asp:LinkButton runat="server" ID="LinkButton11" Text="<%$ Resources:Texts, ArchiveOrders %>" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton17" Text="<%$ Resources:Texts, AnalysisOrders %>" CommandName="SwitchViewByID" CommandArgument="_analysisOrdersView"></asp:LinkButton></div>
		</div>

		<uc3:OrderList runat="server" ID="OrderList2" OrderStatusFilter="ArchiveOrders" />
	</asp:View>
	
	<asp:View runat="server" ID="_analysisOrdersView">
		<%--<span class="right_block"><asp:LinkButton runat="server" ID="LinkButton18" Text="<%$ Resources:Texts, ForWholesale %>" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></span>--%>
		<div class="tab_menu">
			<div><asp:LinkButton runat="server" ID="LinkButton19" Text="<%$ Resources:Texts, RequiredConfirm %>" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton20" Text="<%$ Resources:Texts, OrderList %>" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></div>
			<div><asp:LinkButton runat="server" ID="LinkButton21" Text="<%$ Resources:Texts, ArchiveOrders %>" CommandName="SwitchViewByID" CommandArgument="_archiveOrdersView"></asp:LinkButton></div>
			<div class="on"><asp:LinkButton runat="server" ID="LinkButton22" Text="<%$ Resources:Texts, AnalysisOrders %>" CommandName="SwitchViewByID" CommandArgument="_analysisOrdersView"></asp:LinkButton></div>
		</div>

		<uc8:OrderLineList runat="server" ID="OrderLineList1"></uc8:OrderLineList>
	</asp:View>
	
	<%--<asp:View runat="server" ID="_wholesaleView">
		<span class="right_block"><asp:LinkButton runat="server" ID="LinkButton14" Text="<%$ Resources:Texts, ForRetail %>" CommandName="SwitchViewByID" CommandArgument="_requiresReactionView"></asp:LinkButton></span>--%>
		<%--<span class="right_block"><asp:LinkButton runat="server" ID="LinkButton14" Text="для розничных покупателей" CommandName="SwitchViewByID" CommandArgument="_activeOrdersView"></asp:LinkButton></span>--%>
		<%--<div class="tab_menu">
			<div class="on"><asp:LinkButton runat="server" ID="LinkButton13" CssClass="selected" Text="<%$ Resources:Texts, ListItemsWholesale %>" CommandName="SwitchViewByID" CommandArgument="_wholesaleView"></asp:LinkButton></div>
		</div>

		<uc5:OrderLinesWholesale ID="OrderLinesWholesale1" runat="server" ViewMode="WholesaleMode" />		
		
	</asp:View>--%>
	</asp:MultiView>
    </div>

</asp:Content>
