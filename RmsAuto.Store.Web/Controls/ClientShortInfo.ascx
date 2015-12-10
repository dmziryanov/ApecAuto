<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientShortInfo.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ClientShortInfo" %>

<div class="member" runat="server" id="divMember" visible="false">
	<%--Your balance: <strong>1255.82</strong> USD <span class="small">(Date: 12.05.2014)</span><br>--%>
	<div class="block cart">
    <div class="lefttitle">><%= global::Resources.Texts.IdentityText %></div>
    <asp:Literal ID="lClientName" runat="server"></asp:Literal> <span class="small"><strong><asp:Literal ID="lClientID" runat="server"></asp:Literal></strong>)</span>
        </div>
</div>
<!--end .member --> 