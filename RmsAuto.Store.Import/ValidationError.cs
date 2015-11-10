using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Import
{
    public class ValidationError
    {
        private string _rawRecord;
        private List<ValidationErrorDetailBase> _details = new List<ValidationErrorDetailBase>();

        public ValidationError(string rawRecord, IEnumerable<ValidationErrorDetailBase> details)
        {
            _rawRecord = rawRecord;
            _details.AddRange(details);
        }

        public string RawRecord
        {
            get { return _rawRecord; }
        }

        public IEnumerable<ValidationErrorDetailBase> Details
        {
            get { return _details; } 
        }
    }

	public abstract class ValidationErrorDetailBase
	{
		public abstract void WriteToLog( System.IO.StreamWriter log );
	}

	public class ValidationErrorDetail : ValidationErrorDetailBase
    {
        public string ColumnName { get; internal set; }

        public ValidationFailReason FailReason { get; internal set; }

		public override void WriteToLog( System.IO.StreamWriter log )
		{
			log.WriteLine( this.ColumnName + " - " + this.FailReason.ToTextOrName() );
		}
	}

	public class CustomValidationErrorDetail : ValidationErrorDetailBase
	{
		public string ErrorMessage { get; private set; }

		public CustomValidationErrorDetail( string errorMessage )
		{
			ErrorMessage = errorMessage;
		}

		public override void WriteToLog( System.IO.StreamWriter log )
		{
			log.WriteLine( this.ErrorMessage );
		}
	}

	public enum ValidationFailReason
    {
        None,
        [Text("отсутствует обязательное значение")]NullabilityViolation,
        [Text("значение несоответствует типу данных")] TypeConvertionFailure,
        [Text("превышен допустимый размер значения")] MaxLengthViolation
    }
}
