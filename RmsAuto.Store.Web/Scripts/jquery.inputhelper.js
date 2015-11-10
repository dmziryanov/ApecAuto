function InputHelperIn(obj, text) {
    //если при фокусе значение поля равно значению подсказки, то чистим его и вешаем стили
    if (obj.value == text) {
        $(obj)
					.css({ color: '#000', fontStyle: 'normal' })
					.val('');
    }
}

function InputHelperOut(obj, text) {
    //если при потере фокуса значение поля равно пустоте или значению по умолчанию,
    //то пихаем в него текст подсказки и вешаем стили подсказки
    if (obj.value == '' || obj.value == text) {
        $(obj)
					.css({ color: '#b3b3b3', fontStyle: 'italic' })
					.val(text);
    }
}

function InputHelperCreate(obj, text) {
    //вешаем на поле эвенты. На фокус и потерю фокуса.
    $(obj)
				.bind('focus', function() {
				    InputHelperIn(this, text);
				})
				.bind('blur', function() {
				    InputHelperOut(this, text);
				});

    //первоначальный инит
    InputHelperOut(obj, text);
}