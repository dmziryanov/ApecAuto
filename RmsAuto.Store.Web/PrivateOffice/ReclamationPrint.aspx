<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReclamationPrint.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.ReclamationPrint" meta:resourcekey="PageResource1" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc" %>

<uc:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="StyleSheet" href="<%=ResolveUrl("~/css/style_print.css")%>" type="text/css" />
    <script src="<%= ResolveUrl("~/Scripts/jquery-1.11.1.min.js") %>" type="text/javascript"></script>
	<script src="<%= ResolveUrl("~/Scripts/common.js") %>" type="text/javascript"></script>
</head>
<body>
    <div class="win-bg">
	    <h1><asp:Literal ID="_HeaderText" runat="server" 
			    meta:resourcekey="_HeaderTextResource1"></asp:Literal></h1>
        <div class="information">
	        <table>
		        <tr>
			        <td></td>
			        <td><span class="blue"><asp:Literal ID="lCustomerData" runat="server" Text="Данные по клиенту" 
					        meta:resourcekey="lCustomerDataResource1" /></span></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lManager" runat="server" Text="Менеджер" 
					        meta:resourcekey="lManagerResource1" /></td>
			        <td><asp:Label ID="_ManagerName" runat="server" 
					        meta:resourcekey="_ManagerNameResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lClientName" runat="server" Text="Имя клиента" 
					        meta:resourcekey="lClientNameResource1" /></td>
			        <td><asp:Label ID="_ClientName" runat="server" 
					        meta:resourcekey="_ClientNameResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lClientID" runat="server" Text="Номер клиента" 
					        meta:resourcekey="lClientIDResource1" /></td>
			        <td><asp:Label ID="_ClientID" runat="server" 
					        meta:resourcekey="_ClientIDResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lContactPerson" runat="server" Text="Контактное лицо" 
					        meta:resourcekey="lContactPersonResource1" /></td>
			        <td><asp:Label ID="_ContactPerson" runat="server" 
					        meta:resourcekey="_ContactPersonResource1"></asp:Label></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lContactPhone" runat="server" Text="Контактный телефон" 
					        meta:resourcekey="lContactPhoneResource1" /></td>
			        <td><asp:Label ID="_ContactPhone" runat="server" 
					        meta:resourcekey="_ContactPhoneResource1"></asp:Label></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lReclamationDate" runat="server" Text="Дата подачи запроса" 
					        meta:resourcekey="lReclamationDateResource1" /></td>
			        <td><asp:Label ID="_ReclamationDate" runat="server" 
					        meta:resourcekey="_ReclamationDateResource1" /></td>
		        </tr>
		        <tr>
			        <td></td>
			        <td><span class="blue"><asp:Literal ID="lOrderData" runat="server" Text="Данные по заказу" 
					        meta:resourcekey="lOrderDataResource1" /></span></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lOrderID" runat="server" Text="Номер заказа" 
					        meta:resourcekey="lOrderIDResource1" /></td>
			        <td><asp:Label ID="_OrderID" runat="server" meta:resourcekey="_OrderIDResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lOrderDate" runat="server" Text="Дата заказа" 
					        meta:resourcekey="lOrderDateResource1" /></td>
			        <td><asp:Label ID="_OrderDate" runat="server" 
					        meta:resourcekey="_OrderDateResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lEstSupplyDate" runat="server" 
					        Text="Дата планируемой поставки" meta:resourcekey="lEstSupplyDateResource1" /></td>
			        <td><asp:Label ID="_EstSupplyDate" runat="server" 
					        meta:resourcekey="_EstSupplyDateResource1" /></td>
		        </tr>
		        <asp:PlaceHolder ID="phTorg12Number" runat="server" Visible="False">
			        <%--<tr>
				        <th><asp:Literal ID="lTorg12Number" runat="server" Text="Номер Торг-12" 
						        meta:resourcekey="lTorg12NumberResource1" /></th>
				        <td><asp:Label ID="_Torg12Number" runat="server" 
						        meta:resourcekey="_Torg12NumberResource1"></asp:Label></td>
			        </tr>--%>
			        <tr>
				        <td><asp:Literal ID="lInvoiceNumber" runat="server" Text="Номер счет-фактуры" 
						        meta:resourcekey="lInvoiceNumberResource1" /></td>
				        <td><asp:Label ID="_InvoiceNumber" runat="server" 
						        meta:resourcekey="_InvoiceNumberResource1"></asp:Label></td>
			        </tr>
		        </asp:PlaceHolder>
		        <tr>
			        <td><asp:Literal ID="_lSupplyDate" runat="server" 
					        meta:resourcekey="_lSupplyDateResource1"></asp:Literal></td>
			        <td><asp:Label ID="_SupplyDate" runat="server" 
					        meta:resourcekey="_SupplyDateResource1" /></td>
		        </tr>
		        <tr>
			        <td></td>
			        <td><span class="blue"><asp:Literal ID="lGoodsData" runat="server" Text="Данные по товару" 
					        meta:resourcekey="lGoodsDataResource1" /></span></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lBrand" runat="server" Text="Производитель" 
					        meta:resourcekey="lBrandResource1" /></td>
			        <td><asp:Label ID="_Manufacturer" runat="server" 
					        meta:resourcekey="_ManufacturerResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lPartNumber" runat="server" Text="Номер детали" 
					        meta:resourcekey="lPartNumberResource1" /></td>
			        <td><asp:Label ID="_PartNumber" runat="server" 
					        meta:resourcekey="_PartNumberResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lPartName" runat="server" Text="Наименование" 
					        meta:resourcekey="lPartNameResource1" /></td>
			        <td><asp:Label ID="_PartName" runat="server" 
					        meta:resourcekey="_PartNameResource1" /></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="_lQty" runat="server" meta:resourcekey="_lQtyResource1"></asp:Literal></td>
			        <td><asp:Label ID="_Qty" runat="server" meta:resourcekey="_QtyResource1"></asp:Label></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="lUnitPrice" runat="server" Text="Цена в заказе" 
					        meta:resourcekey="lUnitPriceResource1" /></td>
			        <td><asp:Label ID="_UnitPrice" runat="server" 
					        meta:resourcekey="_UnitPriceResource1" /></td>
		        </tr>
		        <asp:PlaceHolder ID="phUnitQty" runat="server" Visible="False">
			        <tr>
				        <td><asp:Literal ID="lUnitQty" runat="server" Text="Количество в заказе" 
						        meta:resourcekey="lUnitQtyResource1" /></td>
				        <td><asp:Label ID="_UnitQty" runat="server" meta:resourcekey="_UnitQtyResource1" /></td>
			        </tr>
		        </asp:PlaceHolder>
		        <asp:PlaceHolder ID="phTorg12Price" runat="server" Visible="False">
			        <tr>
				        <td><asp:Literal ID="lTorg12Price" runat="server" Text="Цена получения по Торг-12" 
						        meta:resourcekey="lTorg12PriceResource1" /></td>
				        <td><asp:Label ID="_Torg12Price" runat="server" 
						        meta:resourcekey="_Torg12PriceResource1"></asp:Label></td>
			        </tr>
		        </asp:PlaceHolder>
		        <tr>
			        <td></td>
			        <td><span class="blue"><asp:Literal ID="lReason" runat="server" Text="Причина" 
					        meta:resourcekey="lReasonResource1" /></span></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="_lReclamationReason" runat="server" 
					        meta:resourcekey="_lReclamationReasonResource1" /></td>
			        <td><asp:Label ID="_ReclamationReason" runat="server" 
					        meta:resourcekey="_ReclamationReasonResource1"></asp:Label></td>
		        </tr>
		        <tr>
			        <td><asp:Literal ID="_lReclamationDescription" runat="server" 
					        meta:resourcekey="_lReclamationDescriptionResource1" /></td>
			        <td><asp:Label ID="_ReclamationDescription" runat="server" 
					        meta:resourcekey="_ReclamationDescriptionResource1"></asp:Label></td>
		        </tr>
	        </table>
        </div>
        <div class="footer">
	        <a href="javascript:window.print()" class="button">Print</a>
	    </div>
    </div>
</body>
</html>
