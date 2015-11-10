<%@ Control Language="C#" CodeBehind="Html_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.FieldTemplates.Text_EditField" %>

<asp:TextBox ID="_textBox" runat="server" Text='<%# FieldValueEditString %>' CssClass="droplist"></asp:TextBox><br /><asp:Button ID="_editButton" runat="server" Text="Редактировать" />

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="_textBox" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist" ControlToValidate="_textBox" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="_textBox" Display="Dynamic" />