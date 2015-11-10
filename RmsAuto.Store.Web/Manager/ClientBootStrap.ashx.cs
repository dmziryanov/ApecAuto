using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using System.Web.SessionState;
using System.Text;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ClientBootStrap : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!LightBO.IsLight()) return;

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            List<ClientGroup> cl;

            using (DCWrappersFactory<dcCommonDataContext> DC = new DCWrappersFactory<dcCommonDataContext>())
            {
                 cl = DC.DataContext.ClientGroups.ToList();
            }

            using (var ctx = new DCWrappersFactory<StoreDataContext>())
            { 
                //TODO: доделать проверку на секурность
                var id = SiteContext.Current.User.UserId;

                DateTime dateMin;
                DateTime dateMax;
                string RegDateStringMin = "";
                string RegDateStringMax = "";
                string ManagerCondition = "";

                if (DateTime.TryParse(context.Request["dateMin"], CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dateMin))
                {
                    RegDateStringMin = " AND CONVERT(varchar(10), a.CreationTime, 126) >= '" + dateMin.ToString("yyyy-MM-dd") + "'";
                }

                if (DateTime.TryParse(context.Request["dateMax"], CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dateMax))
                {
                    RegDateStringMax = " AND CONVERT(varchar(10), a.CreationTime, 126) <= '" + dateMax.ToString("yyyy-MM-dd") + "'";
                }

                if (context.Request["manager"] != "0")
                {
                    ManagerCondition = " AND ManagerId ='" + context.Request["manager"] + "'";
                }

                IEnumerable<BriefClientInfo> searchResults = ClientSearch.LiteClientSearch(
                    context.Request["client"],
                    context.Request["phone"],
                    ClientSearchMatching.Fuzzy, "0", context.Request["checked"], "0", RegDateStringMin, RegDateStringMax, ManagerCondition);

                searchResults.Each<BriefClientInfo>(x => x.Manager = AcctgRefCatalog.RmsEmployees.Items.Where(y => y.EmployeeId == x.ManagerId).Select(y => y.FullName).FirstOrDefault());
                var ManagerList = AcctgRefCatalog.RmsEmployees.Items.ToList();

                string templatePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ExcelTemplate.ClientList"]);
                FileInfo newFile = new FileInfo(templatePath);
                FileInfo template = new FileInfo(templatePath);

                using (ExcelPackage package = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Sheet"];

                    int startRow = Convert.ToInt32(ConfigurationManager.AppSettings["ExcelTemplate.ClientList.DataRowIndex"]);
                    int rowIndex = startRow;
                    ExcelRange cell = null;

                    var result = from n in searchResults
                               join m in ctx.DataContext.Users on n.ClientID equals m.AcctgID
                               select new
                               {
                                   AcctgID = m.AcctgID,
                                   ClientGroup = m.ClientGroup.ToString(),
                                   ClientGroupID = cl.Where(z => z.ClientGroupID == m.ClientGroup).Select(y => y.ClientGroupName).FirstOrDefault(),
                                   ClientName = m.Clientname,
                                   Email = m.Email,
                                   ContactPhone = m.ContactPhone,
                                   CreationTime = m.CreationTime.ToString(),
                                   Manager = ManagerList.Where(z => z.EmployeeId == m.ManagerId).Select(y => y.FullName).FirstOrDefault() ?? "Not assigned"
                               };

                    sheet.InsertRow(rowIndex, result.Count(), rowIndex);

                    int i = 1;
                    foreach (var item in result)
                    {
                        cell = sheet.Cells[rowIndex, 1];
                        cell.Value = item.AcctgID;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        var border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 2];
                        cell.Value = item.ClientGroup;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3];
                        cell.Value = item.ClientGroupID;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4];
                        cell.Value = item.ClientName;
                        cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5];
                        cell.Value = item.Email;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = item.ContactPhone;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7];
                        cell.Value = item.CreationTime;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = item.Manager;
                        cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border;
                        border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        i++;
                        rowIndex++;
                    }

                    sheet.PrinterSettings.RightMargin = sheet.PrinterSettings.LeftMargin = 0.3M;
                    sheet.PrinterSettings.TopMargin = 0.5M;
                    sheet.PrinterSettings.BottomMargin = 0.9M;

                    byte[] data = package.GetAsByteArray();

                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.Headers.Add("Content-Disposition", "attachment; filename=\"ClientList" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx\";");
                    context.Response.BinaryWrite(data);
                }
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
}
