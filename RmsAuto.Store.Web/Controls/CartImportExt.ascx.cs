extern alias DataStreams1; //to make sure it is DataStreams1.dll, reference alias is set to DataStreams1
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DataStreams1.DataStreams.Common;
using DataStreams1.DataStreams.Csv;
using DataStreams1.DataStreams.Xls;
using DataStreams1.DataStreams.Xlsx;
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

namespace RmsAuto.Store.Web.Controls
{
    [Serializable()]
    public partial class CartImportExt : System.Web.UI.UserControl
    {

        #region Variable

		/// <summary>
		/// Список UserID тех пользователей, которым разрешена загрузка по "вчерашнему" прайсу.
		/// </summary>
		private List<int> SpecialUsers
		{
			get { return ((string)ConfigurationManager.AppSettings["UsersForUploadYesterdayPrices"]).Split( ';' ).Where(s => !string.IsNullOrEmpty(s)).Select( s => int.Parse( s ) ).ToList(); }
		}

		private string ClientNotifyEmail
		{
			get { return (string)ConfigurationManager.AppSettings["ClientNotifyEmail"]; }
		}

		/// <summary>
		/// Список SupplierID собственных складов наличия
		/// </summary>
		private List<int> OwnStores
		{
			get
			{
				if (SpecialUsers.Contains(SiteContext.Current.User.UserId))
				{
                    return StoreRefCatalog.RefOwnStores.Items.Select(x => x.SupplierID).ToList();
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
        /// начальная точка в файле
        /// </summary>
        private readonly Point _order_num_cell = new Point( 2, 1 );
        /// <summary>
        /// номер первой строки в файле
        /// </summary>
        private readonly int _first_row = 5;
        /// <summary>
        /// номер заказа клиента из файла
        /// </summary>
        string CustOrderNum;
        /// <summary>
        /// список успешно обработанных строк
        /// </summary>
        List<CartImportRow> rowsOK = new List<CartImportRow>();
        /// <summary>
        /// список успешно загруженных строк, но с большим отклонением по цене
        /// </summary>
        List<CartImportRow> rowsAccept = new List<CartImportRow>();
        /// <summary>
        /// список строк с ошибками (сумарно любые типы ошибок)
        /// </summary>
        List<CartImportRowError> rowsER = new List<CartImportRowError>();
        /// <summary>
        /// список строк с количественными ошибками
        /// </summary>
        List<CartImportRowERQty> rowsERQty = new List<CartImportRowERQty>();
        /// <summary>
        /// Для проверки количсевтенных показателей у сгруппированных строк
        /// </summary>
        Dictionary<SparePartPriceKey, int> groupQtyRows = new Dictionary<SparePartPriceKey, int>();

        List<SparePartFranch> parts = new List<SparePartFranch>();

        #endregion Variable

        #region PageAction

        protected void Page_Init( object sender, EventArgs e )
        {
            try
            {
                // загрузка текущих данных из сессии
                if ( Session["CartImportExt_rowsOK"] != null )
                {
                    rowsOK = (List<CartImportRow>)Session["CartImportExt_rowsOK"];
                }
                if ( Session["CartImportExt_rowsAccept"] != null )
                {
                    rowsAccept = (List<CartImportRow>)Session["CartImportExt_rowsAccept"];
                }
                if ( Session["CartImportExt_rowsER"] != null )
                {
                    rowsER = (List<CartImportRowError>)Session["CartImportExt_rowsER"];
                }
                if ( Session["CartImportExt_rowsERQty"] != null )
                {
                    rowsERQty = (List<CartImportRowERQty>)Session["CartImportExt_rowsERQty"];
                }
                if ( Session["CustOrderNum"] != null )
                {
                    CustOrderNum = Session["CustOrderNum"].ToString();
                }
                if (Session["CartImportExt_parts"] != null)
                {
                    parts = (List<SparePartFranch>)Session["CartImportExt_parts"];
                }

                _litOK.Text = Resources.CartImport.UploadedSuccessfully + " (" + rowsOK.Count.ToString() + ")";
                _litAccept.Text = Resources.CartImport.ConfirmationIsRequired + " (" + rowsAccept.Count.ToString() + ")";
                _litError.Text = Resources.CartImport.LoadingError + " (" + rowsER.Count.ToString() + ")";
            }
            catch
            {
                Session["CartImportExt_rowsOK"] = null;
                Session["CartImportExt_rowsAccept"] = null;
                Session["CartImportExt_rowsER"] = null;
                Session["CartImportExt_rowsERQty"] = null;
                rowsOK.Clear();
                rowsAccept.Clear();
                rowsER.Clear();
                rowsERQty.Clear();
            }
        }

        protected void Page_PreRender( object sender, EventArgs e )
        {
            CheckCartEmpty();
            _cartVersionValidator.ShoppingCartUrl = GetCartImportUrl();
        }

        protected void Page_Load( object sender, EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                if ( rowsERQty.Count != 0 )
                {
                    UpdateERQtyList();
                    _multiViewLoadReport.ActiveViewIndex = 1;
                } 
                else if ( ( rowsOK.Count != 0 ) || ( rowsAccept.Count != 0 ) || ( rowsER.Count != 0 ) )
                {
                    UpdateListViews();
                    _multiViewLoadReport.ActiveViewIndex = 2;
                }

                using (var DC = new DCWrappersFactory<StoreDataContext>())
                {
                    var userSet = DC.DataContext.spSelUserSetting(SiteContext.Current.User.UserId).FirstOrDefault();
                    byte PrcExcessPrice = userSet == null ? (byte)0 : userSet.PrcExcessPrice;
                    _txtPrc1.Text += string.Format(": <b>{0}</b>", PrcExcessPrice);
                }
            }
            _placeOrderBtn.PostBackUrl = GetCartUrl();
            _importFinalLiteral.Text = "";
            _importSummaryLiteral.Text = "";
            _importResultLiteral.Text = "";
        }

        /// <summary>
        /// установка всем в списке галочек загружать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAddAllOKCheckedChanged( object sender, EventArgs e )
        {
            foreach ( var item in _lvItemsOK.Items )
                ( (CheckBox)( item as Control ).FindControl( "_chkOK" ) ).Checked = ( (CheckBox)sender ).Checked;
        }

        /// <summary>
        /// установка всем в списке галочек загружать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAddAllAcceptCheckedChanged( object sender, EventArgs e )
        {
            foreach ( var item in _lvItemAccept.Items )
                ( (CheckBox)( item as Control ).FindControl( "_chkAccept" ) ).Checked = ( (CheckBox)sender ).Checked;
        }

        #endregion PageAction

        #region PageUpdates

        /// <summary>
        /// Обновление списков отображения
        /// </summary>
        private void UpdateListViews()
        {
            _lvItemsOK.DataSource = rowsOK.OrderBy( t => t.rowN );
            _lvItemsOK.DataBind();
            _lvItemAccept.DataSource = rowsAccept.OrderBy( t => t.rowN );
            _lvItemAccept.DataBind();
            _lvItemsError.DataSource = rowsER.OrderBy( t => t.order ).ThenBy( t => t.rowN );
            _lvItemsError.DataBind();
            _litOK.Text = Resources.CartImport.UploadedSuccessfully + " (" + rowsOK.Count.ToString() + ")";
            _litAccept.Text = Resources.CartImport.ConfirmationIsRequired + " (" + rowsAccept.Count.ToString() + ")";
            _litError.Text = Resources.CartImport.LoadingError + " (" + rowsER.Count.ToString() + ")";
            if ( rowsAccept.Count > 0 )
            {
                //_mvLoadItem.ActiveViewIndex = 1;
                _vItemConfirm.Visible = true;
                _vItemError.Visible = false;
                _vItemOK.Visible = false;
            }
            else if ( rowsER.Count > 0 )
            {
                //_mvLoadItem.ActiveViewIndex = 2;
                _vItemConfirm.Visible = false;
                _vItemError.Visible = true;
                _vItemOK.Visible = false;
            }
            else
            {
                //_mvLoadItem.ActiveViewIndex = 0;
                _vItemConfirm.Visible = false;
                _vItemError.Visible = false;
                _vItemOK.Visible = true;
            }
        }

        /// <summary>
        /// Обновление списка количественных ошибок
        /// </summary>
        private void UpdateERQtyList()
        {
            groupQtyRows.Clear();
            foreach ( CartImportRowERQty rERQty in rowsERQty )
            {
                SparePartPriceKey SPPK = new SparePartPriceKey( rERQty.manufacturer, rERQty.partNumber, rERQty.supplierId );
                if ( groupQtyRows.ContainsKey( SPPK ) )
                {
                    groupQtyRows[SPPK] += rERQty.quantity;
                }
                else
                {
                    groupQtyRows.Add( SPPK, rERQty.quantity );
                }
            }
            _lvERQty.DataSource = rowsERQty.OrderBy( t => t.manufacturer ).ThenBy( t => t.partNumber ).ThenBy( t => t.supplierId ).ThenBy( t => t.referenceID );
            _lvERQty.DataBind();
        }

        #endregion PageUpdates

        #region LoadExcelFile

        /// <summary>
        /// Загрузка Excel файла и его разбор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _submitButton_Click( object sender, EventArgs e )
        {
            if ( CheckCartEmpty() )
            {
                HttpPostedFile file = _orderFileUpload.PostedFile;
                if ( SiteContext.Current.CurrentClientTotals.PartsCount > 0 )
                {
                    AddError( ErrorReason.CartNotEmpty, "" );
                    return;
                }
                if ( string.IsNullOrEmpty( file.FileName ) )
                {
                    AddError( ErrorReason.FileNotSpecified, "" );
                    return;
                }
                if ( !_orderFileUpload.HasFile )
                {
                    AddError( ErrorReason.FileNotFoundOrEmpty, file.FileName );
                    return;
                }

                bool xlsx;
                switch ( Path.GetExtension( file.FileName ).ToLowerInvariant() )
                {
                    case ".xls": xlsx = false; break;
                    case ".xlsx": xlsx = true; break;
                    default:
                        {
                            AddError( ErrorReason.InvalidFileFormat, "" );
                            return;
                        }
                }

                SpreadsheetReader reader = null;
                try
                {
                    reader = xlsx ? (SpreadsheetReader)( new XlsxReader( file.InputStream ) )
                        : (SpreadsheetReader)( new XlsReader( file.InputStream ) );
                    try
                    {
                        // разбор загруженного файла
                        if (SiteContext.Current.InternalFranchName == "rmsauto")
                            LoadOrderFromReader(reader);
                        else
                        {
                            LoadOrderFromReaderFranch(reader);
                        }
                        if ( rowsERQty.Count > 0 )
                        {
                            _multiViewLoadReport.ActiveViewIndex = 1;
                            UpdateERQtyList();
                        }
                        else
                        {
                            _multiViewLoadReport.ActiveViewIndex = 2;
                            UpdateListViews();
                        }
                        Session["CartImportExt_rowsOK"] = rowsOK;
                        Session["CartImportExt_rowsAccept"] = rowsAccept;
                        Session["CartImportExt_rowsER"] = rowsER;
                        Session["CartImportExt_rowsERQty"] = rowsERQty;
                    }
                    catch ( Exception ex )
                    {
                        AddError(ErrorReason.UnknownFailure, ex.Message.ToString());
                    }
                }
                catch
                {
                    AddError( ErrorReason.ReaderFailed, file.FileName );
                }
                finally
                {
                    if ( reader != null ) reader.Close();
                }
            }
        }

        private void LoadOrderFromReaderFranch(SpreadsheetReader reader)
        {
            Dictionary<SparePartPriceKey, int> groupRows = new Dictionary<SparePartPriceKey, int>();
            //проверка наличия в файле единственного листа с заказом
            reader.CurrentSheet = 0;
            if (reader.SheetCount > 1)
            {
                AddError(ErrorReason.SingleSheetRequired, "");
                return;
            }
            if (reader.RecordCount <= _first_row)
            {
                AddError(ErrorReason.EmptyOrder, _first_row.ToString());
                return;
            }
            //импорт номера заказа
            try
            {
                string orderNum = reader[_order_num_cell.Y, _order_num_cell.X];
                CustOrderNum = (orderNum ?? "").Trim();
                Session["CustOrderNum"] = CustOrderNum;
            }
            catch
            {
                AddError(ErrorReason.InvalidOrderNumber, "");
                return;
            }
            int ItemsRead = 0;
            rowsOK.Clear();
            rowsAccept.Clear();
            rowsER.Clear();
            rowsERQty.Clear();
            parts.Clear();
            //импорт строк заказа
            using (var FDC = new DCWrappersFactory<StoreDataContext>())
            {
                //StoreDataContext DC = new StoreDataContext();
                StoreDataContext DC = FDC.DataContext;
                var userSet = DC.spSelUserSetting(SiteContext.Current.User.UserId).FirstOrDefault();
                RmsAuto.Acctg.ClientGroup _clientGroup = SiteContext.Current.CurrentClient.Profile.ClientGroup;
                decimal _personalMarkup = SiteContext.Current.CurrentClient.Profile.PersonalMarkup;
                byte PrcExcessPrice = userSet == null ? (byte)0 : userSet.PrcExcessPrice;
                _txtPrc.Text = string.Format("{0}: <b>{1}</b>", Resources.CartImport.PrcExcessPrice, PrcExcessPrice);

                for (int r = _first_row; r < reader.RecordCount; r++)
                {
                    string[] rec = {
                    reader[r, 0], reader[r, 1], reader[r, 2],
                    reader[r, 3], reader[r, 4], reader[r, 5],
                    reader[r, 6], reader[r, 7] };

                    rec = rec.Each(s => s.Trim()).ToArray();

                    if (rec.All(s => s == ""))
                        continue;

                    if (rec.All(s => string.IsNullOrEmpty(s)))
                    {
                        if (r == _first_row)
                        {
                            AddError(ErrorReason.EmptyOrder, r.ToString());
                        }
                        break;
                    }
                    ItemsRead++;
                    bool recIsValid = true;
                    string errorText = "";
                    int supplierId = 0;
                    if (!int.TryParse(rec[1], out supplierId))
                    {
                        errorText += TextError(ErrorReason.InvalidSupplierID, "") + "<br />";
                        recIsValid = false;
                    }
                    string manufacturer = rec[2];
                    if (string.IsNullOrEmpty(manufacturer))
                    {
                        errorText += TextError(ErrorReason.InvalidPartNumber, "") + "<br />";
                        recIsValid = false;
                    }
                    string partNumber = rec[3];
                    if (string.IsNullOrEmpty(partNumber))
                    {
                        errorText += TextError(ErrorReason.InvalidPartNumber, "") + "<br />";
                        recIsValid = false;
                    }
                    int quantity = 0;
                    if (!int.TryParse(rec[4], out quantity))
                    {
                        errorText += TextError(ErrorReason.InvalidQuantity, "") + "<br />";
                        recIsValid = false;
                    }
                    string referenceID = rec[6] ?? "";
                    if (referenceID.Length > ShoppingCartAddItem.ReferenceIDLength)
                    {
                        referenceID = referenceID.Substring(0, ShoppingCartAddItem.ReferenceIDLength);
                    }
                    bool strictlyThisNumber = !string.IsNullOrEmpty(rec[5]);
                    decimal priceClient = 0M;
                    if (!string.IsNullOrEmpty(rec[7]) && !decimal.TryParse(rec[7], out priceClient))
                    {
                        errorText += TextError(ErrorReason.InvalidPrice, "") + "<br />";
                        recIsValid = false;
                    }

                    SparePartFranch part = null;
                    // проверка существования элемента
                    if (recIsValid)
                    {
                        try
                        {
                            SparePartPriceKey SPPK = new SparePartPriceKey(manufacturer, partNumber, supplierId);
                            //part = SparePartsDac.Load(DC, SPPK);
                            part = SparePartsDacFranch.Load(DC, SPPK);
                            if (part == null)
                            {
                                recIsValid = false;
                                errorText += TextError(ErrorReason.ItemDoesNotExist, "") + "<br />";
                            }
                        }
                        catch (ArgumentNullException e)
                        {
                            recIsValid = false;
                            errorText += TextError(ErrorReason.ItemDoesNotExist, "") + "<br />";
                        }
                        catch (InvalidOperationException e)
                        {
                            recIsValid = false;
                            errorText += TextError(ErrorReason.ItemDoesNotExist, "") + "<br />";
                        }
                        catch (Exception e)
                        {
                            Response.Write(e.StackTrace.ToString());
                            errorText += e.StackTrace.ToString();
                        }
                    }
                    if (recIsValid)
                    {
                        SparePartPriceKey SPPK = new SparePartPriceKey(manufacturer, partNumber, supplierId);
                        // набор сгруппированной корзины для количественного анализа
                        if (groupRows.ContainsKey(SPPK))
                        {
                            groupRows[SPPK] += quantity;
                        }
                        else
                        {
                            groupRows.Add(SPPK, quantity);
                        }

                        parts.Add(part);

                        // проверка цены
                        decimal priceCurr = part.GetFinalSalePrice(_clientGroup, _personalMarkup);
                        decimal pPrc = priceClient == 0 ? 0M : (priceCurr - priceClient) * 100 / priceClient;
                        // формирование положительного и подверждающего списков
                        if (/*priceClient == 0 ||*/ (priceClient + Math.Round(priceClient * PrcExcessPrice / 100, 2) >= priceCurr))
                        {
                            rowsOK.Add(new CartImportRow
                            {
                                rowID = Guid.NewGuid(),
                                manufacturer = manufacturer,
                                partNumber = partNumber,
                                supplierId = supplierId,
                                referenceID = referenceID,
                                deliveryPeriod = part.DisplayDeliveryDaysMin.ToString() + "-" + part.DisplayDeliveryDaysMax.ToString(),
                                itemName = part.PartDescription,
                                priceClient = priceClient,
                                priceCurr = priceCurr,
                                pricePrc = string.Format("{0:#####0.00}", pPrc),
                                quantity = quantity,
                                rowN = rec[0] ?? "",
                                strictlyThisNumber = strictlyThisNumber
                            });
                        }
                        else
                        {
                            rowsAccept.Add(new CartImportRow
                            {
                                rowID = Guid.NewGuid(),
                                manufacturer = manufacturer,
                                partNumber = partNumber,
                                supplierId = supplierId,
                                referenceID = referenceID,
                                deliveryPeriod = part.DisplayDeliveryDaysMin.ToString() + "-" + part.DisplayDeliveryDaysMax.ToString(),
                                itemName = part.PartDescription,
                                priceClient = priceClient,
                                priceCurr = priceCurr,
                                pricePrc = priceClient == 0 ? "100.00" : (PrcExcessPrice == 0
                                    ? pPrc < 0.01M ? "<0.01" : string.Format("{0:#####0.00}", pPrc)
                                    : pPrc > PrcExcessPrice && pPrc < (PrcExcessPrice + 0.01M)
                                        ? string.Format(">{0:#0.00}", PrcExcessPrice)
                                        : string.Format("{0:#####0.00}", pPrc)),
                                quantity = quantity,
                                rowN = rec[0] ?? "",
                                strictlyThisNumber = strictlyThisNumber
                            });
                        }
                    }
                    else
                    {
                        rowsER.Add(new CartImportRowError()
                        {
                            order = 20,
                            error = errorText,
                            manufacturer = rec[2],
                            partNumber = rec[3],
                            supplierId = rec[1],
                            priceClient = rec[7],
                            quantity = rec[4],
                            referenceID = rec[6],
                            rowN = rec[0],
                            strictlyThisNumber = rec[5]
                        });
                    }
                }

                //количественные проверки
                foreach (SparePartPriceKey SPPK in groupRows.Keys)
                {
                    string errorText = "";
                    //SparePartFranch part = SparePartsDac.Load(DC, SPPK);
                    //SparePartFranch part = SparePartsDacFranch.Load(DC, SPPK);
                    SparePartFranch part = parts.Where(x => x.Manufacturer.ToLower() == SPPK.Mfr.ToLower() && x.PartNumber.ToLower() == SPPK.PN.ToLower() && x.SupplierID == SPPK.SupplierId).SingleOrDefault();

                    if (groupRows[SPPK] < part.DefaultOrderQty)
                    {
                        // Минимальное необходимое количество для заказа
                        errorText += String.Format(
                            Resources.CartImport.MinOrderQuantity
                            /*"минимальное необходимое количество для заказа: {0}"*/,
                            part.MinOrderQty);
                    }
                    else if (groupRows[SPPK] % part.DefaultOrderQty != 0)
                    {
                        // Количество должно быть кратным числу деталей в комплекте
                        errorText += Resources.CartImport.ShallDivisibleQuantity /*"количество должно быть кратным числу деталей в комплекте ("*/ +
                        part.DefaultOrderQty
                        .Progression(part.DefaultOrderQty, 5)
                        .Select(i => i.ToString())
                        .Aggregate((acc, s) => acc + "," + s) + Resources.CartImport.etc_withRightBracket /*" и т.д.)"*/;
                    }
                    if (part.QtyInStock.GetValueOrDefault() > 0 && groupRows[SPPK] > part.QtyInStock)
                    {
                        // Количество превышает допустимый лимит
                        errorText += String.Format(
                            Resources.CartImport.QuantityExceedsAvailable
                            /*"заказанное количество превышает остатки склада, доступно: {0}"*/,
                            part.QtyInStock);
                    }
                    if (!string.IsNullOrEmpty(errorText))
                    {
                        // при наличии ошибок перекидываем из успешных списков в список количественных ошибок
                        var tAcc = rowsAccept.Where(t => t.manufacturer == SPPK.Mfr && t.partNumber == SPPK.PN && t.supplierId == SPPK.SupplierId).ToList();
                        AddToERQty(false, errorText, rowsAccept, tAcc);
                        var tOK = rowsOK.Where(t => t.manufacturer == SPPK.Mfr && t.partNumber == SPPK.PN && t.supplierId == SPPK.SupplierId).ToList();
                        AddToERQty(true, errorText, rowsOK, tOK);
                    }
                }
            }
        }

        /// <summary>
        /// Разбор Excel файла
        /// </summary>
        /// <param name="reader">объект чтения</param>
        private void LoadOrderFromReader( SpreadsheetReader reader )
        {
            Dictionary<SparePartPriceKey, int> groupRows = new Dictionary<SparePartPriceKey, int>();
            //проверка наличия в файле единственного листа с заказом
            reader.CurrentSheet = 0;
            if (reader.SheetCount > 1)
            {
                AddError(ErrorReason.SingleSheetRequired, "");
                return;
            }
            if (reader.RecordCount <= _first_row)
            {
                AddError(ErrorReason.EmptyOrder, _first_row.ToString() );
                return;
            }
            //импорт номера заказа
            try
            {
                string orderNum = reader[_order_num_cell.Y, _order_num_cell.X];
                CustOrderNum = (orderNum ?? "").Trim();
                Session["CustOrderNum"] = CustOrderNum;
            }
            catch
            {
                AddError(ErrorReason.InvalidOrderNumber, "");
                return;
            }
            int ItemsRead = 0;
            rowsOK.Clear();
            rowsAccept.Clear();
            rowsER.Clear();
            rowsERQty.Clear();
            //импорт строк заказа
            using (var DC = new DCWrappersFactory<StoreDataContext>())
            {
                var userSet = DC.DataContext.spSelUserSetting(SiteContext.Current.User.UserId).FirstOrDefault();
                RmsAuto.Acctg.ClientGroup _clientGroup = SiteContext.Current.CurrentClient.Profile.ClientGroup;
                decimal _personalMarkup = SiteContext.Current.CurrentClient.Profile.PersonalMarkup;
                byte PrcExcessPrice = userSet == null ? (byte)0 : userSet.PrcExcessPrice;
                _txtPrc.Text = string.Format(Resources.CartImport.PrcExcessPrice + ": <b>{0}</b>", PrcExcessPrice);

                CheckExcelRestrictions(reader.RecordCount - 5/* т.к. в шапке 5 строк, а reader.RecordCount - общее кол-во строк в файле */);

                for (int r = _first_row; r < reader.RecordCount; r++)
                {
                    string[] rec = {
                        reader[r, 0], reader[r, 1], reader[r, 2],
                        reader[r, 3], reader[r, 4], reader[r, 5],
                        reader[r, 6], reader[r, 7] };

                    rec = rec.Each(s => s.Trim()).ToArray();

                    if (rec.All(s => s == ""))
                        continue;

                    if (rec.All(s => string.IsNullOrEmpty(s)))
                    {
                        if (r == _first_row)
                        {
                            AddError(ErrorReason.EmptyOrder, r.ToString());
                        }
                        break;
                    }
                    ItemsRead++;
                    bool recIsValid = true;
                    string errorText = "";
                    int supplierId = 0;
                    if (!int.TryParse(rec[1], out supplierId))
                    {
                        errorText += TextError(ErrorReason.InvalidSupplierID, "") + "<br />";
                        recIsValid = false;
                    }
                    string manufacturer = rec[2];
                    if (string.IsNullOrEmpty(manufacturer))
                    {
                        errorText += TextError(ErrorReason.InvalidPartNumber, "") + "<br />";
                        recIsValid = false;
                    }
                    string partNumber = rec[3];
                    if (string.IsNullOrEmpty(partNumber))
                    {
                        errorText += TextError(ErrorReason.InvalidPartNumber, "") + "<br />";
                        recIsValid = false;
                    }
                    int quantity = 0;
                    if (!int.TryParse(rec[4], out quantity))
                    {
                        errorText += TextError(ErrorReason.InvalidQuantity, "") + "<br />";
                        recIsValid = false;
                    }
                    string referenceID = rec[6] ?? "";
                    if (referenceID.Length > ShoppingCartAddItem.ReferenceIDLength)
                    {
                        referenceID = referenceID.Substring(0, ShoppingCartAddItem.ReferenceIDLength);
                    }
                    bool strictlyThisNumber = !string.IsNullOrEmpty(rec[5]);
                    decimal priceClient = 0M;
                    if (!string.IsNullOrEmpty(rec[7]) && !decimal.TryParse(rec[7], out priceClient))
                    {
                        errorText += TextError(ErrorReason.InvalidPrice, "") + "<br />";
                        recIsValid = false;
                    }
                    SparePartFranch part = null;
                    // проверка существования элемента
                    if (recIsValid)
                    {
                        try
                        {
                            SparePartPriceKey SPPK = new SparePartPriceKey(manufacturer, partNumber, supplierId);
                            part = SparePartsDac.Load(DC.DataContext, SPPK);
                            if (part == null)
                            {
                                recIsValid = false;
                                errorText += TextError(ErrorReason.ItemDoesNotExist, "") + "<br />";
                            }
                        }
                        catch (ArgumentNullException e)
                        {
                            recIsValid = false;
                            errorText += TextError(ErrorReason.ItemDoesNotExist, "") + "<br />";
                        }
                        catch (InvalidOperationException e)
                        {
                            recIsValid = false;
                            errorText += TextError(ErrorReason.ItemDoesNotExist, "") + "<br />";
                        }
                    }
                    if (recIsValid)
                    {
                        SparePartPriceKey SPPK = new SparePartPriceKey(manufacturer, partNumber, supplierId);
                        // набор сгруппированной корзины для количественного анализа
                        if (groupRows.ContainsKey(SPPK))
                        {
                            groupRows[SPPK] += quantity;
                        }
                        else
                        {
                            groupRows.Add(SPPK, quantity);
                        }
                        // проверка цены
                        decimal priceCurr = part.GetFinalSalePrice(_clientGroup, _personalMarkup);
                        decimal pPrc = priceClient == 0 ? 0M : (priceCurr - priceClient) * 100 / priceClient;
                        // формирование положительного и подверждающего списков
                        if ( /*priceClient == 0 ||*/ (priceClient + Math.Round(priceClient * PrcExcessPrice / 100, 2) >= priceCurr))
                        {
                            rowsOK.Add(new CartImportRow
                            {
                                rowID = Guid.NewGuid(),
                                manufacturer = manufacturer,
                                partNumber = partNumber,
                                supplierId = supplierId,
                                referenceID = referenceID,
                                deliveryPeriod = part.DisplayDeliveryDaysMin.ToString() + "-" + part.DisplayDeliveryDaysMax.ToString(),
                                itemName = part.PartDescription,
                                priceClient = priceClient,
                                priceCurr = priceCurr,
                                pricePrc = string.Format("{0:#####0.00}", pPrc),
                                quantity = quantity,
                                rowN = rec[0] ?? "",
                                strictlyThisNumber = strictlyThisNumber
                            });
                        }
                        else
                        {
                            // Если обнаружен факт превышения цены, то проверяем является ли текущий пользователь
                            // "особым" пользователем, который имеет право загружать детали по "вчерашней" цене
                            // и если это так то вычисляем "вчерашнюю" цену добавляем к ней допустимый % превышения
                            // для этого пользователя и записываем в специальное поле (если оно заполнено то в цену
                            // в заказ пойдет цена из этого поля)
                            decimal? priceClientYesterday = null;
                            bool isErrorPrice = false;
                            if (SpecialUsers.Contains(SiteContext.Current.User.UserId) && OwnStores != null && OwnStores.Contains(part.SupplierID))
                            {
                                using (var dc = new dcCommonDataContext())
                                {
                                    try
                                    {
                                        //получаем список "вчерашних" и сегодняшних цен фтп для сравнения
                                        //если загрузка производится в понедельник, то "вчерашние" цены фтп должны браться по пятнице, субботе и воскресенью
                                        #region === old stored procedure ===
                                        //string queryTemplate = "exec srvdb4.az.dbo.spGetPricesOnDate @Number = '{0}', @Brand = '{1}', @Date = '{2}', @PriceColumn = {3}";
                                        #endregion
                                        string queryTemplate = "exec srvdb4.az.dbo.spGetPricesOnPeriod @Number='{0}', @Brand='{1}', @DateFrom='{2}', @DateTo='{3}', @PriceColumn={4}";
                                        DateTime dateFrom = (DateTime.Now.DayOfWeek == DayOfWeek.Monday) ? DateTime.Now.AddDays(-3) : DateTime.Now.AddDays(-1);
                                        DateTime dateTo = DateTime.Now;
                                        List<decimal> prices = dc.ExecuteQuery<decimal>(
                                            string.Format(queryTemplate,
                                                part.PartNumber,
                                                part.Manufacturer,
                                                dateFrom.ToString("yyyyMMdd"),
                                                dateTo.ToString("yyyyMMdd"),
                                                (int)SiteContext.Current.CurrentClient.Profile.ClientGroup))
                                            .ToList<decimal>();

                                        //для каждой из цен производим сравнение с ценой клиента в 'Excel' +- 1 копейка (чтобы учесть возможные погрешности при округлении)
                                        foreach (var price in prices)
                                        {
                                            decimal pExcell = Math.Round(priceClient, 2);	//цена клиента в 'Excel'
                                            decimal pFtp = Math.Round(price, 2);			//одна из вчерашних цен нашего Ftp
                                            if (pExcell > 0 && (pExcell == pFtp ||
                                                pExcell == (pFtp - 0.01M) ||
                                                pExcell == (pFtp + 0.01M)))
                                            {
                                                //добавляем допустимый % превышения (доли копеек отбрасываем)
                                                priceClientYesterday = pFtp + pFtp * PrcExcessPrice / 100;
                                                priceClientYesterday = decimal.Truncate((decimal)priceClientYesterday * 100) / 100;
                                                break;
                                            }
                                        }
                                        //если ни одного совпадения не найдено, то позиция отваливается с ошибкой "Несоответствие цены ФТП-прайсу"
                                        isErrorPrice = (prices.Count() > 0 && !priceClientYesterday.HasValue);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.WriteError(@"При вычислении 'Вчерашней' цены произошла ошибка!", EventLogerID.UnknownError, EventLogerCategory.UnknownCategory, ex);
                                    }
                                    finally
                                    {
                                        if (dc.Connection.State == System.Data.ConnectionState.Open)
                                            dc.Connection.Close();
                                    }
                                }
                            }
                            if (isErrorPrice)
                            {
                                rowsER.Add(new CartImportRowError()
                                {
                                    order = 20,
                                    error = Resources.CartImport.InvalidFtpPrice /*"Несоответствие цены ФТП-прайсу"*/,
                                    manufacturer = rec[2],
                                    partNumber = rec[3],
                                    supplierId = rec[1],
                                    priceClient = rec[7],
                                    quantity = rec[4],
                                    referenceID = rec[6],
                                    rowN = rec[0],
                                    strictlyThisNumber = rec[5]
                                });
                            }
                            else
                            {
                                rowsAccept.Add(new CartImportRow
                                {
                                    rowID = Guid.NewGuid(),
                                    manufacturer = manufacturer,
                                    partNumber = partNumber,
                                    supplierId = supplierId,
                                    referenceID = referenceID,
                                    deliveryPeriod = part.DisplayDeliveryDaysMin.ToString() + "-" + part.DisplayDeliveryDaysMax.ToString(),
                                    itemName = part.PartDescription,
                                    priceClient = priceClient,
                                    priceCurr = priceCurr,
                                    pricePrc = priceClient == 0 ? "100.00" : (PrcExcessPrice == 0
                                        ? pPrc < 0.01M ? "<0.01" : string.Format("{0:#####0.00}", pPrc)
                                        : pPrc > PrcExcessPrice && pPrc < (PrcExcessPrice + 0.01M)
                                            ? string.Format(">{0:#0.00}", PrcExcessPrice)
                                            : string.Format("{0:#####0.00}", pPrc)),
                                    quantity = quantity,
                                    rowN = rec[0] ?? "",
                                    strictlyThisNumber = strictlyThisNumber,
                                    priceClientYesterday = priceClientYesterday
                                });
                            }
                        }
                    }
                    else
                    {
                        rowsER.Add(new CartImportRowError()
                        {
                            order = 20,
                            error = errorText,
                            manufacturer = rec[2],
                            partNumber = rec[3],
                            supplierId = rec[1],
                            priceClient = rec[7],
                            quantity = rec[4],
                            referenceID = rec[6],
                            rowN = rec[0],
                            strictlyThisNumber = rec[5]
                        });
                    }
                }
                // количественные проверки
                foreach (SparePartPriceKey SPPK in groupRows.Keys)
                {
                    string errorText = "";
                    SparePartFranch part = SparePartsDac.Load(DC.DataContext, SPPK);
                    if (groupRows[SPPK] < part.DefaultOrderQty)
                    {
                        // Минимальное необходимое количество для заказа
                        errorText += String.Format(
                            Resources.CartImport.MinOrderQuantity
                            /*"минимальное необходимое количество для заказа: {0}"*/,
                            part.MinOrderQty);
                    }
                    else if (groupRows[SPPK] % part.DefaultOrderQty != 0)
                    {
                        // Количество должно быть кратным числу деталей в комплекте
                        errorText += Resources.CartImport.ShallDivisibleQuantity /*"количество должно быть кратным числу деталей в комплекте ("*/ +
                        part.DefaultOrderQty
                        .Progression(part.DefaultOrderQty, 5)
                        .Select(i => i.ToString())
                        .Aggregate((acc, s) => acc + "," + s) + Resources.CartImport.etc_withRightBracket /*" и т.д.)"*/;
                    }
                    if (part.QtyInStock.GetValueOrDefault() > 0 && groupRows[SPPK] > part.QtyInStock)
                    {
                        // Количество превышает допустимый лимит
                        errorText += String.Format(
                            Resources.CartImport.QuantityExceedsAvailable
                            /*"заказанное количество превышает остатки склада, доступно: {0}"*/,
                            part.QtyInStock);
                    }
                    if (!string.IsNullOrEmpty(errorText))
                    {
                        // при наличии ошибок перекидываем из успешных списков в список количественных ошибок
                        var tAcc = rowsAccept.Where(t => t.manufacturer == SPPK.Mfr && t.partNumber == SPPK.PN && t.supplierId == SPPK.SupplierId).ToList();
                        AddToERQty(false, errorText, rowsAccept, tAcc);
                        var tOK = rowsOK.Where(t => t.manufacturer == SPPK.Mfr && t.partNumber == SPPK.PN && t.supplierId == SPPK.SupplierId).ToList();
                        AddToERQty(true, errorText, rowsOK, tOK);
                    }
                }
            }
        }

        #endregion LoadExcelFile

        #region CheckQuantity

        
        /// <summary>
        /// Продолжение загрузки с удалением всего что не прошло по количествам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbResumeWithDel_Click( object sender, EventArgs e )
        {
            lbResume_Click( null, null );
        }

        /// <summary>
        /// Продолжение загрузки после количественных проверок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbResume_Click( object sender, EventArgs e )
        {
            if ( sender == null )
            {
                foreach ( CartImportRowERQty CIREQ in rowsERQty )
                {
                    rowsER.Add( new CartImportRowError()
                    {
                        order = 10,
                        error = Resources.CartImport.CancelingInOrder + /*"отменено в заказе: "*/ ": " + CIREQ.errorText,
                        manufacturer = CIREQ.manufacturer,
                        partNumber = CIREQ.partNumber,
                        supplierId = CIREQ.supplierId.ToString(),
                        priceClient = CIREQ.priceClient.ToString( new NumberFormatInfo() { NumberDecimalSeparator = "," } ),
                        quantity = CIREQ.quantity.ToString(),
                        referenceID = CIREQ.referenceID,
                        rowN = CIREQ.rowN,
                        strictlyThisNumber = CIREQ.strictlyThisNumber ? Resources.Texts.Yes/*"да"*/ : Resources.Texts.No/*"нет"*/
                    } );
                }
                rowsERQty.Clear();
            }
            else
            {
                // обработка выведенной таблицы
                foreach ( var item in _lvERQty.Items )
                {
                    HiddenField HF = (HiddenField)( item as Control ).FindControl( "hfRowIDQty" );
                    Guid rid = new Guid( HF.Value.ToString() );
                    CartImportRowERQty CIREQ = rowsERQty.FirstOrDefault( t => t.rowID == rid );
                    if ( CIREQ != null )
                    {
                        TextBox TB = (TextBox)( item as Control ).FindControl( "_tbQuantity" );
                        try
                        {
                            // изменяем количество в строке
                            int newQty = string.IsNullOrEmpty( TB.Text ) ? 0 : Convert.ToInt32( TB.Text );
                            int oldQty = CIREQ.quantity;
                            if ( newQty == 0 )
                            {
                                rowsERQty.Remove( CIREQ );
                            }
                            else
                            {
                                CIREQ.quantity = newQty;
                            }
                            // Добавляем отброшенное количество в список ошибок
                            if ( oldQty != newQty )
                            {
                                rowsER.Add( new CartImportRowError()
                                {
                                    order = 10,
                                    error =  Resources.CartImport.CancelingInOrder + /*"отменено в заказе: "*/ ": " + CIREQ.errorText,
                                    manufacturer = CIREQ.manufacturer,
                                    partNumber = CIREQ.partNumber,
                                    supplierId = CIREQ.supplierId.ToString(),
                                    priceClient = CIREQ.priceClient.ToString( new NumberFormatInfo() { NumberDecimalSeparator = "," } ),
                                    quantity = ( oldQty - newQty ).ToString(),
                                    referenceID = CIREQ.referenceID,
                                    rowN = CIREQ.rowN,
									strictlyThisNumber = CIREQ.strictlyThisNumber ? Resources.Texts.Yes/*"да"*/ : Resources.Texts.No/*"нет"*/
                                } );
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                // Собираем сумарные остатки
                Dictionary<SparePartPriceKey, int> groupRows = new Dictionary<SparePartPriceKey, int>();
                var DC = new DCWrappersFactory<StoreDataContext>();
                foreach ( CartImportRowERQty rERQty in rowsERQty )
                {
                    SparePartPriceKey SPPK = new SparePartPriceKey( rERQty.manufacturer, rERQty.partNumber, rERQty.supplierId );
                    if ( groupRows.ContainsKey( SPPK ) )
                    {
                        groupRows[SPPK] += rERQty.quantity;
                    }
                    else
                    {
                        groupRows.Add( SPPK, rERQty.quantity );
                    }
                }
                // количественная проверка с новой простановкой статусов
                // может уже поплыло по другим показателям
                foreach ( SparePartPriceKey SPPK in groupRows.Keys )
                {
                    string errorText = "";
					SparePartFranch part = null;
					if (SiteContext.Current.InternalFranchName == "rmsauto")
					{
						part = SparePartsDac.Load(DC.DataContext, SPPK);
					}
					else
					{
						part = SparePartsDacFranch.Load(DC.DataContext, SPPK);
					}
                    if ( groupRows[SPPK] < part.DefaultOrderQty )
                    {
                        // Минимальное необходимое количество для заказа
                        errorText += String.Format(
							Resources.CartImport.MinOrderQuantity
                            /*"минимальное необходимое количество для заказа: {0}"*/,
                            part.MinOrderQty );
                    }
                    else if ( groupRows[SPPK] % part.DefaultOrderQty != 0 )
                    {
                        // Количество должно быть кратным числу деталей в комплекте
                        errorText += Resources.CartImport.ShallDivisibleQuantity /*"количество должно быть кратным числу деталей в комплекте ("*/ +
                        part.DefaultOrderQty
                        .Progression( part.DefaultOrderQty, 5 )
                        .Select( i => i.ToString() )
                        .Aggregate( ( acc, s ) => acc + "," + s ) + Resources.CartImport.etc_withRightBracket/*" и т.д.)"*/;
                    }
                    if ( part.QtyInStock.GetValueOrDefault() > 0 && groupRows[SPPK] > part.QtyInStock )
                    {
                        // Количество превышает допустимый лимит
                        errorText += String.Format(
							Resources.CartImport.QuantityExceedsAvailable
                            /*"заказанное количество превышает остатки склада, доступно: {0}"*/,
                            part.QtyInStock );
                    }
                    if ( string.IsNullOrEmpty( errorText ) )
                    {
                        // при отсутсвии ошибок переносим в положительные списки
                        FromERQty( SPPK );
                    }
                    else
                    {
                        // обновляем текст ошибки
                        rowsERQty.Where( t => t.manufacturer == SPPK.Mfr && t.partNumber == SPPK.PN && t.supplierId == SPPK.SupplierId )
                            .Each( t => t.errorText = errorText );
                    }
                }
            }
            if ( rowsERQty.Count == 0 )
            {
                UpdateListViews();
                _multiViewLoadReport.ActiveViewIndex = 2;
            }
            else
            {
                UpdateERQtyList();
            }
        }

        /// <summary>
        /// Перенос детали из количественных ошибок в положительные списки
        /// </summary>
        /// <param name="SPPK">Ключ детали</param>
        private void FromERQty( SparePartPriceKey SPPK )
        {
            var tAll = rowsERQty.Where( t => t.manufacturer == SPPK.Mfr && t.partNumber == SPPK.PN && t.supplierId == SPPK.SupplierId ).ToList();
            foreach ( var oneItem in tAll )
            {
                rowsERQty.Remove( oneItem );
                if ( oneItem.isOK )
                {
                    rowsOK.Add( new CartImportRow()
                    {
                        rowID = Guid.NewGuid(),
                        manufacturer = oneItem.manufacturer,
                        partNumber = oneItem.partNumber,
                        supplierId = oneItem.supplierId,
                        referenceID = oneItem.referenceID,
                        deliveryPeriod = oneItem.deliveryPeriod,
                        itemName = oneItem.itemName,
                        priceClient = oneItem.priceClient,
                        priceCurr = oneItem.priceCurr,
                        pricePrc = oneItem.pricePrc,
                        quantity = oneItem.quantity,
                        rowN = oneItem.rowN,
                        strictlyThisNumber = oneItem.strictlyThisNumber
                    } );
                }
                else
                {
                    rowsAccept.Add( new CartImportRow()
                    {
                        rowID = Guid.NewGuid(),
                        manufacturer = oneItem.manufacturer,
                        partNumber = oneItem.partNumber,
                        supplierId = oneItem.supplierId,
                        referenceID = oneItem.referenceID,
                        deliveryPeriod = oneItem.deliveryPeriod,
                        itemName = oneItem.itemName,
                        priceClient = oneItem.priceClient,
                        priceCurr = oneItem.priceCurr,
                        pricePrc = oneItem.pricePrc,
                        quantity = oneItem.quantity,
                        rowN = oneItem.rowN,
                        strictlyThisNumber = oneItem.strictlyThisNumber
                    } );
                }
            }
        }

        /// <summary>
        /// При наличии ошибок в количестве разбор ошибок и перенос строк в таблицу количественных ошибок
        /// </summary>
        /// <param name="isOK">это положительный список не требующий подтверждения</param>
        /// <param name="errorText">текст ошибки</param>
        /// <param name="tAll">ссылка на список</param>
        /// <param name="tWork">отфильтрованный список</param>
        private void AddToERQty( bool isOK, string errorText, List<CartImportRow> tAll, List<CartImportRow> tWork )
        {
            foreach ( var oneItem in tWork )
            {
                tAll.Remove( oneItem );
                rowsERQty.Add( new CartImportRowERQty()
                {
                    rowID = Guid.NewGuid(),
                    isOK = isOK,
                    errorText = errorText,
                    manufacturer = oneItem.manufacturer,
                    partNumber = oneItem.partNumber,
                    supplierId = oneItem.supplierId,
                    referenceID = oneItem.referenceID,
                    deliveryPeriod = oneItem.deliveryPeriod,
                    itemName = oneItem.itemName,
                    priceClient = oneItem.priceClient,
                    priceCurr = oneItem.priceCurr,
                    pricePrc = oneItem.pricePrc,
                    quantity = oneItem.quantity,
                    rowN = oneItem.rowN,
                    strictlyThisNumber = oneItem.strictlyThisNumber
                } );
            }
        }

        #endregion CheckQuantity
        
        #region AddToCart


        /// <summary>
        /// загрузка данных в корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAddToCart_Click( object sender, EventArgs e )
        {
            //if ( ( rowsOK.Count != 0 ) || ( rowsAccept.Count != 0 ) || ( rowsER.Count != 0 ) )
            //{
            //    string clientToHeader = SiteContext.Current.CurrentClient.Profile.ClientName + " (" + SiteContext.Current.CurrentClient.Profile.ClientId + ")";
            //    List<CartImportRow> rowsCart = new List<CartImportRow>();
            //    var items = new List<ShoppingCartAddItem>();

            //    CheckBox check;
            //    HtmlInputCheckBox checkbox;

            //    // обработка успешного списка
            //    foreach ( var item in _lvItemsOK.Items )
            //    {
            //        //check = (CheckBox)(item as Control).FindControl("_chkOK");
            //        checkbox = (HtmlInputCheckBox)(item as Control).FindControl("chkOk");
            //        // если выбрана загружаем
            //        if (checkbox.Checked)
            //        {
            //            //HiddenField HF = (HiddenField)( item as Control ).FindControl( "hfRowIDOK" );
            //            Guid rid = new Guid(checkbox.Value);
            //            CartImportRow CIR = rowsOK.FirstOrDefault( t => t.rowID == rid );
            //            if ( CIR != null )
            //            {
            //                // добавляем в корзину
            //                CheckBox chbSTN = (CheckBox)( item as Control ).FindControl( "_chkStrictlyThisNumberOK" );
            //                items.Add( new ShoppingCartLoadItem()
            //                {
            //                    RowN = CIR.rowN,
            //                    Key = new ShoppingCartKey( CIR.manufacturer, CIR.partNumber, CIR.supplierId, CIR.referenceID ),
            //                    Quantity = CIR.quantity,
            //                    ReferenceID = CIR.referenceID,
            //                    StrictlyThisNumber = chbSTN.Checked
            //                } );
            //                // обновляем списки
            //                rowsCart.Add( CIR );
            //                rowsOK.Remove( CIR );
            //            }
            //        }
            //    }
            //    // обработка списка с превышением цен
            //    foreach ( var item in _lvItemAccept.Items )
            //    {
            //        // если выбрана загружаем
            //        if ( ( (CheckBox)( item as Control ).FindControl( "_chkAccept" ) ).Checked )
            //        {
            //            HiddenField HF = (HiddenField)( item as Control ).FindControl( "hfRowIDAccept" );
            //            Guid rid = new Guid( HF.Value.ToString() );
            //            CartImportRow CIR = rowsAccept.FirstOrDefault( t => t.rowID == rid );
            //            if ( CIR != null )
            //            {
            //                // добавляем в корзину
            //                CheckBox chbSTN = (CheckBox)( item as Control ).FindControl( "_chkStrictlyThisNumberAccept" );
            //                items.Add( new ShoppingCartLoadItem()
            //                {
            //                    RowN = CIR.rowN,
            //                    Key = new ShoppingCartKey( CIR.manufacturer, CIR.partNumber, CIR.supplierId, CIR.referenceID ),
            //                    Quantity = CIR.quantity,
            //                    ReferenceID = CIR.referenceID,
            //                    StrictlyThisNumber = chbSTN.Checked,
            //                    priceClientYesterday = CIR.priceClientYesterday
            //                } );
            //                // обновляем списки
            //                rowsCart.Add( CIR );
            //                rowsAccept.Remove( CIR );
            //            }
            //        }
            //    }
            //    // если есть что загрузить то грузим в корзину
            //    if ( items.Count > 0 )
            //    {
            //        Context.Profile["CustOrderNum"] = CustOrderNum;
            //        ShoppingCart cart = SiteContext.Current.CurrentClient.Cart;
            //        cart.AddRange( items.OrderBy( t => t.ReferenceID ) );
            //        items
            //            .Where( item => item.PartNotFound )
            //            .Select( item => (ShoppingCartLoadItem)item )
            //            .Each( item => AddFinalError( ErrorReason.ItemDoesNotExist, item.RowN ) );
            //        int countLoad = items.Where( item => !item.PartNotFound ).Count();
            //        _importResultLiteral.Text = string.Format( 
            //            Resources.CartImport.UploadCompleteFull
            //            /*"Загрузка заказа выполнена. <br /> В корзину добавлено - {0} строк. <br />"*/, countLoad );

            //        XMLExcel resFile = new XMLExcel( "New Order" );
            //        resFile.InitOKImportMess( CustOrderNum, SiteContext.Current.CurrentClient.Profile.ClientName, Resources.Texts.DollarShort);
            //        var allRows = rowsCart.OrderBy( t => t.rowN ).ToList();
            //        for ( int i = 0; i < allRows.Count; i++ )
            //        {
            //            resFile.AddImportCartRow( i == 0, i == allRows.Count - 1, new XMLExcelCell[] {
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Right,
            //                        CellValue = allRows[i].rowN
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allRows[i].supplierId.ToString()
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allRows[i].manufacturer
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Left,
            //                        CellValue = allRows[i].partNumber
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.Number,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allRows[i].quantity.ToString()
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allRows[i].strictlyThisNumber ? Resources.Texts.Yes /*"да"*/ : ""
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allRows[i].referenceID
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allRows[i].priceCurr.ToString( new NumberFormatInfo(){NumberDecimalSeparator="."} )
            //                    }
            //                } );
            //        }
            //        MemoryStream ms = new MemoryStream( resFile.ToByteArray() );
            //        Attachment xlsFile = new Attachment( ms, "OrderLoad.xls" );
            //        // при запросе заказа отправляем письмо клиенту
            //        if ( ( (CheckBox)_viewConfirm.FindControl( "_chbSendOKToEmail" ) ).Checked )
            //        {
            //            MailEngine.SendMailWithBccAndAttachments( new MailAddress( SiteContext.Current.CurrentClient.Profile.Email, SiteContext.Current.CurrentClient.Profile.ClientName ),
            //                new MailAddress( ClientNotifyEmail ),
            //                new Attachment[] { xlsFile }, new CartImportOK { ClientToHeader = clientToHeader } );
            //        }
            //        else
            //        {
            //            MailEngine.SendMailWithAttachments( new MailAddress( ClientNotifyEmail ),
            //                new Attachment[] { xlsFile }, new CartImportOK { ClientToHeader = clientToHeader } );
            //        }
            //        //if ( rowsER.Count > 0 )
            //        if (rowsER.Count > 0 || rowsAccept.Count > 0) //add by Daniil 2011.12.22 (чтобы при количестве ошибок=0 в файл ошибок попадали не выбранные строки с превышением цены)
            //        {
            //            // инициализация файла с ошибками
            //            resFile = new XMLExcel( "Error load" );
            //            resFile.InitErrorImportMess( CustOrderNum, SiteContext.Current.CurrentClient.Profile.ClientName );
            //            var allAcceptRows = rowsAccept.OrderBy( t => t.rowN ).ToList();
            //            var allErrRows = rowsER.OrderBy( t => t.order ).ThenBy( t => t.rowN ).ToList();
            //            for ( int i = 0; i < allAcceptRows.Count; i++ )
            //            {
            //                // добавление строк
            //                resFile.AddImportCartRow( i == 0, allErrRows.Count == 0 && i == allAcceptRows.Count - 1, new XMLExcelCell[] {
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Right,
            //                        CellValue = allAcceptRows[i].rowN
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allAcceptRows[i].supplierId.ToString()
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allAcceptRows[i].manufacturer
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Left,
            //                        CellValue = allAcceptRows[i].partNumber
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.Number,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allAcceptRows[i].quantity.ToString()
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allAcceptRows[i].strictlyThisNumber ? Resources.Texts.Yes /*"да"*/ : ""
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allAcceptRows[i].referenceID
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.Number,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allAcceptRows[i].priceClient.ToString( new NumberFormatInfo(){NumberDecimalSeparator="."} )
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Left,
            //                        CellValue = Resources.CartImport.OrderCanceledPriceExceed /*"отменено в заказе: цена превышает допустимое отклонение"*/
            //                    }
            //                } );
            //            }
            //            for ( int i = 0; i < allErrRows.Count; i++ )
            //            {
            //                resFile.AddImportCartRow( allAcceptRows.Count == 0 && i == 0, i == allErrRows.Count - 1, new XMLExcelCell[] {
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Right,
            //                        CellValue = allErrRows[i].rowN
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allErrRows[i].supplierId
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allErrRows[i].manufacturer
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Left,
            //                        CellValue = allErrRows[i].partNumber
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allErrRows[i].quantity
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allErrRows[i].strictlyThisNumber
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allErrRows[i].referenceID
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Center,
            //                        CellValue = allErrRows[i].priceClient
            //                    },
            //                    new XMLExcelCell(){
            //                        CellType = XMLExcelCellType.String,
            //                        cellAlignment = XMLExcelAlignment.Left,
            //                        CellValue = allErrRows[i].error.Replace("<br />","; ")
            //                    }
            //                } );
            //            }
            //            ms = new MemoryStream( resFile.ToByteArray() );
            //            xlsFile = new Attachment( ms, "ErrorLoad.xls" );

            //            // при запросе ошибок отправляем письмо клиенту
            //            if ( ( (CheckBox)_viewConfirm.FindControl( "_chbSendErrorToEmail" ) ).Checked )
            //            {
            //                MailEngine.SendMailWithBccAndAttachments( new MailAddress( SiteContext.Current.CurrentClient.Profile.Email, SiteContext.Current.CurrentClient.Profile.ClientName ),
            //                    new MailAddress( ClientNotifyEmail ),
            //                    new Attachment[] { xlsFile }, new CartImportAlert { ClientToHeader = clientToHeader } );
            //            }
            //            else
            //            {
            //                MailEngine.SendMailWithAttachments( new MailAddress( ClientNotifyEmail ),
            //                    new Attachment[] { xlsFile }, new CartImportAlert { ClientToHeader = clientToHeader } );
            //            }
            //        }
            //        else
            //        {
            //            // при запросе ошибок отправляем письмо клиенту
            //            if ( ( (CheckBox)_viewConfirm.FindControl( "_chbSendErrorToEmail" ) ).Checked )
            //            {
            //                //deas 24.05.2011 task4123 при отсутствии ошибок отправка соответвующего письма
            //                MailEngine.SendMailWithBcc( new MailAddress( SiteContext.Current.CurrentClient.Profile.Email, SiteContext.Current.CurrentClient.Profile.ClientName ),
            //                    new MailAddress( ClientNotifyEmail ),
            //                    new CartImportNoAlert { ClientToHeader = clientToHeader } );
            //            }
            //            else
            //            {
            //                MailEngine.SendMail( new MailAddress( ClientNotifyEmail ),
            //                    new CartImportNoAlert { ClientToHeader = clientToHeader } );
            //            }
            //        }
            //    }
            //    else
            //    {
            //        // нечего загружать
            //        _importResultLiteral.Text = Resources.CartImport.UploadingFailed; /*"Загрузка заказа не удалась!";*/
            //    }
            //    // очишаем хранимые списки после загрузки
            //    Session["CartImportExt_rowsOK"] = null;
            //    Session["CartImportExt_rowsAccept"] = null;
            //    Session["CartImportExt_rowsER"] = null;
            //    Session["CartImportExt_rowsERQty"] = null;
            //    _multiViewLoadReport.ActiveViewIndex = 3;
            //}
        }

        #endregion AddToCart

        #region AditionalForCart

        protected bool IsManagerMode
        {
            get
            {
                return SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager;
            }
        }

        protected string GetCartUrl()
        {
            return IsManagerMode ? ClientCart.GetUrl() : UrlManager.GetCartUrl();
        }

        protected string GetCartImportUrl()
        {
            return IsManagerMode ? ClientCartImport.GetUrl() : UrlManager.GetCartImportUrl();
        }

        private bool CheckCartEmpty()
        {
            return _cartVersionValidator.IsValid && ( _cartEmptyValidator.IsValid =
                SiteContext.Current.CurrentClientTotals.PartsCount == 0 );
        }

        #endregion AditionalForCart

        #region ErrorHelper

        private enum ErrorReason
        {
            CartNotEmpty, FileNotSpecified, FileNotFoundOrEmpty, InvalidFileFormat,
            ReaderFailed, InvalidOrderNumber, SingleSheetRequired, EmptyOrder,
            InvalidSupplierID, InvalidManufacturer, InvalidPartNumber, InvalidQuantity,
            InvalidStrictlyNumberFlag, ItemDoesNotExist, UnknownFailure, InvalidPrice
        }
        
        private void AddFinalError( ErrorReason reason, string arg )
        {
            _importFinalLiteral.Text = _importSummaryLiteral.Text + TextError( reason, arg ) + "<br />";
        }

        private void AddError( ErrorReason reason, string arg )
        {
            _importSummaryLiteral.Text = _importSummaryLiteral.Text + TextError( reason, arg ) + "<br />";
        }

        private string TextError( ErrorReason reason, string arg )
        {
            string msg;
            switch ( reason )
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
                    throw new ArgumentException( "Invalid error reason" );
            }
            return string.Format( msg, arg );
        }

        #endregion ErrorHelper


		/// <summary>
		/// Проверяет ограничения загрузки через Excel и перенаправляет на страницу предупреждения
		/// </summary>
		/// <param name="linesCount">кол-во строк данных в текущем Excel-файле</param>
		private void CheckExcelRestrictions(int linesCount)
		{
			int currentUserID = SiteContext.Current.CurrentClient.Profile.UserId;
			
			List<BanClientAction> bans = new List<BanClientAction>();
			using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				//Получаем список банов для данного пользователя
				bans = dc.DataContext.BanClientActions.Where(b => b.UserID == currentUserID).ToList();

				bool entityIsNew = false;
				//ограничения по кол-ву файлов за календарные день
				var filesCountBan = bans.Where(b => b.Ban == ExcelRestrictions.ExcelFilesCount.ToString()).SingleOrDefault();
				if (filesCountBan == null)
				{
					// добавляем новый счетчик
					filesCountBan = new BanClientAction()
						{
							Ban = ExcelRestrictions.ExcelFilesCount.ToString(),
							Count = 1,
							ExpirationDate = DateTime.Now,
							UserID = currentUserID
						};
					entityIsNew = true;
				}
				else
				{
					if (DateTime.Today == filesCountBan.ExpirationDate.Date) { filesCountBan.Count++; } // Если грузит в день бана увеличивем счетчик
					else { filesCountBan.Count = 1; filesCountBan.ExpirationDate = DateTime.Now; }		// иначе "обнуляем" счетчик
				}
				//using (var dc = new StoreDataContext())
				//{
				if (entityIsNew) { dc.DataContext.BanClientActions.InsertOnSubmit(filesCountBan); }
				dc.DataContext.SubmitChanges();
				//}
				//Суммарное кол-во excel-файлов загружаемых за календарный день недолжно превышать 20 (FilesLimitCount в настройках)
				if (filesCountBan.Count > Convert.ToInt32(ConfigurationManager.AppSettings["FilesLimitCount"]))
				{
					RedirectToWarningPage();
				}

				//Кол-во строк данных в одном файле не должно превышать 1500 строк (LinesOnFileLimitCount в настройках)
				if (linesCount > Convert.ToInt32(ConfigurationManager.AppSettings["LinesOnFileLimitCount"]))
				{
					RedirectToWarningPage();
				}

				//ограничения по суммарному кол-ву строк за календарный день
				entityIsNew = false;
				var summaryLinesBan = bans.Where(b => b.Ban == ExcelRestrictions.ExcelLinesCount.ToString()).SingleOrDefault();
				if (summaryLinesBan == null)
				{
					//добавляем новый счетчик
					summaryLinesBan = new BanClientAction()
						{
							Ban = ExcelRestrictions.ExcelLinesCount.ToString(),
							Count = linesCount,
							ExpirationDate = DateTime.Now,
							UserID = currentUserID
						};
					entityIsNew = true;
				}
				else
				{
					if (DateTime.Today == summaryLinesBan.ExpirationDate.Date) // Если в день бана то либо увеличиваем счетчик либо в ошибку(если вышли по кол-ву)
					{
						//Суммарно кол-во строк за календарный день не должно превышать 6000 (LinesLimitCount в настройках)
						if (summaryLinesBan.Count + linesCount > Convert.ToInt32(ConfigurationManager.AppSettings["LinesLimitCount"]))
						{
							RedirectToWarningPage();
						}
						else { summaryLinesBan.Count += linesCount; }
					}
					else { summaryLinesBan.Count = linesCount; summaryLinesBan.ExpirationDate = DateTime.Now; } // либо "обнуляем" счетчик
				}
				//using (var dc = new StoreDataContext())
				//{
				if (entityIsNew) { dc.DataContext.BanClientActions.InsertOnSubmit(summaryLinesBan); }
				dc.DataContext.SubmitChanges();
				//}
			}
		}

		/// <summary>
		/// Загружает из кеша и показывает страницу предупреждения
		/// </summary>
		private void RedirectToWarningPage()
		{
			string pageText = string.Empty;
			if (HttpContext.Current.Cache.Get("ExcelWarningTextPage") == null)
			{
				WebClient client = new WebClient();
				client.Encoding = HttpContext.Current.Request.ContentEncoding;
				byte[] pageBytes = client.DownloadData(UrlManager.MakeAbsoluteUrl("/Cabinet/ExcelWarning.aspx"));
				pageText = System.Text.UTF8Encoding.UTF8.GetString(pageBytes);
				HttpContext.Current.Cache.Insert("ExcelWarningTextPage", pageText, null, DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CacheDuration"])), new TimeSpan(0, 0, 0));
			}
			else
			{
				pageText = (string)HttpContext.Current.Cache["ExcelWarningTextPage"];
			}

			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Write(pageText);
			HttpContext.Current.Response.End();
		}

		private enum ExcelRestrictions
		{
			ExcelFilesCount,
			ExcelLinesCount
		}
    }
}

namespace RmsAuto.Store.Web
{
    public class ShoppingCartLoadItem : ShoppingCartAddItem
    {
        public string RowN;
    }

    #region CartImportClass

    [Serializable()]
    public class CartImportRow
    {
        public Guid rowID { get; set; }
        public string manufacturer { get; set; }
        public string partNumber { get; set; }
        public int supplierId { get; set; }
        public string referenceID { get; set; }
        public string rowN { get; set; }
        public string itemName { get; set; }
        public string deliveryPeriod { get; set; }
        public bool strictlyThisNumber { get; set; }
        public int quantity { get; set; }
        public decimal priceClient { get; set; }
        public decimal priceCurr { get; set; }
        public string pricePrc { get; set; }
		public decimal? priceClientYesterday { get; set; } //сюда записываем (для "особых" пользователей): "вчерашняя" цена + допустимый % превышения
    }

    [Serializable()]
    public class CartImportRowERQty
    {
        public Guid rowID { get; set; }
        public string errorText { get; set; }
        public bool isOK { get; set; }
        public string manufacturer { get; set; }
        public string partNumber { get; set; }
        public int supplierId { get; set; }
        public string referenceID { get; set; }
        public string rowN { get; set; }
        public string itemName { get; set; }
        public string deliveryPeriod { get; set; }
        public bool strictlyThisNumber { get; set; }
        public int quantity { get; set; }
        public decimal priceClient { get; set; }
        public decimal priceCurr { get; set; }
        public string pricePrc { get; set; }
    }

    [Serializable()]
    public class CartImportRowError
    {
        public int order { get; set; }
        public string rowN { get; set; }
        public string manufacturer { get; set; }
        public string partNumber { get; set; }
        public string supplierId { get; set; }
        public string referenceID { get; set; }
        public string strictlyThisNumber { get; set; }
        public string error { get; set; }
        public string quantity { get; set; }
        public string priceClient { get; set; }
    }

    #endregion CartImportClass
}
