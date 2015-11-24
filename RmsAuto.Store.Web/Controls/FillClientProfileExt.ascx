<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FillClientProfileExt.ascx.cs"
    Inherits="RmsAuto.Store.Web.Controls.FillClientProfileExt" %>
<%@ Register Src="~/Controls/WizardTopSideBar.ascx" TagName="WizardTopSideBar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/DiscountCard_Edit.ascx" TagName="DiscountCard_Edit"
    TagPrefix="uc1" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Phone_Edit.ascx" TagName="Phone_Edit" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/EditUser.ascx" TagName="EditUser" TagPrefix="uc1" %>

<script type="text/javascript">
    function _cvDdlIsSelect_ClientValidate(source, arguments)
    {
        if (arguments.Value == 0)
        {
            arguments.IsValid = false;
        }
        else
        {
            arguments.IsValid = true;
        }
    }
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:Wizard ID="_fillProfileWizard" runat="server" OnFinishButtonClick="_fillProfileWizard_FinishButtonClick"
    OnActiveStepChanged="_fillProfileWizard_ActiveStepChanged" DisplaySideBar="false"
    Width="100%" StartNextButtonStyle-CssClass="button" StartNextButtonText="<%$ Resources:Texts, Next %>" StartNextButtonType="Button" StepNextButtonStyle-CssClass="button" StepNextButtonText="<%$ Resources:Texts, Next %>" StepNextButtonType="Button"
    StepPreviousButtonStyle-CssClass="button" StepPreviousButtonText="<%$ Resources:Texts, Back %>" StepPreviousButtonType="Button"
    FinishPreviousButtonStyle-CssClass="button" FinishPreviousButtonType="Button" FinishPreviousButtonText="<%$ Resources:Texts, Back %>" FinishCompleteButtonStyle-CssClass="button"
    FinishCompleteButtonType="Button" FinishCompleteButtonText="<%$ Resources:Texts, GetRegistered %>">
    <NavigationStyle CssClass="wizard_btn" />
    <SideBarStyle CssClass="wizard_left" />
    <HeaderTemplate>
        <h1><% = Title %></h1>
        <table class="steps">
            <tr>
                <uc1:WizardTopSideBar runat="server">
                    <StepElement>
                        <td style="width:25%;">
                            <span><%# Container.StepTitle %></span>
                        </td>
                    </StepElement>
                    <StepElementSelected>
                        <td class="on" style="width:25%;">
                            <span><%# Container.StepTitle %></span>
                        </td>
                    </StepElementSelected>
                </uc1:WizardTopSideBar>
            </tr>
        </table>
    </HeaderTemplate>
    <WizardSteps>
        <asp:WizardStep ID="_generalInfoStep" runat="server" Title="<%$ Resources:RegistrationTexts, Overview %>" StepType="Start">
            <div class="information">
                <table>
                    <tr>
                        <th><asp:Literal ID="lEmail" runat="server" Text="<%$ Resources:Texts, Email %>" />: *</th>
                        <td>
                            <asp:TextBox ID="_txtEmail" runat="server" Width="165px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_emailRequiredValidator" runat="server" ControlToValidate="_txtEmail"
                                Display="Dynamic" ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyEmail %>"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="_txtEmail"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidEmail %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="_emailCustomValidator" runat="server" Display="Dynamic"
                                ControlToValidate="_txtEmail" ErrorMessage="<%$ Resources:ValidatorsMessages, EmailInUse %>"
                                OnServerValidate="_ValidateEmail"></asp:CustomValidator>
                            <uc1:TextItemControl ID="_emailImportantTextItem" runat="server" TextItemID="Registration.EmailImportant"
                                ShowHeader="False" />
                            <uc1:TextItemControl ID="_emailHintTextItem" runat="server" TextItemID="Registration.EmailHint"
                                ShowHeader="False" />
                        </td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCountry" runat="server" Text="<%$ Resources:RegistrationTexts, Country %>" />: *</th>
                        <td>
                            <asp:DropDownList ID="_ddlCountry" runat="server" Width="230px">
                            <asp:ListItem Text="<%$ Resources:RegistrationTexts, SelectDdl %>" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<%$ Resources:ValidatorsMessages, SelectCountry %>"
                                ControlToValidate="_ddlCountry" Display="Dynamic" OnServerValidate="_cvDdlIsSelect_ServerValidate" />
                        </td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lLocalyti" runat="server" Text="<%$ Resources:RegistrationTexts, City %>" />: *</th>
                        <td><asp:TextBox ID="_txtLocality" runat="server" Width="222px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLocality" runat="server" ControlToValidate="_txtLocality"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, CityNotSpecified %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                Display="Dynamic" ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>"
                                ControlToValidate="_txtLocality" ValidationExpression="^[А-ЯЁа-яёA-Za-z\-\s]*$" />
                        </td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lScopeType" runat="server" Text="<%$ Resources:RegistrationTexts, Scope %>" />: *</th>
                        <td>
                            <asp:DropDownList ID="_ddlScopeType" runat="server" Width="230px">
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SelectDdl %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, Scope_OnlineShop %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, Scope_SparePartsShop %>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, Scope_AutoService %>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, Scope_PrivatePerson %>" Value="4"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, Scope_Other %>" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="<%$ Resources:ValidatorsMessages, SelectFromList %>"
                                ControlToValidate="_ddlScopeType" Display="Dynamic" OnServerValidate="_cvDdlIsSelect_ServerValidate"
                                ClientValidationFunction="_cvDdlIsSelect_ClientValidate" />
                            <br />
                            <asp:TextBox ID="_txtScopeType" runat="server" Width="222px" Height="40px" TextMode="MultiLine"
                                ToolTip="<%$ Resources:RegistrationTexts, CommentHint %>"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lHKT" runat="server" Text="<%$ Resources:RegistrationTexts, SourceOfInformation %>" />: *</th>
                        <td>
                            <asp:DropDownList ID="_ddlHKT" runat="server" Width="230px">
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SelectDdl %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_InternetSearch %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_OnlineAdvertising %>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_PreviouslyWorked %>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_Recommended %>" Value="4"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_Other %>" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="<%$ Resources:ValidatorsMessages, SelectSourceInformation %>"
                                ControlToValidate="_ddlHKT" Display="Dynamic" OnServerValidate="_cvDdlIsSelect_ServerValidate"
                                ClientValidationFunction="_cvDdlIsSelect_ClientValidate" />
                            <br />
                            <asp:TextBox ID="_txtHKT" runat="server" Width="222px" Height="40px" TextMode="MultiLine"
							    ToolTip="<%$ Resources:RegistrationTexts, CommentHint %>"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <uc1:EditUser ID="_editUser" runat="server"/>
        </asp:WizardStep>
       <asp:WizardStep ID="_contactInfoStep" runat="server" Title="<%$ Resources:RegistrationTexts, CustomerData %>" StepType="Step">
            <div class="information">
                <table>
                    <tr>
                        <th></th>
                        <td><span class="blue"><asp:Literal ID="lContactInfo" runat="server" Text="<%$ Resources:RegistrationTexts, ContactInformation %>" /></span></td>
                    </tr>
                    <asp:PlaceHolder ID="vreg_LegalIP_202" runat="server">
                        <tr>
                            <th><asp:Literal ID="lContactPosition" runat="server" Text="<%$ Resources:RegistrationTexts, Position %>" /></th>
                            <td><asp:TextBox ID="_txtContactPosition" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator21" runat="server"
                                Display="Dynamic" ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>"
                                ControlToValidate="_txtContactPosition" ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" />
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th><asp:Literal ID="lContactLastName" runat="server" Text="<%$ Resources:RegistrationTexts, Surname %>" />: *</th>
                        <td><asp:TextBox ID="_txtContactLastName" runat="server" Width="222px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_lastNameReqValidator" runat="server" ControlToValidate="_txtContactLastName"
                            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptySurname %>" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>"
                            ControlToValidate="_txtContactLastName" ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" />
                        </td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lContactFirstName" runat="server" Text="<%$ Resources:RegistrationTexts, Name %>" />: *</th>
                        <td><asp:TextBox ID="_txtContactFirstName" runat="server" Width="222px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_firstNameReqValidator" runat="server" ControlToValidate="_txtContactFirstName"
                            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyName %>" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                            ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>" ControlToValidate="_txtContactFirstName"
                            ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" /></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lContactPhone" runat="server" Text="<%$ Resources:RegistrationTexts, Phone %>" />: *</th>
                        <td><uc1:Phone_Edit ID="_ContactPhone" runat="server" Required="true" CheckSummCount="True" /></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lContactExtPhone" runat="server" Text="<%$ Resources:RegistrationTexts, PhoneExt %>" /></th>
                        <td><uc1:Phone_Edit ID="_ContactExtPhone" runat="server" Required="false" /></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:RegistrationTexts, RegisterAs %>" /></th>
                        <td><asp:RadioButtonList ID="SellerInfo" runat="server" Required="false">
                                           <asp:ListItem Text="<%$ Resources:RegistrationTexts, Seller %>" Value="1"></asp:ListItem>
                                           <asp:ListItem Text="<%$ Resources:RegistrationTexts, Customer %>" Value="0"></asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
       <%--             <asp:PlaceHolder ID="vregRO_Opt_272" runat="server">
                        <tr>
                            <th><asp:Literal ID="lContactFax" runat="server" Text="<%$ Resources:RegistrationTexts, Fax %>" /></th>
                            <td><uc1:Phone_Edit ID="_ContactFax" runat="server" Required="false" CheckSummCount="True" /></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lContactPersonEmail" runat="server" Text="<%$ Resources:Texts, Email %>" />: *</th>
                            <td><asp:TextBox ID="_txtContactPersonEmail" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvContactPersonEmail" runat="server" ControlToValidate="_txtContactPersonEmail"
                                Display="Dynamic" ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyEmail %>"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revContactPersonEmail" runat="server" ControlToValidate="_txtContactPersonEmail"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidEmail %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic"></asp:RegularExpressionValidator> 
                            <uc1:TextItemControl ID="TextItemControl3" runat="server" TextItemID="Registration.EmailHint"
                                ShowHeader="False" /></td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th>
                            <asp:Literal ID="lShippingAddress" runat="server" Text="<%$ Resources:RegistrationTexts, DeliveryAddress %>" />: **<br /><br />
                            <asp:PlaceHolder ID="vregRO_Opt_299" runat="server">
                                <uc1:TextItemControl ID="TextItemControl1" runat="server" TextItemID="Registration.ShippingOptInfo"
                                    ShowHeader="False" />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="vregRO_Rozn_303" runat="server">
                                <uc1:TextItemControl ID="TextItemControl2" runat="server" TextItemID="Registration.ShippingRoznInfo"
                                    ShowHeader="False" />
                            </asp:PlaceHolder>
                        </th>
                        <td><asp:TextBox ID="_txtDeliveryAddress" runat="server" Height="100px" MaxLength="1024" TextMode="MultiLine" Width="222px"></asp:TextBox></td>
                    </tr>
                    <asp:PlaceHolder ID="vreg_LegalIP_302" runat="server">
                        <tr>
                            <th></th>
                            <td><span class="blue"><asp:Literal ID="lLegalInfo" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyDetails %>" /></span></td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="vreg_Legal_312" runat="server">
                        <tr>
                            <th><asp:Literal ID="lLegalName" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyName %>" />: *</th>
                            <td><asp:TextBox ID="_txtLegalName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtLegalName"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyCompanyName %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lCompanyRegistrationID" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyRegistrationID %>" />: *</th>
                            <td><asp:TextBox ID="_txtCompanyRegistrationID" runat="server" Width="222px" MaxLength="200"></asp:TextBox>
							<span class="info" title="Trade License No / VAT ID / TAX ID"></span>&nbsp;<asp:RequiredFieldValidator ID="rfvCompanyRegistrationID" runat="server" ControlToValidate="_txtCompanyRegistrationID"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="vreg_LegalIP_392" runat="server">
                        <tr>
                            <th><asp:Literal ID="lLegalAddress" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyAddress %>" />: *</th>
                            <td><asp:TextBox ID="_txtLegalAddress" runat="server" Height="100px" MaxLength="1024"
                                            TextMode="MultiLine" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator CssClass="validator" ID="RequiredFieldValidator6" runat="server" ControlToValidate="_txtLegalAddress"
                                            Display="Dynamic" ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyCompanyAddress %>"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <th></th>
                            <td><span class="blue"><asp:Literal ID="lBankInfo" runat="server" Text="<%$ Resources:RegistrationTexts, BankDetails %>" /></span></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lBankName" runat="server" Text="<%$ Resources:RegistrationTexts, BankName %>"></asp:Literal>: *</th>
                            <td><asp:TextBox ID="_txtBankName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="_txtBankName"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic" /></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lIBAN" runat="server" Text="<%$ Resources:RegistrationTexts, IBAN %>" />: *</th>
                            <td><asp:TextBox ID="_txtIBAN" runat="server" Width="222px" MaxLength="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvIBAN" runat="server" ControlToValidate="_txtIBAN"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lSWIFT" runat="server" Text="<%$ Resources:RegistrationTexts, SWIFT %>" /> *</th>
                            <td><asp:TextBox ID="_txtSWIFT" runat="server" Width="222" MaxLength="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvSWIFT" runat="server" ControlToValidate="_txtSWIFT"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                        <ltr>
                            <th><asp:Literal ID="lBankAddress" runat="server" Text="<%$ Resources:RegistrationTexts, BankAddress %>" />: *</th>
                            <td><asp:TextBox ID="_txtBankAddress" runat="server" MaxLength="200" TextMode="MultiLine" Rows="2"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvBankAddress" CssClass="validator" runat="server" ControlToValidate="_txtBankAddress"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </ltr>
                        <tr>
                            <th></th>
                            <td><span class="blue"><asp:Literal ID="lDirector" runat="server" Text="<%$ Resources:RegistrationTexts, GeneralManager %>" /></span></td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="vreg_LegalIP_446" runat="server">
                        <tr>
                            <th><asp:Literal ID="lDirectorFirstName" runat="server" Text="<%$ Resources:RegistrationTexts, Name %>" />: *</th>
                            <td><asp:TextBox ID="_txtDirectorFirstName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="_txtDirectorFirstName"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>" ControlToValidate="_txtDirectorFirstName"
                                ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" /></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lDirectorLastName" runat="server" Text="<%$ Resources:RegistrationTexts, Surname %>" />: *</th>
                            <td><asp:TextBox ID="_txtDirectorLastName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="_txtDirectorLastName"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>" ControlToValidate="_txtDirectorLastName"
                                ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" /></td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th></th>
                        <td><span class="blue"><asp:Literal ID="lCorrespondentBank" runat="server" Text="<%$ Resources:RegistrationTexts, CorrespondentBank %>" /></span></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentBankName" runat="server" Text="<%$ Resources:RegistrationTexts, BankName %>"></asp:Literal></th>
                        <td><asp:TextBox ID="_txtCorrespondentBankName" runat="server" Width="222px" MaxLength="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentIBAN" runat="server" Text="<%$ Resources:RegistrationTexts, IBAN %>" /></th>
                        <td><asp:TextBox ID="_txtCorrespondentIBAN" runat="server" Width="222px" MaxLength="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentSWIFT" runat="server" Text="<%$ Resources:RegistrationTexts, SWIFT %>" /></th>
                        <td><asp:TextBox ID="_txtCorrespondentSWIFT" runat="server" Width="222px" MaxLength="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentBankAddress" runat="server" Text="<%$ Resources:RegistrationTexts, BankAddress %>" /></th>
                        <td><asp:TextBox ID="_txtCorrespondentBankAddrss" runat="server" MaxLength="200" TextMode="MultiLine" Rows="2"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th></th>
                        <td>* <asp:Literal ID="lReqFields" runat="server" Text="<%$ Resources:Texts, ObligatoryFields %>" /><br />
                        ** <asp:Literal ID="lIfNeedShipping" runat="server" Text="<%$ Resources:Texts, DeliveryIsRequiredField %>" /></td>
                    </tr>--%>
                </table>
            </div>
        </asp:WizardStep>
