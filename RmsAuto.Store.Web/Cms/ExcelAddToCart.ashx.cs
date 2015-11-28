using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Threading;

using RmsAuto.Common.Data;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.Mail;
using RmsAuto.Store.Cms.Mail.Messages;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web.Manager;

using Newtonsoft.Json;

namespace RmsAuto.Store.Web.Cms
{
    /// <summary>
    /// Summary description for ExcelAddToCart
    /// </summary>
    public class ExcelAddToCart : IHttpHandler, IRequiresSessionState
    {
        #region Private members

        private string ClientNotifyEmail
        {
            get { return ConfigurationManager.AppSettings["ClientNotifyEmail"]; }
        }

        private enum ErrorReason
        {
            CartNotEmpty,
            FileNotSpecified,
            FileNotFoundOrEmpty,
            InvalidFileFormat,
            ReaderFailed,
            InvalidOrderNumber,
            SingleSheetRequired,
            EmptyOrder,
            InvalidSupplierID,
            InvalidManufacturer,
            InvalidPartNumber,
            InvalidQuantity,
            InvalidStrictlyNumberFlag,
            ItemDoesNotExist,
            UnknownFailure,
            InvalidPrice
        }

        private string TextError(ErrorReason reason, string arg)
        {
            string msg;
            switch (reason)
            {
                case ErrorReason.CartNotEmpty:
                    msg = Resources.CartImport.ErrCartNotEmpty; /*"Загружать заказ можно только в пустую корзину";*/
                    break;
                case ErrorReason.FileNotSpecified:
                    msg = Resources.CartImport.ErrFileNotSpecified; /*"Файл для импорта заказа не указан";*/
                    break;
                case ErrorReason.InvalidFileFormat:
                    msg = Resources.CartImport.ErrInvalidFileFormat; /*"Файл заказа должен быть в формате .xls или .xlsx";+*/
                    break;
                case ErrorReason.FileNotFoundOrEmpty:
                    msg = Resources.CartImport.ErrFileNotFoundOrEmpty; /*"Файл \"{0}\" не найден или не содержит данных";*/
                    break;
                case ErrorReason.ReaderFailed:
                    msg = Resources.CartImport.ErrReaderFailed; /*"Не удалось прочитать содержимое файла \"{0}\"";*/
                    break;
                case ErrorReason.InvalidOrderNumber:
                    msg = Resources.CartImport.ErrInvalidOrderNumber; /*"В ячейке C2 должен быть указан номер заказа";*/
                    break;
                case ErrorReason.SingleSheetRequired:
                    msg = Resources.CartImport.ErrSingleSheetRequired; /*"В файле заказа должен быть только один лист, удалите лишние листы";*/
                    break;
                case ErrorReason.EmptyOrder:
                    msg = Resources.CartImport.ErrEmptyOrder; /*"На строке {0} ожидается первая строка заказа";*/
                    break;
                case ErrorReason.InvalidSupplierID:
                    msg = Resources.CartImport.ErrInvalidSupplierID; /*"Во 2-м столбце должен быть код поставщика";*/
                    break;
                case ErrorReason.InvalidManufacturer:
                    msg = Resources.CartImport.ErrInvalidManufacturer; /*"В 3-м столбце должен быть производитель";*/
                    break;
                case ErrorReason.InvalidPartNumber:
                    msg = Resources.CartImport.ErrInvalidPartNumber; /*"В 4-м столбце должен быть номер запчасти";*/
                    break;
                case ErrorReason.InvalidQuantity:
                    msg = Resources.CartImport.ErrInvalidQuantity; /*"В 5-м столбце должно быть указано количество";*/
                    break;
                case ErrorReason.InvalidStrictlyNumberFlag:
                    msg = Resources.CartImport.ErrInvalidStrictlyNumberFlag; /*"В 6-м столбце должен быть флаг \"запрет изменения номера\" ";*/
                    break;
                case ErrorReason.InvalidPrice:
                    msg = Resources.CartImport.ErrInvalidPrice; /*"В 7-м столбце не верный формат цены.";*/
                    break;
                case ErrorReason.ItemDoesNotExist:
                    msg = Resources.CartImport.ErrItemDoesNotExist; /*"Запчасть не найдена";*/
                    break;
                case ErrorReason.UnknownFailure:
                    msg = Resources.CartImport.ErrUnknownFailure; /*"Ошибка загрузки заказа ({0})";*/
                    break;
                default:
                    throw new ArgumentException("Invalid error reason");
            }
            return string.Format(msg, arg);
        }

