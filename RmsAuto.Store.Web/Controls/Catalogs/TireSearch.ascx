<%@ Control  Language="C#" AutoEventWireup="true" CodeBehind="TireSearch.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.TireSearch" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.TechDoc.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Data" %>
<%@ Import Namespace="RmsAuto.Store.Web.Controls" %>
<%@ Register src="~/Controls/RMSPagerControl.ascx" tagname="RMSPagerControl" tagprefix="uc1" %>

<script type="text/javascript">

	function validate_qty(qtyBoxId) {
		var val = $.trim($('#' + qtyBoxId).val());
		$('#' + qtyBoxId).val(val);

		if (val == '') {
			alert('<%= global::Resources.Texts.AlertQty %>');
			return false;
		}
		else if (!val.match(/^\d+$/)) {
		alert('<%= global::Resources.Texts.AlertCorrectQty %>');
			return false;
		}
		return true;
}

</script>
<div>
<asp:PlaceHolder runat="server" ID="_resultsPlaceHolder">

<asp:Repeater ID="rptSearchResults" runat="server" EnableViewState = "false" >
<ItemTemplate>
        
<div class="card" style="float:left">
        <div style="float:left"><asp:Label id="_lblKey" runat="server" Visible="false"></asp:Label></div>
<table border="0">
<tbody>
<tr>
<td rowspan="2" style="vertical-align:middle">
    <div><a class="highslide" href='<%# UrlManager.GetFileUrl(Convert.ToInt32(Eval("ImageUrl"))) + "?r=rms"%>' onclick="return hs.expand(this)"><img class="bordgray" src='<%# UrlManager.GetThumbnailUrl(Convert.ToInt32(Eval("ImageUrl")), "tire", "rms")%>' alt="" width="116" height="170" /></a></div>
</td>
<td class="brand_name" colspan="3"><span><%#Server.HtmlEncode(Convert.ToString(Eval("Manufacturer")))%></span><br />
<asp:Label runat="server" ID="ModelName" ToolTip='<%#Server.HtmlEncode(Convert.ToString(Eval("ModelName")))%>'><h3><%#Server.HtmlEncode(Convert.ToString(Eval("ModelName")))%></h3></asp:Label></td>
</tr>
<tr >
<td class="cont">
    <span>ширина:</span><br />
    <span>профиль:</span><br />
    <span>радиус:</span><br />
    <span>индекс скорости:</span><br />
    <span>сезонность:</span>
   <%-- <div>срок поставки:</div>--%>
</td>
<td class="cont2" colspan="2">
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Profile")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Height")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Radius")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("TireIndex")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Season")))%></span><br />
    <%--<div><%# ((TireWithPrice)Container.DataItem).SparePart.DisplayDeliveryDaysMin %>-<%# ((TireWithPrice)Container.DataItem).SparePart.DisplayDeliveryDaysMax%></div>--%>
</td>
</tr>
<tr class="pricerow">
    <td>
        <div class="article"><%#Server.HtmlEncode(Convert.ToString(Eval("TireNumber")))%></div>
    </td>
    <td  class="pricerow" colspan="3">
         <a title="Посмотреть конкретные предложения поставщиков" href='<%# Eval("Ref") as string %>' class="Buy">Цены</a>
<%--        <div style="float:left; margin-top: 2px;">
            цена: <span class="price"><%#Server.HtmlEncode(Convert.ToString(Eval("Price")))%> р.</span>
        </div> 
        <asp:Label ID="_lblDefaultOrderQty" runat="server" Visible="false" Text='<%# ((TireWithPrice)Container.DataItem ).SparePart.DefaultOrderQty %>' />--%>
    </td>
       
    <%--<td class="pricerow">
   
    <div style="float:right; vertical-align:middle" onkeypress="if( event.keyCode==13 ) { $('input[name$=_btnAddToCart]',this.parentElement).click(); return false; }">
    
    <table>
        <tr>
           <td>кол-во: <asp:TextBox ID="_txtQty" runat="server" name="field" size="1" type="text" value="4"></asp:TextBox></td>
           <td><div style="float:right; height:20;"><asp:ImageButton style="margin-top:0px" ImageUrl="~/images/search_basket.png" ID="_btnAddToCart" runat="server" CommandName="AddToCart" Text="<%$ Resources:Texts, AddToCartLC %>"  Visible='<%# !IsRestricted && Convert.ToDecimal(((TireWithPrice)Container.DataItem).Price) != 0 %>' /></div></div></td>
         
        </tr>
    </table>--%>
</td>
</tr>
</tbody>
</table>
</div>
 </ItemTemplate>
</asp:Repeater>
</asp:PlaceHolder>
</div>
<br>
</br>
<div style="width:100%; float:left;  text-align:left; ">
    <uc1:RMSPagerControl ID="TirePagerControl" runat="server" Visible = "false" />
</div>






