using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Controls
{
	public partial class ReclamationList : System.Web.UI.UserControl
	{
		#region === Load Reclamations ===
		public static int CurrentReclamationsCount
		{
			get { return (int)HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationList.CurrentReclamationsCount"]; }
			set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationList.CurrentReclamationsCount"] = value; }
		}

		public static RmsAuto.Store.Entities.Reclamation[] CurrentReclamations
		{
			get { return (RmsAuto.Store.Entities.Reclamation[])HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationList.CurrentReclamations"]; }
			set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsReclamationList.CurrentReclamations"] = value; }
		}

		public static int GetReclamationsCount(
			/*ReclamationTracking.ReclamationType?*/int reclamationType,
			DateTime? reclamationDateBegin,
			DateTime? reclamationDateEnd,
			int sort, int startIndex, int size )
		{
			//if ( CurrentReclamationsCount == null )
			//{
			var filter = new ReclamationTracking.ReclamationFilter();

			if ( reclamationType != -1 )
			{
				filter.ReclamationType = (ReclamationTracking.ReclamationType)reclamationType;
			}
			
			filter.ReclamationDateBegin = reclamationDateBegin;
			filter.ReclamationDateEnd = reclamationDateEnd;
			
			CurrentReclamationsCount = ReclamationTracking.GetReclamationsCount( SiteContext.Current.CurrentClient.Profile.ClientId, filter );
			//}
			return CurrentReclamationsCount;
		}

		public static RmsAuto.Store.Entities.Reclamation[] GetReclamations(
			/*ReclamationTracking.ReclamationType?*/int reclamationType,
			DateTime? reclamationDateBegin,
			DateTime? reclamationDateEnd,
			int sort, int startIndex, int size )
		{
			if ( CurrentReclamations == null )
			{
				var filter = new ReclamationTracking.ReclamationFilter();
				
				if ( reclamationType != -1 )
				{
					filter.ReclamationType = (ReclamationTracking.ReclamationType)reclamationType;
				}

				filter.ReclamationDateBegin = reclamationDateBegin;
				filter.ReclamationDateEnd = reclamationDateEnd;

				CurrentReclamations = ReclamationTracking.GetReclamations( SiteContext.Current.CurrentClient.Profile.ClientId, filter, (ReclamationTracking.ReclamationSortFields)sort, startIndex, size );
			}
			return CurrentReclamations;
		}
		#endregion

		protected void Page_Load( object sender, EventArgs e )
		{
			if ( !IsPostBack )
			{
				//сортировка
				foreach ( ReclamationTracking.ReclamationSortFields sortField in Enum.GetValues( typeof( ReclamationTracking.ReclamationSortFields ) ) )
				{
					_sortBox.Items.Add( new ListItem( sortField.ToTextOrName(), ((int)sortField).ToString() ) );
				}
				//фильтр по типу
				InitReclamationTypeFilter(_filterReclamationType);

				ApplyFilters( false );
			}
		}

		protected void _sortBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
		}

		protected void _pageSizeBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_dataPager.SetPageProperties( 0, Convert.ToInt32( _pageSizeBox.SelectedValue ), true );
		}

		protected void _searchButton_Click( object sender, EventArgs e )
		{
			if ( Page.IsValid )
			{
				ApplyFilters( true );
			}
		}

		protected void _clearFilterButton_Click( object sender, EventArgs e )
		{
			_filterReclamationType.SelectedValue = "-1";
			_filterStartDate.Text = string.Empty;
			_filterEndDate.Text = string.Empty;

			ApplyFilters( true );
		}

		protected string GetReclamationTypeName(ReclamationTracking.ReclamationType type)
		{
			return type.ToTextOrName();
		}

		#region ==== Validators ====
		protected void ValidateDate( object source, ServerValidateEventArgs args )
		{
			string value = args.Value;
			CustomValidator validator = (CustomValidator)source;

			switch ( validator.ControlToValidate )
			{
				case "_filterStartDate":
					//проверяем начальную дату
					try
					{
                        DateTime.Parse(_filterStartDate.Text.Trim(), CultureInfo.CurrentCulture, DateTimeStyles.None);
					}
					catch
					{
						validator.Text = Resources.ValidatorsMessages.FormatError_InitialDate; //"неверный формат начальной даты";
						args.IsValid = false;
						return;
					}
					break;
				case "_filterEndDate":
					//проверяем конечную дату
					DateTime endDate = DateTime.Now;
					try
					{
                        endDate = DateTime.Parse(_filterEndDate.Text.Trim(), CultureInfo.CurrentCulture, DateTimeStyles.None);
					}
					catch
					{
						validator.Text = Resources.ValidatorsMessages.FormatError_FinishDate; //"неверный формат конечной даты";
						args.IsValid = false;
						return;
					}

					if ( !string.IsNullOrEmpty( _filterStartDate.Text ) )
					{
						//проверяем начальную дату
						DateTime startDate = DateTime.Now;
						try
						{
                            startDate = DateTime.Parse(_filterStartDate.Text.Trim(), CultureInfo.CurrentCulture, DateTimeStyles.None);
						}
						catch
						{
							validator.Text = Resources.ValidatorsMessages.FormatError_InitialDate; //"неверный формат начальной даты";
							args.IsValid = false;
							return;
						}
						//сравниваем даты
						if ( endDate < startDate )
						{
							validator.Text = Resources.ValidatorsMessages.StartDateExceedFinishDate; //"начальная дата не может превышать конечную";
							args.IsValid = false;
							return;
						}
					}
					break;
			}
			args.IsValid = true;
		}
		#endregion

		private void InitReclamationTypeFilter( DropDownList ddl )
		{
			//ddl.Items.Add( new ListItem( "все", "-1" ) );
			//ddl.Items.Add( new ListItem( "возврат", "0" ) );
			//ddl.Items.Add( new ListItem( "отказ", "1" ) );
			ddl.Items.Add(new ListItem(Resources.Reclamations.All, "-1"));
			ddl.Items.Add(new ListItem(Resources.Reclamations.Return, "0"));
			ddl.Items.Add(new ListItem(Resources.Reclamations.Refusal, "1"));
		}

		private void ApplyFilters( bool bind )
		{
			_objectDataSource.SelectParameters["reclamationType"].DefaultValue = _filterReclamationType.SelectedValue;
			_objectDataSource.SelectParameters["reclamationDateBegin"].DefaultValue = _filterStartDate.Text.Trim();
			_objectDataSource.SelectParameters["reclamationDateEnd"].DefaultValue = _filterEndDate.Text.Trim();

			if ( bind )
			{
				_dataPager.SetPageProperties( 0, _dataPager.PageSize, true );
			}
		}		
	}
}