<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logon.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.Logon" %>
<div class="feedback" onkeypress="javascript: if(event.keyCode==13) { <%=Page.GetPostBackClientEvent(_btnLogin, String.Empty)%>; return false; }">
    <ul>
        <li>
            <div class="title"><asp:Literal ID="lLogin" runat="server" Text="<%$ Resources:Texts, Login %>" />:</div>
            <asp:TextBox ID="_txtLogin" runat="server" Width="250" MaxLength="20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*"
            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyLogin %>" ControlToValidate="_txtLogin"></asp:RequiredFieldValidator>
        </li>
        <li>
            <div class="title"><asp:Literal ID="lPassword" runat="server" Text="<%$ Resources:Texts, Password %>" />:</div>
            <asp:TextBox ID="_txtPassword" runat="server" TextMode="Password" Width="250" MaxLength="10"></asp:TextBox>
            <asp:RequiredFieldValidator ID="_reqPasswordValidator" runat="server" Text="*"
            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyPassword %>" ControlToValidate="_txtPassword"></asp:RequiredFieldValidator>

        </li>
        <li>
            <div class="title"></div>
            <asp:CheckBox ID="_chkPersist" runat="server" Text="<%$ Resources:Texts, RememberMe %>" Visible="false" />
        </li>
    </ul>
    <div id="_errorMessage" runat="server" style="font-weight: bold; color: #FF0000" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <div style="margin:10px 0 0 180px;">
        <asp:Button ID="_btnLogin" CssClass="button" runat="server" Text="<%$ Resources:Texts, SystemEntrance %>" onclick="_btnLogin_Click" />
        <br /><br />
        <p><asp:HyperLink runat="server" ID="_registerLink"><asp:Literal ID="lRegistration" runat="server" Text="<%$ Resources:Texts, Registration %>" /></asp:HyperLink>&nbsp;&nbsp;<asp:HyperLink runat="server" ID="_recoverPasswordLink"><asp:Literal ID="lChangePassword" runat="server" Text="<%$ Resources:Texts, ForgotChangePassword %>" /></asp:HyperLink></p>
    </div>
</div>
