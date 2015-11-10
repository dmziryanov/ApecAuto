<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReclamationList.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ReclamationList" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl" TagPrefix="uc1" %>

<script type="text/javascript">
	$(document).ready(function() {
		$("#filter").accordion({ collapsible: true });
		$('.date').datepicker();
	});
</script>

<asp:ScriptManager ID="spMain" runat="server"></asp:ScriptManager>

<div id="filter">
	<h3><a href="#"><%= global::Resources.Texts.Filter2 %></a></h3>
	<div>
		<table style="width:auto;">
			<tr>
				<td><%= global::Resources.Reclamations.ReclamationType %>:</td>
				<td><asp:DropDownList ID="_filterReclamationType" runat="server"></asp:DropDownList></td>
			</tr>
			<tr>
				<td><%= global::Resources.Reclamations.ReclamationDateSince %></td>
				<td><asp:TextBox ID="_filterStartDate" runat="server" CssClass="date" Columns="10"></asp:TextBox><asp:CustomValidator ID="_filterStartDateCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterStartDate" 
						ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidFormat %>" 
						onservervalidate="ValidateDate"></asp:CustomValidator>&nbsp;<%= global::Resources.Reclamations.ReclamationDateTill %>&nbsp;<asp:TextBox ID="_filterEndDate" runat="server" CssClass="date" Columns="10"></asp:TextBox><asp:CustomValidator ID="_filterEndDateCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterEndDate" 
						ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidFormat %>" 
						onservervalidate="ValidateDate"></asp:CustomValidator></td>
			</tr>
		</table>
		<br />
		<asp:Button ID="_searchButton" runat="server" 
				Text="<%$ Resources:Texts, Filter %>" CssClass="button" OnClick="_searchButton_Click"/> <asp:Button ID="_clearFilterButton" runat="server" 
				Text="<%$ Resources:Texts, ClearFilters %>" CssClass="button" CausesValidation="False" 
				OnClick="_clearFilterButton_Click"/>
	</div>
</div>
<br />
<uc1:TextItemControl ID="TextItemControl1" runat="server" ShowHeader="false" TextItemID="Reclamation.Documents.Link" />
<div class="tab-text">
    <div class="t-hold">
        <div class="br"></div>
        <div class="bl"></div>
        <table class="table-big">
	        <tr>
		        <th><asp:Literal ID="lHRecNumber" runat="server" Text="<%$ Resources:Reclamations, Head_RecNumber %>" /></th>
		        <th><asp:Literal ID="lHRecDate" runat="server" Text="<%$ Resources:Reclamations, Head_RecDate %>" /></th>
		        <th><asp:Literal ID="lHOrderNumber" runat="server" Text="<%$ Resources:Reclamations, Head_OrderNumber %>" /></th>
		        <th><asp:Literal ID="lHRecType" runat="server" Text="<%$ Resources:Reclamations, Head_RecType %>" /></th>
		        <th><asp:Literal ID="lHPartNumber" runat="server" Text="<%$ Resources:Reclamations, Head_PartNumber %>" /></th>
		        <th><asp:Literal ID="lHBrand" runat="server" Text="<%$ Resources:Reclamations, Head_Brand %>" /></th>
		        <th><asp:Literal ID="lHStatus" runat="server" Text="<%$ Resources:Reclamations, Head_Status %>" /></th>
		        <th><asp:Literal ID="lHStatusDate" runat="server" Text="<%$ Resources:Reclamations, Head_StatusDate %>" /></th>
		        <th><asp:Literal ID="lHPrint" runat="server" Text="<%$ Resources:Reclamations, Head_Print %>" /></th>
	        </tr>
	        <asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource">
		        <LayoutTemplate>
			        <tbody class="list2 list3">
				        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
			        </tbody>
		        </LayoutTemplate>
		        <ItemTemplate>
			        <asp:PlaceHolder runat="server" ID="_placeHolder">
				        <tr>
					        <td><%#Server.HtmlEncode( (string)Eval("sReclamationNumber") )%></td>
					        <td><%#Eval( "ReclamationDate", "{0:dd.MM.yyyy}" )%></td>
					        <td><%#Server.HtmlEncode( Eval("OrderID").ToString()) %></td>
					        <td><%#GetReclamationTypeName((ReclamationTracking.ReclamationType)Eval( "ReclamationType" ))%></td>
					        <td><%#Server.HtmlEncode( (string)Eval( "PartNumber" ) )%></td>
					        <td><%#Server.HtmlEncode( (string)Eval( "Manufacturer" ) )%></td>
					        <td class="nowrap">
						        <%#ReclamationTracking.StatusName( (byte)Eval( "CurrentStatus" ) )%>
						        <asp:PlaceHolder ID="ph1" runat="server" Visible='<%#(byte)Eval("CurrentStatus") == 70%>'>
							        <span style="color:Red">*</span>
							        &nbsp;<span class="info" title="<%$ Resources:Reclamations, ReclamationHint1 %>" runat="server"></span>
						        </asp:PlaceHolder>
						        <asp:PlaceHolder ID="ph2" runat="server" Visible='<%#(byte)Eval("CurrentStatus") == 80%>'>
							        <span style="color:Red">*</span>
							        &nbsp;<span id="Span1" class="info" title="<%$ Resources:Reclamations, ReclamationHint2 %>" runat="server"></span>
						        </asp:PlaceHolder>
						        <asp:PlaceHolder ID="ph3" runat="server" Visible='<%#(byte)Eval("CurrentStatus") == 40%>'>
							        &nbsp;<span id="Span2" class="info" title="<%$ Resources:Reclamations, ReclamationHint3 %>" runat="server"></span>
						        </asp:PlaceHolder>
					        </td>
					        <td><%#Eval( "CurrentStatusDate", "{0:dd.MM.yyyy}" )%></td>
					        <td>
						        <asp:HyperLink ID="linkReclamationPrint" runat="server" Target="_blank" ToolTip="<%$ Resources:Reclamations, PrintTooltip %>"
							        NavigateUrl='<%# "~/Cabinet/Reclamation/ReclamationPrint.aspx?id=" + Eval("ReclamationID").ToString() %>'>
							        <asp:Image ID="imagePrint" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/print.png" />
						        </asp:HyperLink>
					        </td>
				        </tr>
			        </asp:PlaceHolder>
		        </ItemTemplate>
		        <EmptyDataTemplate>
			        <tr><td colspan="9"><%= global::Resources.Reclamations.EmptyList %></td></tr>
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
	                    <asp:DropDownList runat="server" ID="_pageSizeBox" AutoPostBack="True" 
		                    OnSelectedIndexChanged="_pageSizeBox_SelectedIndexChanged">
		                    <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="400" Value="400"></asp:ListItem>
	                    </asp:DropDownList>
                    </li>
                    <li runat="server" id="_sortBlock">
	                    <%= global::Resources.Texts.OrderBy%>
	                    <asp:DropDownList runat="server" ID="_sortBox" AutoPostBack="True" 
		                    OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
	StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
	TypeName="RmsAuto.Store.Web.Controls.ReclamationList" 
	SelectCountMethod="GetReclamationsCount" SelectMethod="GetReclamations">
	<SelectParameters>
		<asp:Parameter Name="reclamationType" Type="Int32" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="reclamationDateBegin" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="reclamationDateEnd" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:ControlParameter Name="sort" ControlID="_sortBox" Type="Int32" DefaultValue="0" />
		<asp:Parameter Name="startIndex" Type="Int32" />
		<asp:Parameter Name="size" Type="Int32" />
	</SelectParameters>
</asp:ObjectDataSource>	