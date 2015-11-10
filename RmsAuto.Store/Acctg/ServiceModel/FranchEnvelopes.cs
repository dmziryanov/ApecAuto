using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    // поля класса синхронизуются с анкетой франча на C:\Projects\RmsAuto\RmsAuto.Store.Web\Controls\FillFranchProfileExt.ascx
    public class FranchEnvelope : Envelope
    {
        [XmlElement("DateTimeFranchBlankCreated")]
        public string DateTimeFranchBlankCreated; // время получения заявки
        [XmlElement("City")]
        public string City; // Город
        [XmlElement("AddressJur")]
        public string AddressJur; // Юридическое название компании
        [XmlElement("AddressGeo")]
        public string AddressGeo; // Фактический адрес торговой площади
        [XmlElement("BusinessType")]
        public string BusinessType; // Сфера деятельности
        [XmlElement("ManagerName")]
        public string ManagerName; // Контактное лицо
        [XmlElement("ManagerAppointment")]
        public string ManagerAppointment; // Должность
        [XmlElement("Phone")]
        public string Phone; // Контактный телефон
        [XmlElement("PhoneSecond")]
        public string PhoneSecond; // Контактный 2-й телефон
        [XmlElement("Email")]
        public string Email; // Электронный адрес
        [XmlElement("Site")]
        public string Site; // Web-адрес сайта компании
        [XmlElement("MainArticlePositions")]
        public string MainArticlePositions; // Основные товарные позиции
        [XmlElement("AutoparkOwn")]
        public string AutoparkOwn; // Наличие своего автопарка
        [XmlElement("FranchHistory")]
        public string FranchHistory; // Наличие бизнеса по системе Франчайзинга
        [XmlElement("PublicityHistory")]
        public string PublicityHistory; // Планирование рекламной деятельности
        [XmlElement("Competitors")]
        public string Competitors; // Присутствие конкурирующих торговых сетей в Вашем  городе
        [XmlElement("TurnoverPerYear")]
        public string TurnoverPerYear; // Товарооборот по компании за последние 12 месяцев
        [XmlElement("PartOfRegionTurnover")]
        public string PartOfRegionTurnover; // Процент своего объема продаж в Вашем регионе
        [XmlElement("EmplyeeNumber")]
        public string EmplyeeNumber; // Количество сотрудников в компании
        [XmlElement("StoreSquare")]
        public string StoreSquare; // Площадь склада
        [XmlElement("ShopSquare")]
        public string ShopSquare; // Площадь магазина
        [XmlElement("OfficeSquare")]
        public string OfficeSquare; // Площадь офиса
        [XmlElement("ArticleAvailabilityNumber")]
        public string ArticleAvailabilityNumber; // Кол-во наименований товара в наличии
        [XmlElement("ArticleOrderedNumber")]
        public string ArticleOrderedNumber; // Кол-во наименований заказных позиций
        [XmlElement("SuppliesNumber")]
        public string SuppliesNumber; // Кол-во поставщиков
        [XmlElement("WholesaleClients")]
        public string WholesaleClients; // Кол-во оптовых клиентов
        [XmlElement("RetailClients")]
        public string RetailClients; // Кол-во розничных клиентов
        [XmlElement("IncomingDocs")]
        public string IncomingDocs; // Кол-во приходных документов в месяц
        [XmlElement("OutgoingDocs")]
        public string OutgoingDocs; // Кол-во расходных документов в месяц
        [XmlElement("PaymentsOutPerMonth")]
        public string PaymentsOutPerMonth; // Кол-во оплат поставщикам в месяц
        [XmlElement("PaymentsInPerMonth")]
        public string PaymentsInPerMonth; // Кол-во оплат от покупателей в месяц
        [XmlElement("AchalandageStrategy")]
        public string AchalandageStrategy; // Способы развития клиентской базы
        [XmlElement("LabelPrinterModel")]
        public string LabelPrinterModel; // Модель принтера для этикеток
        [XmlElement("PayDescModel")]
        public string PayDescModel; // Модель кассового оборудования
        [XmlElement("KKModel")]
        public string KKModel; // Модель ККМ
        [XmlElement("BarcodeScaner")]
        public string BarcodeScaner; // Модель сканера штрихода
        [XmlElement("RegistrationSystemName")]
        public string RegistrationSystemName; // Название учетно-торговой системы
        [XmlElement("RSClientCount")]
        public string RSClientCount; // Кол-во пользователей в учетно-торговой системе
        [XmlElement("ExternalChannelSpeed")]
        public string ExternalChannelSpeed; // Скорость и качество внешнего канала связи, Mb/сек
        [XmlElement("LocalChannelSpeed")]
        public string LocalChannelSpeed; // Скорость и качество локального канала связи, Mb/сек
        [XmlElement("ClientBankName")]
        public string ClientBankName; // Наличие автоматизации клиент-банка
        [XmlElement("AutomatedStoreSystem")]
        public string AutomatedStoreSystem; // Наличие СУС
        [XmlElement("Coments")]
        public string Coments; // Дополнительная информация, комментарии, предложения и вопросы
        [XmlElement("ExternalViewFileName")]
        public string ExternalViewFileName; // Фотография, вид с улицы
        [XmlElement("ExternalViewFileValue")]
        public string ExternalViewFileValue; // 
        [XmlElement("InternalViewFileName")]
        public string InternalViewFileName; // Фотография, вид внутри помещения
        [XmlElement("InternalViewFileValue")]
        public string InternalViewFileValue; //
    }
}
