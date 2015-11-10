<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="BatterySearch.aspx.cs" Inherits="RmsAuto.Store.Web.Store.BatterySearch" %>

<%@ Register src="../Controls/Catalogs/BatterySearch.ascx" tagname="BatterySearch" tagprefix="uc7" %>
<%@ Register Src="../Cms/News/NewsTopList.ascx" TagName="NewsTopList" TagPrefix="uc1" %>
<%@ Register Src="../Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc2" %>
<%@ Register Src="../Controls/RightMenu.ascx" TagName="RightMenu" TagPrefix="uc3" %>
<%@ Register Src="../TecDoc/Controls/TecDocManufacturersFP.ascx" TagName="TecDocManufacturersFP" TagPrefix="uc4" %>
<%@ Register src="../Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc5" %>
<%@ Register Src="../Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl" TagPrefix="uc5" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Routing" %>

<asp:Content ID="Content5" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc5:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="_textContentPlaceHolder">

<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_BatterySearch.css")%>" type="text/css" />

<script type="text/javascript">    // <![CDATA[
    function OpenCat(data) {
        $('#LoadDialog').dialog('open');
        $('.ui-widget-header').hide();
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Battery.aspx") %>';
        tmpStr = tmpStr + '?mfr=' + data;
        location.href = encodeURI(tmpStr + '&PageSize=' + GetRadioValue());                      
    }

    function SearchComplete() {
        $('#LoadDialog').dialog('close');
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
        $('#AjaxLoadDialogDialog').dialog('open'); //открываем модальное окно
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Battery.aspx") %>';
        tmpStr = tmpStr + '?mfr=' + escape($('select[id$="companies"]').val());
        tmpStr = tmpStr + '&Capacity=' + escape($('#Capacity').val());
        tmpStr = tmpStr + '&Polarity=' + escape($('#Polarity').val());
        tmpStr = tmpStr + '&Cleat=' + escape($('#Cleat').val()) + '&PageSize=' + GetRadioValue();
        location.href = tmpStr;   //Перенаправление
        
    }

    $(document).ready(function() {
        //делаем модальное окно загрузки
        $('#LoadDialog').dialog({
            title: 'Подождите, идет загрузка...',
            position: getWindowPosition(500, 200),
            autoOpen: false,
            resizable: false,
            width: '500px', //ширина
            modal: true
        });

        SearchComplete();

        $('.imgCat').click(function(eventObject) {
            if (eventObject.target.alt != undefined)
            { OpenCat(eventObject.target.alt); }
            else
            { OpenCat(eventObject.target.innerHTML); }
        });

        $('select[id$="companies"]').val(unescape(getUrlVars()["mfr"]));
        $('#Capacity').val(unescape(getUrlVars()["Capacity"]));
        $('#Polarity').val(unescape(getUrlVars()["Polarity"]));
        $('#Cleat').val(unescape(getUrlVars()["Cleat"]));


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
      
        //привязка действия к кнопке
        $('#search_button').button()
        {
         
        }

        $("#radioset").buttonset();

        $('#search_button').click(function() {
            $('#search_dialog').dialog('open');
            return false;
        });

        $('#search_dialog').dialog({
            autoOpen: false,
            position: getWindowPosition(400, 200),
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
<button id="search_button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">
    <span class="ui-button-text">Подбор аккумуляторов</span>
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
            <td align="left">Ёмкость:</td>
            <td align="right">
                <select id="Capacity" class="tireSearchCombo" name="Combo5">
                    <option selected="selected">Все</option> 
                    <option>4-25</option> 
                    <option>35-45</option> 
                    <option>51-52</option>
                    <option>53-56</option>
                    <option>60-65</option>
                    <option>66-77</option>
                    <option>80-95</option>
                    <option>100-110</option>
                    <option>132-143</option>
                    <option>180-190</option>
                    <option>200-225</option>
                </select>
            </td>
        </tr>
    <tr>
        <td align="left">Полярность:</td>
        <td align="right">
            <select id="Polarity" class="tireSearchCombo" name="Combo2"> 
               <option selected="selected">Все</option> 
               <option>Прямая</option> 
               <option>Обратная</option> 
            </select>
        </td>
    </tr>
    <tr>
    <td align="left">Тип клемм:</td>
        <td align="right">
            <select id="Cleat" class="tireSearchCombo" name="Combo3"> 
              <option selected="selected">Все</option>
              <option>Стандарт</option>
              <option>Американский</option>
              <option>Японский</option>
              <option>Стандарт/Японский</option>
            </select>
        </td>
    </tr>
    </tbody>
    </table>
</div>

<asp:Table ID="BrandsTable" runat="server"></asp:Table>

<div id="AjaxLoadDialog" style="width: 100%; vertical-align: middle; text-align: center; height: 100%;">
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

<div runat="server" id="Elements_Count" style="margin-top:15px; display:table; width:1000px;" >
    <div style="float:left">&nbsp;Товаров на странице:&nbsp;</div>&nbsp;
    <div id="radioset" style="float:left" class="ui-buttonset">
            <input  type="radio" id="radio1" name="radio" value="10" checked="checked" class="ui-helper-hidden-accessible"><label for="radio1" aria-pressed="true" class="ui-button ui-widget ui-state-default ui-button-text-only  ui-state-active" role="button" aria-disabled="false"><span class="ui-button-text">10</span></label>
            <input  type="radio" id="radio2" name="radio" value="20" class="ui-helper-hidden-accessible"><label for="radio2" class="ui-button ui-widget ui-state-default ui-button-text-only" aria-pressed="false" role="button" aria-disabled="false"><span class="ui-button-text">20</span></label>
            <input  type="radio" id="radio3" name="radio" value="50" class="ui-helper-hidden-accessible"><label for="radio3" aria-pressed="false" class="ui-button ui-widget ui-state-default ui-button-text-only " role="button" aria-disabled="false"><span class="ui-button-text">50</span></label>
    </div>
</div>

<div>
   <uc7:BatterySearch ID="BatterySearchControl" runat="server" />
</div>
    
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
    <uc2:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="_rightContentPlaceHolder">
    <uc3:RightMenu ID="RightMenu1" runat="server" />
</asp:Content>