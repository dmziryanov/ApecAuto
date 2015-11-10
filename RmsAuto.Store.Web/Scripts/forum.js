function getTextCompleted(result)
{
    var div = $("#messageBodyDivContainer_" + result.Id);
    var link = $("#showTextLink_" + result.Id);

    $("#upDownImg_" + result.Id).attr("src", upImage);
    div.empty();
    
    var html = "<div id=\"messageBodyDiv_" + result.Id + "\" class=\"messageBody\">" + result.Body + "</div>";
    
    if (result.IsAuthenticated)
    {
        if (selectedOp != 'quote' || selectedForumPostId != result.Id)
        {
            html += "<a href=\"" +
                    result.QuoteUrl +
                    "\" class=\"underMessage\">Ответить</a>";
        }
        if (result.CanBeDeleted)
        {
            html += "|<a id=\"deleteMesLink_" + result.Id + "\" href=\"" +
                    result.DeleteUrl +
                    "\" class=\"underMessage\">Удалить</a>";
        }           
        if (result.CanBeEdited)
        {
            html += "|<a href=\"" +
                    result.EditUrl +
                    "\" class=\"underMessage\">Править</a>";
        }
    }           
    
    $(html).appendTo(div);
    link.attr("href", "javascript:clearText(" + result.Id + ")");
    $("#deleteMesLink_" + result.Id).bind("click", function() {
        return confirm("Вы действительно хотите удалить данный пост?");
    });
}

function clearText(id)
{
    var div = $("#messageBodyDivContainer_" + id);
    var link = $("#showTextLink_" + id);
    
    $("#upDownImg_" + id).attr("src", downImage);
    div.empty();
    
    link.attr("href", "javascript:getText(" + id + ")");
}


/*********************************************************/

function _g(id)
{
    return document.getElementById(String(id));
}

function switchBlockElement(id)
{
    var elem = _g(id);

    if (typeof(elem) != 'undefined')
    {
        if (elem.style.display == 'none')
        {
            elem.style.display = 'block';
        }
        else
        {
            elem.style.display = 'none';
        }
    }
}
//  switchBlockElement abbr.
function _sw(id) { switchBlockElement(id); }

function switchBlockElementAndChangeImage(id, divPrefix, imgPrefix, offPicUrl, onPicUrl)
{
    switchBlockElement(String(divPrefix) + id);
    
    var imgElem = _g(String(imgPrefix) + id);

    if (typeof(imgElem) != 'undefined')
    {
        if (imgElem.src.indexOf(offPicUrl) >= 0)
        {
            imgElem.src = onPicUrl;
        }
        else
        {
            imgElem.src = offPicUrl;
        }
    }
}
//  switchBlockElementAndChangeImage abbr.
function _swi(id, divPrefix, imgPrefix, offPicUrl, onPicUrl) { switchBlockElementAndChangeImage(id, divPrefix, imgPrefix, offPicUrl, onPicUrl); }