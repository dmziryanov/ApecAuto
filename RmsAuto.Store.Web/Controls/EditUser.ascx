<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUser.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.EditUser" %>

<div class="information">
    <table>
        <tr>
            <th><asp:Literal ID="lLogin" runat="server" Text="Логин" meta:resourcekey="lLoginResource1" />: *</th>
            <td>
                <asp:TextBox ID="_txtUsername" runat="server" 
				    meta:resourcekey="_txtUsernameResource1"></asp:TextBox>
                <span id="Span1" class="info" title="<%$ Resources:Hints, loginHint %>" runat="server"></span>
                <asp:RequiredFieldValidator ID="_usernameRequiredValidator" runat="server" ControlToValidate="_txtUsername"
                 Display="Dynamic" ErrorMessage="Не задан логин" 
				        meta:resourcekey="_usernameRequiredValidatorResource1"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="_txtUsername" ErrorMessage="логин содержит запрещенные символы" 
                    ValidationExpression="\w{0,20}" 
                    Display="Dynamic" meta:resourcekey="RegularExpressionValidator2Resource1"></asp:RegularExpressionValidator>
                <asp:CustomValidator
                        ID="_usernameCustomValidator" runat="server" 
                        ControlToValidate="_txtUsername" 
                        ErrorMessage="Логин уже используется другим пользователем" 
                        onservervalidate="ValidateLogin" 
				        meta:resourcekey="_usernameCustomValidatorResource1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><asp:Literal ID="lPassword" runat="server" Text="Пароль" 
				meta:resourcekey="lPasswordResource1" />: *</th>
            <td>
                <asp:TextBox ID="_txtPassword1" runat="server" 
				    TextMode="Password" meta:resourcekey="_txtPassword1Resource1"></asp:TextBox>
                <span id="Span2" class="info" title="<%$ Resources:Hints, passwordHint %>" runat="server"></span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                    ControlToValidate="_txtPassword1" ErrorMessage="<%$ Resources:Exceptions, PasswordLengthIsInvalid %>" 
                    ValidationExpression=".{1,10}" 
                    Display="Dynamic" meta:resourcekey="RegularExpressionValidator3Resource1"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_txtPassword1" 
                                ErrorMessage="Не задан пароль" 
				        meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><asp:Literal ID="lRePassword" runat="server" Text="Повтор пароля" 
				meta:resourcekey="lRePasswordResource1" />: *</th>
            <td>
                <asp:TextBox ID="_txtPassword2" runat="server" 
				    TextMode="Password" meta:resourcekey="_txtPassword2Resource1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="_txtPassword2" 
                                ErrorMessage="Не задан пароль" Display="Dynamic" 
				        meta:resourcekey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"  ControlToValidate="_txtPassword2" 
                                ControlToCompare="_txtPassword1"
                                ErrorMessage="Введенные пароли не совпадают" 
				        meta:resourcekey="CompareValidator1Resource1"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>* <asp:Literal ID="lReqFields" runat="server" Text="обязательные поля для заполнения" meta:resourcekey="lReqFieldsResource1" /></td>
        </tr>
    </table>
</div>