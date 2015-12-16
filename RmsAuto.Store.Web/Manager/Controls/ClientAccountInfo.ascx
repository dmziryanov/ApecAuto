<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientAccountInfo.ascx.cs"
	Inherits="RmsAuto.Store.Web.Manager.Controls.ClientAccountInfo" %>
<asp:Panel runat="server" ID="_panel">
	<h3>Web-site profile</h3>
	<asp:Label runat="server" ID="_errorLabel" CssClass="error" EnableViewState="false" />
    <div class="information">
	<table>
		<tr>
			<th>
				Status
			</th>
			<td>
				<asp:Literal runat="server" ID="_statusLabel" />
			</td>
		</tr>
		<tr runat="server" id="_loginRow">
			<th>
				Login
			</th>
			<td>
				<asp:Literal runat="server" ID="_loginLabel" />
			</td>
		</tr>
		<tr runat="server" id="_offline1Row">
			<th>
				E-mail
			</th>
			<td>
				<asp:TextBox runat="server" ID="_emailBox"></asp:TextBox>
			</td>
		</tr>
		<tr runat="server" id="_offline2Row">
			<th>
			</th>
			<td>
				<asp:LinkButton runat="server" ID="_createAccountButton"  Text="Send invitation" Class="btn btn-primary btn-sm" ImageUrl="~/images/mng_sendreg_btn.gif" 
					onclick="_createAccountButton_Click" />
			</td>
		</tr>
	</table>
        </div>
</asp:Panel>
