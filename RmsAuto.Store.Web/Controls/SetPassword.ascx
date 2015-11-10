<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetPassword.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.SetPassword" %>

<div class="feedback">
    <ul>
        <li>
            <div class="title"><asp:Literal ID="lEnterPassword" runat="server" Text="<%$ Resources:Texts, EnterPassword %>" /> *</div>
            <asp:TextBox ID="_txtPassword1" runat="server" TextMode="Password" MaxLength="10"></asp:TextBox>
            <span class="info" title="<%$ Resources:Texts, HelpPassword %>" runat="server"></span>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                ControlToValidate="_txtPassword1" ErrorMessage="<%$ Resources:ValidatorsMessages, ExceedLengthPassword %>" 
                ValidationExpression=".{1,10}" 
                Display="Dynamic"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_txtPassword1" 
                            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyPassword %>"></asp:RequiredFieldValidator>
        </li>
        <li>
            <div class="title"><asp:Literal ID="lReEnterPassword" runat="server" Text="<%$ Resources:Texts, ReEnterPassword %>" /> *</div>
            <asp:TextBox ID="_txtPassword2" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="_txtPassword2" 
                        ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyPassword %>" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server"  ControlToValidate="_txtPassword2" 
                        ControlToCompare="_txtPassword1"
                        ErrorMessage="<%$ Resources:ValidatorsMessages, NotMatchPassword %>"></asp:CompareValidator>
        </li>
    </ul>
</div>

