<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reclamation.ascx.cs"
	Inherits="RmsAuto.Store.Web.Controls.Reclamation" %>
<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl"
	TagPrefix="uc1" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
	TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ReclamationOrderLines.ascx" TagName="ReclamationOrderLines"
	TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ReclamationList.ascx" TagName="ReclamationList" TagPrefix="uc3" %>

<uc1:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

<script type="text/javascript">
	function CheckAgreement() {
		if ($('#chbAgree').is(':checked'))
			$('#<%=_btnGo.ClientID %>').removeAttr('disabled');
		else
			$('#<%=_btnGo.ClientID %>').attr('disabled', 'disabled');
	}
</script>

<asp:MultiView ID="_mvReclamations" runat="server" ActiveViewIndex="0">
	<asp:View ID="_viewAgreement" runat="server">
		<div class="tab_menu">
			<div class="on">
				<a href="#"><%= global::Resources.Reclamations.Rules %></a></div>
			<div>
				<asp:LinkButton ID="lbRequests1" runat="server" Text="<%$ Resources:Reclamations, FormRequests %>" CommandName="SwitchViewByID"
					CommandArgument="_viewAgreement"></asp:LinkButton></div>
			<div>
				<asp:LinkButton ID="lbReclamations1" runat="server" Text="<%$ Resources:Reclamations, ReclamationInProcess %>" CommandName="SwitchViewByID"
					CommandArgument="_viewReclamationsInWork"></asp:LinkButton></div>
		</div>
		<br />
		<br />
		<uc2:TextItemControl ID="TextItemControl1" runat="server" ShowHeader="true" TextItemID="ReclamationAgreement" />
		<p>
			<input id="chbAgree" type="checkbox" onclick="CheckAgreement()" />
			<label for="chbAgree">
				<%= global::Resources.Reclamations.IHaveReadAndAccept %></label>
		</p>
		<asp:Button ID="_btnGo" runat="server" Text="<%$ Resources:Reclamations, StartToForm %>" Width="620" Enabled="false"
			CommandName="SwitchViewByID" CommandArgument="_viewRequestsRegistration" />
	</asp:View>
	<asp:View ID="_viewRequestsRegistration" runat="server">
		<div class="tab_menu">
			<div>
				<asp:LinkButton ID="lbRules2" runat="server" Text="<%$ Resources:Reclamations, Rules %>" CommandName="SwitchViewByID"
					CommandArgument="_viewAgreement"></asp:LinkButton></div>
			<div class="on">
				<a href="#"><%= global::Resources.Reclamations.FormRequests %></a></div>
			<div>
				<asp:LinkButton ID="lbReclamations2" runat="server" Text="<%$ Resources:Reclamations, ReclamationInProcess %>" CommandName="SwitchViewByID"
					CommandArgument="_viewReclamationsInWork"></asp:LinkButton></div>
		</div>
		<br />
		<uc2:TextItemControl ID="TextItemControl2" runat="server" ShowHeader="false" TextItemID="Reclamation.OrderLinesHelp" />
		<uc3:ReclamationOrderLines ID="ReclamationOrderLines1" runat="server" />
	</asp:View>
	<asp:View ID="_viewReclamationsInWork" runat="server">
		<div class="tab_menu">
			<div>
				<asp:LinkButton ID="lbRules3" runat="server" Text="<%$ Resources:Reclamations, Rules %>" CommandName="SwitchViewByID"
					CommandArgument="_viewAgreement"></asp:LinkButton></div>
			<div>
				<asp:LinkButton ID="lbRequests3" runat="server" Text="<%$ Resources:Reclamations, FormRequests %>" CommandName="SwitchViewByID"
					CommandArgument="_viewAgreement"></asp:LinkButton></div>
			<div class="on">
				<a href="#"><%= global::Resources.Reclamations.ReclamationInProcess %></a></div>
		</div>
		<uc3:ReclamationList ID="ReclamationList1" runat="server"></uc3:ReclamationList>
	</asp:View>
</asp:MultiView>