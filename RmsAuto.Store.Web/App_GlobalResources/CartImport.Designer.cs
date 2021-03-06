//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CartImport {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CartImport() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.CartImport", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отменено в заказе.
        /// </summary>
        internal static string CancelingInOrder {
            get {
                return ResourceManager.GetString("CancelingInOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Требуется изменение количества в загруженных строках.
        /// </summary>
        internal static string ChangeQuantityRequired {
            get {
                return ResourceManager.GetString("ChangeQuantityRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Требуется подтверждение.
        /// </summary>
        internal static string ConfirmationIsRequired {
            get {
                return ResourceManager.GetString("ConfirmationIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Продолжить загрузку.
        /// </summary>
        internal static string ContinueLoading {
            get {
                return ResourceManager.GetString("ContinueLoading", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отказаться от всех строк с ошибками и продолжить загрузку в корзину.
        /// </summary>
        internal static string DeclineErrorsLines {
            get {
                return ResourceManager.GetString("DeclineErrorsLines", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Загружать заказ можно только в пустую корзину.
        /// </summary>
        internal static string ErrCartNotEmpty {
            get {
                return ResourceManager.GetString("ErrCartNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to На строке {0} ожидается первая строка заказа.
        /// </summary>
        internal static string ErrEmptyOrder {
            get {
                return ResourceManager.GetString("ErrEmptyOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл \&quot;{0}\&quot; не найден или не содержит данных.
        /// </summary>
        internal static string ErrFileNotFoundOrEmpty {
            get {
                return ResourceManager.GetString("ErrFileNotFoundOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл для импорта заказа не указан.
        /// </summary>
        internal static string ErrFileNotSpecified {
            get {
                return ResourceManager.GetString("ErrFileNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл заказа должен быть в формате .xls или .xlsx.
        /// </summary>
        internal static string ErrInvalidFileFormat {
            get {
                return ResourceManager.GetString("ErrInvalidFileFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В 3-м столбце должен быть производитель.
        /// </summary>
        internal static string ErrInvalidManufacturer {
            get {
                return ResourceManager.GetString("ErrInvalidManufacturer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В ячейке C2 должен быть указан номер заказа.
        /// </summary>
        internal static string ErrInvalidOrderNumber {
            get {
                return ResourceManager.GetString("ErrInvalidOrderNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В 4-м столбце должен быть номер запчасти.
        /// </summary>
        internal static string ErrInvalidPartNumber {
            get {
                return ResourceManager.GetString("ErrInvalidPartNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В 7-м столбце не верный формат цены..
        /// </summary>
        internal static string ErrInvalidPrice {
            get {
                return ResourceManager.GetString("ErrInvalidPrice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В 5-м столбце должно быть указано количество.
        /// </summary>
        internal static string ErrInvalidQuantity {
            get {
                return ResourceManager.GetString("ErrInvalidQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В 6-м столбце должен быть флаг \&quot;запрет изменения номера\&quot;.
        /// </summary>
        internal static string ErrInvalidStrictlyNumberFlag {
            get {
                return ResourceManager.GetString("ErrInvalidStrictlyNumberFlag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Во 2-м столбце должен быть код поставщика.
        /// </summary>
        internal static string ErrInvalidSupplierID {
            get {
                return ResourceManager.GetString("ErrInvalidSupplierID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Запчасть не найдена.
        /// </summary>
        internal static string ErrItemDoesNotExist {
            get {
                return ResourceManager.GetString("ErrItemDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось прочитать содержимое файла \&quot;{0}\&quot;.
        /// </summary>
        internal static string ErrReaderFailed {
            get {
                return ResourceManager.GetString("ErrReaderFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В файле заказа должен быть только один лист, удалите лишние листы.
        /// </summary>
        internal static string ErrSingleSheetRequired {
            get {
                return ResourceManager.GetString("ErrSingleSheetRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка загрузки заказа ({0}).
        /// </summary>
        internal static string ErrUnknownFailure {
            get {
                return ResourceManager.GetString("ErrUnknownFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to и т.д.).
        /// </summary>
        internal static string etc_withRightBracket {
            get {
                return ResourceManager.GetString("etc_withRightBracket", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Несоответствие цены ФТП-прайсу.
        /// </summary>
        internal static string InvalidFtpPrice {
            get {
                return ResourceManager.GetString("InvalidFtpPrice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to список строк пуст.
        /// </summary>
        internal static string ListIsEmpty {
            get {
                return ResourceManager.GetString("ListIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибки загрузки.
        /// </summary>
        internal static string LoadingError {
            get {
                return ResourceManager.GetString("LoadingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to минимальное необходимое количество для заказа: {0}.
        /// </summary>
        internal static string MinOrderQuantity {
            get {
                return ResourceManager.GetString("MinOrderQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to отменено в заказе: цена превышает допустимое отклонение.
        /// </summary>
        internal static string OrderCanceledPriceExceed {
            get {
                return ResourceManager.GetString("OrderCanceledPriceExceed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Оформить заказ.
        /// </summary>
        internal static string PlaceOrder {
            get {
                return ResourceManager.GetString("PlaceOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Укажите файл для загрузки.
        /// </summary>
        internal static string PointFileToDownload {
            get {
                return ResourceManager.GetString("PointFileToDownload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Допустимое значение превышения цены (%).
        /// </summary>
        internal static string PrcExcessPrice {
            get {
                return ResourceManager.GetString("PrcExcessPrice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to заказанное количество превышает остатки склада, доступно: {0}.
        /// </summary>
        internal static string QuantityExceedsAvailable {
            get {
                return ResourceManager.GetString("QuantityExceedsAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Повторить загрузку.
        /// </summary>
        internal static string Reload {
            get {
                return ResourceManager.GetString("Reload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отправить список ошибок на e-mail.
        /// </summary>
        internal static string SendListErrorToEmail {
            get {
                return ResourceManager.GetString("SendListErrorToEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отправить загруженный заказ на e-mail.
        /// </summary>
        internal static string SendUploadedOrderToEmail {
            get {
                return ResourceManager.GetString("SendUploadedOrderToEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to количество должно быть кратным числу деталей в комплекте (.
        /// </summary>
        internal static string ShallDivisibleQuantity {
            get {
                return ResourceManager.GetString("ShallDivisibleQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Загрузка заказа выполнена. &lt;br /&gt; В корзину добавлено - {0} строк. &lt;br /&gt;.
        /// </summary>
        internal static string UploadCompleteFull {
            get {
                return ResourceManager.GetString("UploadCompleteFull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Успешно загружено.
        /// </summary>
        internal static string UploadedSuccessfully {
            get {
                return ResourceManager.GetString("UploadedSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Загрузка заказа не удалась!.
        /// </summary>
        internal static string UploadingFailed {
            get {
                return ResourceManager.GetString("UploadingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Загрузить в корзину.
        /// </summary>
        internal static string UploadToCart {
            get {
                return ResourceManager.GetString("UploadToCart", resourceCulture);
            }
        }
    }
}
