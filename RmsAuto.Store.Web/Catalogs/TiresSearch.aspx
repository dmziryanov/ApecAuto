<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TiresSearch.aspx.cs" MasterPageFile="~/PageTwoColumnsNEW.Master" Inherits="RmsAuto.Store.Web.TiresSearch" %>

<%@ Register Src="../Cms/News/NewsTopList.ascx" TagName="NewsTopList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc2" %>
<%@ Register Src="../Controls/RightMenu.ascx" TagName="RightMenu" TagPrefix="uc3" %>
<%@ Register Src="../TecDoc/Controls/TecDocManufacturersFP.ascx" TagName="TecDocManufacturersFP" TagPrefix="uc4" %>
<%@ Register src="../Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc5" %>
<%@ Register Src="../Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl" TagPrefix="uc5" %>
<%@ Register src="~/Controls/Catalogs/TireSearch.ascx" tagname="TireSearch" tagprefix="uc7" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Routing" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="_mainContentPlaceHolder">

<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_tiresSearch.css")%>" type="text/css" />

<script type="text/javascript">    // <![CDATA[
    
    function OpenCat(data) {
        $("#LoadDialog").dialog('open');
        $('.ui-widget-header').hide();
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Tires.aspx") %>';
        tmpStr = tmpStr + '?mfr=' + data;
        location.href = encodeURI(tmpStr + '&PageSize=' + GetRadioValue());                      
    }

    function SearchComplete() {
        $('#AjaxLoadDialogDialog').dialog('close');
        $('.ui-widget-header').show();
    }

    function GetRadioValue() {
        var var_name = $("input[name^='radio']:checked").val();
        if (var_name != undefined)
            return var_name;
        else
            return 10;
    }

    function getUrlVars() {
        var tmp = new Array(); // два вспомогательных массива tmp и tmp2
        var tmp2 = new Array();
        var param = new Array();

        var getstr = location.search; // строка GET запроса
        if (getstr != '') {
            tmp = (getstr.substr(1)).split('&'); // разделяем переменные
            for (var i = 0; i < tmp.length; i++) {
                tmp2 = tmp[i].split('='); // массив param будет содержать
                param[tmp2[0]] = tmp2[1]; // пары ключ(имя переменной)->значение
            }
        }
        return param;
    }

    function DoSearch() {
        $('.ui-widget-header').hide();
        $(this).dialog("close");
        $('#LoadDialog').dialog('open'); //открываем модальное окно
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Tires.aspx") %>';
        tmpStr = tmpStr + '?mfr=' + escape($('select[id$="companies"]').val());
        tmpStr = tmpStr + '&Height=' + escape($('#height').val());
        tmpStr = tmpStr + '&Width=' + escape($('#Width').val());
        tmpStr = tmpStr + '&Radius=' + escape($('#radius').val());
        tmpStr = tmpStr + '&Season=' + escape($('#Season').val()) + '&PageSize=' + GetRadioValue();
        location.href = tmpStr;   //Перенаправление
    }

    $(document).ready(function() {
        SearchComplete();

        $('.imgCat').click(function(eventObject) {
            if (eventObject.target.alt != undefined)
                { OpenCat(eventObject.target.alt); }
            else
                { OpenCat(eventObject.target.innerHTML); }
        });

        $('select[id$="companies"]').val(unescape(getUrlVars()["mfr"]));
        $('#height').val(unescape(getUrlVars()["Height"]));
        $('#Width').val(unescape(getUrlVars()["Width"]));
        $('#radius').val(unescape(getUrlVars()["Radius"]));
        $('#Season').val(unescape(getUrlVars()["Season"]));

        if ($('input[name^="radio"]').length > 0) {
            $('input[name^="radio"]').next().removeClass("ui-state-active");
            $('input[name^="radio"]')[0].checked = false;

            $('input[name^="radio"]').click(DoSearch);

            if (getUrlVars()["PageSize"] != undefined) {
                $('input[value =' + (getUrlVars()["PageSize"]).toString() + ']').attr("checked", "checked");
            }
            else {
                $('input[value = 10]').attr("checked", "checked");
            }
        }
        //делаем модальное окно загрузки
        $('#LoadDialog').dialog({
            title: 'Подождите, идет загрузка..',
            position: getWindowPosition(500, 200),
            autoOpen: false,
            resizable: false,
            width: '500px', //ширина
            modal: true
        });

        //привязка действия к кнопке
        $('#search_button').button()
        { }

        $("#radioset").buttonset();

        $('#search_button').click(function() {
            $('#search_dialog').dialog('open');
            return false;
        });

        $('#search_dialog').dialog({
            position: getWindowPosition(400, 200),
            autoOpen: false,
            width: 400,
            resizable: false,
            buttons: {
                "Найти": DoSearch,
                "Отмена": function() {
                    $(this).dialog("close");
                }
            }
        });
    });
    // ]]></script>

<div>
<p>
<button id="search_button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only my-ui-button" role="button" aria-disabled="false">
    <span class="ui-button-text">Подбор шин</span>
</button>
</p>
</div>				