<%--        <asp:WizardStep ID="_reviewContractOffer" runat="server" Title="<%$ Resources:RegistrationTexts, Contract %>" StepType="Step">
            <div class="sales-contract">
                <uc1:TextItemControl ID="TextItem_SalesContract" runat="server" TextItemID="Registration.SalesContract" ShowHeader="false" />
            </div>
            <center>
                <asp:CheckBox ID="_cbAccept" runat="server" Text="<%$ Resources:RegistrationTexts, IHaveReadAndAccept %>" />
                <br />
                <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="<%$ Resources:RegistrationTexts, RegistrationCannotBeProceeded %>"
                    Display="Static" OnServerValidate="_cvIsAccept_ServerValidate" />
            </center>
            <br />
        </asp:WizardStep>--%>
        <asp:WizardStep ID="_reviewClientInfo" runat="server" Title="<%$ Resources:RegistrationTexts, RegistrationConfirmation %>" StepType="Finish">

            <div class="information">
                <table>
                    <tr>
                        <th><asp:Literal ID="lCFUserName" runat="server" Text="<%$ Resources:Texts, Login %>" /></th>
                        <td><%= Login  %></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCFEmail" runat="server" Text="<%$ Resources:Texts, Email %>" /></th>
                        <td><%= Email %></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCFCountry" runat="server" Text="<%$ Resources:RegistrationTexts, Country %>" /></th>
                        <td><%= _ddlCountry.SelectedItem.Text%></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCFLocality" runat="server" Text="<%$ Resources:RegistrationTexts, City %>" /></th>
                        <td><%= Locality %></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCFScopeType" runat="server" Text="<%$ Resources:RegistrationTexts, Scope %>" /></th>
                        <td><%= ScopeType %></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lHowKnow" runat="server" Text="<%$ Resources:RegistrationTexts, SourceOfInformation %>" /></th>
                        <td><%= lHowKnow.Text %></td>
                    </tr>
                    <tr>
                        <th></th>
                        <td><span class="blue"><asp:Literal ID="lCFContact" runat="server" Text="<%$ Resources:RegistrationTexts, ContactInformation %>" /></span></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCFContactLastName" runat="server" Text="<%$ Resources:RegistrationTexts, Surname %>" /></th>
                        <td><%= ContactPersonSurname %></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCFContactFirstName" runat="server" Text="<%$ Resources:RegistrationTexts, Name %>" /></th>
                        <td><%= ContactPersonName %></td>
                    </tr>
 <%--                   <tr>
                        <th><asp:Literal ID="lCFShippingAddress" runat="server" Text="<%$ Resources:RegistrationTexts, DeliveryAddress %>" /></th>
                        <td><%= DeliveryAddress %></td>
                    </tr>
                    <asp:PlaceHolder ID="vreg_LegalIP_798" runat="server">
                        <tr>
                            <th></th>
                            <td><span class="blue"><asp:Literal ID="lCFLegal" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyDetails %>" /></span></td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="vreg_Legal_808" runat="server">
                        <tr>
                            <th><asp:Literal ID="lCompanyNameInfo" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyName %>" /></th>
                            <td><%= CompanyName %></td>
                        </tr>
                        <tr>
						    <th><asp:Literal ID="lCompanyRegistrationIDInfo" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyRegistrationID %>" /></th>
						    <td><%= CompanyRegistrationID %></td>
                        </tr>
                        <tr>
						    <th><asp:Literal ID="lCompanyAddress" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyAddress %>" /></th>
						    <td><%= CompanyAddress %></td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th></th>
					    <td><span class="blue"><asp:Literal ID="lBankingInformation" runat="server" Text="<%$ Resources:RegistrationTexts, BankDetails %>" /></span></td>
                    </tr>
                    <tr>
					    <th><asp:Literal ID="lBankNameInfo" runat="server" Text="<%$ Resources:RegistrationTexts, BankName %>" /></th>
					    <td><%= BankName %></td>
                    </tr>
                    <tr>
					    <th><asp:Literal ID="lIBANInfo" runat="server" Text="<%$ Resources:RegistrationTexts, IBAN %>" /></th>
					    <td><%= IBAN %></td>
                    </tr>
                    <tr>
					    <th><asp:Literal ID="lSWIFTInfo" runat="server" Text="<%$ Resources:RegistrationTexts, SWIFT %>" /></th>
					    <td><%= SWIFT %></td>
                    </tr>
                    <tr>
					    <th><asp:Literal ID="lBankAddressInfo" runat="server" Text="<%$ Resources:RegistrationTexts, BankAddress %>" /></th>
					    <td><%= BankAddress %></td>
                    </tr>
                    <asp:PlaceHolder ID="vreg_LegalIP_903" runat="server">
                        <tr>
                            <th></th>
						    <td><span class="blue"><asp:Literal ID="lCFDirector" runat="server" Text="<%$ Resources:RegistrationTexts, GeneralManager %>" /></span></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lCFDirectorFirstName" runat="server" Text="<%$ Resources:RegistrationTexts, Name %>" /></th>
                            <td><%= DirectorName %></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lCFDirectorLastName" runat="server" Text="<%$ Resources:RegistrationTexts, Surname %>" /></th>
                            <td><%= DirectorSurname %></td>
                        </tr>
                    </asp:PlaceHolder>--%>
                    <tr>
                        <th>
                            <asp:Literal ID="lCaptcha" runat="server" Text="<%$ Resources:RegistrationTexts, CaptchaCode %>" />: *
                        </th>
                        <td>
                            <div onkeydown="javascript: if(event.keyCode==13) { finishWizard(); return false; }">
                                <uc1:Captcha runat="server" ID="_captcha" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td>* <asp:Literal ID="lCaptchaHelp" runat="server" Text="<%$ Resources:RegistrationTexts, CaptchaHelp %>" /></td>
                    </tr>
                </table>
            </div>
        </asp:WizardStep>
    </WizardSteps>
</asp:Wizard>