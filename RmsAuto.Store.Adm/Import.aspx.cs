using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using RmsAuto.Store.Import;

namespace RmsAuto.Store.Adm
{
    public partial class Import : Security.BasePage /*System.Web.UI.Page*/
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void _importWizard_ActiveStepChanged(object sender, EventArgs e)
        {
            if (_importWizard.ActiveStepIndex ==
                _importWizard.WizardSteps.IndexOf(_selectModeStep))
            {
                if (_ddlCsvFormat.SelectedValue == ImportFacade.CrossesFormatName)
                {
                    _importWizard.MoveTo(_selectFileStep);
                }
            }
        }

        protected void _importWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            ImportMode mode = ImportMode.Smart;
            if (_ddlCsvFormat.SelectedValue == ImportFacade.PricesFormatName)
                mode = (ImportMode)Enum.Parse(typeof(ImportMode), _rblImportMode.SelectedValue, true);
            var report = ImportFacade.ImportData(
                _ddlCsvFormat.SelectedValue,
                mode, 
                _fileUpload.FileContent,
                _fileUpload.FileName);
            _importReportView.BindReportData(report);
        }
    }
}