<div id="search_dialog" style="width: 220px;" title="Для поиска введите">
    <table style="width: 220px;" border="0">
    <tbody>
        <tr>
        <td width="50" align="left">Производитель:</td>
        <td align="right">
            <asp:DropDownList class="tireSearchManufacturerCombo" ID="companies" runat="server">
            </asp:DropDownList>
        </td>
        </tr>
        <tr>
            <td align="left">Сезон:</td>
            <td align="right">
                <select id="Season" class="tireSearchCombo" name="Combo5">
                    <option selected="selected">Все</option>
                    <option>Лето</option>
                    <option>Зима</option>
                    <option>Все</option>
                </select>
            </td>
        </tr>
    <tr>
        <td align="left">Ширина:</td>
        <td align="right">
            <select id="Width" class="tireSearchCombo" name="Combo2"> 
                <option selected="selected">Все</option>
                <option>30</option>
                <option>31</option>
                <option>32</option>
                <option>33</option>
                <option>35</option>
                <option>37</option>
                <option>175</option>
                <option>185</option>
                <option>195</option>
                <option>205</option>
                <option>215</option>
                <option>225</option>
                <option>235</option>
                <option>245</option>
                <option>255</option>
                <option>265</option>
                <option>275</option>
                <option>285</option>
                <option>295</option>
                <option>305</option>
                <option>315</option>
                <option>325</option>
                <option>335</option>
                <option>345</option>
            </select>
        </td>
    </tr>
    <tr>
    <td align="left">Профиль:</td>
        <td align="right">
            <select id="height" class="tireSearchCombo" name="Combo3"> 
                <option selected="selected">Все</option>
                <option>9.5</option>
                <option>10.5</option>
                <option>11.5</option>
                <option>12.5</option>
                <option>13.5</option>
                <option>25</option>
                <option>30</option>
                <option>35</option>
                <option>40</option>
                <option>45</option>
                <option>50</option>
                <option>55</option> 
                <option>60</option> 
                <option>65</option> 
                <option>70</option>
                <option>75</option>
                <option>80</option>
                <option>85</option>
            </select>
        </td>
    </tr>
    <tr>
        <td align="left">Радиус:</td>
        <td align="right">
            <select id="radius"  class="tireSearchCombo" name="Combo4">
                <option selected="selected">Все</option>
                <option>13</option>
                <option>14</option>
                <option>15</option>
                <option>16</option>
                <option>17</option>
                <option>18</option>
                <option>19</option>
                <option>20</option>
                <option>21</option>
                <option>22</option>
            </select>
        </td>
    </tr>
    </tbody>
    </table>
</div>

<asp:Table ID="BrandsTable" runat="server"></asp:Table>

<div id="LoadDialog" style="width: 100%; vertical-align: middle; text-align: center; height: 100%;">
    <div style="width: 100%; vertical-align: middle; text-align: center; height: 100%;">
        <table style="width: 100%; height: 150px;">
            <tr style="width: 100%; height: 100%;">
                <td style="width: 50%; text-align: right;">Подождите, идет загрузка...</td>
                <td style="width: 50%;"><img style="text-align: center; float: left;" src="/images/ajax-loader-small.gif" alt="" width="30" height="30" /></td>
            </tr>
        </table>
    </div>
</div>


<span runat="server" id="info_block" class="main_block">

    <div><img title="Info" src="/images/info_red.png" alt="Info" width="18" height="18" /></div>
    
    <div><b>Информация:</b></div>
    <div id="Div1" runat="server">
        <div style="float:none; padding:0px"><asp:Label runat="server" ID="PriceWarningText" Text="&mdash; указанные цены действительны при месячном объеме закупок более 400 000 руб."></asp:Label></div>
        <asp:Label runat="server" ID="QuantityText" Text="&mdash; найдено товаров:"></asp:Label><b><asp:Label runat="server" ID="Quantity" ></asp:Label></b>
        <div style="padding:0px"><asp:Label runat="server" ID="QuantityWarningText"></asp:Label></div>
    </div>
</span>

<span runat="server" id="Elements_Count" style="margin-top:15px; display:table; width:1000px;" >
    <div style="float:left">&nbsp;Товаров на странице:&nbsp;</div>&nbsp;
    <div id="radioset" style="float:left" class="ui-buttonset">
            <input  type="radio" id="radio1" name="radio" value="10" checked="checked" class="ui-helper-hidden-accessible"><label for="radio1" aria-pressed="true" class="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-state-active" role="button" aria-disabled="false"><span class="ui-button-text">10</span></label>
            <input  type="radio" id="radio2" name="radio" value="20" class="ui-helper-hidden-accessible"><label for="radio2" class="ui-button ui-widget ui-state-default ui-button-text-only" aria-pressed="false" role="button" aria-disabled="false"><span class="ui-button-text">20</span></label>
            <input  type="radio" id="radio3" name="radio" value="50" class="ui-helper-hidden-accessible"><label for="radio3" aria-pressed="false" class="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right" role="button" aria-disabled="false"><span class="ui-button-text">50</span></label>
    </div>
</span>

<div>
   <uc7:TireSearch ID="TireSearchControl" runat="server" />
</div>
    
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
    <uc2:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
