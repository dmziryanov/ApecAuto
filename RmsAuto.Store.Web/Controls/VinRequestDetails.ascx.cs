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

namespace RmsAuto.Store.Web.Controls
{
    public partial class VinRequestDetails : System.Web.UI.UserControl
    {
        public VinRequest VinRequest { get; set; }
        protected decimal TotalSum { get; set; }

		protected void Page_PreRender( object source, EventArgs e )
		{
			if( this.VinRequest != null )
			{
				_garageCarDetails.CarParameters = this.VinRequest;
                var items = this.VinRequest.VinRequestItems;
                if (!this.VinRequest.Proceeded)
                {
                    _rptRequestItems.DataSource = items;
                    _rptRequestItems.DataBind();
                }
                else
                {
                    this.TotalSum = items.Sum(i => i.PricePerUnit.Value * i.Quantity);
                    _rptAnsweredRequestItems.DataSource = items;
                    _rptAnsweredRequestItems.DataBind();
                }
			}
		}

        #region Interface properties & methods

        public string GetReadyClass()
        {
            return this.VinRequest != null ? (this.VinRequest.Proceeded ? "green" : "red") : null;
        }

        public string GetLeftDate()
        {
            if (this.VinRequest != null)
            {
                if (this.VinRequest.RequestDate.Date == DateTime.Now.Date)
                {
                    return String.Format("Сегодня, {0}", this.VinRequest.RequestDate.ToShortTimeString());
                }
                else
                {
                    return this.VinRequest.RequestDate.ToShortDateString();
                }
            }
            return null;
        }

        public string GetBottomDate()
        {
            if (this.VinRequest != null)
            {
                if (this.VinRequest.AnswerDate.HasValue)
                {
                    if (this.VinRequest.AnswerDate.Value.Date == DateTime.Now.Date)
                    {
                        return String.Format("Готов сегодня, {0}", this.VinRequest.AnswerDate.Value.ToShortTimeString());
                    }
                    else
                    {
                        return String.Format("Готов {0}", this.VinRequest.AnswerDate.Value.ToShortDateString());
                    }
                }
                else
                {
                    return "Пока не готов";
                }
            }
            return null;
        }

        #endregion
    }
}