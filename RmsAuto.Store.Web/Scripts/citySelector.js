$(document).ready(function() {
    InnerFlag = false;

    MakeCityDomain = function(Url) {
        return 'http://' + Url.split('/')[2];
    }

    AppRestart = function(InternalFranchName) {

        //Убрать после переделки CatalogsItemsCache
        $.ajax({
            url: '/AppRestart.ashx?InternalFranchName=' + InternalFranchName,
            dataType: 'json',
            succes: function() { alert('success') },
            error: function(XMLHttpRequest, textStatus, errorThrown) {

            }
        }); //ajax
    }

//    prepareDialog = function() {
//		
//		$.noConflict(true); //нужно чтобы работали Laximo, на других страницах этого не должно быть

//		$('#RegionList').dialog({ title: 'Ваш город',
//            position: [260, 210],
//            autoOpen: false,
//            resizable: false,
//            height: 500,
//            width: '678px', //ширина
//            modal: true
//           });

//        //Применяем стили к окну
//        $(".ui-dialog").addClass("dialogWithDropShadow");
//        $(".ui-dialog").addClass("overlay");
//        $('.ui-widget-header').removeClass('ui-widget-header');
//    }

    function setCookie(name, value) {
        var expires = new Date();
        expires.setTime(expires.getTime() + 31536000000);
        document.cookie = name + "=" + value + "; expires=" + expires.toGMTString() + "; path=/; domain=rmsauto.ru;";
    }

    function delCookie(name, value) {
        var expires = new Date();
        expires.setTime(expires.getTime() - 31536000000);
        document.cookie = name + "=" + value + "; expires=" + expires.toGMTString() + "; path=/; domain=rmsauto.ru;";
    }

    LoadData = function(data) {
        {
            if (typeof (data) == 'String') {
                $("#UpperDiv").html(data);
                return;
            }

            var tbl_body = "";
            try {
                $.each(data, function() {
                    var tbl_row = "";
                    //Цикл по всем полям
                    //Обработчик : название поля, значение поля
                    var ID = 0;

                    $.each(this, function(k, v) {
                        if (k == 'CityID')
                            ID = v;
                        if (k == 'Name')
                            tbl_row += "<div ID='" + ID + "' class='GrayTextStyle2'>" + v + "</div>";
                    })
                    tbl_body += tbl_row;
                });
            }
            catch (e) {

            }

            $("#UpperDiv").html(tbl_body);


            tbl_body = "";
            //Кладем обработчик
            $('#UpperDiv .GrayTextStyle2').click(function() {
                cityName = this.innerHTML;
                //Получить список городов
                $("#LowerDiv").empty(); //Очистить список городов

                $('.roundedCity').removeClass('roundedCity');
                $(this).addClass('roundedCity');


                strID = $(this).attr('ID');
                $.ajax({
                    url: '/Cms/ShopYaCoordList.ashx?Region=City&ID=' + strID,
                    dataType: 'json',

                    //Заполняем список магазинов
                    success: function(data) {
                        tbl_body = "";
                        var SiteUrl;
                        var FranchName;
                        var InternalFranchName;

                        i = 0;
                        {
                            $.each(data, function() {
                                var tbl_row = "";
                                //Цикл по всем полям
                                //Обработчик : название поля, значение поля
                                var Name;


                                //k -- ключ поля, v -- значение поля
                                $.each(this, function(k, v) {
                                    if (k == 'InternalFranchName')
                                        InternalFranchName = v;

                                    if (k == 'Name')
                                        Name = v;

                                    if (k == 'SiteUrl') {
                                        tbl_row += "<a name='" + InternalFranchName + "' class='urlSiteStyle'>" + Name + "</a><wbr>";
                                        SiteUrl = v;
                                    }


                                    if (k == 'FranchName') {
                                        if (FranchName != v) {
                                            if (FranchName != 'undefined') {
                                                tbl_row += "</div><wbr>"
                                            }

                                            FranchName = v;
                                            tbl_row += "<div class='GrayBlock'>";
                                            tbl_row += tbl_row += "<div class='PartnerTextStyle'>" + FranchName + "</div><wbr>";
                                        }
                                    }
                                })

                                i++; //Подсчитать счетчик если один закрывать окно и делать редирект
                                tbl_body += tbl_row;
                            });
                            //При реализации через архитектуру "один сайт" редирект делать не нужно

                            if (i == 1) { // Если
                                setCookie('InternalFranchName', InternalFranchName);
                                AppRestart(InternalFranchName);
                                setCookie('cityName', cityName);
                                $('#RegionList').dialog('close');
                                setTimeout(function() { location.reload(); $('#RegionList').dialog('close'); }, 500);
                                //window.location.replace(SiteUrl); //Положить в куку
                            }
                            else {
                                //Добавляем в нижний див список
                                $("#LowerDiv").attr('innerHTML', tbl_body);
                                $('.urlSiteStyle').click(function() {
                                    setCookie('InternalFranchName', this.name);
                                    AppRestart(this.name);
                                    setCookie('cityName', cityName);
                                    setTimeout(function() { location.reload(); $('#RegionList').dialog('close'); }, 500);
                                });
                            }
                        }
                    },
                    Error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            });
        } // for

    }

    //функция загрузки списка магазинов
    fillDialog = function(flag) {

        InnerFlag = flag;
        $.ajax({
            url: '/Cms/RegionList.ashx',
            dataType: 'json',
            success: LoadData, //success
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                $("#UpperDiv").html("Для смены региона, пожалуйста выйдите из системы");
            }
        }); //ajax

    };  //function fillDialog
});