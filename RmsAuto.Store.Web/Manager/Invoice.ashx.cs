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

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for Invoice
    /// </summary>
    public class Invoice : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            try
            {
                string type = context.Request["type"];
                string templatePath = String.Empty;
                switch (type)
                {
                    case "commercial":
                        templatePath = ConfigurationManager.AppSettings["ExcelTemplate.InvoiceCommercial"];
                        break;

                    case "packing":
                        templatePath = ConfigurationManager.AppSettings["ExcelTemplate.InvoiceCumPacking"];
                        break;
                }

                string country = HttpUtility.UrlDecode(context.Request["country"] ?? "");
                string descr = HttpUtility.UrlDecode(context.Request["descr"] ?? "");
                string pkgs = HttpUtility.UrlDecode(context.Request["pkgs"] ?? "");
                string weight = HttpUtility.UrlDecode(context.Request["weight"] ?? "");

                FileInfo newFile = new FileInfo(templatePath);
                FileInfo template = new FileInfo(templatePath);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet sheet = xlPackage.Workbook.Worksheets["Invoice"];

                    RmsAuto.Store.Acctg.ClientProfile profile = RmsAuto.Store.Acctg.ClientProfile.Load((string)context.Session["ClientID"]);
                    object source = context.Session["PrintOrderLineIDs"];
                    SellerInfo seller = GetSellerInfo();

                    if (source != null && !string.IsNullOrEmpty(source.ToString()))
                    {
                        string[] str = source.ToString().Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                        int[] ids = Array.ConvertAll(str, s => int.Parse(s));

                        PrintOrderLineInfo[] printLines = null;
                        using (var dc = new DCWrappersFactory<StoreDataContext>())
                        {
                            printLines = (from ol in dc.DataContext.OrderLines
                                          where ids.Contains(ol.OrderLineID)
                                          select new PrintOrderLineInfo()
                                          {
                                              Number = ol.OrderID,
                                              PartNumber = ol.PartNumber,
                                              PartName = ol.PartName,
                                              Manufacturer = ol.Manufacturer,
                                              Qty = ol.Qty,
                                              Price = ol.UnitPrice,
                                              Total = ol.Total
                                          }).ToArray();
                        }

                        int startRow = 0;
                        int rowIndex = 0;
                        ExcelRange cell = null;

                        switch (type)
                        {
                            case "commercial":
                                
                                startRow = 27;
                                rowIndex = startRow;
                                sheet.InsertRow(rowIndex, printLines.Length, rowIndex);

                                int i = 1;
                                foreach (var item in printLines)
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
                                    cell.Style.WrapText = true;
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
                                cell.Value = printLines.Sum(l => l.Total);

                                cell = sheet.Cells[13, 6];
                                cell.Value = DateTime.Now.ToShortDateString();

                                cell = sheet.Cells[14, 7];
                                cell.Value = profile.AcctgId + "/" + DateTime.Now.ToString("hhmmss");

                                cell = sheet.Cells[14, 1];
                                cell.Value = profile.ClientName;

                                cell = sheet.Cells[15, 6];
                                cell.Value = String.Join(", ", printLines.Select(x => x.Number.ToString()).ToArray());

                                cell = sheet.Cells[18, 1];
                                cell.Value = profile.DeliveryAddress;

                                cell = sheet.Cells[21, 1];
                                cell.Value = cell.Value.ToString() + descr;

                                cell = sheet.Cells[22, 1];
                                cell.Value = cell.Value.ToString() + country;

                                break;

                            case "packing":

                                cell = sheet.Cells[16, 1];
                                cell.Value = profile.ClientName;

                                cell = sheet.Cells[20, 1];
                                cell.Value = profile.DeliveryAddress;

                                cell = sheet.Cells[15, 6];
                                cell.Value = DateTime.Now.ToShortDateString();

                                cell = sheet.Cells[16, 6];
                                cell.Value = profile.AcctgId + "/" + DateTime.Now.ToString("hhmmss");

                                cell = sheet.Cells[17, 6];
                                cell.Value = String.Join(", ", printLines.Select(x => x.Number.ToString()).ToArray());

                                cell = sheet.Cells[24, 1];
                                cell.Value = descr;

                                cell = sheet.Cells[24, 2];
                                cell.Value = String.Join(", ", printLines.Select(x => x.Manufacturer).ToArray());

                                cell = sheet.Cells[24, 3];
                                cell.Value = country;

                                cell = sheet.Cells[24, 4];
                                cell.Value = printLines.Sum(l => l.Qty);

                                cell = sheet.Cells[24, 5];
                                cell.Value = pkgs;

                                cell = sheet.Cells[24, 6];
                                cell.Value = printLines.Sum(l => l.Total);

                                cell = sheet.Cells[24, 7];
                                cell.Value = weight;

                                break;
                        }

                        cell = sheet.Cells[7, 1];
                        cell.Value = String.Format(ConfigurationManager.AppSettings["ExcelTemplate.SellerInfo"], seller.License ?? "", seller.Issued ?? DateTime.MinValue, seller.Address ?? "");
                    }

                    ExcelHeaderFooterText footer = sheet.HeaderFooter.OddFooter;
                    footer.LeftAlignedText = ConfigurationManager.AppSettings["ExcelTemplate.FooterText"].Replace("\\r\\n", Environment.NewLine);
                    sheet.HeaderFooter.differentOddEven = false;

                    sheet.PrinterSettings.RightMargin = sheet.PrinterSettings.LeftMargin = 0.3M;
                    sheet.PrinterSettings.TopMargin = 0.5M;
                    sheet.PrinterSettings.BottomMargin = 0.9M;

                    byte[] data = xlPackage.GetAsByteArray();

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

        #region Private members

        private SellerInfo GetSellerInfo()
        {
            SellerInfo seller = new SellerInfo();
            seller.CompanyName = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Name;
            seller.Address = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Address;
            seller.License = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].License;
            seller.Issued = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Issued;

            return seller;
        }

        private class PrintOrderLineInfo
        {
            public int Number { get; set; }
            public string PartNumber { get; set; }
            public string PartName { get; set; }
            public string Manufacturer { get; set; }
            public int Qty { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }
        }
        
        #endregion
    }
}