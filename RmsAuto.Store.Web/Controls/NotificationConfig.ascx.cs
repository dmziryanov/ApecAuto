using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Dac;
using System.Web.UI.HtmlControls;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Controls
{
    public partial class NotificationConfig : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Dictionary<byte, string> dNames = new Dictionary<byte, string>();

                using (var dc = new DCWrappersFactory<StoreDataContext>())
                {
                    //deas 30.03.2011 task3586
                    // добавлена сортировка и условие отображения
                    foreach ( var statusElement in dc.DataContext.OrderLineStatuses.OrderBy(t => t.ClientShowOrder))
                    {
                        var status = statusElement.OrderLineStatusID;
                        if (OrderLineStatusUtil.IsShow(status))
                        {
                            dNames.Add( status, OrderLineStatusUtil.DisplayName( status ) );
                        }
                    }
                }

                _listView.DataSource = dNames;
                _listView.DataBind();

                InitControls();
            }
        }

        protected void btnSave_Click( object sender, EventArgs e )
        {
            //сохраняем настройки рассылки для текущего пользователя
            //string clientId = SiteContext.Current.User.AcctgID;
            //int hour = -1;
            //if ( ddlNotificationFrequency.SelectedIndex == 1 ) { hour = Convert.ToInt32( ddlPeriod.SelectedValue ); }
            //string statusIds = hfSelectedStatuses.Value;

            //ClientAlertConfigDac.SaveClientAlertConfig( clientId, hour, statusIds );
            int hour = -1;
            if ( ddlNotificationFrequency.SelectedIndex == 1 ) {hour = Convert.ToInt32(ddlPeriod.SelectedValue); }
            string statusIds = hfSelectedStatuses.Value;

            using (var DC = new DCWrappersFactory<StoreDataContext>())
            {
                DC.DataContext.spUpdUSAlertConfig(SiteContext.Current.User.UserId, hour, statusIds);
            }

            InitControls();
        }

        private void InitControls()
        {
            //выставляем настройки рассылки текущего пользователя
            //var alertConfig = ClientAlertConfigDac.GetAlertConfigByClientId( SiteContext.Current.User.AcctgID );
            var DC = new DCWrappersFactory<StoreDataContext>();
            var alertConfig = DC.DataContext.spSelUserSetting( SiteContext.Current.User.UserId ).FirstOrDefault();

            if ( alertConfig != null )
            {
                if ( alertConfig.AlertHourOfPeriod > 0 )
                {
                    ddlNotificationFrequency.SelectedIndex = 1; //Раз в сутки
                    foreach ( ListItem item in ddlPeriod.Items )
                    {
                        if ( item.Value == alertConfig.AlertHourOfPeriod.ToString() )
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }

				if (alertConfig.AlertStatusIDs != null && alertConfig.AlertStatusIDs != "all")
				{
					string[] statusIds = alertConfig.AlertStatusIDs.Split(';');
					foreach (var item in _listView.Items)
					{
						HtmlInputCheckBox chb = (HtmlInputCheckBox)item.Controls.FindControl("chkStatus");
						if (statusIds.Length > 0 && !statusIds.Contains(chb.Value))
							chb.Checked = false;
						if (statusIds.Length == 0) chb.Checked = false;
					}
				}
				else if(alertConfig.AlertStatusIDs == "all") //по умолчанию выбраны все статусы
				{
					foreach (var item in _listView.Items)
					{
						HtmlInputCheckBox chb = (HtmlInputCheckBox)item.Controls.FindControl("chkStatus");
						chb.Checked = true;
					}
				}
            }
        }

    }
}