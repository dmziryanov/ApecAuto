using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Misc.Thumbnails;
using System.Net;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Cms
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService( Namespace = "http://tempuri.org/" )]
	[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class ChartSupplierStatHandler : IHttpHandler
	{

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
		
		public void ProcessRequest( HttpContext context )
		{
            SparePartPriceKey key = SparePartPriceKey.Parse(context.Request.QueryString["ID"]);
            SparePartFranch part = SparePartsDac.Load(key);
            if (part != null)
            {
                Int32? beforeTime = part.BeforeTime;
                Int32? onTime = part.OnTime;
                Int32? delay = part.Delay;
                Int32? nonDelivery = part.NonDelivery;

                //beforeTime = 100; onTime = 75; delay = 60; nonDelivery = 35;

                if (beforeTime != null && onTime != null && delay != null && nonDelivery != null)
                {
                
                    Chart ChartStat = new System.Web.UI.DataVisualization.Charting.Chart();

                    //LiteralControl lc = new LiteralControl();

                    //lc.Text = "123";

                    //ChartStat.Controls.Add(lc);

                    Title t = new Title(("Рейтинг" + Environment.NewLine + "поставщика"), Docking.Top);

                    t.Font = new Font(t.Font.FontFamily,(float)t.Font.SizeInPoints * (float)1.325);

                    t.Position = new ElementPosition(18, 2, 65, 20);

                    ChartStat.Titles.Add(t);

                    ChartStat.Series.Add(new Series("SeriesStat"));
                    ChartStat.ChartAreas.Add(new ChartArea("ChartAreaStat"));

                    int[] yValues = { (int) beforeTime, (int) onTime, (int) delay, (int) nonDelivery};
                    string[] xValues = { "Раньше", "Вовремя", "Задержка", "Непоставка" };
                    ChartStat.Series["SeriesStat"].Points.DataBindXY(xValues, yValues);
                    ChartStat.Series["SeriesStat"].LabelForeColor = Color.FromArgb(0, Color.White);

                    ChartStat.Series["SeriesStat"].Points[0].Color = Color.FromArgb(0, 127, 255); // синий Раньше
                    ChartStat.Series["SeriesStat"].Points[1].Color = Color.FromArgb(47, 229, 107); // зеленый Вовремя
                    ChartStat.Series["SeriesStat"].Points[2].Color = Color.FromArgb(255, 242, 157); // желтый Задержка
                    ChartStat.Series["SeriesStat"].Points[3].Color = Color.FromArgb(254, 192, 192); // красный Непоставка

                    // Set chart type
                    ChartStat.Series["SeriesStat"].ChartType = SeriesChartType.Pie;

                    // Set labels style
                    ChartStat.Series["SeriesStat"]["PieLabelStyle"] = "Inside";

                    // Enable 3D
                    ChartStat.ChartAreas["ChartAreaStat"].Area3DStyle.Enable3D = true;

                    // Show a 30% perspective
                    ChartStat.ChartAreas["ChartAreaStat"].Area3DStyle.Perspective = 30;

                    // Set the X Angle to 70
                    ChartStat.ChartAreas["ChartAreaStat"].Area3DStyle.Inclination = 70;

                    // Set the Y Angle to -2
                    ChartStat.ChartAreas["ChartAreaStat"].Area3DStyle.Rotation = -2;

                    ChartStat.ChartAreas["ChartAreaStat"].Position = new ElementPosition(16, 5, 68, 68);

                    System.Web.UI.DataVisualization.Charting.Legend l = new Legend("LegendStat");

                    //l.TitleFont = new Font();
                    l.Title = "Указанное время доставки:" + Environment.NewLine +
                        part.DisplayDeliveryDaysMin.ToString() + " - " + part.DisplayDeliveryDaysMax.ToString() + " (в рабочих днях)";
                    l.TitleAlignment = StringAlignment.Center;
                    l.TitleFont = new Font(l.TitleFont.FontFamily, l.TitleFont.SizeInPoints * (float) 1.15);

                    ChartStat.Legends.Add(l);

                    LegendCellColumn firstColumn = new LegendCellColumn();
                    firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
                    ChartStat.Legends["LegendStat"].CellColumns.Add(firstColumn);

                    LegendCellColumn secondColumn = new LegendCellColumn();
                    secondColumn.ColumnType = LegendCellColumnType.Text;
                    secondColumn.Text = "#LEGENDTEXT";
                    secondColumn.Alignment = ContentAlignment.MiddleLeft;
                    secondColumn.HeaderBackColor = Color.WhiteSmoke;
                    ChartStat.Legends["LegendStat"].CellColumns.Add(secondColumn);

                    LegendCellColumn thirdColumn = new LegendCellColumn();
                    thirdColumn.ColumnType = LegendCellColumnType.Text;
                    thirdColumn.Text = "#PERCENT{0#%}";
                    //thirdColumn.Text = String.Format({0:#%},thirdColumn.Text);
                    thirdColumn.Alignment = ContentAlignment.MiddleRight;
                    thirdColumn.HeaderBackColor = Color.WhiteSmoke;
                    ChartStat.Legends["LegendStat"].CellColumns.Add(thirdColumn);

                    ChartStat.Legends[0].Enabled = true;
                    //(34, 63, 40, 30)
                    ChartStat.Legends[0].Position = new ElementPosition(22, 58, 60, 44);

                    context.Response.ContentType = "image/bmp";
                    context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);

                    using (System.IO.MemoryStream imageStream = new System.IO.MemoryStream())
                    {
                        ChartStat.SaveImage(imageStream, ChartImageFormat.Bmp);
                        context.Response.BinaryWrite(imageStream.ToArray());
                    }
                    context.Response.End();
                }
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
            }
		}
	}
}
