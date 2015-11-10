<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartItems.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ShoppingCartItems" %>
<%@ Register Src="OrderLineOptions.ascx" TagName="OrderLineOptions" TagPrefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>

<div class="orders">
    <div class="tab-text">
        <div class="t-hold">
            <div class="br"></div>
            <div class="bl"></div>
            <table class="table-big">
                <asp:Repeater ID="_cartItemsRepeater" runat="server" onitemdatabound="_cartItemsRepeater_ItemDataBound" onitemcommand="_cartItemsRepeater_ItemCommand">
                    <HeaderTemplate>
                    <tr>
                        <th class="nowrap"><%=global::Resources.Texts.AddToOrder%><br/><asp:CheckBox ID="_chkAddToOrderAll" runat="server" Text="<%$ Resources:Texts, all %>" AutoPostBack="true" OnCheckedChanged="CheckBoxAddToOrderAllCheckedChanged" /></th>
                        <th><%=global::Resources.Texts.Manufacturer %></th>
                        <th><%=global::Resources.Texts.PartName %></th>
                        <th><%=global::Resources.Texts.Article %></th>
                        <th><%=global::Resources.Texts.Price %></th>
                        <th><%=global::Resources.Texts.Qty %></th>
                        <th><%=global::Resources.Texts.DeliveryPeriod %></th>
                        <th class="nowrap"><%=global::Resources.Texts.ReferenceID %><span class="info" title="<%=global::Resources.Texts.Cart_CopyReferenceID %>"></span></th>
                        <th><%=global::Resources.Texts.Summ %></th>
                        <th class="nowrap"><%=global::Resources.Texts.Delete %><br/><asp:CheckBox ID="_chkRemoveAll" runat="server" Text="<%$ Resources:Texts, all %>" AutoPostBack="true" OnCheckedChanged="CheckBoxRemoveAllCheckedChanged" /></th>
                        <th id="_thThisNum" runat="server" width="76px"><%=global::Resources.Texts.WithoutCounterParts %><span class="info" title="<%=global::Resources.Texts.DontOfferAnalogs %>"></span><asp:CheckBox ID="_chkStrictlyAll" runat="server" AutoPostBack="true" OnCheckedChanged="_chkStrictlyAll_CheckedChanged" /></th>
                        <th class="nowrap" id="_thVin" runat="server" visible='<%# this.DisplayVinOptions %>'><%=global::Resources.Texts.VinCheck %><span class="info" title="<%=global::Resources.Texts.Cart_CheckByCar %>"></span></th>
                    </tr>   
                    </HeaderTemplate>
                    <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="_chkAddToOrder" runat="server" Checked='<%# Convert.ToBoolean( Eval("AddToOrder") ) %>' />
                        </td>
                        <td>
                            <%#Server.HtmlEncode((string)Eval("Manufacturer"))%>
                        </td>
                        <td>
                            <asp:HiddenField ID="_itemID" runat="server" Value='<%# Eval("ItemID") %>' />
                            <asp:Label ID="_hiddenItemKey" runat="server" Visible="false" />
                            <a id="_sparePartDetails" runat="server" target="_blank" href="<%# GetSparePartDetailsUrl(((ShoppingCartItem)Container.DataItem).SparePart)%>" 
        	                onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
                            <%# Server.HtmlEncode((string)Eval("PartDescription")) %>
                            </a>
                        </td>
                        <td>
                            <%#Server.HtmlEncode((string)Eval("PartNumber"))%>
                        </td>
                        <td class="price">
                            <span class="blue"><%# Eval("UnitPrice", "{0:### ### ##0.00}")%></span>
                        </td>
                        <td class="nowrap">
                            <asp:TextBox ID="_txtQty" runat="server" Columns="1" Text='<%# Eval("Qty") %>' Width="33px" MaxLength="10" />
                            <asp:RegularExpressionValidator ID="_qtyExpressionValidator" runat="server" ErrorMessage="*"
                                   ControlToValidate="_txtQty" ValidationExpression="\d{1,10}" Display="Dynamic"
                                   Title="<%$ Resources:ValidatorsMessages, InvalidQty %>" />
                            <asp:CustomValidator id="_qtyValidator" runat="server" ControlToValidate="_txtQty" Display="Dynamic"
                                ErrorMessage="*" OnServerValidate="_qtyValidator_OnServerValidate" ValidateEmptyText="true"
                                Title="<%$ Resources:ValidatorsMessages, InvalidQty %>">
                            </asp:CustomValidator>
                        </td>
                        <td class="nowrap">
					        <%#( ( (ShoppingCartItem)Container.DataItem ).SparePart == null ? "" : ( (ShoppingCartItem)Container.DataItem ).SparePart.DisplayDeliveryDaysMin.ToString() )%>-<%#( ( (ShoppingCartItem)Container.DataItem ).SparePart == null ? "" : ( (ShoppingCartItem)Container.DataItem ).SparePart.DisplayDeliveryDaysMax.ToString() )%>
                        </td>
                        <td class="nowrap">
                            <asp:TextBox ID="_txtReferenceID" runat="server" Columns="1" Text='<%# Eval("ReferenceID") %>' Width="60px" MaxLength="50">
                            </asp:TextBox>
					        <asp:ImageButton ID="_btnCopyPosition" runat="server" ImageUrl="~/images/copy_in_cart.jpg"
					            CommandName="CopyPosition" AlternateText="<%$ Resources:Texts, Copy %>"
					            title='<%# global::Resources.Texts.Cart_CopyReferenceID %>' />
                        </td>
                        <td class="price">
                            <span class="blue"><%# Eval("ItemTotal", "{0:### ### ##0.00}")%></span>
                            <span>
						        <asp:Label ID="_lblWarning" runat="server" ForeColor="Red" />
						        <img id="_imgQtyInStock" alt="" src="" runat="server" />
					        </span>
                        </td>
                        <td>
                            <asp:CheckBox ID="_chkRemove" runat="server" /><br />
                            <asp:LinkButton ID="_btnSearch" runat="server" Visible="false"><%# global::Resources.Texts.SearchAnotherDate %></asp:LinkButton>
                        </td>
                            <uc1:OrderLineOptions ID="_options" runat="server" DisplayVinOptions='<%# this.DisplayVinOptions %>' />
                        <td id="optEmpty" runat="server" visible="false"></td>
                    </tr>
                    </ItemTemplate>
             </asp:Repeater>
             <tr>
                <td class="itog">&nbsp;</td>
                <td class="itog nowrap"><%=global::Resources.Texts.Total%>:</td>
                <td class="itog">&nbsp;</td>
                <td class="itog">&nbsp;</td>
                <td class="itog">&nbsp;</td>
                <td class="itog"><%=PartsCount%></td>
                <td class="itog price"><span class="blue"><%=string.Format("{0:### ### ##0.00}", Total) %></span>&nbsp;<b><%= IsGuest&&PartsCount>0?"*":"" %></b></td>
                <td class="itog"><asp:LinkButton ID="_btnRecalc" runat="server"  Text="<%$ Resources:Texts, Recalc %>" onclick="_btnRecalc_Click"/></td>
                <td class="itog" colspan="<%= this.DisplayVinOptions ? "4" : "3" %>">&nbsp;</td>
             </tr>
            </table>
        </div>
        <div class="bottom">
            <div class="left">
                <asp:Literal ID="lOrder" runat="server" Text="<%$ Resources:Texts, Sorting %>" /> <asp:DropDownList runat="server" ID="_sortBox" AutoPostBack="true" OnSelectedIndexChanged="_sortBox_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="right">
                <%=global::Resources.Texts.AllParts%> <strong><%=PartsCount%></strong> <span style="visibility:hidden;"> USD</span>
                <br />
                <%=global::Resources.Texts.OnSumm%> <strong><% =string.Format("{0:### ### ##0.00}", Total)%></strong> <%=global::Resources.Texts.DollarShort %>
            </div>
        </div>
    </div>
</div>



