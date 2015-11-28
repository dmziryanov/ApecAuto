<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartPrint.aspx.cs"
	Inherits="RmsAuto.Store.Web.PrivateOffice.CartPrint" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>

<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrManager" />


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title><%=global::Resources.Texts.Order %></title>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/style_print.css")%>" type="text/css"/>
</head>
<body>
    <div class="body_bgr"><img runat=server src="~/images/print_bgr.gif" /></div>
    <div class="body">
        <img runat=server src="<%$ Resources:ImagesURL, print_logo %>" align=left />
        <table class="tel">
            <tr>
                <td><span><%= Phone %></span>
                <td class="img"><img id="Img1" runat="server" src="~/images/tel_img2.gif" alt="" width="14" height="20" border="0" /></td>
            </tr>
        </table>
	    <div class="client">
		    <%=global::Resources.Texts.Client %>:<br />
		    <b><asp:Literal runat="server" ID="_clientNameLabel" /></b><br />
		    <span><%= DateTime.Now %></span>
	    </div>
	    <br style="clear:both;" />
	<div class="cart">
	    <asp:PlaceHolder runat="server" ID="_customOrderNumPlaceHolder">
			<div><%=global::Resources.Texts.CustomOrderNumber %>: <b><asp:Literal runat="server" ID="_customOrderNumLiteral" /></b></div>
	    </asp:PlaceHolder>
		<div><%=global::Resources.Texts.AllParts %>
			<b><asp:Literal runat="server" ID="_totalCountLiteral" /></b> <% =global::Resources.Texts.OnSumm %> 
			<b><asp:Literal runat="server" ID="_totalSumLiteral" /></b> <% =global::Resources.Texts.RoubleShort %></div>
		<asp:Repeater ID="_cartItemsRepeater" runat="server">
			<HeaderTemplate>
                <div class="tab-text">
				<table>
					<tr>
						<th>
							<%=global::Resources.Texts.Manufacturer %>
						</th>
						<th>
							<%=global::Resources.Texts.PartNumber %>
						</th>
						<th>
							<%=global::Resources.Texts.PartName %>
						</th>
						<th class="nowrap">
							<%=global::Resources.Texts.DeliveryPeriod %>
						</th>
						<th class="nowrap">
							<%=global::Resources.Texts.Price %>&nbsp;<%=global::Resources.Texts.DollarWithBrackets %>
						</th>
						<th class="nowrap">
							<%=global::Resources.Texts.QtyLC %>
						</th>
						<th class="nowrap">
							<%=global::Resources.Texts.Summ %>&nbsp;<%=global::Resources.Texts.DollarWithBrackets %>
						</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
                    <td>
                        <%#Server.HtmlEncode((string)Eval("Manufacturer"))%>
                    </td>
					<td id="_td1" runat="server" visible='<%#ShowPartNumbers%>'>
						<%#Server.HtmlEncode((string)Eval("PartNumber"))%>
					</td>
					<td>
						<%#Server.HtmlEncode((string)Eval("PartName"))%>
					</td>
					<td class="center_align">
						<%# Eval("DisplayDeliveryDaysMin")%><%# Eval("DisplayDeliveryDaysMax")!=null?"-"+Eval("DisplayDeliveryDaysMax"):""%>
					</td>
					<td class="price">
						<%# Eval("UnitPrice", "{0:### ### ##0.00}")%>
					</td>
					<td class="center_align">
						<%# Eval("Qty") %>
					</td>
					<td class="price">
						<%# Eval("ItemTotal", "{0:### ### ##0.00}")%>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				<tr>
					<td class="itog">
						<%=global::Resources.Texts.Total %>:
					</td>
					<td class="itog">
						&nbsp;
					</td>
					<td class="itog">
						&nbsp;
					</td>
					<td class="itog">
						&nbsp;
					</td>
					<td class="itog">
						&nbsp;
					</td>
					<td class="itog center_align">
						<%#_totalCountLiteral.Text%>
					</td>
					<td class="itog price nowrap">
						<%#_totalSumLiteral.Text%>
					</td>
				</tr>
				</table>
                </div>
			</FooterTemplate>
		</asp:Repeater>
	</div>
	<div class="footer">
	    <a href="javascript:window.print()" class="button">Print</a>
	</div>
</div>	
</body>
</html>
