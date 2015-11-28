<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlaceOrderWhSl.ascx.cs"
    Inherits="RmsAuto.Store.Web.Controls.PlaceOrderWhSl" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Register Src="WizardTopSideBar.ascx" TagName="WizardTopSideBar" TagPrefix="uc1" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc1" %>

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

        objcharlen.innerHTML = '<%= global::Resources.Texts.YouCanEnter %>' + (maxLen - object.value.length) + '<%= global::Resources.Texts.Max236Digits %>';
    }

</script>

<asp:Wizard ID="_placeOrderWizard" runat="server" OnNextButtonClick="_placeOrderWizard_NextButtonClick"
    DisplayCancelButton="false" CancelButtonText="Отмена" CancelDestinationPageUrl="~/Default.aspx"
    OnCancelButtonClick="_placeOrderWizard_CancelButtonClick" CssClass="wizard_form"
    StartNextButtonText="Send Order" DisplaySideBar="false" Width="100%" 
    CancelButtonType="Button" CancelButtonStyle-CssClass="button"
    StartNextButtonType="Button" StepNextButtonText="Send Order" StartNextButtonStyle-CssClass="button">
    <HeaderTemplate>
        <h1>
            <asp:Literal ID="lHeaderPlaceOrder" runat="server" Text="<%$ Resources:PlaceOrder, PlaceOrderInfo %>" /></h1>
        <%--    Этот контрол можно использовать "внутри" хидера или сайд-бара визарда, а так же отдельно от визарда, но тогда
                надо задать параметр WizardToNavigate этого контрола с ID-шником необходимого визарда --%>
        <table class="steps">
            <tr>
                <uc1:WizardTopSideBar ID="WizardTopSideBar1" runat="server">
                    <StepElement>
                        <td>
                            <span><%# Container.StepTitle %></span>
                        </td>
                    </StepElement>
                    <StepElementSelected>
                        <td class="on" style="width:50%;">
                            <span><%# Container.StepTitle %></span>
                        </td>
                    </StepElementSelected>
