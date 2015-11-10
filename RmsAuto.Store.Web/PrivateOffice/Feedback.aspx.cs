using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Text;
using RmsAuto.Common.Web.UI;
using RmsAuto.Store.Cms.Dac;
//using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Web.Cms
{
    public partial class Feedback : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
            _sendPanel.Visible = true;
			_sendOkPanel.Visible = false;
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;
            
			if( !IsPostBack )
			{
				_phUnauthorized1.Visible = _phUnauthorized2.Visible = !User.Identity.IsAuthenticated;

				_recipientList.DataTextField = "RecipientName";
				_recipientList.DataValueField = "RecipientID";
				_recipientList.DataSource = FeedbackDac.GetFeedbackRecipients(SiteContext.CurrentCulture);
				_recipientList.DataBind();
                _recipientList.AppendDataBoundItems = true;
                _recipientList.Items.Insert(0, new ListItem("", ""));
                _recipientList.SelectedValue = "";
                _recipientValidator.InitialValue = "";
			}
		}

		protected void _sendButton_Click( object sender, EventArgs e )
		{
			if( Page.IsValid )
			{
				string email, profileInfo;

				if( User.Identity.IsAuthenticated )
				{
                    email = SiteContext.Current.CurrentClient.Profile.Email;
					profileInfo = SiteContext.Current.CurrentClient.Profile.Category==RmsAuto.Store.Acctg.ClientCategory.Legal ? SiteContext.Current.CurrentClient.Profile.ContactPerson : SiteContext.Current.CurrentClient.Profile.ClientName;
				}
				else
				{
					email = profileInfo = _txtEmail.Text;
				}

				FeedbackBO.SendMessage(
					Convert.ToInt32( _recipientList.SelectedValue ),
					profileInfo,
					email,
					_messageBox.Text.Trim() );

				_sendPanel.Visible = false;
				_sendOkPanel.Visible = true;
			}
			else
			{
				_errorLabel.Text = "Возникла ошибка";
			}
		}
	}
}
