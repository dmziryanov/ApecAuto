extern alias DataStreams1; //to make sure it is DataStreams1.dll, reference alias is set to DataStreams1

using DataStreams1.DataStreams.Csv;
using DataStreams1.DataStreams.Common;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
	class ValidateRecordEventArgs : EventArgs
	{
		public readonly IDataRecord Record;

		DataReaderBase.RecordValuesCollection _values;
		List<ValidationErrorDetailBase> _validationErrorDetails;
		ImportActivity _importActivity;

		public ValidateRecordEventArgs( IDataRecord record, DataReaderBase.RecordValuesCollection values, ImportActivity importActivity )
		{
			if( record == null )
				throw new ArgumentNullException( "record" );
			Record = record;

			_values = values;
			_importActivity = importActivity;
		}

		public void AddValidationErrorDetail( ValidationErrorDetailBase validationErrorDetail )
		{
			if( _validationErrorDetails == null )
				_validationErrorDetails = new List<ValidationErrorDetailBase>();
			_validationErrorDetails.Add( validationErrorDetail );
		}

		public bool IsValid
		{
			get { return _validationErrorDetails == null; }
		}

		public ValidationError Error
		{
			get { return _validationErrorDetails != null ? new ValidationError( _importActivity.RestoreRawRecord( _values ), _validationErrorDetails ) : null; }
		}

		public bool SkipRecord
		{
			get;
			set;
		}

	}
}
