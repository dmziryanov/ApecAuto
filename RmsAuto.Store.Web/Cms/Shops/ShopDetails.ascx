<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopDetails.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Shops.ShopDetails" %>

<%@ Register src="../Vacancies/ShopVacancyBriefList.ascx" tagname="ShopVacancyBriefList" tagprefix="uc1" %>

<%@ Register src="ShopEmployeeList.ascx" tagname="ShopEmployeeList" tagprefix="uc2" %>


<h1><asp:Literal ID="lHead" runat="server" Text="Офисы продаж" 
		meta:resourcekey="lHeadResource1" /></h1>

<h2><%= Server.HtmlEncode( _item.ShopName ) %></h2>

<div style="float:left; width:100%;">


<table>

<tr>

<th rowspan=6 >
    <div style="margin: 10px 10px 0px 0px">
     <script src="http://api-maps.yandex.ru/1.1/?key=AAgeyEsBAAAA1MTzSAIAg2TEnfwSnds92MO5pW2X-KF6TWAAAAAAAAAAAAAsz6VC6jKHlnKJ-0sytxTPrryTug==&wizard=constructor" type="text/javascript"></script>


<script type="text/javascript">
    $(document).ready(function () {
        var map = new YMaps.Map(YMaps.jQuery("#YMapsID-444")[0]);

               
          XMax = 0;
          XMin = 1000;

          YMax = 0;
          YMin = 1000;

        $.ajax({
            url: '/Cms/ShopYaCoordList.ashx',
            data: { Region: 'Shop', ID: <%= ViewState["ShopID"] %> },
            dataType: 'json',
            success: function(data) {
                for (i = 0; i < data.length; i++) {
                    if (data[i].X > XMax) XMax = data[i].X;
                    if (data[i].X < XMin) XMin = data[i].X;
                    if (data[i].Y > YMax) YMax = data[i].Y;
                    if (data[i].Y < YMin) YMin = data[i].Y;
                    
                    var s = new YMaps.Style();
                    // Создает стиль значка метки
                    s.iconStyle = new YMaps.IconStyle();

                    s.iconStyle.href = "/images/YMap.jpg";
                    s.iconStyle.size = new YMaps.Point(38, 53);
                    s.iconStyle.offset = new YMaps.Point(-19, -53);
                    s.hasBalloon = false;

                    //Создали PlaceMark
                    var placemark = new YMaps.Placemark(new YMaps.GeoPoint(data[i].X, data[i].Y), { style: s });
                    
                    YMaps.Events.observe(placemark, placemark.Events.Click, function () {
                        $('#YMAP2').dialog('open');
                    });
                    
                    map.addOverlay(placemark);
                    //map2.addOverlay(placemark);
                }
                map.setCenter(new YMaps.GeoPoint((XMax + XMin) / 2, (YMax + YMin) / 2), 13, YMaps.MapType.MAP);
                
            }
            
            
        });
        
        
         document.getElementById("myframe").frameBorder="0";
        
         $('#YMAP2').dialog({ title: '<%= _item.ShopName %>',
                    position:  getWindowPosition(478, 500),
                    autoOpen: false,
                    resizable: false,
                    width: '478px', //ширина
                    modal: true
              });
              
         $(".ui-dialog").addClass("dialogWithDropShadow"); 
    });
</script>


    <div id="YMapsID-444" class="filialyandexmap">
    </div>


    <div id="YMAP2" style="width:450px;height:460px; text-align:center">
        <iframe id="myframe" src='<%= UrlManager.MakeAbsoluteUrl("") %>/cms/shops/ymap.htm?ID=<%= _item.ShopID %>' width="450px" height="450px" align="middle">
        </iframe>
    </div>


</th>
    <th><asp:Literal ID="lPhone" runat="server" Text="Телефоны" meta:resourcekey="lPhoneResource1" /></th><td><%=  _item.ShopPhones  %></td>
</tr>
<tr>
    <th><asp:Literal ID="lAdres" runat="server" Text="Адрес" 
			meta:resourcekey="lAdresResource1" /></th>
    <td>
		<asp:HyperLink runat="server" ID="_mapLink" Target="_blank" 
			meta:resourcekey="_mapLinkResource1" >
			<asp:Image runat="server" ID="_mapImage" CssClass="img_map" 
			meta:resourcekey="_mapImageResource1" />
		
</asp:HyperLink>
		<%= _item.ShopAddress %>; <%= _item.ShopMetro %>
    </td>
</tr>
<tr runat="server" id="_galleryRow">
	<th><asp:Literal ID="lFoto" runat="server" Text="Фотогалерея" 
			meta:resourcekey="lFotoResource1" /></th>
	<td>
		<div class="highslide-gallery">
    	<asp:Repeater runat="server" ID="GalleryHeader" >
		<ItemTemplate>
			<a target="_blank" class="highslide" href='<%# UrlManager.GetFileUrl((int)Eval("FileID")) %>'
			    onclick="$('.highslide-controls').hide(); return hs.expand(this, galleryOptions)">
				<asp:Image runat="server" ID="PhotoImage" 
				ImageUrl='<%# UrlManager.GetThumbnailUrl((int)Eval("FileID"), "photo", "" ) %>' 
				meta:resourcekey="PhotoImageResource1" />
			</a>
		</ItemTemplate>
		</asp:Repeater>
        <div class="hidden-container">
		<asp:Repeater runat="server" ID="_galleryRepeater" >
		<ItemTemplate>
			<a target="_blank" class="highslide" href='<%# UrlManager.GetFileUrl( (int)Eval("FileID") ) %>'
			    onclick="return hs.expand(this)">
				<asp:Image runat="server" ID="PhotoImage" 
				ImageUrl='<%# UrlManager.GetThumbnailUrl( (int)Eval("FileID"), "photo", "" ) %>' 
				meta:resourcekey="PhotoImageResource1" />
			</a>
		</ItemTemplate>
		</asp:Repeater>
		</div>
		</div>
		<!--onclick='info=window.open(this.href,&#039;&#039;,&#039;width=<%# Convert.ToInt32(Eval("ImageWidth"))+40 %>,height=<%# Convert.ToInt32(Eval("ImageHeight"))+30 %>,scrollbars=yes,resizable=yes&#039;); info.focus(); return false;'>-->
	</td>
</tr>
<tr>
    <th><asp:Literal ID="lShopWorkTime" runat="server" Text="Время работы" 
			meta:resourcekey="lShopWorkTimeResource1" /></th><td><%=  _item.ShopWorkTime %></td>
</tr>

<tr>
    <th><asp:Literal ID="Literal1" runat="server" Text="Комментарий" 
			 /></th><td><%=  _item.ShopNote %></td>
</tr>

<tr>
<%--    <th><asp:Literal ID="Literal2" runat="server" Text="Сайт магазина" 
			 /></th><td><a href="<%=  _item.SiteUrl %>"><%=  _item.SiteUrl %></a></td>--%>
</tr>

<tr>
    <!--<th><asp:Literal ID="lShopEmployee" runat="server" Text="Сотрудники" 
			meta:resourcekey="lShopEmployeeResource1" /></th>
    <td><uc2:ShopEmployeeList ID="_shopEmployeeList" runat="server" /></td>-->
</tr>
<tr>
	<th colspan=3><asp:Literal ID="lVacancy" runat="server" Text="Вакансии" 
			meta:resourcekey="lVacancyResource1" /></th>

</tr>
<tr>
	<th colspan = 3><uc1:ShopVacancyBriefList ID="_shopVacancyBriefList1" runat="server" />
	</th>
</tr>
</table>

</div>
