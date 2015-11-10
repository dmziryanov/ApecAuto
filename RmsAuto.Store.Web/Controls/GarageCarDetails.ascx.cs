using System;
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

namespace RmsAuto.Store.Web.Controls
{
	public partial class GarageCarDetails : System.Web.UI.UserControl
	{
        public ViewType CarViewType { get; set; }
		public ICarParameters CarParameters { get; set; }

		protected void Page_PreRender( object sender, EventArgs e )
		{
            if (this.CarParameters != null)
            {
                _trVinPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.VIN);
                _vinLabel.Text = Server.HtmlEncode(this.CarParameters.VIN ?? String.Empty);

                _trFrameNumberPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.Frame);
                _frameLabel.Text = Server.HtmlEncode(this.CarParameters.Frame ?? String.Empty);

                _trBrandPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.Brand);
                _brandLabel.Text = Server.HtmlEncode(this.CarParameters.Brand ?? String.Empty);

                _trModelPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.Model);
                _modelLabel.Text = Server.HtmlEncode(this.CarParameters.Model ?? String.Empty);

                _trModificationPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.Modification);
                _modificationLabel.Text = Server.HtmlEncode(this.CarParameters.Modification ?? String.Empty);

                _yearLabel.Text = string.Format("{0}", this.CarParameters.Year);

                _trMonthPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || this.CarParameters.Month.HasValue;
                _monthLabel.Text = string.Format("{0}", this.CarParameters.Month);

                _trEngineTypePlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || this.CarParameters.EngineType != EngineType.NotDefined;
                _engineTypeLabel.Text = this.CarParameters.EngineType.ToTextOrName();

                _trEngineNumberPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.EngineNumber);
                _engineNumberLabel.Text = Server.HtmlEncode(this.CarParameters.EngineNumber ?? String.Empty);

                _trEngineCCMPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || this.CarParameters.EngineCCM.HasValue;
                _engineCCMLabel.Text = string.Format("{0}", this.CarParameters.EngineCCM);

                _trEngineHPPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || this.CarParameters.EngineHP.HasValue;
                _engineHPLabel.Text = String.Format("{0}", this.CarParameters.EngineHP);

                _trTransmissionTypePlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || this.CarParameters.TransmissionType != TransmissionType.NotDefined;
                _transmissionTypeLabel.Text = this.CarParameters.TransmissionType.ToTextOrName();

                _trTransmissionNumberPlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || !String.IsNullOrEmpty(this.CarParameters.TransmissionNumber);
                _transmissionNumberLabel.Text = Server.HtmlEncode(this.CarParameters.TransmissionNumber ?? String.Empty);

                _trBodyTypePlace.Visible = this.CarViewType != ViewType.OnlyPresentFields || this.CarParameters.BodyType != BodyType.NotDefined;
                _bodyTypeLabel.Text = this.CarParameters.BodyType.ToTextOrName();
            }
            else
            {
                _table.Visible = false;
            }
		}

        public enum ViewType
        {
            AllFields = 0,
            OnlyPresentFields = 1
        }
    }
}