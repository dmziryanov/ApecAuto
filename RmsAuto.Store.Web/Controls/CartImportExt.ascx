<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartImportExt.ascx.cs"
    Inherits="RmsAuto.Store.Web.Controls.CartImportExt" %>
<%--<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl"
    TagPrefix="uc1" %>--%>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Store.Web" Assembly="RmsAuto.Store" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%--<uc1:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrManager" />--%>

<script type="text/javascript" src="/scripts/litetabs.jquery.js"></script>
<script type="text/javascript" src="/scripts/jquery.blockUI.js"></script>

<script type="text/javascript">
    function upload() {
        $.blockUI();
    }
</script>

<p>
    <asp:Literal ID="_importSummaryLiteral" runat="server" />
</p>
<asp:MultiView ID="_multiViewLoadReport" runat="server" ActiveViewIndex="0">
    <asp:View ID="_viewLoad" runat="server">
        <p>
            <asp:Literal ID="_txtPrc1" runat="server" Text="<%$ Resources:CartImport, PrcExcessPrice %>" />
        </p>
        <p>
            <rmsauto:ShoppingCartVersionValidator ID="_cartVersionValidator" runat="server" Mode="ChangeCartContent"
                ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidCartVersion %>" />
            <asp:CustomValidator ID="_cartEmptyValidator" runat="server" ErrorMessage="<%$ Resources:ValidatorsMessages, CartNotEmpty %>"
                Display="Dynamic" />
            <asp:Literal ID="_selectFileLiteral" runat="server" Text="<%$ Resources:CartImport, PointFileToDownload %>" />
        </p>
        <p>
            <asp:FileUpload ID="_orderFileUpload" runat="server" Width="400" />
        </p>
        <p>
            <asp:Button ID="_submitButton" CssClass="btn btn-primary btn-sm" runat="server" OnClick="_submitButton_Click" OnClientClick="upload();" Text="<%$ Resources:Texts, OrderImport %>" />
        </p>
        <br />
        <uc2:TextItemControl ID="_instructionTextItem" runat="server" TextItemID="CartImportInstruction.Text" />
    </asp:View>
    <asp:View ID="_viewQtyError" runat="server">
        <h1>
            <asp:Literal ID="lNeedChangeQty" runat="server" Text="<%$ Resources:CartImport, ChangeQuantityRequired %>" />
        </h1>
        <p>
            <span class="link_block">
                <asp:LinkButton ID="lbResumeWithDel" CssClass="btn btn-primary btn-sm" runat="server" OnClick="lbResumeWithDel_Click" Text="<%$ Resources:CartImport, DeclineErrorsLines %>"></asp:LinkButton>
            </span>
        </p>
        <div class="tab-text">
            <div class="t-hold">
                <table class="table-big">
                    <tr>
                        <th>
                            <%=global::Resources.Texts.RowNumber %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.DifferencePrc%>
                        </th>
                        <th>
                            <%=global::Resources.Texts.Manufacturer %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.Article %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.PartName %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.Price %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.PriceCurrent %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.DifferencePrice%>
                        </th>
                        <th>
                            <%=global::Resources.Texts.Qty %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.Summ %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.ReferenceID %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.DeliveryPeriod %>
                        </th>
                        <th>
                            <%=global::Resources.Texts.WithoutCounterParts %>
                        </th>
                    </tr>
                    <asp:ListView runat="server" ID="_lvERQty" OnDataBind="_lvERQty_DataBind">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <asp:PlaceHolder runat="server" ID="_placeHolder">
                                <tr>
                                    <asp:HiddenField ID="hfRowIDQty" runat="server" Value='<%# Eval( "rowID" )%>' />
                                    <td>
                                        <%# Eval( "rowN" ) %>
                                    </td>
                                    <td class="price">
                                        <%# Eval( "pricePrc" ) %>
                                    </td>
                                    <td>
                                        <%# Eval( "manufacturer" )%>
                                    </td>
                                    <td>
                                        <%# Eval( "partNumber" )%>
                                    </td>
                                    <td>
                                        <%# Eval( "itemName" )%>
                                    </td>
                                    <td class="price">
                                        <span class="blue"><%# Eval( "priceClient", "{0:### ### ##0.00}" )%></span>
                                    </td>
                                    <td class="price">
                                        <span class="blue"><%# Eval( "priceCurr", "{0:### ### ##0.00}" )%></span>
                                    </td>
                                    <td class="price">
                                        <span class="blue"><%# string.Format( "{0:### ### ##0.00}", Convert.ToInt32( Eval( "priceClient" ) ) == 0 ? 0 : Convert.ToDecimal( Eval( "priceCurr" ) ) - Convert.ToDecimal( Eval( "priceClient" ) ) )%></span>
                                    </td>
                                    <td class="nowrap text-center">
                                        <asp:TextBox ID="_tbQuantity" runat="server" Text='<%# Eval( "quantity" ) %>' Columns="1"
                                            Width="25px" MaxLength="10" />
                                    </td>
                                    <td class="price">
                                        <span class="blue"><%# string.Format( "{0:### ### ##0.00}", Convert.ToInt32( Eval( "quantity" ) ) * Convert.ToDecimal( Eval( "priceCurr" ) ) )%></span>
                                    </td>
                                    <td>
                                        <%# Eval( "referenceID" )%>
                                    </td>
                                    <td>
                                        <%# Eval( "deliveryPeriod" )%>
                                    </td>
                                    <td class="text-center">
                                        <asp:CheckBox ID="_chkStrictlyThisNumberOK" runat="server" Checked='<%# Eval( "strictlyThisNumber" )%>'
                                            Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="14" style="text-align: right;">
                                        <asp:RegularExpressionValidator ID="_qtyExpressionValidator" runat="server" ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidQty %>"
                                            ControlToValidate="_tbQuantity" ValidationExpression="\d{1,10}" Display="Dynamic" />
                                        <span style="color: Red">
                                            <%# Eval( "errorText" )%>
                                        </span>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <tr>
                                <td colspan="14">
                                    <asp:Literal ID="lEmptyList" runat="server" Text="<%$ Resources:CartImport, ListIsEmpty %>" />
                                </td>
                            </tr>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </table>
            </div>
        </div>
        <span class="link_block">
            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-primary btn-sm" CommandName="SwitchViewByID"
                CommandArgument="_viewLoad"><asp:Literal ID="lReload1" runat="server" Text="<%$ Resources:CartImport, Reload %>" /></asp:LinkButton>
        </span><span class="link_block">
            <asp:LinkButton ID="lbResume" CssClass="btn btn-primary btn-sm" runat="server" OnClick="lbResume_Click"><asp:Literal ID="lContinue" runat="server" Text="<%$ Resources:CartImport, ContinueLoading %>" /></asp:LinkButton>
        </span>
    </asp:View>
    <asp:View ID="_viewConfirm" runat="server">
        <p>
            <asp:Literal ID="_txtPrc" runat="server" />
        </p>
        <div id="tabs" class="liteTabs rounded" style="width:100%;">
            <ul>
		        <li><a href="#itemOk"><asp:Literal ID="_litOK" runat="server"/></a></li>
		        <li><a href="#itemConfirm"><asp:Literal ID="_litAccept" runat="server"/></a></li>
		        <li><a href="#itemError"><asp:Literal ID="_litError" runat="server"/></a></li>
	        </ul>
            <div name="#itemOk">
                <div class="tab-text">
                    <div class="t-hold">
                        <table class="table-big">
                            <tbody>
                                <tr>
                                    <th>
                                        <%=global::Resources.Texts.RowNumber %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Add%><br />
                                        <input type="checkbox" id="allOk" checked="checked" /><asp:Literal Text="<%$ Resources:Texts, all %>" runat="server" />
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DifferencePrc%>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Manufacturer %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Article %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.PartName %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Price %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.PriceCurrent %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DifferencePrice%>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Qty %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Summ %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.ReferenceID %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DeliveryPeriod %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.WithoutCounterParts %>
                                    </th>
                                </tr>
                            </tbody>
                            <asp:ListView runat="server" ID="_lvItemsOK" EnableViewState="false">
                                <LayoutTemplate>
                                    <tbody id="dataOk">
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                                    </tbody>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:PlaceHolder runat="server" ID="_placeHolder">
                                        <tr>
                                            <td>
                                                <%# Eval( "rowN" ) %>
                                            </td>
                                            <td>
                                                <input class="ok" type="checkbox" checked="checked" value='<%# Eval( "rowID" )%>' />
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "pricePrc" ) %></span>
                                            </td>
                                            <td>
                                                <%# Eval( "manufacturer" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "partNumber" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "itemName" )%>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "priceClient", "{0:### ### ##0.00}" )%></span>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "priceCurr", "{0:### ### ##0.00}" )%></span>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# string.Format( "{0:### ### ##0.00}", Convert.ToInt32( Eval( "priceClient" ) ) == 0 ? 0 : Convert.ToDecimal( Eval( "priceCurr" ) ) - Convert.ToDecimal( Eval( "priceClient" ) ) )%></span>
                                            </td>
                                            <td class="nowrap text-center">
                                                <%# Eval( "quantity" ) %>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# string.Format( "{0:### ### ##0.00}", Convert.ToInt32( Eval( "quantity" ) ) * Convert.ToDecimal( Eval( "priceCurr" ) ) )%></span>
                                            </td>
                                            <td>
                                                <%# Eval( "referenceID" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "deliveryPeriod" )%>
                                            </td>
                                            <td class="text-center">
                                                <input type="checkbox" class="strictly" <%# (bool)Eval( "strictlyThisNumber" ) ? " checked=\"checked\"" : "" %> />
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="14">
                                            <asp:Literal ID="lEmptyLoadList" runat="server" Text="<%$ Resources:CartImport, ListIsEmpty %>" />
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </table>
                    </div>
                </div>
            </div>
            <div name="#itemConfirm">
                <div class="tab-text">
                    <div class="t-hold">
                        <table class="table-big">
                            <tbody">
                                <tr>
                                    <th>
                                        <%=global::Resources.Texts.RowNumber %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Add%><br />
                                        <input id="allAccept" type="checkbox" /><asp:Literal Text="<%$ Resources:Texts, all %>" runat="server" />
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DifferencePrc%>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Manufacturer %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Article %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.PartName %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Price %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.PriceCurrent %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DifferencePrice%>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Qty %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Summ %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.ReferenceID %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DeliveryPeriod %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.WithoutCounterParts %>
                                    </th>
                                </tr>
                            </tbody>
                            <asp:ListView runat="server" ID="_lvItemAccept" EnableViewState="false">
                                <LayoutTemplate>
                                    <tbody id="dataAccept">
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                                    </tbody>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:PlaceHolder runat="server" ID="_placeHolder">
                                        <tr>
                                            <td>
                                                <%# Eval( "rowN" ) %>
                                            </td>
                                            <td>
                                                <input type="checkbox" class="accept" value='<%# Eval( "rowID" )%>' />
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "pricePrc" ) %></span>
                                            </td>
                                            <td>
                                                <%# Eval( "manufacturer" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "partNumber" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "itemName" )%>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "priceClient", "{0:### ### ##0.00}" )%></span>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "priceCurr", "{0:### ### ##0.00}" )%></span>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# string.Format( "{0:### ### ##0.00}", Convert.ToInt32( Eval( "priceClient" ) ) == 0 ? 0 : Convert.ToDecimal( Eval( "priceCurr" ) ) - Convert.ToDecimal( Eval( "priceClient" ) ) )%></span>
                                            </td>
                                            <td class="nowrap text-center">
                                                <%# Eval( "quantity" ) %>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# string.Format( "{0:### ### ##0.00}", Convert.ToInt32( Eval( "quantity" ) ) * Convert.ToDecimal( Eval( "priceCurr" ) ) )%></span>
                                            </td>
                                            <td>
                                                <%# Eval( "referenceID" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "deliveryPeriod" )%>
                                            </td>
                                            <td class="text-center">
                                                <input type="checkbox" class="strictly" <%# (bool)Eval( "strictlyThisNumber" ) ? " checked=\"checked\"" : "" %> />
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="14">
                                            <asp:Literal ID="lEmptyConfirmList" runat="server" Text="<%$ Resources:CartImport, ListIsEmpty %>" />
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </table>
                    </div>
                </div>
            </div>
            <div name="#itemError">
                <div class="tab-text">
                    <div class="t-hold">
                        <table>
                            <tbody>
                                <tr>
                                    <th>
                                        <%=global::Resources.Texts.RowNumber %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Manufacturer %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Article %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Price %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.Qty %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.ReferenceID %>
                                    </th>
                                    <th>
                                        <%=global::Resources.Texts.DescriptionError%>
                                    </th>
                                </tr>
                            </tbody>
                            <asp:ListView runat="server" ID="_lvItemsError" EnableViewState="false">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:PlaceHolder runat="server" ID="_placeHolder">
                                        <tr>
                                            <td>
                                                <%# Eval( "rowN" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "manufacturer" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "partNumber" )%>
                                            </td>
                                            <td class="price">
                                                <span class="blue"><%# Eval( "priceClient", "{0:### ### ##0.00}" ) %></span>
                                            </td>
                                            <td class="text-center">
                                                <%# Eval( "quantity" ) %>
                                            </td>
                                            <td>
                                                <%# Eval( "referenceID" )%>
                                            </td>
                                            <td>
                                                <%# Eval( "error" )%>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="14">
                                            <asp:Literal ID="lEmptyErrorList" runat="server" Text="<%$ Resources:CartImport, ListIsEmpty %>" />
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="_vItemError" runat="server">
            <script type="text/javascript">
                $(function () {
                    $('#tabs').liteTabs({ rounded: true, width: '100%', selectedTab: 3 });
                });
            </script>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="_vItemOK" runat="server">
            <script type="text/javascript">
                $(function () {
                    $('#tabs').liteTabs({ rounded: true, width: '100%' });
                });
            </script>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="_vItemConfirm" runat="server">
            <script type="text/javascript">
                $(function () {
                    $('#tabs').liteTabs({ rounded: true, width: '100%', selectedTab: 2 });
                });
            </script>
        </asp:PlaceHolder>
        <script type="text/javascript" src="/scripts/jquery.jmsajax.min.js"></script>
        <script type="text/javascript">
            $(function () {
                $('#allOk').change(function (e) {
                    $('input:checkbox.ok').prop('checked', $(this).prop('checked'));
                });
                $('input:checkbox.ok').change(function (e) {
                    if ($('input:checkbox.ok:checked').length == $('input:checkbox.ok').length) {
                        $('#allOk').prop('checked', true);
                    } else {
                        $('#allOk').prop('checked', false);
                    }
                });

                $('#allAccept').change(function (e) {
                    $('input:checkbox.accept').prop('checked', $(this).prop('checked'));
                });
                $('input:checkbox.accept').change(function (e) {
                    if ($('input:checkbox.accept:checked').length == $('input:checkbox.accept').length) {
                        $('#allAccept').prop('checked', true);
                    } else {
                        $('#allAccept').prop('checked', false);
                    }
                });

                $('#addToCart').bind('click', function (e) {
                    e.preventDefault();

                    var rowsOk = $.map($('#dataOk tr'),
                        function (v, i) {
                            v = $(v);
                            ok = $('input:checkbox.ok:checked', v);
                            if (ok.length) {
                                return { Id: ok.val(), Strictly: $('input:checkbox.strictly', v).prop('checked') };
                            }
                        });

                    var rowsAccept = $.map($('#dataAccept tr'),
                        function (v, i) {
                            v = $(v);
                            ok = $('input:checkbox.accept:checked', v);
                            if (ok.length) {
                                return { Id: ok.val(), Strictly: $('input:checkbox.strictly', v).prop('checked') };
                            }
                        });

                    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                    $.jmsajax({
                        url: '/cms/exceladdtocart.ashx',
                        data: { RowsOk: rowsOk, RowsAccept: rowsAccept, SendOkEmail: $('#sendOKToEmail').prop('checked'), SendErrorEmail: $('#sendErrorToEmail').prop('checked') },
                        error: function (data, s, m) {
                            $('.alert-error').alert(JSON.parse(data.responseText).Message);
                        },
                        success: function (data) {
                            $('.alert-success').alert(data.Message);
                            $('.goods-quantity').text(data.Quantity);
                            $('.goods-amount').text(data.Amount);
                            $('#tabs,.buttons').hide();
                        }
                    });
                });
            });
        </script>
        <p class="buttons">
            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary btn-sm" CommandName="SwitchViewByID" CommandArgument="_viewLoad"><asp:Literal ID="lReload2" runat="server" Text="<%$ Resources:CartImport, Reload %>" /></asp:LinkButton> 
