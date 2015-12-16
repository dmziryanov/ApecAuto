<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpareParts.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.SpareParts" %>
<%@ Register src="SparePartView.ascx" tagname="SparePartView" tagprefix="uc1" %>
<%@ Register src="~/Controls/SparePartInformation.ascx" tagname="SparePartInfo" tagprefix="uc1" %>
<%@ Register Src="~/Controls/SparePartInformationExt.ascx" TagName="SparePartInfoExt" TagPrefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<%@ Register Src="~/Controls/RMSPagerControl.ascx" TagName="RMSPagerControl" TagPrefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.TechDoc.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Data" %>
<%@ Import Namespace="RmsAuto.Store.Web.Controls" %>

<script runat="server">

    enum SparePartItemTypeForHeader
    {
        OwnStore = -1,
        Exact = 0,
        Transition = 1,
        Analogue = 2
    }
    
    System.Collections.Generic.List<SparePartItemTypeForHeader> _groups = new System.Collections.Generic.List<SparePartItemTypeForHeader>();
	bool IsGroupHeaderVisible( object part, SparePartItemType itemType )
	{
        SparePartItemTypeForHeader _itemType;
        if (((SearchResultItem)part).IsOwnStore && itemType == SparePartItemType.Exact)
        {
            _itemType = SparePartItemTypeForHeader.OwnStore;
        }
        else
        {
            _itemType = (SparePartItemTypeForHeader)Convert.ToInt32(itemType);
        }
        
		if( ( (SearchResultItem)part ).ItemType == itemType && !_groups.Contains( _itemType ) )
		{
			_groups.Add( _itemType );
			return true;
		}
		else
		{
			return false;
		}
	}

</script>

<script type="text/javascript">

	function validate_qty(qtyBoxId) {
		var val = $.trim($('#' + qtyBoxId).val());
		$('#' + qtyBoxId).val(val);

		if (val == '') {
			alert('<%= global::Resources.Texts.AlertQty %>');
			return false;
		}
		else if (!val.match(/^\d+$/)) {
		alert('<%= global::Resources.Texts.AlertCorrectQty %>');
			return false;
		}
		return true;
	}

