using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    public class ImportReport
    {
        private List<Exception> _errors;
        private List<ValidationError> _validationErrors;
        private LoadCounters _counters = new LoadCounters();
                
        public string Filename {  get; internal set;  }
        public int LoadResult { get; set; }

        public ImportMode Mode { get; internal set; }
        
        public int TotalCount {  get; internal set;  }

        public int InvalidCount 
        {
            get { return _validationErrors != null ? _validationErrors.Count : 0; }
        }

        public LoadCounters Counters
        {
            get { return _counters; }
            internal set { _counters = value; }
        }

        public ImportReport()
        {
            LoadResult = 0;
        }

        public TimeSpan Elapsed { get; internal set; }
                
        public IEnumerable<Exception> Errors
        {
            get { return _errors; }
        }

        public bool HasErrors
        {
            get { return _errors != null && _errors.Count > 0; }
        }
        
        public void AddError(Exception errorInfo)
        {
            if (errorInfo == null)
                throw new ArgumentNullException("errorInfo");
            if (_errors == null)
                _errors = new List<Exception>();
            _errors.Add(errorInfo);
        }

        public IEnumerable<ValidationError> ValidationErrors
        {
            get { return _validationErrors; }
        }

        public bool HasValidationErrors
        {
            get { return _validationErrors != null && _validationErrors.Count > 0; }
        }

        public void AddError(ValidationError error)
        {
            if (error == null)
                throw new ArgumentNullException("error");
            if (_validationErrors == null)
                _validationErrors = new List<ValidationError>();
            _validationErrors.Add(error);
        }
    }
}
