<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReclamationOrderLines.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ReclamationOrderLines" %>

<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/OrderLineReclamationTemplate.ascx" tagname="OrderLineReclamationTemplate" tagprefix="uc2" %>

<uc1:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrManager" />

<script type="text/javascript">
	$(document).ready(function() {
		$("#filter").accordion({ collapsible: true });
	});
</script>
<div id="filter">
	<h3><a href="#"><%= global::Resources.Texts.Filter2 %></a></h3>
	<div>
		<table>
			<tr>
				<td><%= global::Resources.Texts.OrderNumber %>:</td>
				<td>
					<asp:TextBox ID="_filterOrderNumber" runat="server"></asp:TextBox>
					<span class="info" title="<%$ Resources:ValidatorsMessages, IntegerValueOnly %>" runat="server"></span>
					<asp:CustomValidator ID="_OrderNumberCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterOrderNumber" ErrorMessage="<%$ Resources:ValidatorsMessages, IntegerValueOnly %>"
						OnServerValidate="ValidateOrderNumber"></asp:CustomValidator>
				</td>
			</tr>
			<tr>
				<td><%= global::Resources.Texts.PartNumber %>:</td>
				<td><asp:TextBox ID="_filterPartNumber" runat="server"></asp:TextBox></td>
			</tr>
		</table>
		<asp:Button ID="_searchButton" CssClass="button" runat="server" Text="<%$ Resources:Texts, Filter %>" OnClick="_searchButton_Click" />
		<asp:Button ID="_clearFilterButton" CssClass="button" runat="server" Text="<%$ Resources:Texts, ClearFilters %>" CausesValidation="false" OnClick="_clearFilterButton_Click" />
	</div>
</div>

<div class="tab-text">
    <div class="t-hold">
        <div class="br"></div>
        <div class="bl"></div>
        <table class="table-big">
	        <tr>
		        <th><asp:Literal ID="lHOrderNumber" runat="server" Text="<%$ Resources:Reclamations, Head_OrderNumber %>"></asp:Literal></th>
                <th><asp:Literal ID="lHCustOrderNumber" runat="server" Text="<%$ Resources:Reclamations, Head_CustOrderNumber %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHOrderDate" runat="server" Text="<%$ Resources:Reclamations, Head_OrderDate %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHReferenceID" runat="server" Text="<%$ Resources:Reclamations, Head_ReferenceID %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHManufacturer" runat="server" Text="<%$ Resources:Reclamations, Head_Brand %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHPartNumber" runat="server" Text="<%$ Resources:Reclamations, Head_PartNumber %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHPartName" runat="server" Text="<%$ Resources:Reclamations, Head_PartName %>"></asp:Literal></th>
		        <th class="nowrap"><asp:Literal ID="lHQty" runat="server" Text="<%$ Resources:Reclamations, Head_Qty %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHPrice" runat="server" Text="<%$ Resources:Reclamations, Head_Price %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHSum" runat="server" Text="<%$ Resources:Reclamations, Head_Summ %>"></asp:Literal></th>
		        <th><asp:Literal ID="lHEstDate" runat="server" Text="<%$ Resources:Reclamations, Head_EstDate %>"></asp:Literal><span style="color:red">*</span></th>
		        <th><asp:Literal ID="lHStatus" runat="server" Text="<%$ Resources:Reclamations, Head_Status %>"></asp:Literal></th>
		        <th></th>
		        <th></th>
	        </tr>
	        <asp:ListView ID="_listView" runat="server" DataSourceID="_objectDataSource">
		        <LayoutTemplate>
			        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
		        </LayoutTemplate>
		        <ItemTemplate>
			        <asp:PlaceHolder ID="_placeHolder" runat="server">
				        <uc2:OrderLineReclamationTemplate ID="OrderLineReclamationTemplate1" runat="server" ShowCustOrderNum="true" />
			        </asp:PlaceHolder>
		        </ItemTemplate>
		        <EmptyDataTemplate>
			        <tr>
				        <td colspan="14"><asp:Literal ID="lEmptyList" runat="server" Text="<%$ Resources:Texts, EmptyList %>"></asp:Literal></td>
			        </tr>
		        </EmptyDataTemplate>
	        </asp:ListView>
        </table>
    </div>
    <div class="bottom">
        <div class="left">
            <div class="pages">
                <ul>
                    <li>
                        <asp:DataPager ID="_dataPager" runat="server" PagedControlID="_listView">
	                        <Fields>
		                        <asp:NextPreviousPagerField ButtonCssClass="prev" ShowFirstPageButton="False" 
			                        ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="" />
		                        <asp:NumericPagerField />
		                        <asp:NextPreviousPagerField ButtonCssClass="next" ShowLastPageButton="False" 
			                        ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="" />
	                        </Fields>
                        </asp:DataPager>
                    </li>
                    <li runat="server" id="_pagerSettingsBlock">
                        <%= global::Resources.Texts.DisplayOn%>
	                    <asp:DropDownList ID="_pageSizeBox" runat="server" AutoPostBack="true"
		                    OnSelectedIndexChanged="_pageSizeBox_SelectedIndexChanged">
		                    <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
		                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
		                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
		                    <asp:ListItem Text="400" Value="400"></asp:ListItem>
	                    </asp:DropDownList>
                    </li>
                    <li runat="server" id="_sortBlock">
	                    <%= global::Resources.Texts.OrderBy%>
	                    <asp:DropDownList ID="_sortBox" runat="server" AutoPostBack="true"
		                    OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>

<asp:ObjectDataSource ID="_objectDataSource" runat="server" EnablePaging="true"
	StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"
	TypeName="RmsAuto.Store.Web.Controls.ReclamationOrderLines"
	SelectCountMethod="GetOrderLinesCount" SelectMethod="GetOrderLines" 
	onselected="_objectDataSource_Selected">
	<SelectParameters>
		<asp:Parameter Name="orderNumber" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partNumber" Type="String" ConvertEmptyStringToNull="true" />
		<asp:ControlParameter Name="sort" ControlID="_sortBox" Type="Int32" DefaultValue="0" />
		<asp:Parameter Name="startIndex" Type="Int32" />
		<asp:Parameter Name="size" Type="Int32" />
	</SelectParameters>
</asp:ObjectDataSource>