<%--            <asp:LinkButton ID="lbAddToCart" Class="btn btn-primary btn-sm" runat="server" OnClick="lbAddToCart_Click"><asp:Literal ID="lLoadToCart" runat="server" Text="<%$ Resources:CartImport, UploadToCart %>" /></asp:LinkButton> --%>
            <a id="addToCart" href="#" Class="btn btn-primary btn-sm"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CartImport, UploadToCart %>" /></a>
            <input id="sendOKToEmail" class="iCheck-helper" type="checkbox" /><label for="sendOKToEmail"><asp:Literal Text="<%$ Resources:CartImport, SendUploadedOrderToEmail %>" runat="server" /></label>
            <input id="sendErrorToEmail" class="iCheck-helper" type="checkbox" /><label for="sendErrorToEmail"><asp:Literal Text="<%$ Resources:CartImport, SendListErrorToEmail %>" runat="server" /></label>
        </p>
        <div class="alert alert-success"></div>
        <div class="alert alert-error"></div>
    </asp:View>
    <asp:View ID="_viewReport" runat="server">
        <p>
            <asp:Literal ID="_importResultLiteral" runat="server" />
        </p>
        <p>
            <asp:Literal ID="_importFinalLiteral" runat="server" />
        </p>
        <p>
            <span class="link_block">
                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary btn-sm" CommandName="SwitchViewByID"
                    CommandArgument="_viewLoad"><asp:Literal ID="lReload3" runat="server" Text="<%$ Resources:CartImport, Reload %>" /></asp:LinkButton>
            </span><span class="link_block">
                <asp:LinkButton ID="_placeOrderBtn" CssClass="btn btn-primary btn-sm" runat="server"><asp:Literal ID="lPlaceOrder" runat="server" Text="<%$ Resources:CartImport, PlaceOrder %>" /></asp:LinkButton>
            </span>
        </p>
    </asp:View>
</asp:MultiView>