<%--                    <SeparatorTemplate>
                        <td class="empty">
                            <img id="Img1" runat="server" src="~/images/1pix.gif" width="2" height="2" border="0">
                        </td>
                    </SeparatorTemplate>--%>
                </uc1:WizardTopSideBar>
            </tr>
        </table>
    </HeaderTemplate>
    <StepStyle CssClass="wizard_right_form" />
    <NavigationStyle CssClass="wizard_btn" HorizontalAlign="Left" />
    <WizardSteps>
        <asp:WizardStep ID="_orderReviewStep" runat="server" Title="<%$ Resources:PlaceOrder, PlaceOrderInfo %>" StepType="Start"
            AllowReturn="false">
            <div class="tab-text">
                <div class="t-hold">
                    <table>
                        <tr>
                            <th>
                                <asp:Literal ID="lName" runat="server" Text="<%$ Resources:PlaceOrder, Description %>" />
                            </th>
                            <th>
                                <asp:Literal ID="lDetailCode" runat="server" Text="<%$ Resources:PlaceOrder, PartNumber %>" />
                            </th>
                            <th>
                                <asp:Literal ID="lPrice" runat="server" Text="<%$ Resources:PlaceOrder, Price %>" />
                            </th>
                            <th>
                                <asp:Literal ID="lQty" runat="server" Text="<%$ Resources:PlaceOrder, Quantity %>" />
                            </th>
                            <th>
                                <asp:Literal ID="lSumm" runat="server" Text="<%$ Resources:PlaceOrder, Summ %>" />
                            </th>
                            <th>
                                <asp:Literal ID="lNote" runat="server" Text="<%$ Resources:PlaceOrder, Notes %>" />
                            </th>
                        </tr>
                        <asp:Repeater ID="_shoppingCartItems" runat="server" OnItemDataBound="_shoppingCartItems_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Server.HtmlEncode((string)Eval("PartName"))%>
                                    </td>
                                    <td>
                                        <%#Server.HtmlEncode((string)Eval("PartNumber"))%>
                                    </td>
                                    <td>
                                        <span class="blue"><%# Eval("UnitPrice", "{0:### ### ##0.00}")%></span>
                                    </td>
                                    <td>
                                        <b><%# Eval("Qty") %></b>
                                    </td>
                                    <td class="price">
                                        <span class="blue"><%# Eval("ItemTotal", "{0:### ### ##0.00}")%></span>
                                    </td>
                                    <td>
                                        <asp:Literal ID="_ltrThisNum" runat="server" Text="<%$ Resources:PlaceOrder, StrictlyThisNumber %>" Visible='<%#(bool)Eval("StrictlyThisNumber")%>' /><br />
                                        <asp:Literal ID="_ltrVinData" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td class="itog">
                                <asp:Literal ID="lAll" runat="server" Text="<%$ Resources:Texts, Total %>" />:
                            </td>
                            <td class="itog"></td>
                            <td class="itog"></td>
                            <td class="itog">
                                <%=SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().PartsCount%>
                            </td>
                            <td class="itog price">
                                <%=string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().Total )%>
                            </td>
                            <td class="itog"></td>
                        </tr>
                    </table>
                </div>
                <div class="bottom">
                    <div class="right">
                        <asp:Literal ID="lAllDetails" runat="server" Text="<%$ Resources:PlaceOrder, AllGoods %>" />: <strong>
                        <%=SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().PartsCount%></strong> <span style="visibility:hidden;"> USD</span>
                        <br />
                        <%=global::Resources.Texts.OnSumm %> <strong><% =string.Format( "{0:### ### ##0.00}", SiteContext.Current.CurrentClient.Cart.GetAddToOrderTotals().Total )%></strong> <%=global::Resources.Texts.RoubleShort%>
                    </div>
                </div>
            </div>
            <div class="right_block3" runat="server" id="_powerOfAttorneyBlock">
                <uc1:TextItemControl ID="_powerOfAttorney_TextItemControl" runat="server" TextItemID="PowerOfAttorney.Info"
                    ShowHeader="false" />
            </div>
            <div class="feedback">
                <ul>
                    <li>
                        <div class="title"><asp:Literal Text="<%$ Resources:PlaceOrder, CustOrderNumber %>" runat="server"/>:</div>
                        <asp:TextBox runat="server" ID="_txtCustOrderNum" MaxLength="50"></asp:TextBox>
                    </li>
                    <li>
                        <div class="title"><asp:Literal Text="<%$ Resources:PlaceOrder, OrderNotes %>" runat="server" />:</div>
                        <textarea id="_txtOrderNotes" runat="server" cols="50" rows="10" onfocus="CheckTextLength(this);"></textarea>
                    </li>
                </ul>
            </div>            
        </asp:WizardStep>
        <asp:WizardStep ID="_confirmOrderStep" runat="server" Title="<%$ Resources:PlaceOrder, OrderFinished %>" AllowReturn="False"
            StepType="Complete">
            <h1>
                <asp:Literal ID="lCFPlaceOrder" runat="server" Text="<%$ Resources:PlaceOrder, PlaceOrderInfo %>" /></h1>
            <asp:Label runat="server" ID="_errorLabel" Text="<%$ Resources:PlaceOrder, OrderErrorPlaced %>" CssClass="error" EnableViewState="false" Visible="false" />
            <asp:Panel runat="server" ID="_placeOrderSuccessPanel" Visible="false">
                <asp:Label runat="server" ID="_placeOrderStatusLabel" Text="<%$ Resources:PlaceOrder, OrderSuccessfullyPlaced %>" />
                <br />
                <asp:Literal ID="lCFOrderNumber" runat="server" Text="<%$ Resources:PlaceOrder, OrderNumber %>" />:
                <asp:Label runat="server" ID="_placeOrderNumberLabel" />
                <asp:PlaceHolder runat="server" ID="_custOrderNumPlaceHolder">
                    <br />
                    <asp:Literal ID="lCFClientNumber" runat="server" Text="<%$ Resources:PlaceOrder, CustOrderNumber %>" />:
                    <asp:Label runat="server" ID="_custOrderNumLabel" />
                </asp:PlaceHolder>
                <br />
                <br />
            	<asp:HyperLink runat="server" ID="_orderPrintLink" Target="_blank" CssClass="button"><asp:Literal ID="lPrintOrder" runat="server" Text="<%$ Resources:PlaceOrder, PrintOrder %>" /></asp:HyperLink>&nbsp;
            	 <%--<asp:HyperLink Visible="false" runat="server" ID="_paymentOrderPrintLink" Target="_blank" CssClass="button"><asp:Literal ID="lPrintPayment" runat="server" Text="Распечатать квитанцию" /></asp:HyperLink><br />
            	<asp:Label Visible="false" ID="divider" CssClass="GrayTextStyle" runat="server" Text=" / " /><br/>
			   <asp:HyperLink Visible="false" runat="server" ID="_paymentOrderPrintLinkOpt" Target="_blank" CssClass="button"><asp:Literal ID="optPrintPayment" runat="server" Text="Invoice payment" /></asp:HyperLink><br />--%>
                <br />
                <div class="link_block">
                    <asp:HyperLink ID="_backLink" runat="server" Text="<%$ Resources:Texts, MainPage %>" /></div>
            </asp:Panel>
        </asp:WizardStep>
    </WizardSteps>
    <SideBarButtonStyle CssClass="wizard_test" />
    <SideBarStyle CssClass="wizard_left_form" />
</asp:Wizard>
