using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;
using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public class VehiclesList : GuayaquilTemplate
    {
        private bool _quickGroupsEnable;
        private VehicleInfo[] _vehicles;

        private readonly string[] _defaultColumns = new string[]
                                        {
                                            "name", "date", "datefrom", "dateto", "model", "framecolor", "trimcolor",
                                            "modification", "grade", "frame", "engine", "engineno", "transmission", "doors",
                                            "manufactured", "options", "creationregion", "destinationregion", "description"
                                        };

        private string[] _columns;

        public VehiclesList(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        public VehicleInfo[] Vehicles
        {
            get { return _vehicles; }
            set { _vehicles = value; }
        }

        public bool QuickGroupsEnable
        {
            get { return _quickGroupsEnable; }
            set { _quickGroupsEnable = value; }
        }

        public string[] Columns
        {
            get
            {
                if(_columns == null || _columns.Length == 0)
                {
                    _columns = _defaultColumns;
                }
                return _columns;
            }
            set { _columns = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            List<string> actualColumns = GetActualColumnsFromVehicles();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "guayaquil_table");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            WriteHeader(writer, actualColumns);

            foreach (VehicleInfo vehicle in Vehicles)
            {
                WriteRow(writer, vehicle, actualColumns);
            }

            writer.RenderEndTag();

            GuayaquilHelper.WriteGuayquilLabel(writer);
        }

        protected virtual void WriteRow(HtmlTextWriter writer, VehicleInfo vehicle, List<string> actualColumns)
        {
            PrepareVihecleInfo(vehicle, actualColumns);

            string link = FormatLink("vehicle", vehicle);

            //<tr onmouseout="this.className=\'\';" onmouseover="this.className=\'over\';" onclick="window.location=\''.$link.'\'">
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("window.location = '{0}'", link));
            writer.AddAttribute("onmouseout", "this.className='';");
            writer.AddAttribute("onmouseover", "this.className='over';");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (string column in actualColumns)
            {
                WriteCell(writer, vehicle, column, link);
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteCell(HtmlTextWriter writer, VehicleInfo vehicle, string column, string link)
        {
            //'<td>'.$this->DrawCellValue($row, $column, $link).'</td>';
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            WriteCellValue(writer, vehicle, column, link);

            writer.RenderEndTag();
        }

        protected virtual void WriteCellValue(HtmlTextWriter writer, VehicleInfo vehicle, string column, string link)
        {
            PropertyInfo propertyInfo = vehicle.GetType().GetProperty(column);
            object value = propertyInfo.GetValue(vehicle, null);
            string stringValue = value == null ? string.Empty : value.ToString();

            //<a href="'.$link.'">'.(string)$value.'</a>
            writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(stringValue);
            writer.RenderEndTag();
        }

        private void PrepareVihecleInfo(VehicleInfo vehicle, IEnumerable<string> actualColumns)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string column in actualColumns)
            {
                PropertyInfo propertyInfo = vehicle.GetType().GetProperty(column);
                object value = propertyInfo.GetValue(vehicle, null);

                if(value == null) continue;

                sb.AppendFormat("{0}: {1}; ", GetLocalizedString(string.Format("ColumnVehicle{0}", column)), value);
            }
            vehicle.PathData = sb.ToString().TrimEnd(' ', ';');
        }

        protected virtual void WriteHeader(HtmlTextWriter writer, List<string> actualColumns)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (string column in actualColumns)
            {
                WriteHeaderCell(writer, column);
            }

            writer.RenderEndTag();
        }

        protected virtual void WriteHeaderCell(HtmlTextWriter writer, string column)
        {
            //'<th>'.$this->DrawHeaderCellValue($column).'</th>';
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            WriteHeaderCellValue(writer, column);
            writer.RenderEndTag();
        }

        protected virtual void WriteHeaderCellValue(HtmlTextWriter writer, string column)
        {
            writer.Write(GetLocalizedString(String.Format("ColumnVehicle{0}", column)));
        }

        private List<string> GetActualColumnsFromVehicles()
        {
            List<string> actualColumns = new List<string>();
            foreach (string column in Columns)
            {
                if (actualColumns.Contains(column)) continue;

                PropertyInfo propertyInfo = typeof(VehicleInfo).GetProperty(column);

                foreach (VehicleInfo vehicle in Vehicles)
                {
                    object value = propertyInfo.GetValue(vehicle, null);

                    if (value != null)
                    {
                        actualColumns.Add(propertyInfo.Name);
                        break;
                    }
                }
            }
            return actualColumns;
        }
    }
}