</script>
<div class="orders">
    <asp:Label runat="server" ID="_messageLabel" EnableViewState="false"></asp:Label>
    <asp:PlaceHolder runat="server" ID="_resultsPlaceHolder">
		    <b><%=global::Resources.Texts.ByCodeNumber %>&nbsp;<asp:Label runat="server" ID="_searchCodeLabel"></asp:Label></b> <%=global::Resources.Texts.FoundLC %>&nbsp;<asp:Label runat="server" ID="_countLabel"></asp:Label>&nbsp;<%=global::Resources.Texts.PricesOffer %>
		    <div class="legenda">
			    <img id="Img1" runat="server" src="~/images/search_desc.png" alt="<%$ Resources:Texts, DescriptionLC %>" border="0" /> - <%=global::Resources.Texts.DescriptionLC%>;&nbsp;&nbsp;
			    <img id="Img2" runat="server" src="~/images/search_photo.png" alt="<%$ Resources:Texts, PhotoLC %>" border="0" /> - <%=global::Resources.Texts.PhotoLC %>;&nbsp;&nbsp;
			    <img id="Img3" runat="server" src="~/images/search_statistic.png" alt="<%$ Resources:Texts, StatOfSelectedSupplier %>" border="0" /> - <%=global::Resources.Texts.StatOfSelectedSupplier %>;&nbsp;&nbsp;
			    <img id="Img4" runat="server" src="~/images/search_auto.png" alt="<%$ Resources:Texts, ApplyingToCarLC %>" border="0" /> - <%=global::Resources.Texts.ApplyingToCarLC %>;&nbsp;&nbsp;
			    <img id="Img5" runat="server" src="~/images/search_info.png" alt="<%$ Resources:Texts, AddInfo %>" border="0" /> - <%=global::Resources.Texts.AddInfo %>;&nbsp;&nbsp;
			    <%--<img id="imgDefect" runat="server" src="~/images/search_defect.gif" alt="Уцененная запчасть" width="14" height="15" border="0" /> - Уцененная запчасть&nbsp;&nbsp;--%>
		    </div>
        <uc1:TextItemControl ID="_guestPriceHint_TextItemControl" runat="server" 
	        TextItemID="SpareParts.GuestPriceHint"
	        ShowHeader="false" />
	    <uc1:TextItemControl ID="TextItemControl_2" runat="server" Visible="false" 
            TextItemID="SpareParts.DeliveryHint"
            ShowHeader="false" />
        <div class="tab-text">
            <div class="t-hold">
                <div class="br"></div>
                <div class="bl"></div>
                <table class="table-big">
		            <tr>
			            <th><%=global::Resources.Texts.Manufacturer %></th>
			            <th><%=global::Resources.Texts.Article %></th>
			            <th><%=global::Resources.Texts.PartName %></th>
                        <th><%=global::Resources.Texts.WeightPhysical %>,&nbsp;<%=global::Resources.Texts.Kg %></th>
			            <th><%=global::Resources.Texts.Availability %></th>
			            <th><%=global::Resources.Texts.DeliveryPeriod %><asp:Label ID="Label_2" ForeColor="Red" runat="server" Visible="false">*</asp:Label></th>
			            <th><%=global::Resources.Texts.Info %></th>
			            <% if( IsManagerMode ) { %>
				            <th><asp:Literal ID="lTextDay" runat="server" Text="Days from moment of last renewval, d." /></th>
			            <% } %>
			            <th><%=global::Resources.Texts.Price %></th>
                        
			            <% if (IsManagerMode)
			               { %>
			            <th class="nowrap"><%=DiscountName1%><span class="info" title="1-st level of price (offer to persons, who successfully registered on our site)"></span></th>
			            <th class="nowrap"><%=DiscountName2%><span class="info" title="2-nd level of price (offer, to customers whos total amount of orders for whole period of cooperation exeeds  1 500 $)"></span></th>
			            <th class="nowrap"><%=DiscountName3%><span class="info" title="this price is effective if month amount of purchasing exeeds 12 000 $ (Now it's showing to unregistered users)"></span></th>
 			            <% } %>
			            <th colspan="2">&nbsp;</th>
                        <th><%=global::Resources.Texts.ContactSupplier %></th>
                        
	               </tr>
		            <asp:Repeater ID="_partsRepeater" runat="server"
			            onitemcommand="_partsRepeater_ItemCommand" 
			            onitemdatabound="_partsRepeater_ItemDataBound">
			            <ItemTemplate>
			
			            <asp:PlaceHolder ID="phOwnStores" runat="server" Visible="<%# ((SearchResultItem)Container.DataItem).IsOwnStore && IsGroupHeaderVisible( Container.DataItem, SparePartItemType.Exact ) %>">
			            <tr>
				            <th colspan="<%#IsManagerMode?13:10%>" class="empty">&nbsp;</th>
			            </tr>
			            <tr>
			                <td colspan="<%#IsManagerMode?13:10%>" class="th">
			                <asp:Literal ID="lOwnStores" runat="server" Text="<%$ Resources:Texts, OwnStockWarehouses %>" />
			                </td>
			            </tr>
			            </asp:PlaceHolder>
			
			            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="<%# !((SearchResultItem)Container.DataItem).IsOwnStore && IsGroupHeaderVisible( Container.DataItem, SparePartItemType.Exact )%>">
			            <tr>
				            <th colspan="<%#IsManagerMode?13:10%>" class="empty">
				            &nbsp;
				            </th>
			            </tr>
			            <tr>
				            <td colspan="<%#IsManagerMode?13:10%>" class="th">
				            <asp:Literal ID="lSearchCode" runat="server" Text="<%$ Resources:Texts, RequestedArticle %>" />
				            </td>
			            </tr>
			            </asp:PlaceHolder>

			            <asp:PlaceHolder runat="server" ID="_phCPHeader" Visible="<%# !((SearchResultItem)Container.DataItem).IsOwnStore && IsGroupHeaderVisible( Container.DataItem, SparePartItemType.Transition )%>">
			            <tr>
				            <th colspan="<%#IsManagerMode?13:10%>" class="empty">
				            &nbsp;
				            </th>
			            </tr>
			            <tr>
				            <td colspan="<%#IsManagerMode?13:10%>" class="th">
				            <asp:Literal ID="lSearchCrosses" runat="server" Text="<%$ Resources:Texts, SupersededForArticle %>" />
				            </td>
			            </tr>
			            </asp:PlaceHolder>

			            <asp:PlaceHolder runat="server" ID="PlaceHolder2" Visible="<%# !((SearchResultItem)Container.DataItem).IsOwnStore && IsGroupHeaderVisible( Container.DataItem, SparePartItemType.Analogue )%>">
			            <tr>
				            <th colspan="<%#IsManagerMode?13:10%>" class="empty">
				            &nbsp;
				            </th>
			            </tr>
			            <tr>
				            <td colspan="<%#IsManagerMode?13:10%>" class="th">
				            <asp:Literal ID="lSearchAnalogue" runat="server" Text="<%$ Resources:Texts, AnalogsOfArticle %>" />
				            </td>
			            </tr>
			            </asp:PlaceHolder>
			
			            <tr class='<%#( (SearchResultItem)Container.DataItem ).SparePartGroup!=null ? "row"+( (SearchResultItem)Container.DataItem ).SparePartGroup.SparePartGroupID : ""%>'>
				            <td>
					            <%# Server.HtmlEncode( ((SearchResultItem)Container.DataItem).SparePart.Manufacturer ) %>
				            </td>
			               <td class="nowrap"><asp:Label id="_lblKey" runat="server" Visible="false"></asp:Label>
			               <%# Server.HtmlEncode( ((SearchResultItem)Container.DataItem).SparePart.PartNumber )%>
			   
			               <asp:PlaceHolder runat="server" ID="_phGroup" Visible='<%#((SearchResultItem)Container.DataItem).SparePartGroup!=null%>'>					
					            <sup style="color:red" class="help_img" title=" <%# ( (SearchResultItem)Container.DataItem ).SparePartGroup!=null ? Server.HtmlEncode(((SearchResultItem)Container.DataItem).SparePartGroup.GroupName) : "" %>">
						            <%#( (SearchResultItem)Container.DataItem ).SparePartGroupIndex+1%>
					            </sup>
			               </asp:PlaceHolder>
			   
			               </td>
                            
			        
			               <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%#((SearchResultItem)Container.DataItem).ShowInfo&&((SearchResultItem)Container.DataItem).ShowPrices%>'>
				               <td>
					               <span style="font-weight:bold; color: #A13340"><%# ((SearchResultItem)Container.DataItem).AdditionalInfoExt != null && ((SearchResultItem)Container.DataItem).AdditionalInfoExt.HasDefectPics ? "«УЦЕНКА ПО БРАКУ»   " : ""%></span>
					               <a target="_blank" href="<%#GetSparePartDetailsUrl(((SearchResultItem)Container.DataItem).SparePart)%>" 
						               onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					               <%# Server.HtmlEncode(((SearchResultItem)Container.DataItem).SparePart.PartDescription )%>
					               </a>
				               </td>
				            </asp:PlaceHolder>
				
				            <td>
                                <%# ((SearchResultItem)Container.DataItem).SparePart.WeightPhysical.HasValue ? Server.HtmlEncode( ((SearchResultItem)Container.DataItem).SparePart.WeightPhysical.ToString() ) : "-" %>
                            </td>

                            <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%#((SearchResultItem)Container.DataItem).ShowInfo&&!((SearchResultItem)Container.DataItem).ShowPrices%>'>
				               <td>
					               <%# Server.HtmlEncode(((SearchResultItem)Container.DataItem).SparePart.PartDescription )%>
				               </td>
				            </asp:PlaceHolder>
				            <asp:PlaceHolder ID="PlaceHolder5" runat="server" Visible='<%#!((SearchResultItem)Container.DataItem).ShowInfo%>'>
				               <td style="text-align:center">&#x2014;</td>
				            </asp:PlaceHolder>
				
				            <asp:PlaceHolder ID="PlaceHolder6" runat="server" Visible='<%# ((SearchResultItem)Container.DataItem).ShowPrices %>'>
				               <td style="text-align:center">
						            <%# (((SearchResultItem)Container.DataItem).SparePart.QtyInStock??0)!=0 ? OrderBO.GetQtyInStockString( ((SearchResultItem)Container.DataItem).SparePart ) : "" %>
						            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/qty_full.gif" Visible='<%#(((SearchResultItem)Container.DataItem).SparePart.QtyInStock??0)==0%>' ToolTip="<%$ Resources:Texts, AvailableAtSupplierWarehouse %>"  />
				               </td>           
				               <td><%# ((SearchResultItem)Container.DataItem).SparePart.DisplayDeliveryDaysMin %>-<%# ( (SearchResultItem)Container.DataItem ).SparePart.DisplayDeliveryDaysMax %></td>
				            </asp:PlaceHolder>
				            <asp:PlaceHolder ID="PlaceHolder7" runat="server" Visible='<%# !((SearchResultItem)Container.DataItem).ShowPrices %>'>
				               <td style="text-align:center">&#x2014;</td>           
				               <td style="text-align:center">&#x2014;</td>
				            </asp:PlaceHolder>

			   
			               <asp:PlaceHolder ID="PlaceHolder8" runat="server" Visible='<%#GetInfoStatVisibility((SearchResultItem)Container.DataItem) %>'>
			                   <td>
			                        <asp:PlaceHolder ID="PlaceHolder9" runat="server" Visible='<%#((SearchResultItem)Container.DataItem).ShowInfo%>'>
					                    <uc1:SparePartInfoExt ID="SparePartInfoExt1" runat="server" AdditionalInfoExt="<%# ((SearchResultItem)Container.DataItem).AdditionalInfoExt %>" />
					                    <uc1:SparePartInfo ID="SparePartInfo1" runat="server" AdditionalInfo=<%# ((SearchResultItem)Container.DataItem).AdditionalInfo %> />
					                    <img id="Img5" runat="server" src="~/images/search_info.png" title='<%# string.Format( Resources.Hints.QuantityInOrder, ((SearchResultItem)Container.DataItem).SparePart.DefaultOrderQty ) %>' border="0" Visible='<%# ( (SearchResultItem)Container.DataItem ).SparePart.DefaultOrderQty > 1 %>' />
			                        </asp:PlaceHolder>
					                <asp:PlaceHolder ID="PlaceHolderStat" runat="server" Visible='<%#((SearchResultItem)Container.DataItem).ShowSupplierStatistic%>'>
                                        <a title="Статистика работы поставщика" target="_blank"
                                        onclick="info=window.open(this.href,'','width=332,height=352,scrollbars=no,resizable=no'); info.focus(); return false;"
                                        href="<%# GetSupplierStatisticUrl(((SearchResultItem)Container.DataItem).SparePart)%>">
                                        <img id="Img4" src="~/images/search_statistic.png" runat="server" border="0" /></a>&nbsp;&nbsp;
                                    </asp:PlaceHolder>
			                   </td>
			               </asp:PlaceHolder>
				            <asp:PlaceHolder ID="PlaceHolder10" runat="server" Visible='<%#!((SearchResultItem)Container.DataItem).ShowInfo%>'>
				               <td style="text-align:center">&#x2014;</td>
				            </asp:PlaceHolder>

			               <asp:PlaceHolder ID="PlaceHolder11" runat="server" Visible='<%# ((SearchResultItem)Container.DataItem).ShowPrices %>'>
					            <asp:PlaceHolder ID="PlaceHolder12" runat="server" Visible='<%# IsManagerMode %>'>
					            <td style="text-align:center">
						            <%# string.Format( "{0:0}", (DateTime.Today - ((SearchResultItem)Container.DataItem).SparePart.PriceDate.Date).TotalDays ) %>
					            </td>
					            </asp:PlaceHolder>
    				            <td class="price">
    					            <span class="blue"><%# string.Format( "{0:### ### ##0.00}", ((SearchResultItem)Container.DataItem).FinalSalePrice  ) %>&nbsp;<%# IsGuest?"*":"" %></span>
					            </td>
					            <td class="price" runat="server" id="Td1" visible='<%# IsManagerMode %>'>
    					            <span class="blue"><%# string.Format( "{0:### ### ##0.00}", ( (SearchResultItem)Container.DataItem ).SalePrice1 )%></span>
					            </td>
					            <td class="price" runat="server" id="Td2" visible='<%# IsManagerMode %>'>
    					            <span class="blue"><%# string.Format( "{0:### ### ##0.00}", ( (SearchResultItem)Container.DataItem ).SalePrice2 )%></span>
					            </td>
					            <td class="price" runat="server" id="Td3" visible='<%# IsManagerMode %>'>
    					            <span class="blue"><%# string.Format( "{0:### ### ##0.00}", ( (SearchResultItem)Container.DataItem ).SalePrice3 )%></span>
					            </td>
					            <td style="width:25px;text-align:center;" 
					            onkeypress="if( event.keyCode==13 ) { $('input[name$=_btnAddToCart]',this.parentElement).click(); return false; }">
						            <asp:PlaceHolder runat="server" ID="_qtyPlaceHolder" Visible='<%# !IsRestricted %>'>
						              <nobr>
						              <asp:TextBox ID="_txtQty" runat="server" Columns="3" Text='<%# ((SearchResultItem)Container.DataItem).SparePart.DefaultOrderQty %>'></asp:TextBox>
						              </nobr>
						               <asp:Label ID="_lblDefaultOrderQty" runat="server" Visible="false" Text='<%# ( (SearchResultItem)Container.DataItem ).SparePart.DefaultOrderQty %>' />
						            </asp:PlaceHolder>
					            </td>
					            <td align="center">
					                <asp:LinkButton CssClass="fa fa-shopping-cart" ID="_btnAddToCart" runat="server" CommandName="AddToCart" Text="<%$ Resources:Texts, AddToCartLC %>"  Visible='<%# !IsRestricted && Convert.ToDecimal(((SearchResultItem)Container.DataItem).FinalSalePrice) != 0 %>'/>
                                    </td>
                               <td>
                                    <a target="_blank" <%--onclick="info=window.open(this.href,'','width=332,height=352,scrollbars=no,resizable=no'); info.focus(); return false;"  --%>
                                        href="<%# GetSellerMessageUrl(((SearchResultItem)Container.DataItem).SparePart.InternalFranchName)%>" data-toggle="tooltip" data-placement="left" title="Сообщение продавцу"> 
                                    <i class="fa fa-envelope-o"></i></a>
                                    <a  href="#">
                                    <i class="fa fa-info-circle"></i></a>
					            </td>
				            </asp:PlaceHolder>
				            <asp:PlaceHolder ID="PlaceHolder13" runat="server" Visible='<%# !((SearchResultItem)Container.DataItem).ShowPrices %>'>
					            <asp:PlaceHolder ID="PlaceHolder14" runat="server" Visible='<%# IsManagerMode %>'>
						            <td style="text-align:center">&#x2014;</td>
					            </asp:PlaceHolder>
					            <td style="text-align:center">&#x2014;</td>
					            <td style="text-align:center" runat="server" id="Td4" visible='<%# IsManagerMode %>' nowrap>&#x2014;</td>
					            <td style="text-align:center" runat="server" id="Td5" visible='<%# IsManagerMode %>' nowrap>&#x2014;</td>
					            <td  style="text-align:center" runat="server" id="Td6" visible='<%# IsManagerMode %>' nowrap>&#x2014;</td>
					            <td width="25"></td>
					            <td></td>
				            </asp:PlaceHolder>
			            </tr>
			            </ItemTemplate>
			            <SeparatorTemplate>
			            </SeparatorTemplate>
		            </asp:Repeater>
	            </table>
            </div>
            <div class="bottom">
                <div class="pages">
                    <ul>
                        <li><uc1:RMSPagerControl ID="_searchResultPager" runat="server" Visible="false" /></li>
                        <li>
                            <%=global::Resources.Texts.Prices %>&nbsp;<asp:DropDownList ID="_currencyBox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="_currencyBox_OnSelectedIndexChanged" />
                        </li>
                        <li>
                            <%=global::Resources.Texts.Sorting %>&nbsp;<asp:DropDownList ID="_sortOptionsBox" runat="server"  AutoPostBack="true" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>

	    <asp:Repeater runat="server" ID="_usedGroupsRepeater">
	    <ItemTemplate>
		    <br />
		    <sup style="color:red"><%#Container.ItemIndex+1%></sup> <%#Server.HtmlEncode((string)Eval("GroupName"))%>
		    <style>
			    tr.row<%#Eval("SparePartGroupID")%> td { background-color: #<%#Eval("BackgroundColor")%> }
		    </style>
	    </ItemTemplate>
	    <%--<FooterTemplate>
	        <asp:Literal runat="server" ID="_pagingLinks" Text="<%#GetPagingLinks()%>" />
	    </FooterTemplate>--%>
	    </asp:Repeater>

	    <uc1:TextItemControl ID="_discountNotes_TextItemControl" runat="server" 
		    TextItemID="SpareParts.DiscountNotes"
		    ShowHeader="false" />

	    <uc1:TextItemControl ID="_callToManagerHint_TextItemControl" runat="server" 
		    TextItemID="SearchSpareParts.CallToManagerHint"
		    ShowHeader="false" />
		
    </asp:PlaceHolder>
</div>
                    <div class="modal inmodal fade" id="myModal5" role="dialog" aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    <h4 class="modal-title">Modal title</h4>
                                    <small class="font-bold">Lorem Ipsum is simply dummy text of the printing and typesetting industry.</small>
                                </div>
                                <div class="modal-body">
                                    <p>
                                        <strong>Lorem Ipsum is simply dummy</strong> text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown
                                        printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting,
                                        remaining essentially unchanged.
                                    </p>
                                    <p>
                                        <strong>Lorem Ipsum is simply dummy</strong> text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown
                                        printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting,
                                        remaining essentially unchanged.
                                    </p>
                                </div>

                                <div class="modal-footer">
                                    <button type="button" class="btn btn-white" data-dismiss="modal">Close</button>
                                    <button type="button" class="btn btn-primary">Save changes</button>
                                </div>
                            </div>
                        </div>
                    </div>