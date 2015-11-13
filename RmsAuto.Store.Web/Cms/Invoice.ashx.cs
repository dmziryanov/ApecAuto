using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;

using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace RmsAuto.Store.Web.Cms
{
    /// <summary>
    /// Summary description for Invoice
    /// </summary>
    public class Invoice : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);

            try
            {
                string type = context.Request["type"];
                InvoiceProcessorBase processor = null;

                switch (type)
                {
                    case "commercial":
                        processor = new InvoiceCommercialProcessor(context, HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ExcelTemplate.InvoiceCommercial"]), 
                            Convert.ToInt32(ConfigurationManager.AppSettings["ExcelTemplate.InvoiceCommercial.DataRowIndex"]));
                        break;

                    case "packing":
                        processor = new InvoicePackingProcessor(context, HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ExcelTemplate.InvoiceCumPacking"]), 
                            Convert.ToInt32(ConfigurationManager.AppSettings["ExcelTemplate.InvoiceCumPacking.DataRowIndex"]));
                        break;

                    case "payment":
                        processor = new InvoicePaymentProcessor(context, HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ExcelTemplate.InvoicePayment"]),
                            Convert.ToInt32(ConfigurationManager.AppSettings["ExcelTemplate.InvoicePayment.DataRowIndex"]));
                        break;
                }

                if (processor == null)
                    throw new ArgumentException("Unknown parameter");

                FileInfo newFile = new FileInfo(processor.TemplatePath);
                FileInfo template = new FileInfo(processor.TemplatePath);

                using (ExcelPackage package = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Sheet"];

                    processor.FillExcelData(sheet);

                    ExcelHeaderFooterText footer = sheet.HeaderFooter.OddFooter;
                    footer.LeftAlignedText = ConfigurationManager.AppSettings["ExcelTemplate.FooterText"].Replace("\\r\\n", Environment.NewLine);
                    sheet.HeaderFooter.differentOddEven = false;

                    sheet.PrinterSettings.RightMargin = sheet.PrinterSettings.LeftMargin = 0.3M;
                    sheet.PrinterSettings.TopMargin = 0.5M;
                    sheet.PrinterSettings.BottomMargin = 0.9M;

                    byte[] data = package.GetAsByteArray();

                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + type + "Invoice" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx\";");
                    context.Response.BinaryWrite(data);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLog(ex.Message + Environment.NewLine + ex.StackTrace, EventLogerType.Error);
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class OrderLineInfo
    {
        public int OrderNumber { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string Manufacturer { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

    public abstract class InvoiceProcessorBase
    {
        public string TemplatePath { get; set; }

        public int DataRowIndex { get; set; }

        protected HttpContext Context { get; set; }

        protected IList<OrderLineInfo> Data { get; set; }

        public InvoiceProcessorBase(HttpContext context, string templatePath, int dataRowIndex)
        {
            Context = context;
            Data = null;
            TemplatePath = templatePath;
            DataRowIndex = dataRowIndex;
        }

        public virtual void FillData()
        {
            if (Data != null)
                return;

            object source = Context.Session["PrintOrderLineIDs"];

            if (source != null && !string.IsNullOrEmpty(source.ToString()))
            {
                string[] str = source.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int[] ids = Array.ConvertAll(str, s => int.Parse(s));

                using (var dc = new DCFactory<StoreDataContext>())
                {
                    dc.DataContext.DeferredLoadingEnabled = false;
                    Data =
                        (from ol in dc.DataContext.OrderLines
                         where ids.Contains(ol.OrderLineID)
                         select new OrderLineInfo()
                         {
                             OrderNumber = ol.OrderID,
                             PartNumber = ol.PartNumber,
                             PartName = ol.PartName,
                             Manufacturer = ol.Manufacturer,
                             Qty = ol.Qty,
                             Price = ol.UnitPrice,
                             Total = ol.Total
                         }).ToList();
                }
            }
            return;
        }

        protected SellerInfo GetSellerInfo()
        {
            SellerInfo seller = new SellerInfo();
            seller.CompanyName = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Name;
            seller.Address = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Address;
            seller.License = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].License;
            seller.Issued = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Issued;

            return seller;
        }

        public abstract void FillExcelData(ExcelWorksheet sheet);
    }

    public class InvoiceCommercialProcessor : InvoiceProcessorBase
    {
        public InvoiceCommercialProcessor(HttpContext context, string templatePath, int dataRowIndex)
            : base(context, templatePath, dataRowIndex) { }

        public override void FillExcelData(ExcelWorksheet sheet)
        {
            FillData();

            if (Data == null || Data.Count == 0)
                return;

            int startRow = DataRowIndex;
            int rowIndex = startRow;
            ExcelRange cell = null;

            RmsAuto.Store.Acctg.ClientProfile profile = RmsAuto.Store.Acctg.ClientProfile.Load((string)Context.Session["ClientID"]);
            SellerInfo seller = GetSellerInfo();

            sheet.InsertRow(rowIndex, Data.Count(), rowIndex);

            int i = 1;
            foreach (var item in Data)
            {
                cell = sheet.Cells[rowIndex, 1];
                cell.Value = i;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                var border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 2];
                cell.Value = item.Manufacturer;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3];
                cell.Value = item.PartNumber;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4];
                cell.Value = item.PartName;
                cell.Style.WrapText = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5];
                cell.Value = item.Qty;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6];
                cell.Value = item.Price;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7];
                cell.Value = item.Price * item.Qty;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                i++;
                rowIndex++;
            }

            cell = sheet.Cells[rowIndex + 1, 7];
            cell.Value = Data.Sum(l => l.Total);

            cell = sheet.Cells[13, 6];
            cell.Value = DateTime.Now.ToShortDateString();

            cell = sheet.Cells[14, 7];
            cell.Value = profile.AcctgId + "/" + DateTime.Now.ToString("hhmmss");

            cell = sheet.Cells[14, 1];
            cell.Value = profile.ClientName;

            cell = sheet.Cells[15, 6];
            cell.Value = String.Join(", ", Data.Select(x => x.OrderNumber.ToString()).Distinct().ToArray());

            cell = sheet.Cells[18, 1];
            cell.Value = profile.DeliveryAddress;

            cell = sheet.Cells[21, 1];
            cell.Value = cell.Value.ToString() + HttpUtility.UrlDecode(Context.Request["descr"] ?? "");

            cell = sheet.Cells[22, 1];
            cell.Value = cell.Value.ToString() + HttpUtility.UrlDecode(Context.Request["country"] ?? "");

            cell = sheet.Cells[7, 1];
            cell.Value = String.Format(ConfigurationManager.AppSettings["ExcelTemplate.SellerInfo"], seller.License ?? "", seller.Issued.HasValue ? seller.Issued.Value.ToShortDateString() : "-", seller.Address ?? "");
        }
    }

    public class InvoicePackingProcessor : InvoiceProcessorBase
    {
        public InvoicePackingProcessor(HttpContext context, string templatePath, int dataRowIndex)
            : base(context, templatePath, dataRowIndex) { }

        public override void FillExcelData(ExcelWorksheet sheet)
        {
            FillData();

            if (Data == null || Data.Count == 0)
                return;

            int startRow = DataRowIndex;
            RmsAuto.Store.Acctg.ClientProfile profile = RmsAuto.Store.Acctg.ClientProfile.Load((string)Context.Session["ClientID"]);
            SellerInfo seller = GetSellerInfo();
            ExcelRange cell = null;

            cell = sheet.Cells[16, 1];
            cell.Value = profile.ClientName;

            cell = sheet.Cells[20, 1];
            cell.Value = profile.DeliveryAddress;

            cell = sheet.Cells[15, 6];
            cell.Value = DateTime.Now.ToShortDateString();

            cell = sheet.Cells[16, 6];
            cell.Value = profile.AcctgId + "/" + DateTime.Now.ToString("hhmmss");

            cell = sheet.Cells[17, 6];
            cell.Value = String.Join(", ", Data.Select(x => x.OrderNumber.ToString()).Distinct().ToArray());

            cell = sheet.Cells[startRow, 1];
            cell.Value = HttpUtility.UrlDecode(Context.Request["descr"] ?? "");

            cell = sheet.Cells[startRow, 2];
            cell.Value = String.Join(", ", Data.Select(x => x.Manufacturer).Distinct().ToArray());

            cell = sheet.Cells[startRow, 3];
            cell.Value = HttpUtility.UrlDecode(Context.Request["country"] ?? "");

            cell = sheet.Cells[startRow, 4];
            cell.Value = Data.Sum(l => l.Qty);

            cell = sheet.Cells[startRow, 5];
            cell.Value = HttpUtility.UrlDecode(Context.Request["pkgs"] ?? "");

            cell = sheet.Cells[startRow, 6];
            cell.Value = Data.Sum(l => l.Total);

            cell = sheet.Cells[startRow, 7];
            cell.Value = HttpUtility.UrlDecode(Context.Request["weight"] ?? "");

            cell = sheet.Cells[7, 1];
            cell.Value = String.Format(ConfigurationManager.AppSettings["ExcelTemplate.SellerInfo"], seller.License ?? "", seller.Issued.HasValue ? seller.Issued.Value.ToShortDateString() : "-", seller.Address ?? "");
        }
    }

    public class InvoicePaymentProcessor : InvoiceProcessorBase
    {
        public InvoicePaymentProcessor(HttpContext context, string templatePath, int dataRowIndex)
            : base(context, templatePath, dataRowIndex) { }

        public override void FillData()
        {
            if (Data != null)
                return;

            int orderID = 0;

            if (int.TryParse(Context.Request["order"], NumberStyles.Integer, CultureInfo.InvariantCulture, out orderID))
            {
                using (var dc = new DCFactory<StoreDataContext>())
                {
                    dc.DataContext.DeferredLoadingEnabled = false;
                    Data =
                        (from ol in dc.DataContext.OrderLines
                         where ol.OrderID == orderID
                         select new OrderLineInfo()
                         {
                             OrderNumber = ol.OrderID,
                             PartNumber = ol.PartNumber,
                             PartName = ol.PartName,
                             Manufacturer = ol.Manufacturer,
                             Qty = ol.Qty,
                             Price = ol.UnitPrice,
                             Total = ol.Total
                         }).ToList();
                }
            }
        }

        public override void FillExcelData(ExcelWorksheet sheet)
        {
            FillData();

            if (Data == null || Data.Count == 0)
                return;

            int startRow = DataRowIndex;
            int rowIndex = startRow;
            ExcelRange cell = null;

            RmsAuto.Store.Acctg.ClientProfile profile = SiteContext.Current.CurrentClient.Profile;
            SellerInfo seller = GetSellerInfo();

            sheet.InsertRow(rowIndex, Data.Count(), rowIndex);

            int i = 1;
            foreach (var item in Data)
            {
                cell = sheet.Cells[rowIndex, 1];
                cell.Value = i;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                var border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 2];
                cell.Value = item.Manufacturer;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3];
                cell.Value = item.OrderNumber;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4];
                cell.Value = item.PartNumber;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5];
                cell.Value = item.PartName;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6];
                cell.Value = item.Qty;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7];
                cell.Value = item.Price;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8];
                cell.Value = item.Price * item.Qty;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                border = cell.Style.Border;
                border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                i++;
                rowIndex++;
            }

            cell = sheet.Cells[rowIndex + 1, 8];
            cell.Value = Data.Sum(x => x.Total);

            cell = sheet.Cells[1, 1];
            cell.Value = String.Format(ConfigurationManager.AppSettings["ExcelTemplate.InvoiceTitle"], profile.AcctgId + "/" + DateTime.Now.ToString("hhmmss"), DateTime.Now.ToShortDateString());

            cell = sheet.Cells[rowIndex + 3, 1];
            cell.Value = String.Format(ConfigurationManager.AppSettings["ExcelTemplate.InvoiceTotal"], Data.Sum(x => x.Qty), Data.Sum(x => x.Total));

            cell = sheet.Cells[4, 1];
            cell.Value = cell.Value.ToString() + seller.CompanyName + ", " + seller.Address + ", " + seller.Phone;

            cell = sheet.Cells[5, 1];
            cell.Value = cell.Value.ToString() + profile.ClientName + ", " + profile.ShippingAddress;
        }
    }
}