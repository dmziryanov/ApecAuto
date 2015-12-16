<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientSearchList.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientSearchList" %>

<div style="height: 400px;">
	<asp:TextBox id="_txtClientName" runat="server" Width="194px" EnableViewState="true"></asp:TextBox>&nbsp 
	<asp:Button ID="_btnSearchClient" Class="btn btn-primary btn-sm" runat="server" Text="Find" onclick="_btnSearchClient_Click" />
	<br />
	<br />
	<asp:ListBox ID="_listSearchResults" runat="server" onselectedindexchanged="_listSearchResults_SelectedIndexChanged" 
		DataTextField="ClientName" AutoPostBack="True" EnableViewState="true" CausesValidation="True" 
		DataValueField="UserID" Height="80%" Width="259px">
	</asp:ListBox>
	
	<input runat="server" enableviewstate="true" id="hfUserID" type="hidden" value="" />
    <input runat="server" enableviewstate="true" id="hfClientName" type="hidden" value="" />
	
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</div>
