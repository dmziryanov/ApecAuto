<%@ Control  Language="C#" AutoEventWireup="true" CodeBehind="DiscSearch.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.DiscSearch" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Entities" %>
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
<asp:Repeater ID="rptSearchResults" runat="server" EnableViewState = "false"     >
<ItemTemplate>
<div class="card" style="float:left">
<div style="float:left"><asp:Label id="_lblKey" runat="server" Visible="false"></asp:Label></div>
<table border="0">
<tbody>
<tr>
<td rowspan="2" style="vertical-align:middle">
<div><a class="highslide" onclick="return hs.expand(this)" href='<%# UrlManager.GetFileUrl(Convert.ToInt32(Eval("ImageUrl"))) + "?r=rms" %>' ><img class="bordgray" src='<%# UrlManager.GetThumbnailUrl(Convert.ToInt32(Eval("ImageUrl")), "battery", "rms")%>' alt="" width="150" height="150" /></a></div>
</td>
<td class="brand_name" colspan="3"><span><%#Server.HtmlEncode(Convert.ToString(Eval("Manufacturer")))%></span><br />
<asp:Label runat="server" ID="ModelName" ToolTip='<%#Server.HtmlEncode(Convert.ToString(Eval("ModelName")))%>'><h3><%#Server.HtmlEncode(Convert.ToString(Eval("ModelName")))%></h3></asp:Label></td>
</tr>
<tr/>
<td class="cont">
    <span>Dia:         </span><br />
    <span>PCD:         </span><br />
    <span>вылет, мм.:  </span><br />
    <span>диаметр:     </span><br />
    <span>ширина обода:</span><br />
</td>
<td class="cont2" colspan="2">
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Dia")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("PCD")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Gab")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Diameter")))%></span><br />
    <span><%#Server.HtmlEncode(Convert.ToString(Eval("Width")))%></span><br />
</td>
</tr>
<tr class="pricerow">
    <td>
        <div class="article"><%#Server.HtmlEncode(Convert.ToString(Eval("PartNumber")))%></div>
    </td>
    <td  class="pricerow" colspan="3">
         <a title="Посмотреть конкретные предложения поставщиков" href='<%# Eval("Ref") as string %>' class="Buy">Цены</a>
    </td>
  <%--      <div style="float:left; margin-top: 2px;">
            цена: <span class="price"><%#Server.HtmlEncode(Convert.ToString(Eval("Price")))%> р.</span>
        </div> 
        <asp:Label ID="_lblDefaultOrderQty" runat="server" Visible="false" Text='<%# ((SparePart)((Battery)Container.DataItem).SparePart).DefaultOrderQty %>' />
    </td>
    <td class="pricerow">
    <div style="float:right;width:100%; vertical-align:middle;" onkeypress="if( event.keyCode==13 ) { $('input[name$=_btnAddToCart]',this.parentElement).click(); return false; }">
    
    <table>
        <tr>
            <td>кол-во:<asp:TextBox ID="_txtQty" runat="server" name="field" size="1" type="text" value="1"></asp:TextBox></td>
            <td><div style="float:right; height:20;"><asp:ImageButton style="margin-top:0px" ImageUrl="~/images/search_basket.png" ID="_btnAddToCart" runat="server" CommandName="AddToCart" Text="<%$ Resources:Texts, AddToCartLC %>"  Visible='<%# !IsRestricted && Convert.ToDecimal(((Battery)Container.DataItem).Price) != 0 %>' />
            </div></td>
        </tr>
    </table>
    </div>--%>
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






