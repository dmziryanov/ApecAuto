<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="Feedback.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.Feedback" Title="" meta:resourcekey="PageResource1" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrAnonymous" />

	<h1>
        <asp:Literal runat="server" ID="_pageTitleLiteral" 
			meta:resourcekey="_pageTitleLiteralResource1" /></h1>

    <asp:Label ID="_errorLabel" runat="server" Font-Bold="True" ForeColor="Red" 
			meta:resourcekey="_errorLabelResource1"></asp:Label>

    <asp:Panel runat="server" ID="_sendPanel" meta:resourcekey="_sendPanelResource1">
        <div class="feedback">
            <ul>
                <li>
                    <div class="title">
                        <asp:Literal ID="lWhoAddress" runat="server" Text="Кому адресовать:" 
						    meta:resourcekey="lWhoAddressResource1" /> *
                    </div>
                    <asp:DropDownList runat="server" ID="_recipientList" Width="225px" 
						    meta:resourcekey="_recipientListResource1">
                        </asp:DropDownList>
                    <span id="Span1" class="info" title="Выберите отдел" meta:resourcekey="_selectDepKey" runat="server"></span>
                    <asp:RequiredFieldValidator
					    ID="_recipientValidator" runat="server" ControlToValidate="_recipientList"
					    ErrorMessage="не выбран отдел" ValidationGroup="FeedbackGroup" 
						    meta:resourcekey="_recipientValidatorResource1"></asp:RequiredFieldValidator>
                </li>
                <asp:PlaceHolder ID="_phUnauthorized2" runat="server">
                <li>
                    <div class="title">
                        <asp:Literal ID="lYouEmail" runat="server" Text="Ваш электронный адрес:" 
						    meta:resourcekey="lYouEmailResource1" /> *
                    </div>
                    <asp:TextBox ID="_txtEmail" runat="server" ValidationGroup="FeedbackGroup" 
					    meta:resourcekey="_txtEmailResource1" Width="218" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="FeedbackGroup"
                        ControlToValidate="_txtEmail" ErrorMessage="<%$ Resources:Exceptions, EmailIsInvalid %>"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
					    Display="Dynamic" meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtEmail" ValidationGroup="FeedbackGroup"
                        ErrorMessage="<%$ Resources:Exceptions, EnterEmail %>" Display="Dynamic" meta:resourcekey="RequiredFieldValidatorResource1" 
					    ></asp:RequiredFieldValidator>
                </li>
                </asp:PlaceHolder>
                <li>
                    <div class="title">
                        <asp:Literal ID="lYouMessage" runat="server" Text="Ваше сообщение:" 
						    meta:resourcekey="lYouMessageResource1" />
                    </div>
                    <asp:TextBox ValidationGroup="FeedbackGroup" runat="server" ID="_messageBox" TextMode="MultiLine"
                        Columns="50" Rows="10" Width="450px" Height="140px" CssClass="fdb" 
					    meta:resourcekey="_messageBoxResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_messageBox" ValidationGroup="FeedbackGroup"
                        ErrorMessage="<%$ Resources:Exceptions, EnterText %>" Display="Dynamic" meta:resourcekey="RequiredFieldValidatorResource2" 
					    ></asp:RequiredFieldValidator>
                </li>
                <asp:PlaceHolder ID="_phUnauthorized1" runat="server">
                    <li>
                        <div class="title">
                            <asp:Literal ID="lCaptcha" runat="server" Text="Код подтверждения" 
							    meta:resourcekey="lCaptchaResource1" /> *
                        </div>
                        <uc1:Captcha runat="server" ID="_captcha" />
                    </li>
                </asp:PlaceHolder>
                <li>
                    <uc1:TextItemControl runat="server" ID="TextBlock" ShowHeader="True" TextItemID="FeedbackHint" />
	                <br />
                    <asp:Button ID="_sendButton" OnClick="_sendButton_Click" CssClass="button" runat="server" Text="<%$ Resources:Texts, Send %>"
                            ValidationGroup="FeedbackGroup" meta:resourcekey="_sendButtonResource1" />
                </li>
            </ul>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="_sendOkPanel" 
		meta:resourcekey="_sendOkPanelResource1">
        <asp:Literal ID="lMessageSend" runat="server" Text="Ваше сообщение отправлено." 
			meta:resourcekey="lMessageSendResource1" />
    </asp:Panel>
</asp:Content>
