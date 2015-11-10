<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PageTwoColumns.Master" CodeBehind="YandexMap.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.YandexMap" %>
<%@ Register src="ShopDetails.ascx" tagname="ShopDetails" tagprefix="uc1" %>
<%@ Register src="ShopEmployeeList.ascx" tagname="ShopEmployeeList" tagprefix="uc2" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" RenderCurrentNodeAsLink="True" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <style type="text/css">
    
    .body
    {
    	background-color:#FFFFFF;
    }
    
    .scroll
    {
        overflow: scroll;
        overflow-x: hidden;
        height: 702px;
        background-color: #F4F4F4;
        list-style-type: decimal;
    }
    
    .content
    {
        width: 100%;
    }
    
    ol li 
    {
	    background: url('') no-repeat 0px 0px;
	    padding: 3px 0px 3px 0px;
	    list-style-position: outside;
	    margin: 0px 5px 0px 15px;
    }

</style>
<script src="http://api-maps.yandex.ru/1.1/?key=AAgeyEsBAAAA1MTzSAIAg2TEnfwSnds92MO5pW2X-KF6TWAAAAAAAAAAAAAsz6VC6jKHlnKJ-0sytxTPrryTug==&wizard=constructor" type="text/javascript"></script>
<script type="text/javascript">

function getRegionParam()
{
    var results = window.location.pathname.split('/');
    return results[4];
}

function getIDParam() {
    var results = window.location.pathname.split('/');
    return results[5].split('.')[0];
}


YMaps.jQuery(function() {

    $(function() {


    });

    function cloneObject(source) {
        for (i in source) {
            if (typeof source[i] == 'source') {
                this[i] = new cloneObject(source[i]);
            }
            else {
                this[i] = source[i];
            }
        }
    }


    var map = new YMaps.Map(YMaps.jQuery("#YMapsID-5758")[0]);
    map.addControl(new YMaps.Zoom());

    YMaps.Styles.add("constructor#pmdblPlacemark666", {
        iconStyle: {
            href: "http://www.rmsauto.ru/images/info.png",
            size: new YMaps.Point(36, 41),
            offset: new YMaps.Point(-13, -40)
        }
    });

    XMax = 0;
    XMin = 1000;

    YMax = 0;
    YMin = 1000;



    $.ajax({
        url: '/Cms/ShopYaCoordList.ashx',
        data: { Region: getRegionParam(), ID: getIDParam() },
        dataType: 'json',
        success: function(data) {
            array = new Array();

            for (i = 0; i < data.length; i++) {
                if (data[i].X > XMax) XMax = data[i].X;
                if (data[i].X < XMin) XMin = data[i].X;
                if (data[i].Y > YMax) YMax = data[i].Y;
                if (data[i].Y < YMin) YMin = data[i].Y;

                var s = new YMaps.Style();

                // Создает стиль значка метки
                s.iconStyle = new YMaps.IconStyle();

                s.iconStyle.href = "/Files/2459.ashx";
                s.iconStyle.size = new YMaps.Point(38, 53);
                s.iconStyle.offset = new YMaps.Point(-19, -53);

                //Создали PlaceMark
                placemark = new YMaps.Placemark(new YMaps.GeoPoint(data[i].X, data[i].Y), { style: s });
                placemark.description = data[i].Description;
                array.push(placemark);

                map.addOverlay(placemark);

                $('li:nth-child(' + (i + 1) + ')').bind('click', { pl: placemark }, (function(event) {
                    event.data.pl.openBalloon();
                }));
            }
            if (getRegionParam() == 'City')
                map.setCenter(new YMaps.GeoPoint((XMax + XMin) / 2, (YMax + YMin) / 2), 12, YMaps.MapType.MAP);
            else
                map.setCenter(new YMaps.GeoPoint((XMax + XMin) / 2, (YMax + YMin) / 2), 10, YMaps.MapType.MAP);
        }
    });
    //Вычислить центр карты в массиве

    function createObject(type, point, style, description) {
        var allowObjects = ["Placemark", "Polyline", "Polygon"],
            index = YMaps.jQuery.inArray(type, allowObjects),
            constructor = allowObjects[(index == -1) ? 0 : index];
        description = description || "";

        var object = new YMaps[constructor](point, { style: style, hasBalloon: !!description });
        object.description = description;

        return object;
    }
});
</script>

<h2><%= Server.HtmlEncode(r == null ? Cities.Find(x => x.CityID == firstShop.CityID).Name : r.RegionName)%></h2>


<div id="YMapsID-5758" class="filialyandexmap" style="width:70%;height:700px;float:left;"></div>

<div id="panel" class="scroll">

<div style="float:left;">
<asp:Repeater  ID="CityRepeater" runat="server" >
<HeaderTemplate>
    <ol start="1" class="">
</HeaderTemplate>
<ItemTemplate>
  <li class="">
        <asp:HyperLink runat="server" ID="_itemLink" CssClass="GrayTextStyle"><%#Server.HtmlEncode((string)Eval( "ShopString" ))%></asp:HyperLink>
  </li>
</ItemTemplate>
<FooterTemplate>
<ol>
</FooterTemplate>
</asp:Repeater>
</div>
</div>


<asp:LinqDataSource ID="_linqDataSource" runat="server"
	ContextTypeName="RmsAuto.Store.Cms.Entities.CmsDataContext" 
    TableName="Shops" 
    Where="ShopVisible"
    OrderBy="ShopPriority" >
</asp:LinqDataSource>


</asp:Content>