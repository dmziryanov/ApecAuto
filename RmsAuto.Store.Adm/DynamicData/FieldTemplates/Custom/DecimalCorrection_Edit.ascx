<%@ Control Language="C#" CodeBehind="DecimalCorrection_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.FieldTemplates.DecimalCorrection_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" MaxLength="4" Columns="4" Text='<%# PercentEditString %>' CssClass="droplist"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RangeValidator runat="server" ID="RangeValidator1" CssClass="droplist" ControlToValidate="TextBox1" Type="Integer" MinimumValue="-100" MaximumValue="100" Display="Dynamic" 
    ErrorMessage="коэффициент должен быть целым числом от -100 до +100" />
<asp:RegularExpressionValidator runat="server" ID="RegexValidator1" ControlToValidate="TextBox1" 
 ValidationExpression="-?\d{1,3}" ErrorMessage="неверный формат коэффициента" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" />