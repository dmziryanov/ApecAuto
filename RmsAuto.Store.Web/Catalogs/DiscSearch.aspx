<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="DiscSearch.aspx.cs" Inherits="RmsAuto.Store.Web.Store.DiscSearch" %>

<%@ Register src="~/Controls/Catalogs/DiscSearch.ascx" tagname="DiscSearch" tagprefix="uc7" %>
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

<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_DiscSearch.css")%>" type="text/css" />

<script type="text/javascript">    // <![CDATA[
    function OpenCat(data) {
        $('#AjaxLoadDialogDialog').dialog('open');
        $('.ui-widget-header').hide();
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Disc.aspx") %>';
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
        $('#AjaxLoadDialogDialog').dialog('open'); //открываем модальное окно
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Disc.aspx") %>';
        tmpStr = tmpStr + '?mfr=' + escape($('select[id$="companies"]').val());
        tmpStr = tmpStr + '&Dia=' + escape($('#Dia').val());
        tmpStr = tmpStr + '&PCD=' + escape($('#PCD').val());
        tmpStr = tmpStr + '&Gab=' + escape($('#Gab').val());
        tmpStr = tmpStr + '&Width=' + escape($('#Width').val());
        tmpStr = tmpStr + '&Diameter=' + escape($('#Diameter').val());
        tmpStr = tmpStr + '&PageSize=' + GetRadioValue();
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
        $('#Dia').val(unescape(getUrlVars()["Dia"]));
        $('#PCD').val(unescape(getUrlVars()["PCD"]));
        $('#Gab').val(unescape(getUrlVars()["Gab"]));
        $('#Width').val(unescape(getUrlVars()["Width"]));
        $('#Diameter').val(unescape(getUrlVars()["Diameter"]));

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
        $('#AjaxLoadDialogDialog').dialog({ title: 'Подождите, идет загрузка...',
            position: getWindowPosition(500, 200),
            autoOpen: false,
            resizable: false,
            width: '500px', //ширина
            modal: true
        });

        //привязка действия к кнопке
        $('#search_button').button()
        { 
        }

        $("#radioset").buttonset();

        $('#search_button').click(function()
        {
            $('#search_dialog').dialog('open');
            return false;
        });

        $('#search_dialog').dialog(
        {
            position: getWindowPosition(400, 200),
            autoOpen: false,
            width: 400,
            resizable: false,
            buttons:
            {
                "Найти": DoSearch,
                "Отмена": function()
                {
                    $(this).dialog("close");
                }
            }
        });

        //привязка действия к кнопке diskInfo_button
        $('#diskInfo_button').button()
        {
        }

        $('#diskInfo_button').click(function()
        {
            $('#discInfoWindow').dialog('open');
            return false;
        });

        $('#discInfoWindow').dialog
        ({
            autoOpen: false,
            width: 800,
            resizable: false
        });
    });
    // ]]></script>

<div>
<p>
<button id="search_button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">
    <span class="ui-button-text">Подбор дисков</span>
</button>
<button id="diskInfo_button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">
    <span class="ui-button-text">Справка по дискам</span>
</button>
</p>
</div>				

