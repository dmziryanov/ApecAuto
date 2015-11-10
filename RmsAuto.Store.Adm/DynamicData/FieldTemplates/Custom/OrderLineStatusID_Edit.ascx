<%@ Control Language="C#" CodeBehind="OrderLineStatusID_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.OrderLineStatusID_Edit" %>

<asp:TextBox ID="TextBox1" runat="server" Text="<%# FieldValueEditString %>" Columns="40" CssClass="droplist"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