        string importFinal = String.Empty;
        private void AddFinalError(ErrorReason reason, string arg)
        {
            importFinal = importFinal + TextError(reason, arg) + "<br />";
        }

        #endregion

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ProcessRequest(HttpContext context)
        {
            var result = new ExcelCartDataResult();
            int statusCode = 200;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);

            try
            {
                var jsonString = String.Empty;

                context.Request.InputStream.Position = 0;
                using (var inputStream = new StreamReader(context.Request.InputStream))
                {
                    jsonString = inputStream.ReadToEnd();
                }

                ExcelCartData data = JsonConvert.DeserializeObject<ExcelCartData>(jsonString);

                List<CartImportRow> rowsOK = (List<CartImportRow>)context.Session["CartImportExt_rowsOK"];
                List<CartImportRow> rowsAccept = (List<CartImportRow>)context.Session["CartImportExt_rowsAccept"];
                List<CartImportRowError> rowsER = (List<CartImportRowError>)context.Session["CartImportExt_rowsER"];

                string clientToHeader = SiteContext.Current.CurrentClient.Profile.ClientName + " (" + SiteContext.Current.CurrentClient.Profile.ClientId + ")";

                if ((rowsOK != null && rowsOK.Count > 0) || (rowsAccept != null && rowsAccept.Count > 0))
                {
                    var items = new List<ShoppingCartAddItem>();
                    List<CartImportRow> rowsCart = new List<CartImportRow>();

                    // обработка успешного списка
                    foreach (var item in data.RowsOk)
                    {
                        Guid rid = new Guid(item.Id);
                        CartImportRow CIR = rowsOK.FirstOrDefault(t => t.rowID == rid);
                        if (CIR != null)
                        {
                            // добавляем в корзину
                            items.Add(new ShoppingCartLoadItem()
                            {
                                RowN = CIR.rowN,
                                Key = new ShoppingCartKey(CIR.manufacturer, CIR.partNumber, CIR.supplierId, CIR.referenceID),
                                Quantity = CIR.quantity,
                                ReferenceID = CIR.referenceID,
                                StrictlyThisNumber = item.Strictly
                            });
                            // обновляем списки
                            rowsCart.Add(CIR);
                            rowsOK.Remove(CIR);
                        }
                    }

                    // обработка списка с превышением цен
                    foreach (var item in data.RowsAccept)
                    {
                        Guid rid = new Guid(item.Id);
                        CartImportRow CIR = rowsAccept.FirstOrDefault(t => t.rowID == rid);
                        if (CIR != null)
                        {
                            // добавляем в корзину
                            items.Add(new ShoppingCartLoadItem()
                            {
                                RowN = CIR.rowN,
                                Key = new ShoppingCartKey(CIR.manufacturer, CIR.partNumber, CIR.supplierId, CIR.referenceID),
                                Quantity = CIR.quantity,
                                ReferenceID = CIR.referenceID,
                                StrictlyThisNumber = item.Strictly,
                                priceClientYesterday = CIR.priceClientYesterday
                            });
                            // обновляем списки
                            rowsCart.Add(CIR);
                            rowsAccept.Remove(CIR);
                        }
                    }

                    string CustOrderNum = context.Session["CustOrderNum"].ToString();

                    // если есть что загрузить то грузим в корзину
                    if (items.Count > 0)
                    {
                        context.Profile["CustOrderNum"] = CustOrderNum;
                        ShoppingCart cart = SiteContext.Current.CurrentClient.Cart;
                        cart.AddRange(items.OrderBy(t => t.ReferenceID));
                        items
                            .Where(item => item.PartNotFound)
                            .Select(item => (ShoppingCartLoadItem)item)
                            .Each(item => AddFinalError(ErrorReason.ItemDoesNotExist, item.RowN));
                        int countLoad = items.Where(item => !item.PartNotFound).Count();
                        result.Message = string.Format(
                            Resources.CartImport.UploadCompleteFull
                            /*"Загрузка заказа выполнена. <br /> В корзину добавлено - {0} строк. <br />"*/, countLoad);

                        result.Quantity = SiteContext.Current.CurrentClientTotals.PartsCount;
                        result.Amount = string.Format("{0:### ### ##0.00}", SiteContext.Current.CurrentClientTotals.Total);

                        XMLExcel resFile = new XMLExcel("New Order");
                        resFile.InitOKImportMess(CustOrderNum, SiteContext.Current.CurrentClient.Profile.ClientName, Resources.Texts.RoubleShort);
                        var allRows = rowsCart.OrderBy(t => t.rowN).ToList();
                        for (int i = 0; i < allRows.Count; i++)
                        {
                            resFile.AddImportCartRow(i == 0, i == allRows.Count - 1, new XMLExcelCell[] {
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Right,
                                    CellValue = allRows[i].rowN
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allRows[i].supplierId.ToString()
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allRows[i].manufacturer
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Left,
                                    CellValue = allRows[i].partNumber
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.Number,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allRows[i].quantity.ToString()
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allRows[i].strictlyThisNumber ? Resources.Texts.Yes /*"да"*/ : ""
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allRows[i].referenceID
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allRows[i].priceCurr.ToString( new NumberFormatInfo(){NumberDecimalSeparator="."} )
                                }
                            });
                        }
                        MemoryStream ms = new MemoryStream(resFile.ToByteArray());
                        Attachment xlsFile = new Attachment(ms, "OrderLoad.xls");
                        // при запросе заказа отправляем письмо клиенту
                        if (data.SendOkEmail)
                        {
                            MailEngine.SendMailWithBccAndAttachments(new MailAddress(SiteContext.Current.CurrentClient.Profile.Email, SiteContext.Current.CurrentClient.Profile.ClientName),
                                new MailAddress(ClientNotifyEmail),
                                new Attachment[] { xlsFile }, new CartImportOK { ClientToHeader = clientToHeader });
                        }
                        else
                        {
                            MailEngine.SendMailWithAttachments(new MailAddress(ClientNotifyEmail),
                                new Attachment[] { xlsFile }, new CartImportOK { ClientToHeader = clientToHeader });
                        }
                        //if ( rowsER.Count > 0 )
                        if (rowsER.Count > 0 || rowsAccept.Count > 0) //add by Daniil 2011.12.22 (чтобы при количестве ошибок=0 в файл ошибок попадали не выбранные строки с превышением цены)
                        {
                            // инициализация файла с ошибками
                            resFile = new XMLExcel("Error load");
                            resFile.InitErrorImportMess(CustOrderNum, SiteContext.Current.CurrentClient.Profile.ClientName);
                            var allAcceptRows = rowsAccept.OrderBy(t => t.rowN).ToList();
                            var allErrRows = rowsER.OrderBy(t => t.order).ThenBy(t => t.rowN).ToList();
                            for (int i = 0; i < allAcceptRows.Count; i++)
                            {
                                // добавление строк
                                resFile.AddImportCartRow(i == 0, allErrRows.Count == 0 && i == allAcceptRows.Count - 1, new XMLExcelCell[] {
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Right,
                                    CellValue = allAcceptRows[i].rowN
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allAcceptRows[i].supplierId.ToString()
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allAcceptRows[i].manufacturer
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Left,
                                    CellValue = allAcceptRows[i].partNumber
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.Number,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allAcceptRows[i].quantity.ToString()
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allAcceptRows[i].strictlyThisNumber ? Resources.Texts.Yes /*"да"*/ : ""
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allAcceptRows[i].referenceID
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.Number,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allAcceptRows[i].priceClient.ToString( new NumberFormatInfo(){NumberDecimalSeparator="."} )
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Left,
                                    CellValue = Resources.CartImport.OrderCanceledPriceExceed /*"отменено в заказе: цена превышает допустимое отклонение"*/
                                }
                            });
                            }
                            for (int i = 0; i < allErrRows.Count; i++)
                            {
                                resFile.AddImportCartRow(allAcceptRows.Count == 0 && i == 0, i == allErrRows.Count - 1, new XMLExcelCell[] {
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Right,
                                    CellValue = allErrRows[i].rowN
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allErrRows[i].supplierId
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allErrRows[i].manufacturer
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Left,
                                    CellValue = allErrRows[i].partNumber
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allErrRows[i].quantity
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allErrRows[i].strictlyThisNumber
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allErrRows[i].referenceID
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Center,
                                    CellValue = allErrRows[i].priceClient
                                },
                                new XMLExcelCell(){
                                    CellType = XMLExcelCellType.String,
                                    cellAlignment = XMLExcelAlignment.Left,
                                    CellValue = allErrRows[i].error.Replace("<br />","; ")
                                }
                            });
                            }
                            ms = new MemoryStream(resFile.ToByteArray());
                            xlsFile = new Attachment(ms, "ErrorLoad.xls");

                            // при запросе ошибок отправляем письмо клиенту
                            if (data.SendErrorEmail)
                            {
                                MailEngine.SendMailWithBccAndAttachments(new MailAddress(SiteContext.Current.CurrentClient.Profile.Email, SiteContext.Current.CurrentClient.Profile.ClientName),
                                    new MailAddress(ClientNotifyEmail),
                                    new Attachment[] { xlsFile }, new CartImportAlert { ClientToHeader = clientToHeader });
                            }
                            else
                            {
                                MailEngine.SendMailWithAttachments(new MailAddress(ClientNotifyEmail),
                                    new Attachment[] { xlsFile }, new CartImportAlert { ClientToHeader = clientToHeader });
                            }
                        }
                        else
                        {
                            // при запросе ошибок отправляем письмо клиенту
                            if (data.SendErrorEmail)
                            {
                                //deas 24.05.2011 task4123 при отсутствии ошибок отправка соответвующего письма
                                MailEngine.SendMailWithBcc(new MailAddress(SiteContext.Current.CurrentClient.Profile.Email, SiteContext.Current.CurrentClient.Profile.ClientName),
                                    new MailAddress(ClientNotifyEmail),
                                    new CartImportNoAlert { ClientToHeader = clientToHeader });
                            }
                            else
                            {
                                MailEngine.SendMail(new MailAddress(ClientNotifyEmail),
                                    new CartImportNoAlert { ClientToHeader = clientToHeader });
                            }
                        }

