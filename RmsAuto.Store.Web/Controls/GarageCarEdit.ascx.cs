using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms;
using RmsAuto.Common.Data;
using RmsAuto.Common.Web;
using RmsAuto.TechDoc;
using RmsAuto.Store.Web.TecDoc;

namespace RmsAuto.Store.Web.Controls
{
	public partial class GarageCarEdit : System.Web.UI.UserControl
	{
		#region Events

		protected void ddEngineTypesInit( object sender, EventArgs e )
		{
			ddEngineTypes.FillItemsFromEnumTexts<EngineType>();
		}

		protected void ddBodyTypesInit( object sender, EventArgs e )
		{
			ddBodyTypes.FillItemsFromEnumTexts<BodyType>();
		}

		protected void ddTransmissionTypesInit( object sender, EventArgs e )
		{
			ddTransmissionTypes.FillItemsFromEnumTexts<TransmissionType>();
		}

		protected void ValidateFrameOrVIN( object source, ServerValidateEventArgs args )
		{
			args.IsValid = vrVIN.Text != String.Empty || vrFrameNumber.Text != String.Empty;
		}

		protected void _cvldModelValidate( object source, ServerValidateEventArgs args )
		{
			args.IsValid = !String.IsNullOrEmpty( _txtModel.Text ) || !String.IsNullOrEmpty( _ddModels.SelectedValue );
		}

		protected void _cvldBrandValidate( object source, ServerValidateEventArgs args )
		{
			args.IsValid = !String.IsNullOrEmpty( _txtBrand.Text ) || !String.IsNullOrEmpty( _ddBrands.SelectedValue );
		}

		protected void _cvldTransmissionTypeValidate( object source, ServerValidateEventArgs args )
		{
			var tt = (TransmissionType)Enum.Parse( typeof( TransmissionType ), ddTransmissionTypes.SelectedValue );
			args.IsValid = tt != TransmissionType.NotDefined;
		}


		protected void Page_Init( object sender, EventArgs e )
		{
			if( !IsPostBack )
			{
				BindBrands();
			}
		}

		void BindBrands()
		{
			_ddBrands.Items.Clear();
			_ddBrands.Items.Add( new ListItem( "не задана", String.Empty ) );
			foreach( var m in TecDocAggregator.GetManufacturers() )
			{
				_ddBrands.Items.Add( new ListItem( m.Brand.Name.ToUpper(), m.ID.ToString() ) );
			}
			_ddBrands.Items.Add( new ListItem( "другая марка", "~" ) );
		}

		void BindModels()
		{
			_ddModels.Items.Clear();

			_ddModels.Items.Add( new ListItem( "не задана", String.Empty ) );

			if( !string.IsNullOrEmpty( _ddBrands.SelectedValue ) && _ddBrands.SelectedValue != "~" )
			{
				foreach( var m in TecDocAggregator.GetModels( int.Parse( _ddBrands.SelectedValue ), null ) )
				{
					_ddModels.Items.Add( new ListItem( m.Name.Text.ToUpper(), m.ID.ToString() ) );
				}
			}
			_ddModels.Items.Add( new ListItem( "другая модель", "~" ) );
		}

		void BindModifications()
		{
			_ddModifications.Items.Clear();

			_ddModifications.Items.Add( new ListItem( "Не задана", String.Empty ) );

			if( !string.IsNullOrEmpty( _ddModels.SelectedValue ) && _ddModels.SelectedValue != "~" )
			{
				foreach( var m in TecDocAggregator.GetModifications( int.Parse( _ddModels.SelectedValue ) ) )
				{
					_ddModifications.Items.Add( new ListItem( m.Name.Text.ToUpper(), m.ID.ToString() ) );
				}
			}
			_ddModifications.Items.Add( new ListItem( "другая модификация", "~" ) );
		}

		string GetBrand()
		{
			if( _ddBrands.SelectedValue == "~" ) return _txtBrand.Text;
			else return !string.IsNullOrEmpty( _ddBrands.SelectedValue ) ? _ddBrands.SelectedItem.Text : "";
		}
		string GetModel()
		{
			if( _ddModels.SelectedValue == "~" ) return _txtModel.Text;
			else return !string.IsNullOrEmpty( _ddModels.SelectedValue ) ? _ddModels.SelectedItem.Text : "";
		}
		string GetModification()
		{
			if( _ddModifications.SelectedValue == "~" ) return _txtModification.Text;
			else return !string.IsNullOrEmpty( _ddModifications.SelectedValue ) ? _ddModifications.SelectedItem.Text : "";
		}

		void SetBrand( string manufacturerName )
		{
			var item = _ddBrands.Items.FindByText( manufacturerName.ToUpper() );
			if( item != null )
			{
				_ddBrands.SelectedValue = item.Value;
				_txtBrand.Text = "";
			}
			else
			{
				_ddBrands.SelectedValue = !string.IsNullOrEmpty( manufacturerName ) ? "~" : "";
				_txtBrand.Text = manufacturerName;
			}
		}

