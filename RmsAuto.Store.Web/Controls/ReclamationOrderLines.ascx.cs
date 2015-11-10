using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Controls
{
	public partial class ReclamationOrderLines : System.Web.UI.UserControl
	{
		#region === Load OrderLines ===
		public static OrderTracking.OrderLineTotals CurrentOrderLineTotals
		{
			get { return (OrderTracking.OrderLineTotals)HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationOrderLines.CurrentOrderLineTotals"]; }
			set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationOrderLines.CurrentOrderLineTotals"] = value; }
		}

		public static OrderLine[] CurrentOrderLines
		{
			get { return (OrderLine[])HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationOrderLines.CurrentOrderLines"]; }
			set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationOrderLines.CurrentOrderLines"] = value; }
		}

		public static int GetOrderLinesCount(
			string orderNumber,
			string partNumber,
			int sort, int startIndex, int size )
		{
			if (CurrentOrderLineTotals == null)
			{
				var filter = new OrderTracking.OrderLineAnalysisFilter();

				if (!string.IsNullOrEmpty( orderNumber ))
				{
					filter.OrderIDs = new List<int> { Convert.ToInt32( orderNumber ) };
				}
				if (!string.IsNullOrEmpty( partNumber ))
				{
					filter.PartNumbers = new List<string> { partNumber };
				}

				filter.StatusIDs = new List<int> {
					OrderLineStatusUtil.StatusByte("InStock") /* поступил на склад */, 
					OrderLineStatusUtil.StatusByte("ReceivedByClient") /* получен клиентом */};

				CurrentOrderLineTotals = OrderTracking.GetOrderLinesCount( SiteContext.Current.CurrentClient.Profile.ClientId, filter );
			}
			return CurrentOrderLineTotals.TotalCount;
		}

		public static OrderLine[] GetOrderLines(
			string orderNumber,
			string partNumber,
			int sort, int startIndex, int size )
		{
			if (CurrentOrderLines == null)
			{
				var filter = new OrderTracking.OrderLineAnalysisFilter();

				if (!string.IsNullOrEmpty( orderNumber ))
				{
					filter.OrderIDs = new List<int> { Convert.ToInt32( orderNumber ) };
				}
				if (!string.IsNullOrEmpty( partNumber ))
				{
					filter.PartNumbers = new List<string> { partNumber };
				}

				filter.StatusIDs = new List<int> {
					OrderLineStatusUtil.StatusByte("InStock") /* поступил на склад */, 
					OrderLineStatusUtil.StatusByte("ReceivedByClient") /* получен клиентом */};

				CurrentOrderLines = OrderTracking.GetOrderLines( SiteContext.Current.CurrentClient.Profile.ClientId, filter, (OrderTracking.OrderLineSortFields)sort, startIndex, size );
			}
			return CurrentOrderLines;
		}
		#endregion

		protected void Page_Load( object sender, EventArgs e )
		{
			if (!IsPostBack)
			{
				//сортировка
				foreach (OrderTracking.OrderLineSortFields sortField in Enum.GetValues( typeof( OrderTracking.OrderLineSortFields ) ))
				{
					_sortBox.Items.Add( new ListItem( sortField.GetDescription(), ((int)sortField).ToString()));
				}
				_sortBox.SelectedValue = Convert.ToString((int)OrderTracking.OrderLineSortFields.OrderIDDesc);

				ApplyFilters(false);
			}
		}

		#region === Validators ===
		protected void ValidateOrderNumber(object source, ServerValidateEventArgs args)
		{
			int res = 0;
			args.IsValid = int.TryParse( args.Value.Trim(), out res );
		}
		#endregion

		protected void _searchButton_Click( object sender, EventArgs e )
		{
			if (Page.IsValid)
			{
				ApplyFilters( true );
			}
		}

		protected void _clearFilterButton_Click( object sender, EventArgs e )
		{
			_filterOrderNumber.Text = _filterPartNumber.Text = string.Empty;
			ApplyFilters( true );
		}

		protected void _objectDataSource_Selected( object sender, ObjectDataSourceStatusEventArgs e )
		{
			if (e.ReturnValue != null && e.ReturnValue.GetType() == typeof( int ))
			{
				int count = Convert.ToInt32( e.ReturnValue );
				//_totalsBlock.Visible = count != 0;
				_dataPager.Visible = count != 0;
				_pagerSettingsBlock.Visible = count != 0;
			}
		}

		protected void _pageSizeBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, Convert.ToInt32( _pageSizeBox.SelectedValue ), true );
		}

		protected void _sortBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
		}

		private void ApplyFilters( bool bind )
		{
			_objectDataSource.SelectParameters["orderNumber"].DefaultValue = _filterOrderNumber.Text.Trim();
			_objectDataSource.SelectParameters["partNumber"].DefaultValue = _filterPartNumber.Text.Trim();

			if (bind)
			{
				_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
			}
		}
	}
}