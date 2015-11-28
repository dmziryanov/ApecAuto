<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlaceOrder.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.PlaceOrder" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Register src="WizardTopSideBar.ascx" tagname="WizardTopSideBar" tagprefix="uc1" %>
<%@ Register src="ShippingOptions.ascx" tagname="ShippingOptions" tagprefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>

<script type="text/javascript">

    var timeoutid = null;
    var spanChars = null;
    var maxLen = 236;

    function CheckTextLength(object) {
        if (!spanChars) {
            spanChars = document.createElement('span');
            spanChars.id = 'span1';
            object.parentNode.insertBefore(spanChars, object.nextSibling);
        }

        if (timeoutid) {
            clearInterval(clearInterval);
        }

        var truncatetext = function() { TruncateText(object, spanChars); };
        timeoutid = setInterval(truncatetext, 300);
    }

    function TruncateText(object, objcharlen) {
        if (object.value.length > maxLen) {
            object.value = object.value.substring(0, maxLen);
        }

        objcharlen.innerHTML = '<br/>можно ввести [' + (maxLen - object.value.length) + '] максимум [236] символов';
    }

</script>

<asp:Wizard ID="_placeOrderWizard" runat="server"  
    onactivestepchanged="_placeOrderWizard_ActiveStepChanged" 
    onfinishbuttonclick="_placeOrderWizard_FinishButtonClick" 
    DisplayCancelButton="false" CancelButtonText="Отмена" 
    CancelDestinationPageUrl="~/Default.aspx" 
    oncancelbuttonclick="_placeOrderWizard_CancelButtonClick" 
    CssClass="wizard_form" FinishCompleteButtonText="Отправить заказ" 
    FinishPreviousButtonText="Назад" StartNextButtonText="Дальше" 
    StepNextButtonText="Дальше" StepPreviousButtonText="Назад"
    DisplaySideBar="false" Width=100% 
    StartNextButtonImageUrl="<%$ Resources:ImagesURL, bask_nextstep %>" StartNextButtonType="Image" 
    CancelButtonImageUrl="<%$ Resources:ImagesURL, bask_cencel %>" CancelButtonType="Image" 
    StepNextButtonImageUrl="<%$ Resources:ImagesURL, bask_nextstep %>" StepNextButtonType="Image" 
    StepPreviousButtonImageUrl="<%$ Resources:ImagesURL, bask_prevstep %>" StepPreviousButtonType="Image" 
    FinishPreviousButtonImageUrl="~/images/bask_prevstep.gif" FinishPreviousButtonType="Image" 
    FinishCompleteButtonImageUrl="<%$ Resources:ImagesURL, bask_send %>" FinishCompleteButtonType="Image">
    
    <HeaderTemplate>
    
        <h1><asp:Literal ID="lHeaderPlaceOrder" runat="server" Text="Оформление заказа" /></h1>
    
        <%--    Этот контрол можно использовать "внутри" хидера или сайд-бара визарда, а так же отдельно от визарда, но тогда
                надо задать параметр WizardToNavigate этого контрола с ID-шником необходимого визарда --%>

        
        <table class="steps">
            <tr>
            <uc1:WizardTopSideBar ID="WizardTopSideBar1" runat="server">
                <StepElement>
                    <td><%# Container.StepTitle %></td>
                </StepElement>
                <StepElementSelected>
                    <td class="on">
                   <%# Container.StepTitle %>
                    </td>
                </StepElementSelected>
            </uc1:WizardTopSideBar>
            </tr>
        </table>
    </HeaderTemplate>
    
    <StepStyle CssClass="wizard_right_form" />
    <NavigationStyle CssClass="wizard_btn" HorizontalAlign=Left />
    <WizardSteps>
        <asp:WizardStep ID="_shippingMethodStep" runat="server" Title="Выбор способа доставки" StepType="Start">
        <div class="cart">
            <uc1:ShippingOptions ID="_shippingOptions" runat="server" />
        </div>
        </asp:WizardStep>
        <asp:WizardStep ID="_paymentInfoStep" runat="server" Title="Выбор формы оплаты" StepType="Step">
         <div class="cart">
            <br />
            <asp:RadioButtonList ID="_rblPaymentMethod" runat="server" />
            <br />
        </div>
        </asp:WizardStep>
        <asp:WizardStep ID="_orderReviewStep" runat="server" Title="<%$ Resources:PlaceOrder, PlaceOrderInfo %>" StepType="Finish">
            <div class="cart">
            <br />
            <span><asp:Literal ID="lAllDetails" runat="server" Text="Всего товаров:" /> <b><%=SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().PartsCount%></b> <asp:Literal ID="lOnSumm" runat="server" Text="на сумму" /> <b><% =string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().Total )%></b> <%=global::Resources.Texts.RoubleShort%></span>
            <center>
            <table cellpadding="0" cellspacing="0" class="list" width="97%">
                     <tr>
                     <th><asp:Literal ID="lName" runat="server" Text="<%$ Resources:PlaceOrder, Description %>" /></th>
                     <th><asp:Literal ID="lDetailCode" runat="server" Text="<%$ Resources:PlaceOrder, PartNumber %>" /></th>
                     <th><asp:Literal ID="lPrice" runat="server" Text="<%$ Resources:PlaceOrder, Price %>" /></th>
                     <th><asp:Literal ID="lQty" runat="server" Text="<%$ Resources:PlaceOrder, Quantity %>" /></th>
                     <th><asp:Literal ID="lSumm" runat="server" Text="<%$ Resources:PlaceOrder, Summ %>" /></th>
                     <th><asp:Literal ID="lNote" runat="server" Text="<%$ Resources:PlaceOrder, Notes %>" /></th>
                     </tr>   
                    <asp:Repeater ID="_shoppingCartItems" runat="server" OnItemDataBound="_shoppingCartItems_ItemDataBound">
                    <HeaderTemplate></HeaderTemplate>   
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Server.HtmlEncode((string)Eval("PartName"))%>
                            </td>
                            <td>
                                <%#Server.HtmlEncode((string)Eval("PartNumber"))%>
                            </td>
                            <td>
                                <%# Eval("UnitPrice", "{0:### ### ##0.00}")%>
                            </td>
                            <td><b><%# Eval("Qty") %></b></td>
                            <td class="price">
                                <%# Eval("ItemTotal", "{0:### ### ##0.00}")%>
                            </td>
                            <td>
                                <asp:Literal ID="_ltrThisNum" runat="server" Text="только этот номер" Visible='<%#(bool)Eval("StrictlyThisNumber")%>' /><br />
                                <asp:Literal ID="_ltrVinData" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        </ItemTemplate>
                     </asp:Repeater>
                     <tr>
                        <td class="itog"><asp:Literal ID="lAll" runat="server" Text="Итого:" /></td>
                        <td class="itog">&nbsp;</td>
                        <td class="itog">&nbsp;</td>
                        <td class="itog"><nobr><%=SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().PartsCount%></nobr></td>
                        <td class="itog"><nobr><%=string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().Total )%></nobr></td>
                     </tr>
            </table>
            </center>
            </div>
            <div class="right_block3" runat="server" id="_powerOfAttorneyBlock">
				<uc1:TextItemControl ID="_powerOfAttorney_TextItemControl" runat="server" 
					TextItemID="PowerOfAttorney.Info"
					ShowHeader="false" />
            </div>
            <table cellpadding=0 cellspacing=0>
            <tr>
                <td>
                    <h3><asp:Literal ID="lShipping" runat="server" Text="Доставка" /></h3>
                </td>
                <td align=right>
                    <h3><asp:Literal ID="lMoney" runat="server" Text="Оплата" /></h3>
                </td>
            </tr>
            <tr>
                <td>
                     <div class="cart" style="padding:15px;border-right:1px dashed #cccfd1;">
                        <asp:Literal ID="_ltrShippingInfo" runat="server" />
                    </div>
                </td>
                <td>
                     <div class="cart" style="padding:15px;">
                        <asp:Literal ID="_ltrPaymentMethod" runat="server" />: <b><%=string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().Total )%> <%=global::Resources.Texts.RoubleShort%></b>
                    </div>
                </td>
            </tr>
            </table>
            <br />
            <h3><asp:Literal ID="lClientNumber" runat="server" Text="<%$ Resources:PlaceOrder, CustOrderNumber %>" />:</h3>
            <asp:TextBox runat="server" ID="_txtCustOrderNum" MaxLength="50"></asp:TextBox>
            <br />
            <br />
            <h3><asp:Literal ID="lOrderNote" runat="server" Text="<%$ Resources:PlaceOrder, OrderNotes %>" />:</h3>
                        
            <textarea ID="_txtOrderNotes" runat="server" cols="50" rows="10" onfocus="CheckTextLength(this);"></textarea>
            <asp:PlaceHolder runat="server" ID="_contractTermsPlaceHolder">
				<br />
				
				<br />       
				<h3><asp:Literal ID="lContractTerms" runat="server" Text="Условия договора" /></h3>
				<iframe runat="server" id="_contractTermsFrame" width="100%" height="200" scrolling="auto" frameborder="1">
				</iframe>
				<table>
				<tr>
				<td><asp:CheckBox runat="server" ID="_contractTermsAgreementBox" Text="Принять условия договора" /></td>
				<td><asp:Literal runat="server" ID="_contractTermsLinkLiteral" /></td>
				</tr>
				</table>
				<asp:Label runat="server" ID="_contractTermsMessageLabel" CssClass="error" Visible="false"><asp:Literal ID="lTextAccept" runat="server" Text="Для оформления заказа необходимо принять условия договора" /></asp:Label></asp:PlaceHolder></asp:WizardStep><asp:WizardStep ID="_confirmOrderStep" runat="server" Title="Заказ сделан" AllowReturn="False" StepType="Complete"> 
			<h1><asp:Literal ID="lPlaceOrder" runat="server" Text="<%$ Resources:PlaceOrder, PlaceOrderInfo %>" /></h1>
			<asp:Label runat="server" ID="_errorLabel" Text="<%$ Resources:PlaceOrder, OrderErrorPlaced %>" CssClass="error" EnableViewState="false" Visible="false" />
			<asp:Panel runat="server" ID="_placeOrderSuccessPanel" Visible="false">
				<asp:Label runat="server" ID="_placeOrderStatusLabel" Text="<%$ Resources:PlaceOrder, OrderSuccessfullyPlaced %>"/>
				<br />
				<asp:Literal ID="lOrderNumber" runat="server" Text="Номер заказа:" /> <asp:Label runat="server" ID="_placeOrderNumberLabel"/>
				<asp:PlaceHolder runat="server" ID="_custOrderNumPlaceHolder">
				<br />
				<asp:Literal ID="lClientNumberView" runat="server" Text="Ваш номер заказа:" /> <asp:Label runat="server" ID="_custOrderNumLabel" />
				</asp:PlaceHolder>
				<br /><br />
				    <asp:HyperLink runat="server" ID="_orderPrintLink" Target="_blank" CssClass="button"><asp:Literal ID="lPrintOrder" runat="server" Text="<%$ Resources:PlaceOrder, PrintOrder %>" /></asp:HyperLink>
                    <asp:HyperLink  Visible="false" runat="server" ID="_paymentOrderPrintLink" Target="_blank" CssClass="button"><asp:Literal ID="lPrintPayment" runat="server" Text="Print an receipt" /></asp:HyperLink><nobr><asp:Label Visible="false" ID="divider" CssClass="GrayTextStyle" runat="server" Text=" / " /><nobr>
    		        <asp:HyperLink Visible="false" runat="server" ID="_paymentOrderPrintLinkOpt" Target="_blank" CssClass="button"><asp:Literal ID="optPrintPayment" runat="server" Text="Print an account" /></asp:HyperLink>
				<br />
				<div class="link_block"><asp:HyperLink ID="_backLink" runat="server" Text="<%$ Resources:Texts, MainPage %>" /></div>
			</asp:Panel>
        </asp:WizardStep>
    </WizardSteps>
    <SideBarButtonStyle CssClass="wizard_test" />
    <SideBarStyle CssClass="wizard_left_form" />
</asp:Wizard>
