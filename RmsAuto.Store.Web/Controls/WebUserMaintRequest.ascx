<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserMaintRequest.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.WebUserMaintRequest" %>
<%@ Register src="ConfirmationMessage.ascx" tagname="ConfirmationMessage" tagprefix="uc1" %>
<%@ Register src="~/Controls/Captcha.ascx" tagname="Captcha" tagprefix="uc1" %>
<asp:Panel ID="_emailPanel" runat="server">
<h1><%=global::Resources.Texts.ChangePassword %></h1>
<div id="_errorMsg" runat="server" class="error" ></div>     

<div class="feedback">
    <ul>
        <li>
            <div class="title"><%=global::Resources.Texts.EnterEmail %>: *</div>
            <asp:TextBox ID="_txtEmail" runat="server" Width="228"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ControlToValidate="_txtEmail" ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidEmail %>" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                Display="Dynamic"></asp:RegularExpressionValidator> 
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyEmail %>" ControlToValidate="_txtEmail" 
                Display="Dynamic"></asp:RequiredFieldValidator>
        </li>
        <li>
            <div class="title"><%=global::Resources.Texts.Captcha %>: *</div>
            <uc1:captcha runat="server" id="_captcha" />
        </li>
    </ul>
</div> 
<div style="margin:10px 0 0 180px;">
    <asp:Button ID="_btnSubmit" CssClass="button" runat="server" Text="<%$Resources:Texts, Send %>" onclick="_btnSubmit_Click"/>
</div>
</asp:Panel>
<uc1:ConfirmationMessage ID="ConfirmationMessage1" runat="server" Visible="false" 
MessageText="<%$ Resources:Texts, ConfirmationMessage %>"  />