                        result.Success = true;
                    }
                    else
                    {
                        // нечего загружать
                        result.Message = Resources.CartImport.UploadingFailed; /*"Загрузка заказа не удалась!";*/
                        result.Success = false;
                        //statusCode = 500;
                    }
                    // очишаем хранимые списки после загрузки
                    context.Session["CartImportExt_rowsOK"] = null;
                    context.Session["CartImportExt_rowsAccept"] = null;
                    context.Session["CartImportExt_rowsER"] = null;
                    context.Session["CartImportExt_rowsERQty"] = null;
                    context.Session["CartImportExt_parts"] = null;
                }
                else
                {
                    result.Message = Resources.CartImport.UploadingFailed; /*"Загрузка заказа не удалась!";*/
                    result.Success = false;
                    //statusCode = 500;
                }
            }
            catch
            {
                result.Success = false;
                //statusCode = 500;
            }

            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.StatusCode = statusCode;
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class ExcelCartDataResult
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public int Quantity { get; set; }

        public string Amount { get; set; }
    }

    public class ExcelCartData
    {
        public ExcelCartDataRow[] RowsOk { get; set; }

        public ExcelCartDataRow[] RowsAccept { get; set; }

        public bool SendOkEmail { get; set; }

        public bool SendErrorEmail { get; set; }
    }

    public class ExcelCartDataRow
    {
        public string Id { get; set; }

        public bool Strictly { get; set; }
    }
}