		void SetModel( string modelName )
		{
			var item = _ddModels.Items.FindByText( modelName.ToUpper() );
			if( item != null )
			{
				_ddModels.SelectedValue = item.Value;
				_txtModel.Text = "";
			}
			else
			{
				_ddModels.SelectedValue = !string.IsNullOrEmpty( modelName ) ? "~" : "";
				_txtModel.Text = modelName;
			}
		}

		void SetModification( string modificationName )
		{
			var item = _ddModifications.Items.FindByText( modificationName.ToUpper() );
			if( item != null )
			{
				_ddModifications.SelectedValue = item.Value;
				_txtModification.Text = "";
			}
			else
			{
				_ddModifications.SelectedValue = !string.IsNullOrEmpty( modificationName ) ? "~" : "";
				_txtModification.Text = modificationName;
			}
		}

		protected void _ddBrandsIndexChanged( object sender, EventArgs e )
		{
			string modelName = GetModel();
			string modificationName = GetModification();

			BindModels();
			SetModel( modelName );

			BindModifications();
			SetModification( modificationName );
		}

		protected void _ddModelsIndexChanged( object sender, EventArgs e )
		{
			string modificationName = GetModification();

			//SetModel( GetModel() );

			BindModifications();
			SetModification( modificationName );
		}

		protected void _ddModificationsIndexChanged( object sender, EventArgs e )
		{
			//SetModification( GetModification() );
		}

		#endregion

		#region interface

		public void Clear()
		{
			ddEngineTypes.SetSelected( String.Empty );
			ddBodyTypes.SetSelected( String.Empty );
			ddTransmissionTypes.SetSelected( String.Empty );
			_ddMonths.SelectedIndex = 0;

			SetBrand( "" );

			BindModels();
			SetModel( "" );

			BindModifications();
			SetModification( "" );

			vrEngine.Text = String.Empty;
			vrEngineCCM.Text = String.Empty;
			vrEngineHP.Text = String.Empty;
			vrYear.Text = String.Empty;
			vrVIN.Text = String.Empty;
			vrFrameNumber.Text = String.Empty;
			_txtBrand.Text = String.Empty;
			_txtModel.Text = String.Empty;
			_txtModification.Text = String.Empty;
		}

		public void FillCarData<T>( T carData ) where T : ICarParameters
		{
			carData.BodyType = (BodyType)Enum.Parse( typeof( BodyType ), ddBodyTypes.SelectedValue );
			carData.EngineType = (EngineType)Enum.Parse( typeof( EngineType ), ddEngineTypes.SelectedValue );
			carData.TransmissionType = (TransmissionType)Enum.Parse( typeof( TransmissionType ), ddTransmissionTypes.SelectedValue );
			carData.Brand = GetBrand();
			carData.Model = GetModel();
			carData.Modification = GetModification();
			carData.EngineNumber = vrEngine.Text;
			carData.EngineCCM = !String.IsNullOrEmpty( vrEngineCCM.Text ) ? Int16.Parse( vrEngineCCM.Text ) : (short?)null;
			carData.EngineHP = !String.IsNullOrEmpty( vrEngineHP.Text ) ? Int16.Parse( vrEngineHP.Text ) : (short?)null;
			carData.Frame = vrFrameNumber.Text;
			carData.Month = _ddMonths.SelectedValue != String.Empty ? Byte.Parse( _ddMonths.SelectedValue ) : (byte?)null;
			carData.Year = Int16.Parse( vrYear.Text );
			carData.TransmissionNumber = vrTransmissionNumber.Text;
			carData.VIN = vrVIN.Text;
		}

		public T GetNewCarData<T>() where T : ICarParameters, new()
		{
			T cd = new T();
			FillCarData<T>( cd );
			return cd;
		}

		public void SetFields( ICarParameters selectedCar )
		{
			ddEngineTypes.SetSelected( selectedCar.EngineType );
			ddBodyTypes.SetSelected( selectedCar.BodyType );
			ddTransmissionTypes.SetSelected( selectedCar.TransmissionType );

			if( String.IsNullOrEmpty( selectedCar.Brand ) ||
				String.IsNullOrEmpty( selectedCar.Model ) )
			{
				throw new ArgumentException( "Brand и Model не должны быть пусты" );
			}

			SetBrand( selectedCar.Brand );
			BindModels();
			SetModel( selectedCar.Model );
			BindModifications();
			SetModification( selectedCar.Modification );

			vrEngine.Text = selectedCar.EngineNumber;
			vrEngineCCM.Text = selectedCar.EngineCCM.ToString();
			vrEngineHP.Text = selectedCar.EngineHP.ToString();
			vrYear.Text = selectedCar.Year.ToString();
			if( selectedCar.Month.HasValue )
			{
				_ddMonths.SelectedValue = selectedCar.Month.ToString();
			}
			vrVIN.Text = selectedCar.VIN;
			vrTransmissionNumber.Text = selectedCar.TransmissionNumber;
			vrFrameNumber.Text = selectedCar.Frame;
		}

		#endregion


		protected void Page_PreRender( object sender, EventArgs e )
		{
			_txtBrand.Visible = _ddBrands.SelectedValue == "~";
			_txtModel.Visible = _ddModels.SelectedValue == "~";
			_txtModification.Visible = _ddModifications.SelectedValue == "~";
		}
	}
}