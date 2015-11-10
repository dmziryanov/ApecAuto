<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReclamationRequest.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ReclamationRequest"  EnableViewState="true"%>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc1" %>

<script type="text/javascript">
	function CheckMultilineInput(txt, length) {
		var maxlength = new Number(length);
		if (txt.value.length > maxlength) {
			txt.value = txt.value.substring(0, maxlength - 1);
			alert("В описании разрешено только " + maxlength + " символов!");
		}
	}
</script>

<uc1:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrManager" />

<h1><asp:Literal ID="_HeaderText" runat="server" 
		meta:resourcekey="_HeaderTextResource1"></asp:Literal></h1>
    <div class="information">
        <table>
	        <tr>
		        <th></th>
		        <td><span class="blue"><asp:Literal ID="lClentData" runat="server" Text="Данные по клиенту" 
				        meta:resourcekey="lClentDataResource1" /></span></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lManager" runat="server" Text="Менеджер" 
				        meta:resourcekey="lManagerResource1" /></th>
		        <td><asp:Label ID="_ManagerName" runat="server" 
				        meta:resourcekey="_ManagerNameResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lClientName" runat="server" Text="Имя клиента" 
				        meta:resourcekey="lClientNameResource1" /></th>
		        <td><asp:Label ID="_ClientName" runat="server" 
				        meta:resourcekey="_ClientNameResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lClientID" runat="server" Text="Номер клиента" 
				        meta:resourcekey="lClientIDResource1" /></th>
		        <td><asp:Label ID="_ClientID" runat="server" 
				        meta:resourcekey="_ClientIDResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lContactPerson" runat="server" Text="Контактное лицо" 
				        meta:resourcekey="lContactPersonResource1" /></th>
		        <td>
			        <asp:TextBox ID="txtContactPerson" runat="server" Width="300px" MaxLength="100" 
				        meta:resourcekey="txtContactPersonResource1"></asp:TextBox>
			        <asp:RequiredFieldValidator ID="rfvContactPerson" runat="server" ControlToValidate="txtContactPerson"
				        Display="Dynamic" ErrorMessage="Поле является обязательным к заполнению" 
				        meta:resourcekey="rfvContactPersonResource1"></asp:RequiredFieldValidator>
		        </td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lContactPhone" runat="server" Text="Контактный телефон" 
				        meta:resourcekey="lContactPhoneResource1" /></th>
		        <td>
			        <asp:TextBox ID="txtContactPhone" runat="server" Width="150px" MaxLength="50" 
				        meta:resourcekey="txtContactPhoneResource1"></asp:TextBox>
			        <asp:RequiredFieldValidator ID="rfvContactPhone" runat="server" ControlToValidate="txtContactPhone"
				        Display="Dynamic" ErrorMessage="Поле является обязательным к заполнению" 
				        meta:resourcekey="rfvContactPhoneResource1"></asp:RequiredFieldValidator>
		        </td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lReclamationDate" runat="server" Text="Дата подачи запроса" 
				        meta:resourcekey="lReclamationDateResource1" /></th>
		        <td><asp:Label ID="_ReclamationDate" runat="server" 
				        meta:resourcekey="_ReclamationDateResource1" /></td>
	        </tr>
	        <tr>
		        <th></th>
		        <td><span class="blue"><asp:Literal ID="lOrderData" runat="server" Text="Данные по заказу" 
				        meta:resourcekey="lOrderDataResource1" /></span></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lOrderNumber" runat="server" Text="Номер заказа" 
				        meta:resourcekey="lOrderNumberResource1" /></th>
		        <td><asp:Label ID="_OrderID" runat="server" meta:resourcekey="_OrderIDResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lOrderDate" runat="server" Text="Дата заказа" 
				        meta:resourcekey="lOrderDateResource1" /></th>
		        <td><asp:Label ID="_OrderDate" runat="server" 
				        meta:resourcekey="_OrderDateResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lEstSupplyDate" runat="server" 
				        Text="Дата планируемой поставки" meta:resourcekey="lEstSupplyDateResource1" /></th>
		        <td><asp:Label ID="_EstSupplyDate" runat="server" 
				        meta:resourcekey="_EstSupplyDateResource1" /></td>
	        </tr>
	        <asp:PlaceHolder ID="phTorg12Number" runat="server" Visible="False">
		        <%--<tr>
			        <th><asp:Literal ID="lTorg12Number" runat="server" Text="Номер Торг-12" 
					        meta:resourcekey="lTorg12NumberResource1" /></th>
			        <td>
				        <asp:TextBox ID="txtTorg12Number" runat="server" Width="150" MaxLength="50" 
					        meta:resourcekey="txtTorg12NumberResource1"></asp:TextBox>
				        <asp:RequiredFieldValidator ID="rfvTorg12Number" runat="server" ControlToValidate="txtTorg12Number"
					        Display="Dynamic" ErrorMessage="Поле является обязательным к заполнению" 
					        meta:resourcekey="rfvTorg12NumberResource1"></asp:RequiredFieldValidator>
			        </td>
		        </tr>--%>
		        <tr>
			        <th><asp:Literal ID="lInvoiceNumber" runat="server" Text="Номер счет-фактуры" 
					        meta:resourcekey="lInvoiceNumberResource1" /></th>
			        <td>
				        <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="150" MaxLength="50" 
					        meta:resourcekey="txtInvoiceNumberResource1"></asp:TextBox>
			        </td>
		        </tr>
	        </asp:PlaceHolder>
	        <tr>
		        <th><asp:Literal ID="_lSupplyDate" runat="server" 
				        meta:resourcekey="_lSupplyDateResource1"></asp:Literal></th>
		        <td><asp:Label ID="_SupplyDate" runat="server" 
				        meta:resourcekey="_SupplyDateResource1" /></td>
	        </tr>
	        <tr>
		        <th></th>
		        <td><span class="blue"><asp:Literal ID="lGoodsData" runat="server" Text="Данные по товару" 
				        meta:resourcekey="lGoodsDataResource1" /></span></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lBrand" runat="server" Text="Производитель" 
				        meta:resourcekey="lBrandResource1" /></th>
		        <td><asp:Label ID="_Manufacturer" runat="server" 
				        meta:resourcekey="_ManufacturerResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lPartNumber" runat="server" Text="Номер детали" 
				        meta:resourcekey="lPartNumberResource1" /></th>
		        <td><asp:Label ID="_PartNumber" runat="server" 
				        meta:resourcekey="_PartNumberResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lPartName" runat="server" Text="Наименование" 
				        meta:resourcekey="lPartNameResource1" /></th>
		        <td><asp:Label ID="_PartName" runat="server" 
				        meta:resourcekey="_PartNameResource1" /></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="_Qty" runat="server" meta:resourcekey="_QtyResource1"></asp:Literal></th>
		        <td>
			        <asp:TextBox ID="txtQty" runat="server" Width="50px" 
				        meta:resourcekey="txtQtyResource1"></asp:TextBox>
			        <asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txtQty"
				        Display="Dynamic" ErrorMessage="Поле является обязательным к заполнению" 
				        meta:resourcekey="rfvQtyResource1"></asp:RequiredFieldValidator>
			        <asp:CustomValidator ID="cvQty" runat="server" ControlToValidate="txtQty"
				        Display="Dynamic" ErrorMessage="Количество некорректно" OnServerValidate="ValidateQty" 
				        meta:resourcekey="cvQtyResource1"></asp:CustomValidator>
		        </td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="lUnitPrice" runat="server" Text="Цена в заказе" 
				        meta:resourcekey="lUnitPriceResource1" /></th>
		        <td><asp:Label ID="_UnitPrice" runat="server" 
				        meta:resourcekey="_UnitPriceResource1" /></td>
	        </tr>
	        <asp:PlaceHolder ID="phUnitQty" runat="server" Visible="False">
		        <tr>
			        <th><asp:Literal ID="lUnitQty" runat="server" Text="Количество в заказе" 
					        meta:resourcekey="lUnitQtyResource1" /></th>
			        <td><asp:Label ID="_UnitQty" runat="server" meta:resourcekey="_UnitQtyResource1" /></td>
		        </tr>
	        </asp:PlaceHolder>
	        <asp:PlaceHolder ID="phTorg12Price" runat="server" Visible="False">
		        <tr>
			        <th><asp:Literal ID="lTorg12Price" runat="server" Text="Цена получения по Торг-12" 
					        meta:resourcekey="lTorg12PriceResource1" /></th>
			        <td>
				        <asp:TextBox ID="txtTorg12Price" runat="server" Width="50" 
					        meta:resourcekey="txtTorg12PriceResource1"></asp:TextBox>
				        <asp:RequiredFieldValidator ID="rfvTorg12Price" runat="server" ControlToValidate="txtTorg12Price"
					        Display="Dynamic" ErrorMessage="Поле является обязательным к заполнению" 
					        meta:resourcekey="rfvTorg12PriceResource1"></asp:RequiredFieldValidator>
				        <asp:CustomValidator ID="cvTorg12Price" runat="server" ControlToValidate="txtTorg12Price"
					        Display="Dynamic" ErrorMessage="Цена некорректна" OnServerValidate="ValidateTorg12Price" 
					        meta:resourcekey="cvTorg12PriceResource1"></asp:CustomValidator>
			        </td>
		        </tr>
	        </asp:PlaceHolder>
	        <tr>
		        <th></th>
		        <td><span class="blue"><asp:Literal ID="lReason" runat="server" Text="Причина" 
				        meta:resourcekey="lReasonResource1" /></span></td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="_ReclamationReason" runat="server" 
				        meta:resourcekey="_ReclamationReasonResource1" /></th>
		        <td>
			        <asp:DropDownList ID="ddlReclamationReason" runat="server" Width="400px" 
				        meta:resourcekey="ddlReclamationReasonResource1">
				        <asp:ListItem Text="< Выбрать >" Value="noselected" 
					        meta:resourcekey="ListItemResource1"></asp:ListItem>
				        <asp:ListItem Text="несоответствие по качеству" 
					        meta:resourcekey="ListItemResource2"></asp:ListItem>
				        <asp:ListItem Text="невыполнение срока поставки" 
					        meta:resourcekey="ListItemResource3"></asp:ListItem>
				        <asp:ListItem Text="несоответствие по количеству" 
					        meta:resourcekey="ListItemResource4"></asp:ListItem>
				        <asp:ListItem Text="неверное вложение/пересорт" 
					        meta:resourcekey="ListItemResource5"></asp:ListItem>
				        <asp:ListItem Text="недопустимая замена номера" 
					        meta:resourcekey="ListItemResource6"></asp:ListItem>
				        <asp:ListItem Text="недопустимое увеличение цены" 
					        meta:resourcekey="ListItemResource7"></asp:ListItem>
				        <asp:ListItem Text="ошибка покупателя при заказе" 
					        meta:resourcekey="ListItemResource8"></asp:ListItem>
				        <asp:ListItem Text="несоответствие номера детали заказу покупателя" 
					        meta:resourcekey="ListItemResource9"></asp:ListItem>
			        </asp:DropDownList>
			        <asp:CustomValidator ID="cvReclamationReason" runat="server" ControlToValidate="ddlReclamationReason"
				        Display="Dynamic" ErrorMessage="Необходимо выбрать причину" 
				        OnServerValidate="ValidateReclamationReason" 
				        meta:resourcekey="cvReclamationReasonResource1"></asp:CustomValidator>
		        </td>
	        </tr>
	        <tr>
		        <th><asp:Literal ID="_ReclamationDescription" runat="server" 
				        meta:resourcekey="_ReclamationDescriptionResource1" /></th>
		        <td>
			        <asp:TextBox ID="txtReclamationDescription" runat="server" TextMode="MultiLine" 
				        Rows="5" MaxLength="300" Width="400px"
				        onkeyup="javascript:CheckMultilineInput(this, 300);" 
				        onchange="javascript:CheckMultilineInput(this, 300);" 
				        meta:resourcekey="txtReclamationDescriptionResource1"></asp:TextBox>
			        <asp:RequiredFieldValidator ID="rfvReclamationDescription" runat="server" ControlToValidate="txtReclamationDescription"
				        Display="Dynamic" ErrorMessage="Поле является обязательным к заполнению" 
				        meta:resourcekey="rfvReclamationDescriptionResource1"></asp:RequiredFieldValidator>
		        </td>
	        </tr>
	        <tr>
		        <th></th>
		        <td><asp:Button ID="btnSendRequest" runat="server" CssClass="button" Text="Отправить заявку" 
				        OnClick="btnSendRequest_Click" meta:resourcekey="btnSendRequestResource1" /></td>
	        </tr>
        </table>
    </div>

<asp:HiddenField ID="hfReclamation" runat="server" />
<asp:HiddenField ID="hfOrderLine" runat="server" />
<asp:HiddenField ID="hfReclamationType" runat="server" />