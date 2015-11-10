
$(function () {
    $('div.information tr:even').addClass('bg');
    $('table.menu_off a').click(function (e) { e.preventDefault(); });
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
