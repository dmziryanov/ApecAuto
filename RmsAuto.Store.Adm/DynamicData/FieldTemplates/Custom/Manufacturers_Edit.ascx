﻿<%@ Control Language="C#" CodeBehind="Manufacturers_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.FieldTemplates.Custom.Manufacturers_EditField" %>

<asp:DropDownList ID="DropDownList1" runat="server" CssClass="droplist"></asp:DropDownList>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" />