<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlaceOrderOptions.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.PlaceOrderOptions" %>
<%@ Register src="~/Controls/ShippingOptions.ascx" tagname="ShippingOptions" tagprefix="uc" %>
<div class="tab_text">
    <div class="t-hold">
        <table>
            <tr>
                <td class="nowrap">Delivery method</td>
                <td class="nowrap"><uc:ShippingOptions id="_shippingOptions" runat="server" /></td>
            </tr>
            <tr>
                <td class="nowrap">Payment method</td>
                <td><asp:DropDownList ID="_ddlPaymentOptions" runat="server"></asp:DropDownList></td>
            </tr>
        </table>
    </div>
</div>
