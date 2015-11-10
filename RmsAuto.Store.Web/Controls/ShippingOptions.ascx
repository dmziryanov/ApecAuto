<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingOptions.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ShippingOptions" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>

<table>
    <tr>
        <td>
            <input type="radio" name='<% =Method %>' <% =(_Method == Pickup) ? "checked" : "" %> value='<% = Pickup %>' />&nbsp;<asp:Literal 
				ID="lPickup" runat="server" Text="Store pickup" meta:resourcekey="lPickupResource1" /> 
        </td>
        <td style="padding-left:10px;width:100%;">
                <asp:Literal ID="lOutPlace" runat="server" Text="Pickup point:" 
				    meta:resourcekey="lOutPlaceResource1" /> <% =StoreName.IfEmpty( global::Resources.Texts.NoData )%><br />
                <asp:Literal ID="lAddress" runat="server" Text="Address:" 
				    meta:resourcekey="lAddressResource1" /> <% =StoreAddress.IfEmpty( global::Resources.Texts.NoData )%><br />  
                <asp:Literal ID="lPhone" runat="server" Text="Phone number:" 
				    meta:resourcekey="lPhoneResource1" /> <% =StorePhone.IfEmpty( global::Resources.Texts.NoData )%>  
        </td>
    </tr>
    <asp:Repeater ID="_profileAddresses" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <input value='<%# Address + Container.ItemIndex.ToString() %>' type="radio" name='<% =Method %>'
                     
						<%# (_Method == Address + Container.ItemIndex.ToString()) ? "checked" : "" %> />&nbsp;<asp:Literal 
						ID="lProfileAddress" runat="server" Text="Courier delivery" 
						meta:resourcekey="lProfileAddressResource1" />
                </td>
                <td>                
                    <asp:Label ID="_lblAddress" runat="server" Text='<%# Container.DataItem %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr id="_newAddressRow" runat="server">
        <td>
            <input id="rbNewAddress" type="radio" name='<% =Method %>' value='<% =NewAddress %>'
                <% =(_Method == NewAddress) ? "checked" : "" %>  />&nbsp;<asp:Literal 
				ID="lNewAddress" runat="server" Text="Courier delivery" 
				meta:resourcekey="lNewAddressResource1" />
        </td>
        <td>
            <asp:TextBox ID="_addressEdit" runat="server" TextMode="MultiLine" 
				Width="228px" Height="50px" MaxLength="500"></asp:TextBox>
            <asp:CustomValidator ID="_addressValidator" runat="server" 
				ControlToValidate="_addressEdit" ClientValidationFunction="validateAddress" 
            ErrorMessage="не задан адрес доставки" ValidateEmptyText="True" 
				OnServerValidate="ValidateNewAddress" 
				meta:resourcekey="_addressValidatorResource1"></asp:CustomValidator>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function validateAddress(source, args) {
        var btnNew = document.getElementById("rbNewAddress");
        if (btnNew.checked) {
            var txtAddr = document.getElementById('<% =_addressEdit.ClientID %>');
            args.IsValid = txtAddr.value.length > 0;
        } else {
        args.IsValid = true;
        }
    }
</script>
