<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Address_Edit.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.Address_Edit" %>
<table cellpadding="0" cellspacing="0" class="info" width="100%">
    <tr>
        <th><asp:Literal ID="lPostalCode" runat="server" Text="Почтовый индекс" 
				meta:resourcekey="lPostalCodeResource1" /> <font color=red>*</font></th>
        <td><asp:TextBox ID="_txtPostalCode" runat="server" MaxLength="6" Width="140px" 
				meta:resourcekey="_txtPostalCodeResource1"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_postalCodeReqValidator" runat="server" ControlToValidate="_txtPostalCode"
        Display="Dynamic" ErrorMessage="не указан почтовый индекс" 
				meta:resourcekey="_postalCodeReqValidatorResource1"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="_postalCodeRegexValidator" runat="server" ControlToValidate="_txtPostalCode"
        Display="Dynamic" ErrorMessage="неверный формат почтового индекса" 
				ValidationExpression="\d{6}" 
				meta:resourcekey="_postalCodeRegexValidatorResource1"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th><asp:Literal ID="lCity" runat="server" Text="Город" 
				meta:resourcekey="lCityResource1" /> <font color=red>*</font></th>
        <td><asp:TextBox ID="_txtCity" runat="server" MaxLength="50" Width="140px" 
				meta:resourcekey="_txtCityResource1"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtCity"
        Display="Dynamic" ErrorMessage="не указан город" 
				meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="_txtCity"
        Display="Dynamic" ErrorMessage="название города некорректно" 
				ValidationExpression="[а-яА-Яa-zA-Z0-9]{1,50}" 
				meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>
</td>
    </tr>
    <tr>
        <th><asp:Literal ID="lRegion" runat="server" Text="Регион" 
				meta:resourcekey="lRegionResource1" /></th>
        <td><asp:TextBox ID="_txtRegion" runat="server" MaxLength="256" Width="420px" 
				meta:resourcekey="_txtRegionResource1"></asp:TextBox></td>
    </tr>
    <tr valign="top">
        <th><asp:Literal ID="lAdress" runat="server" Text="Адрес" 
				meta:resourcekey="lAdressResource1" /> <font color=red>*</font></th>
        <td><asp:TextBox ID="_txtAddress" runat="server" MaxLength="500" TextMode="MultiLine" 
        Columns="50" Rows="10" meta:resourcekey="_txtAddressResource1"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_txtAddress"
        Display="Dynamic" ErrorMessage="не указан адрес" 
				meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
