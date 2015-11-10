// JScript File for TextArea
// Keep user from entering more than maxLength characters
function doKeypress(control) {
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength && value.length > maxLength - 1) {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
    }
}
// Cancel default behavior
function doBeforePaste(control) {
    maxLength = control.attributes["maxLength"].value;
    if (maxLength) {
        event.returnValue = false;
    }
}
// Cancel default behavior and create a new paste routine
function doPaste(control) {
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength) {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
        var oTR = control.document.selection.createRange();
        var iInsertLength = maxLength - value.length + oTR.text.length;
        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
        oTR.text = sData;
    }
}