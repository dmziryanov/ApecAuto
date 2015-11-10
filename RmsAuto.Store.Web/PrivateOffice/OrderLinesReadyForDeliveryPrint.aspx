<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderLinesReadyForDeliveryPrint.aspx.cs"
	Inherits="RmsAuto.Store.Web.PrivateOffice.OrderLinesReadyForDeliveryPrint" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>

<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title><asp:Literal ID="lTitle" runat="server" Text="Готовые к выдаче позиции"
				meta:resourcekey="lTitleResource1" /></title>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_print.css")%>" type="text/css" />
</head>
<body>
	<div class="body_bgr">
		<img id="Img1" runat="server" src="~/images/print_bgr.gif" /></div>
	<div class="body">
		<img id="Img2" runat="server" src="<%$ Resources:ImagesURL, print_logo %>" align="left" />
		<div runat="server" id="_managerBlock" class="client">
			<%--<asp:Literal ID="lMenager" runat="server" Text="Персональный менеджер:" 
				meta:resourcekey="lMenagerResource1" /> <asp:Literal runat="server" 
				ID="_managerNameLabel" /><br />
			<asp:Literal ID="lPhoneManager" runat="server" Text="Телефон менеджера:" 
				meta:resourcekey="lPhoneManagerResource1" /> <asp:Literal runat="server" 
				ID="_managerPhoneLable" />--%>
			<asp:Literal ID="lOfficePhone" runat="server" Text="Office phone"
				meta:resourcekey="lOfficePhoneResource1" />&nbsp;
			<asp:Literal ID="lOfficePhoneValue" runat="server" />
		</div>
		<br style="clear: both;" />
		<div class="cart">
			<h2><asp:Literal ID="lAllready" runat="server" Text="Готовые к выдаче позиции" 
					meta:resourcekey="lAllreadyResource1" /></h2>
			<asp:Repeater ID="_itemsRepeater" runat="server">
				<HeaderTemplate>
					<table cellpadding="0" cellspacing="0" class="list" width="97%">
						<tr>
							<th>Order No</th>
							<th>Brand</th>
							<th>Part No</th>
							<th>Description</th>
							<th>Price, USD</th>
							<th>Qnt</th>
							<th>Total, USD</th>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<%#GetOrderDisplayNumber( (RmsAuto.Store.Entities.Order)Eval( "Order" ) )%>
						</td>
						<td>
							<%#Server.HtmlEncode((string)Eval("Manufacturer"))%>
						</td>
						<td>
							<%#Server.HtmlEncode((string)Eval("PartNumber"))%>
						</td>
						<td>
							<%#Server.HtmlEncode((string)Eval("PartName"))%>
						</td>
						<td class="price">
							<%#Eval("UnitPrice", "{0:### ### ##0.00}")%>
						</td>
						<td>
							<%# Eval("Qty") %>
						</td>
						<td class="price">
							<%# Eval( "Total", "{0:### ### ##0.00}" )%><br />
						</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
				</FooterTemplate>
			</asp:Repeater>
			<asp:Literal ID="lTotalParts" runat="server" Text="Итого позиций к выдаче:" 
				meta:resourcekey="lTotalPartsResource1" /> <asp:Literal runat="server" 
				ID="_linesCountLabel" /><br />
			<asp:Literal ID="lTotalSumm" runat="server" Text="Итого сумма:" 
				meta:resourcekey="lTotalSummResource1" /> <asp:Literal runat="server" 
				ID="_totalSumLabel" /> 
			<asp:Literal ID="lRub" runat="server" Text="руб." 
				meta:resourcekey="lRubResource1" />
			
		</div>
		<br />
		<%--<asp:PlaceHolder runat="server" ID="_mapPlaceHolder">
		<asp:Literal ID="lMap" runat="server" Text="Схема проезда:" 
				meta:resourcekey="lMapResource1" /> <br />
		<asp:Image runat="server" ID="_storeMapImage"  align="left" 
				/>
		</asp:PlaceHolder>--%>
		<%--<div runat="server" id="_storeBlock" class="store">
			<asp:Literal ID="lStoreName" runat="server" Text="Пункт выдачи:" 
				meta:resourcekey="lStoreNameResource1" /> <asp:Literal runat="server" 
				ID="_storeNameLabel" /><br />
			<asp:Literal ID="lStoreAddress" runat="server" Text="Адрес:" 
				meta:resourcekey="lStoreAddressResource1" /> <asp:Literal runat="server" 
				ID="_storeAddressLabel" /><br />
			<asp:Literal ID="lStorePhone" runat="server" Text="Телефоны:" 
				meta:resourcekey="lStorePhoneResource1" /> <asp:Literal runat="server" 
				ID="_storePhonesLabel" /><br />
		</div>--%>
		
		<div class="footer">
			<a href="javascript:window.print()">
				<img id="Img3" runat="server" src="<%$ Resources:ImagesURL, print_btn %>" /></a>
		</div>
	</div>
</body>
</html>
