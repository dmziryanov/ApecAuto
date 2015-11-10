<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientPaymentsView.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientPaymentsView" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>

<script type="text/javascript">
    $(function() {
        $('.date').datepicker();
    });

    $(document).ready(function() {

        var k = $('td[id$="ClientTable"]').length;

         //Скрываем поле в таблице
        if ('<%# CurrentUserID != "-1" %>' == 'True')
            $('td[id$="ClientTable"]').hide();

        if (k == 0) {
            $("a[id*='PaymentsBootstrap']").hide();
        }
    });
</script>

<input runat="server" enableviewstate="true" id="hfUserID" type="hidden" value="" />
<input runat="server" enableviewstate="true" id="hfClientName" type="hidden" value="" />
<input runat="server" enableviewstate="true" id="hfClientBalance" type="hidden" value="" />
<input runat="server" enableviewstate="true" id="hfVaryParam" type="hidden" value="" />
<input runat="server" enableviewstate="true" id="hfCount" type="hidden" value="0" />

<div style="width:20%; float:left; height: 400px; min-width: 300px;">
	<asp:TextBox id="_txtClientName" runat="server" Width="194px" EnableViewState="true"></asp:TextBox>&nbsp 
	<asp:Button ID="_btnSearchClient" CssClass="button" runat="server" Text="<%$Resources:Texts, Search %>" onclick="_btnSearchClient_Click" />
	<br />
	<br />
	<asp:ListBox  ID="_listSearchResults" runat="server" onselectedindexchanged="_listSearchResults_SelectedIndexChanged" 
		DataTextField="ClientName" AutoPostBack="True" EnableViewState="true" CausesValidation="True" 
		DataValueField="UserID" Height="80%" Width="259px">
	</asp:ListBox>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</div>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"  
    TypeName="RmsAuto.Store.Web.Manager.Controls.ClientPaymentsView" 
    SelectCountMethod="GetPaymentsCount" SelectMethod="GetPayments" onselected="_objectDataSource_Selected">
    <SelectParameters>
        <asp:ControlParameter Name="userID" PropertyName="value" ControlID="hfUserID" Type="Int32" DefaultValue="0" />
        <asp:ControlParameter Name="dateFrom" ControlID="_DateFrom" Type="DateTime" />
        <asp:ControlParameter Name="dateTo" ControlID="_DateTo" Type="DateTime" />
        <asp:ControlParameter Name="VaryParam" PropertyName="value" ControlID="hfVaryParam" Type="String" DefaultValue="0" />
    </SelectParameters>
</asp:ObjectDataSource>

<div style="width:70%; float:left; min-width: 500px;">
	<h1 runat="server" id="ClientName"><%= global::Resources.Texts.BalanceList %> : <%= CurrentClientName %></h1>
	<%= global::Resources.Texts.EnterPaymentType %>&nbsp;
	<asp:DropDownList ID="ddlPaymentTypes" runat="server">
		<asp:ListItem Text="<%$Resources:Texts, Payment %>" Value="1" />
		<asp:ListItem Text="<%$Resources:Texts, GoodsReturn %>" Value="2" />
		<asp:ListItem Text="<%$Resources:Texts, MoneyReturn %>" Value="3" />
	</asp:DropDownList>
	<div>
	    <%= global::Resources.Texts.KindOfPayment %>:&nbsp<asp:RadioButtonList ID="_rblPaymentMethod" runat="server" />
	</div>&nbsp<asp:TextBox ID="txtInputPayment" runat="server" />
	    <asp:Button ID="btnInputPayment" runat="server" Text="<%$Resources:Texts, Enter %>" CssClass="button" onclick="btnInputPayment_Click" />
	<br />
    <br />
	<%= global::Resources.Texts.Balance %>:&nbsp;<%# CurrentClientBalance %>
	<br />
	<%= global::Resources.Texts.DateFrom %>:&nbsp;<asp:TextBox CssClass="date" runat="server" ID="_DateFrom" Text="" AutoPostBack="False" Width="100" />		
    &nbsp;&nbsp;<%= global::Resources.Texts.DateTill %>:&nbsp;<asp:TextBox CssClass="date" runat="server" ID="_DateTo" Text="" AutoPostBack="False" Width="100"  />
    &nbsp;<asp:Button ID="btnSearchPayments" runat="server" Text="<%$Resources:Texts, BalanceListView %>" CssClass="button" onclick="btnSearchPayments_Click" />
    <asp:HyperLink ID="PaymentsBootstrap" CssClass="button" runat="server" Text="<%$Resources:Texts, Download %>" Visible="true" />
	<br />
	<br />
    <div class="tab-text">
        <div class="t-hold">
            <table class="table-big">
		        <tbody>
			        <tr>
                        <th runat="server" id = "ClientColumn"><%= global::Resources.Texts.Client %></th>
				        <th><%= global::Resources.Texts.PaymentDate %></th>
				        <th><%= global::Resources.Texts.Summ %></th>
				        <th><%= global::Resources.Texts.TypeOfPayment %></th>
			        </tr>
		        </tbody>
			        <asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource" ondatabound="_listView_DataBound" ondatabinding="_listView_DataBinding" >
			        <LayoutTemplate>
				        <tbody class="list2 list3" id="linesTable" >
					        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
				        </tbody>
			        </LayoutTemplate>
			        <ItemTemplate>
				        <asp:PlaceHolder runat="server" ID="_placeHolder">
				        <tr>
                            <td id="ClientTable" runat="server"><%# Eval("ClientName") %></td>
					        <td><%# ((DateTime)Eval("PaymentDate")).ToString("yyyy.MM.dd") %></td>
					        <td class="price"><span class="blue"><%# ( Math.Abs((decimal)Eval("PaymentSum"))).ToString("### ### ##0.00") %></span></td>
					        <td><%# ((RmsAuto.Store.BL.LightPaymentType)Eval("PaymentType")).GetDescription() %></td>
				        </tr>
				        </asp:PlaceHolder>					
			        </ItemTemplate>
			        <EmptyDataTemplate>
				        <tr><td colspan="3"><%= global::Resources.Texts.ListOfLinesIsEmpty %></td></tr>
			        </EmptyDataTemplate>
			        </asp:ListView>
	        </table>
        </div>
        <div class="bottom">
            <div class="pages">
                <ul>
                    <li>
                        <asp:DataPager ID="_dataPager" runat="server" PagedControlID="_listView">
		                    <Fields>
			                    <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="&lt;" />
			                    <asp:NumericPagerField />
			                    <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="&gt;"/>
		                    </Fields>
	                    </asp:DataPager>
                    </li>
                    <li runat="server" id="_pagerSettingsBlock">
                        <%= global::Resources.Texts.DisplayBy %>:
	                    <asp:DropDownList runat="server" ID="_pageSizeBox" AutoPostBack="True" 
		                    OnSelectedIndexChanged="_pageSizeBox_SelectedIndexChanged">
		                    <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="400" Value="400"></asp:ListItem>
	                    </asp:DropDownList>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>