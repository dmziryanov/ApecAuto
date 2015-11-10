<%@ Control Language="C#" CodeBehind="Password_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.FieldTemplates.Password_EditField" %>

<asp:TextBox ID="_passwordBox" runat="server" TextMode="Password" CssClass="droplist"></asp:TextBox>
<br />
<asp:CheckBox runat="server" ID="_keepPasswordBox" Text="Не изменять пароль" />

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="_passwordBox" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist" ControlToValidate="_passwordBox" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="_passwordBox" Display="Dynamic" />