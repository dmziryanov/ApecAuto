using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Adm
{
	public partial class PendingLogins : Security.BasePage/*System.Web.UI.Page*/
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if( !IsPostBack )
            {
                _errorPanel.Visible = false;
            }
        }

        protected void LoginsGridView_OnRowCommand(object sender, GridViewCommandEventArgs args)
        {
            if (args.CommandName == "SendActivationEmail")
            {
                var maintUid = new Guid(args.CommandArgument.ToString());
                ClientBO.ResendActivationEmailForNewClient(maintUid, MakeActivationUrl);
                this.ShowMessage("Письмо с активационной ссылкой успешно отправлено");
            }
            else if (args.CommandName == "FlashActivation")
            {
                //DisplayError("Логин пользователя успешно активирован", MessageType.Info);
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "redirect", "location.href = '" + "http://localhost/RmsAuto.Store.Adm/PendingLogins.aspx" + "'", true);


                Guid maintUid = new Guid(args.CommandArgument.ToString());

                if (ClientBO.GetUserMaintEntryPurpose(maintUid, true)
                    == UserMaintEntryPurpose.NewClientRegistration)
                {
                    try
                    {
                        if (SiteContext.Current != null)
                        {
                            ClientBO.CompleteOnlineRegistration(maintUid, true, SiteContext.Current.InternalFranchName);
                        }
                        else
                        { 
                             ClientBO.CompleteOnlineRegistration(maintUid, true, ConfigurationManager.AppSettings["InternalFranchName"]);

                        }
                        // deas 28.04.2011 task3979
                        // убрана проверка загрузки профиля, т.к. это не возможно при новой регистрации
                        //try
                        //{
                        //    var profile = ClientProfile.Load(clientId);
                            DisplayError("Регистрация пользователя закончена, логин пользователя будет активирован в течении нескольких минут.", MessageType.Info);
                            Response.Redirect(Request.RawUrl);
                            //_loginGridViewUpdatePanel.Update();
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "redirect", "location.href = '" + UrlManager.MakeAbsoluteUrl("/PendingLogins.aspx") + "'", true);
                            //_loginsGridView.DataSource = _loginsDataSource;
                            //_loginsGridView.DataBind();
                            //if (profile.TradingVolume == TradingVolume.Retail)
                            //    DisplayError("Логин пользователя со свойством \"" + TradingVolume.Retail.ToTextOrName() + "\" успешно активирован", MessageType.Info);
                            //else if (profile.TradingVolume == TradingVolume.Wholesale)
                            //    DisplayError("Логин пользователя со свойством \"" + TradingVolume.Wholesale.ToTextOrName() + "\" успешно активирован", MessageType.Info);
                        //}
                        //catch
                        //{
                        //}
                    }
                    catch (Acctg.AcctgException ex)
                    {
                        if (ex.ErrorCode == RmsAuto.Store.Acctg.AcctgError.ClientAlreadyExists)
                            DisplayError("Ошибка активации. Регистрационные данные уже используются другим клиентом интернет-магазина", MessageType.Error);
                        else
                            throw ex;
                    }
                }
                else
                    DisplayError("Некорректное состояние регистрационных данных. Можно активировать только при статусе " + UserMaintEntryPurpose.NewClientRegistration.ToTextOrName(), MessageType.Error);

            }
        }

        
        private enum MessageType: byte 
        {
            Error,
            Info
        }

        private void DisplayError(string errorMessage, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    _errorMessageLabel.CssClass = "error";
                    break;
                case MessageType.Info:
                default:
                    _errorMessageLabel.CssClass = "info";
                    break;
            }
            _errorMessageLabel.Text = errorMessage.Replace("\n", "<br/>");
            _errorPanel.Visible = true;
        }



        private string MakeActivationUrl(Guid maintUid)
        {
            return UrlManager.GetClientActivationUrl(maintUid, String.Compare(ConfigurationManager.AppSettings["InternalFranchName"], SiteContext.Current.InternalFranchName, true) == 0 ? null : SiteContext.Current.InternalFranchName);

                //ConfigurationManager.AppSettings["WebSiteUrl"] +
                //"/Cabinet/Activation.aspx?" + 
                //UrlKeys.Activation.MaintUid + "=" + maintUid.ToString() + "&" + UrlKeys.Activation.FranchCode + "=rmsauto";
        }

        protected void _btnApplyFilter_Click(object sender, EventArgs e)
        {
            _loginsGridView.PageIndex = 0;
        }
    }
}