<div id="search_dialog" title="Для поиска введите">

    <table style="width: auto;" border="0">
    
    <tbody>
        <tr>
        <td style="width:218px;" align="left";>Производитель:</td>
        <td style="width:80px;" align="right";><asp:DropDownList class="" ID="companies" runat="server"/></td>
        </tr>
        
        <tr>
        <td align="left" style="width: 218px">Вылет, мм:</td>
        <td align="right">
                <select id="Gab" name="Combo5" style="width: 77px; margin-left: 0px">
                    <option selected="selected">Все</option>
                    <option>< 0</option>
                    <option>0-10</option>
                    <option>11-20</option>
                    <option>21-30</option>
                    <option>31-40</option>
                    <option>41-50</option>
                    <option>> 50</option>
                </select>
        </td>
        </tr>
        
        <tr>
        <td align="left" style="width: 218px">Диаметр, дюймы:</td>
        <td align="right">
            <select id="Diameter" class="" name="Combo2" style="width: 77px"> 
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
        
        <tr>
        <td align="left" style="width: 218px">Ширина обода, дюймы:</td>
        <td align="right">
            <select id="Width" name="Combo3" style="width: 77px"> 
                <option selected="selected">Все</option>
                <option>4</option>
                <option>4.5</option>
                <option>5</option>
                <option>5.5</option>
                <option>6</option>
                <option>6.5</option>
                <option>7</option>
                <option>7.5</option>
                <option>8</option>
                <option>8.5</option>
                <option>9</option>
                <option>9.5</option> 
                <option>10</option> 
                <option>11</option> 
                <option>15</option>
                <option>16</option>
                <option>17</option>
                <option>18</option>
                <option>19</option>
                <option>19.5</option>
                <option>20</option>
                <option>21</option>
                <option>22</option>
                <option>22.5</option>
                <option>24</option>
           </select>
        </td>
        </tr>
    
        <tr>
        <td align="left" style="width:218px">PCD:</td>
        <td align="right">
            <select id="Quantity" name="Combo4" style="width: 77px">
                <!--option selected="selected" value = "Все"/-->
                
                <option selected="selected">Все</option>
               
                <option>3x98</option>
                <option>3x98</option>
                <option>3x112</option>
                <option>3x256</option>
                <option>4x98</option>
                <option>4x100</option>
                <option>4x108</option>
                <option>4x114,3</option>
                <option>5x98</option>
                <option>5x100</option>
                <option>5x105</option>
                <option>5x108</option>
                <option>5x110</option>
                <option>5x112</option>
                <option>5x114,3</option>
                <option>5x115</option>
                <option>5x118</option>
                <option>5x120</option>
                <option>5x127</option>
                <option>5x130</option>
                <option>5x135</option>
                <option>5x139,7</option>
                <option>5x150</option>
                <option>5x160</option>
                <option>6x114,3</option>
                <option>6x115</option>
                <option>6x125</option>
                <option>6x127</option>
                <option>6x130</option>
                <option>6x135</option>
                <option>6x139,7</option>
            </select>
        </td>
    </tr>

    <!--tr>
        <td align="left" style="width: 285px">Кол-во болтов:</td>
        <td align="right">
            <select id="Quantity" class="tireSearchCombo" name="Combo4">
                <option selected="selected">Все</option>
                <option>3</option>
                <option>4</option>
                <option>5</option>
                <option>6</option>
            </select>
        </td>
    </tr-->

    <!--tr>
      <td align="left" style="width: 285px">Межболтовое расстояние, мм:</td>
        <td align="right">
            <select id="Distance" class="tireSearchCombo" name="Combo3"> 
                <option selected="selected">Все</option>
                <option>98</option>
                <option>100</option>
                <option>108</option>
                <option>110</option>
                <option>112</option>
                <option>114</option>
                <option>114.1</option>
                <option>115</option>
                <option>118</option>
                <option>120</option>
                <option>127</option>
                <option>130</option>
                <option>170</option>
                <option>190</option>
            </select>
        </td>
    </tr-->
    </tbody>
    </table>
</div>



<div id="discInfoWindow" title="Параметры дисков, используемые для поиска">

    <table border="0">

    <tbody>
        <tr>
      
        <td><asp:Image runat="server" ImageUrl="~/images/discInfo.jpg" /></td>
        
        <td>
        
        <div style="width: 100%; vertical-align: middle;">
        <p style="padding-left:12px;"><span style="color:red;"><b>A</b></span> – ширина обода, измеряется в дюймах.</p>
        <p style="padding-left:12px;"><span style="color:red;"><b>B</b></span> – диаметр диска, измеряется в дюймах.</p>
        <p style="padding-left:12px;"><span style="color:red;"><b>Dia</b></span> – диаметр центрального отверстия, измеряется в мм.</p>
        <p style="padding-left:12px;"><span style="color:red;"><b>PCD</b></span> – диаметр окружности, проходящий через центры крепежных отверстий диска, измеряется в мм.</p>
        <p style="padding-left:12px;"><span style="color:red;"><b>ET</b></span> – вылет, расстояние от прилегающей к ступице плоскости колеса до плоскости, которую можно провести через центр ширины обода, измеряется в мм.</p>
        </div>
        
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
   <uc7:DiscSearch ID="DiscSearchControl" runat="server" />
</div>
    
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
    <uc2:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
