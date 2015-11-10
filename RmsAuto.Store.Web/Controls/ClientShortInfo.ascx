<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientShortInfo.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ClientShortInfo" %>

<div class="member" runat="server" id="divMember" visible="false">
	<%--Your balance: <strong>1255.82</strong> USD <span class="small">(Date: 12.05.2014)</span><br>--%>
	<asp:Literal ID="lClientName" runat="server"></asp:Literal> <span class="small">(ClientID: <strong><asp:Literal ID="lClientID" runat="server"></asp:Literal></strong>)</span>
</div>
<!--end .member --> 