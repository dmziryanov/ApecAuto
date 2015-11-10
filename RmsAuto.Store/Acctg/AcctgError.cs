using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Acctg
{
    public enum AcctgError
    {
        [Text("команда выполнена корректно")] None = 0,
        [Text("неизвестная ошибка")] Unknown = -2,
        [Text("ошибка авторизации (неверный логин или пароль)")] AuthenticationFailed = -1,
        [Text("учетная запись клиента уже существует")] ClientAlreadyExists = -10,
        [Text("некорректное поле для сохранения учетной записи клиента")] IncorrectClientProfileFieldValue = -11,
        [Text("учетная запись клиента не найдена")] ClientNotFound = -12,
        [Text("слишком широкие критерии поиска учетных данных клиента")] TooManyClientsFound = -13,
        [Text("Не заполнено поле признак Юр. Физ. Лицо")] ClientCategoryNotSpecified = -14,
        [Text("Не заполнено поле Наименование Клиента")] ClientNameNotSpecified = -15,
        [Text("Неверное значение поля Пункт Выдачи")] IncorrectRmsStoreId = -16,
        [Text("Неверное значение поля Регион")] IncorrectShippingAreaId = -17,
        [Text("Не заполнено поле Контактное лицо")] ContactPersonNotSpecified = -18,
        [Text("Не заполнено поле Телефон")] PhoneNotSpecified = -19,
        [Text("товар не найден")] ArticleNotFound = -20,
        [Text("товар уже существует")] ArticleAlreadyExists = -21,
        [Text("некорректные данные для сохранения записи товара")] IncorrectArticleFieldValue = -22,
        [Text("неизвестный идентификатор поставщика")] UnknownSupplierId = -30,
        [Text("неизвестный статус")] UnknownOrderLineStatus = -40,
        [Text("нет данных для передачи")] NoDataToRespond = -50,
        [Text("неизвестный номер заказа")] UnknownOrderNumber = -60,
        [Text("номер заказа уже существует")] OrderNumberAlreadyAssigned = -61,
        [Text("неизвестный код справочника")] UnknownReferenceCode = -70,
        [Text("некорректное значение поля данных заказа")] IncorrectOrderFieldValue = -80
    }
}

