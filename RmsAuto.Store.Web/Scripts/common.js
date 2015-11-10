function initTextBoxTrimmer() {
    if( typeof(ValidatorGetValue)=='function' ) {
		ValidatorGetValue = function(id) {
				var control;
				control = document.getElementById(id);
				if (typeof(control.value) == "string") {
					control.value = $.trim( control.value );
					return control.value;
				}
				return ValidatorGetValueRecursive(control);
			}
	}
	$('input[type=text]').change( function() { this.value = $.trim( this.value ); } );
}/**/

//Функция возвращает массив координат для окошка
function getWindowPosition(windowWidth, windowHeight) {
    return window != top ? [(parent.document.body.clientWidth - windowWidth) / 2, (parent.document.body.clientHeight - windowHeight) / 2] : ['center', 'center'];
}

//для выплывания окна логина
$(function () {
    $.fn.extend({
        alert: function (text) {
            var container = $(this);
            $('div.alert').hide();
            if (text) container.html(text);
            container.slideDown();
        }
    });

	$('#login').bind('click', function (e) {
		e.preventDefault();
		e.stopPropagation();
		$('div.modal').slideToggle('fast');
	});

	$(document).bind('click', function (e) {
		if ($(e.target).closest("div.modal").length) return;
		$("div.modal").slideUp("fast");
		e.stopPropagation();
	});

	$('div.information tr:even').addClass('bg');

	$('div.side-menu table:first td:eq(1) a').addClass('tr');
	$('div.side-menu table:last td:eq(1) a').addClass('br');
});

function getCookie(name) {
    var prefix = name + "=";
    var start = document.cookie.indexOf(prefix);
    if (start == -1)
        return "";
    var end = document.cookie.indexOf(";", start + prefix.length);
    if (end == -1)
        end = document.cookie.length;
    var value = document.cookie.substring(start + prefix.length, end);
    return unescape(value);
}

/* Russian (UTF-8) initialisation for the jQuery UI date picker plugin. */
/* Written by Andrew Stromnov (stromnov@gmail.com). */
jQuery(function ($) {
    $.datepicker.regional['ru'] = {
        closeText: 'Закрыть',
        prevText: '&#x3c;Пред',
        nextText: 'След&#x3e;',
        currentText: 'Сегодня',
        monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
        'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
        monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
        'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
        dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
        dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
        dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
        weekHeader: 'Не',
        dateFormat: 'dd.mm.yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
});
