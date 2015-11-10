<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineTemplateLite.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLineTemplateLite" %>
<%@ Register TagPrefix="rms" Namespace="RmsAuto.Store.Web.Controls" Assembly="RmsAuto.Store.Web" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<%@ Import Namespace="RmsAuto.Store.Web.Controls" %>



		<tr runat="server" id="_oldRow" visible='<%# Eval("ParentOrderLine")!=null %>' class="old">
			<td>
			    <%--<asp:Label ID="Label_PartNumberTransition" ForeColor="Red" runat="server" Visible='<%# (byte)Eval( "CurrentStatus" )==RmsAuto.Store.Entities.OrderLineStatusUtil.StatusByte("PartNumberTransition") %>' >**</asp:Label>--%>
			    
			</td>
			<td runat="server" Visible='<%#ShowCustOrderNum%>'></td>
			<td><%#Eval("Order.OrderDate","{0:dd.MM.yyyy}") %></td>
			
			<td><%#Server.HtmlEncode((string)Eval("ReferenceID"))%></td>
			<td><%#Server.HtmlEncode((string)Eval("ParentOrderLine.Manufacturer"))%></td>
			<td><%#Server.HtmlEncode((string)Eval("ParentOrderLine.PartNumber"))%></td>
			<td>
				<a target="_blank" href="<%#GetSparePartDetailsUrl((RmsAuto.Store.Entities.OrderLine)Eval("ParentOrderLine"))%>"
					onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					<%#Server.HtmlEncode((string)Eval( "ParentOrderLine.PartName"))%>
				</a>
			</td>
			<td align="center"><%#Eval("ParentOrderLine.Qty")%></td>
			<td class="price"><span class="blue"><%#Eval("ParentOrderLine.UnitPrice", "{0:### ### ##0.00}")%></span></td>
			<td class="price"><span class="blue"><%#Eval("ParentOrderLine.Total", "{0:### ### ##0.00}")%></span></td>
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
			<td visible="false"><input type="hidden" ID="OrderLineID" runat="server" value='<%# Eval("OrderLineID")%>' /> 
			                    <input type="hidden" ID="OrderLineStatusID" runat="server" value='<%# Eval( "CurrentStatus" )%>' />
			</td>
			<td nowrap><asp:Image runat="server" ID="_imgDown" ImageUrl="~/images/down_arc.gif" Visible='<%#Eval("ParentOrderLine")!=null%>' /><%#GetOrderDisplayNumber((RmsAuto.Store.Entities.Order)Eval( "Order" ))%></td>
			<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#ShowCustOrderNum%>'>
				<td><%#Server.HtmlEncode(Convert.ToString(Eval( "Order.CustOrderNum" )))%></td>
			</asp:PlaceHolder>
			
			<td><%#Eval("Order.OrderDate","{0:dd.MM.yyyy}") %></td>
			<td><%#Server.HtmlEncode((string)Eval("ReferenceID"))%></td>
			<td><%#Server.HtmlEncode((string)Eval("Manufacturer"))%></td>
			<td><%#Server.HtmlEncode((string)Eval("PartNumber"))%></td>
			<td>
				<a target="_blank" href="<%#GetSparePartDetailsUrl((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem())%>"
					onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					<%#Server.HtmlEncode((string)Eval( "PartName" ))%>
				</a>
			</td>
			<td align="center"><%#Eval( "Qty" )%></td>
			<td class="price"><span class="blue"><%#Eval("UnitPrice", "{0:### ### ##0.00}")%></span></td>
			<td class="price"><span class="blue"><%#Eval("Total", "{0:### ### ##0.00}")%></span></td>
			<td><%#Eval( "EstSupplyDate", "{0:dd.MM.yyyy}" )%></td>
			<td>
		            <asp:DropDownList DataSourceID="AllStatuses" DataTextField="Client" 
                        DataValueField="OrderLineStatusID" style="width: 200px;" 
                        runat="server"  ID="_StatusBoxList" 
                        Visible='<%# ((OrderLine)Page.GetDataItem()).CanChangeLiteStatus((byte)Eval("CurrentStatus")) %>'
                        ondatabinding="_StatusBoxList_DataBinding" 
                        onprerender="_StatusBoxList_PreRender" /><wbr>
                    <asp:Label ID="Label1" Visible='<%# !((OrderLine)Page.GetDataItem()).CanChangeLiteStatus((byte)Eval("CurrentStatus"))  %>' runat="server"> 
                        <%#GetStatusName((byte)Eval("CurrentStatus"))%>
                    </asp:Label>
				    <asp:Button visible ='<%# ((OrderLine)Page.GetDataItem()).CanChangeLiteStatus((byte)Eval("CurrentStatus")) %>' CommandName="Update" class="bluebutton" runat="server" ID="UpdateLineButton" Text="Update" />
				        <%--Visible='<%# (byte)Eval( "CurrentStatus" )==RmsAuto.Store.Entities.OrderLineStatusUtil.StatusByte("RefusedBySupplier") %>'--%>
				    <asp:PlaceHolder runat="server" ID="_reorderPlaceHolder" visible="<%#ShowReorderPlaceHolder(Page.GetDataItem())%>" >
					    <a ID="_reorderLink" visible="false" href='<%#GetReorderSearchUrl(Page.GetDataItem())%>'>
						    <asp:Literal ID="lReOrder" runat="server" Text="Reorder" />
					    </a>
				    </asp:PlaceHolder>
				    <asp:PlaceHolder runat="server" ID="_reorderFinishPlaceHolder" visible="<%#ShowReorderFinishPlaceHolder(Page.GetDataItem())%>" >
				        <asp:Label style="color:red;" ID="_reorder_Label" runat="server" Text="<%#GetReorder_Label_Text(Page.GetDataItem())%>">
	                    </asp:Label>
				    </asp:PlaceHolder>
			</td>
	
	<%--		<td runat="server" id="td1" visible='<%# Eval("ParentOrderLine") == null %>' class="answ1">
			    <img id="Img2" runat="server" src='~/images/question.gif' 
				    class="help_img" title='<%# "" + GetStatusDescription((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem()) + ""%>'
				    visible='<%#GetStatusDescription((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem())!=null%>' />
			    
			</td>
			<td runat="server" id="td2" visible='<%# Eval("ParentOrderLine")==null %>' class="answ2" align="center">
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
			<td runat="server" id="td3" visible='<%# Eval("ParentOrderLine")==null %>' class="answ2">
				<asp:HyperLink runat="server" ID="_trackingLink" NavigateUrl='<%#GetOrderLineTrackingUrl((int)Eval("OrderLineID"))%>' Visible='false'><%--Visible='<%#(DateTime?)Eval("CurrentStatusDate")!=null%>'
					<img src="<%#ResolveUrl("~/images/history.jpg")%>" alt="история по строке" border="0" /></asp:HyperLink>
			</td>--%>
		</tr>
		
	<asp:ObjectDataSource ID="AllStatuses" runat="server" 
        TypeName="RmsAuto.Store.Web.Controls.OrderLineTemplateLite" SelectMethod="GetStatuses" >
    </asp:ObjectDataSource>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server">
</asp:ObjectDataSource>
