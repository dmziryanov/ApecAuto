<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineTemplateToLoad.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLineTemplateToLoad" %>
<%@ Register TagPrefix="rms" Namespace="RmsAuto.Store.Web.Controls" Assembly="RmsAuto.Store.Web" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>

		<tr runat="server" id="_oldRow" visible='<%# Eval("ParentOrderLine")!=null %>' class="old">
			<td>
			    <%--<asp:Label ID="Label_PartNumberTransition" ForeColor="Red" runat="server" Visible='<%# (byte)Eval( "CurrentStatus" )==RmsAuto.Store.Entities.OrderLineStatusUtil.StatusByte("PartNumberTransition") %>' >**</asp:Label>--%>
			    <%#GetOrderDisplayNumber((RmsAuto.Store.Entities.Order)Eval("Order"))%>			    
			</td>
		    			
			<td id="Td1" runat="server" Visible='<%#ShowCustOrderNum%>'></td>
			<td><%#Eval( "Order.OrderDate","{0:dd.MM.yyyy}" ) %></td>
			
			<td><%#Server.HtmlEncode((string)Eval( "ReferenceID" ))%></td>
			
			<td><%#Server.HtmlEncode( (string)Eval( "ParentOrderLine.Manufacturer" ) )%></td>
			<td><%#Server.HtmlEncode( (string)Eval( "ParentOrderLine.PartNumber" ) )%></td>
			<td>
				<a target="_blank" href="<%#GetSparePartDetailsUrl((RmsAuto.Store.Entities.OrderLine)Eval("ParentOrderLine"))%>"
					onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					<%#Server.HtmlEncode( (string)Eval( "ParentOrderLine.PartName" ) )%>
				</a>
			</td>
			<td align="center"><%#Eval( "ParentOrderLine.Qty" )%></td>
			<td class="price" nowrap><%#Eval("ParentOrderLine.UnitPrice", "{0:### ### ##0.00}")%></td>
			<td class="price" nowrap><%#Eval("ParentOrderLine.Total", "{0:### ### ##0.00}")%></td>
			<td><%#Eval( "ParentOrderLine.EstSupplyDate", "{0:dd.MM.yyyy}" )%></td>
			<td><%--#Eval("ParentOrderLine")!=null ? GetStatusName( (RmsAuto.Store.Entities.OrderLineStatus)Eval( "ParentOrderLine.CurrentStatus" ) ) : null--%></td>
			
			<td class="answ1" rowspan="2">
				<img id="Img1" runat="server" src='~/images/question.gif'  
					class="help_img" title='<%# "" + GetStatusDescription((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem()) + ""%>'
					visible='<%#GetStatusDescription((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem())!=null%>' />
			</td>
			<td class="answ2" align="center" rowspan="2">
				<rms:OrderLineStatusView id="OrderLineStatusView1" runat="server" 
					DisplayAccept="подтверждение" DisplayDecline="отказ"
					OnChangeReaction="OnClientChangeReaction"
					OrderLineStatusChange='<%# Eval("LastStatusChange") %>'>
                    <StatusNameTemplate></StatusNameTemplate>
                    <ReactionInfoTemplate><asp:Literal ID="lYouSend" runat="server" Text="вы отправили" /> "<%# Container.Reaction %>"<br /> <%# string.Format( "{0:dd.MM.yyyy HH:mm:ss}", Container.ReactionTime ) %></ReactionInfoTemplate>
                    <ReactionCommandsTemplate>
                        <asp:Button id="_btnAccept" runat="server" CommandName="Reaction" CommandArgument="Accept" Text="Принять"  />
                        <asp:Button id="_btnDecline" runat="server" CssClass="no" CommandName="Reaction" CommandArgument="Decline" Text="Отказаться"  />
                    </ReactionCommandsTemplate>
                </rms:OrderLineStatusView>
   			</td>
			<td class="answ2" rowspan="2">
				<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%#GetOrderLineTrackingUrl((int)Eval("OrderLineID"))%>' Visible='false'><%--Visible='<%#(DateTime?)Eval("CurrentStatusDate")!=null%>'--%>
					<img src="<%#ResolveUrl("~/images/history.jpg")%>" alt="история по строке" border="0" /></asp:HyperLink>
			</td>
		</tr>	
		<tr>
            <td><input type="checkbox" name="orderline_id" 
					value='<%# Eval("OrderlineId") %>' onclick="RefreshCheckBoxes()" /> </td>
			<td nowrap><asp:Image runat="server" ID="_imgDown" ImageUrl="~/images/down_arc.gif" Visible='<%#Eval("ParentOrderLine")!=null%>' /><%#GetOrderDisplayNumber( (RmsAuto.Store.Entities.Order)Eval( "Order" ) )%></td>
			<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#ShowCustOrderNum%>'>
				<td><%#Server.HtmlEncode(Convert.ToString(Eval( "Order.CustOrderNum" )))%></td>
			</asp:PlaceHolder>
			<td><%#Eval("Order.OrderDate","{0:dd.MM.yyyy}") %></td>
			
			<td><%#Server.HtmlEncode((string)Eval( "ReferenceID" ))%></td>
			
			<td><%#Server.HtmlEncode((string)Eval( "Manufacturer" ))%></td>
			<td><%#Server.HtmlEncode((string)Eval( "PartNumber" ))%></td>
			<td>
				<a target="_blank" href="<%#GetSparePartDetailsUrl((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem())%>"
					onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					<%#Server.HtmlEncode((string)Eval( "PartName" ))%>
				</a>
			</td>
			<td align="center"><%#Eval( "Qty" )%></td>
			<td class="price" nowrap><%#Eval("UnitPrice", "{0:### ### ##0.00}")%></td>
			<td class="price" nowrap><%#Eval("Total", "{0:### ### ##0.00}")%></td>
			<td><%#Eval( "EstSupplyDate", "{0:dd.MM.yyyy}" )%></td>
			<td>
				<%#GetStatusName( (byte)Eval( "CurrentStatus" ) )%>
				<%--Visible='<%# (byte)Eval( "CurrentStatus" )==RmsAuto.Store.Entities.OrderLineStatusUtil.StatusByte("RefusedBySupplier") %>'--%>
				<asp:PlaceHolder runat="server" ID="_reorderPlaceHolder" visible="<%#ShowReorderPlaceHolder(Page.GetDataItem())%>" >
					<a ID="_reorderLink" visible="false" href='<%#GetReorderSearchUrl(Page.GetDataItem())%>'>
						<asp:Literal ID="lReOrder" runat="server" Text="Перезаказать" />
					</a>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="_reorderFinishPlaceHolder" visible="<%#ShowReorderFinishPlaceHolder(Page.GetDataItem())%>" >
				    <asp:Label style="color:red;" ID="_reorder_Label" runat="server" Text="<%#GetReorder_Label_Text(Page.GetDataItem())%>">
	                </asp:Label>
				</asp:PlaceHolder>
			</td>
			<%--<td runat="server" id="td2" visible='<%# Eval("ParentOrderLine")==null %>' class="answ1">
			    <img id="Img2" runat="server" src='~/images/question.gif' 
				    class="help_img" title='<%# "" + GetStatusDescription((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem()) + ""%>'
				    visible='<%#GetStatusDescription((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem())!=null%>' />
			    
			</td>
			<td runat="server" id="td3" visible='<%# Eval("ParentOrderLine")==null %>' class="answ2" align="center">
				<rms:OrderLineStatusView id="_statusView" runat="server" 
					DisplayAccept="подтверждение" DisplayDecline="отказ"
					OnChangeReaction="OnClientChangeReaction"
					OrderLineStatusChange='<%# Eval("LastStatusChange") %>'>
                    <StatusNameTemplate></StatusNameTemplate>
                    <ReactionInfoTemplate><asp:Literal ID="lYouSend2" runat="server" Text="вы отправили" /> "<%# Container.Reaction %>"<br /> <%# string.Format( "{0:dd.MM.yyyy HH:mm:ss}", Container.ReactionTime ) %></ReactionInfoTemplate>
                    <ReactionCommandsTemplate>
                        <asp:Button id="_btnAccept" runat="server" CommandName="Reaction" CommandArgument="Accept" Text="Принять"  />
                        <asp:Button id="_btnDecline" runat="server" CssClass="no" CommandName="Reaction" CommandArgument="Decline" Text="Отказаться"  />
                    </ReactionCommandsTemplate>
                </rms:OrderLineStatusView>
   			</td>
			<td runat="server" id="td4" visible='<%# Eval("ParentOrderLine")==null %>' class="answ2">
				<asp:HyperLink runat="server" ID="_trackingLink" NavigateUrl='<%#GetOrderLineTrackingUrl((int)Eval("OrderLineID"))%>' Visible='false'>
					<img src="<%#ResolveUrl("~/images/history.jpg")%>" alt="история по строке" border="0" /></asp:HyperLink>
			</td>--%>
		</tr>