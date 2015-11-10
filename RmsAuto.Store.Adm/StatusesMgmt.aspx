<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatusesMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.StatusesMgmt" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

	<style type="text/css">@import "scripts/datepick/redmond.datepick.css";</style> 
	<script type="text/javascript" src="scripts/datepick/jquery.datepick.min.js"></script>
	<script type="text/javascript" src="scripts/datepick/jquery.datepick-ru.js"></script>
	<script type="text/javascript">
		$(function(){
		$('.date').datepick({firstDay: 1,dateFormat: 'dd.mm.yy'}); 
		});
	</script>
    <script type="text/javascript">
        function ToggleAllStatuses()
        {
            var checkAllStatuses = $('input[name=check_all_statuses]');
            var statusCheckBoxes = $('input[name*=chkStatus]');
            var checked = checkAllStatuses.get(0).checked;
            $.each(statusCheckBoxes, function() { this.checked = !this.checked; });
        }
    </script>
	
	<h4>Работа со статусами строк заказа</h4>
	
	<table>
		<tr>
            <td>
                <input type="checkbox" style="margin-left: 10px;" name="check_all_statuses" onclick="ToggleAllStatuses()" />
            </td>
		
			<td>Номер заказа:</td>
			<td><asp:TextBox ID="_txtOrderID" runat="server"></asp:TextBox></td>
			<td colspan="5"><asp:Button ID="_btnGetOrderLines" runat="server" Text="Показать строки заказа" OnClick="_btnGetOrderLines_Click" /></td>
			<td>Новый статус</td>
			<td><asp:DropDownList ID="_ddlStatuses" runat="server"></asp:DropDownList></td>
			<td>Дата статуса</td>
			<td><asp:TextBox ID="_txtStatusDate" runat="server" CssClass="date" Columns="10" /></td>
			<td><asp:Button ID="_btnChangeStatus" runat="server" Text="Изменить статус" OnClick="_btnChangeStatus_Click" OnClientClick="return confirm('Вы уверены что хотите изменить статус строки?')" /></td>
		</tr>
	</table>
	
	<br />
	
	<asp:GridView ID="_gvOrderLines" runat="server" CssClass="gridview" AlternatingRowStyle-CssClass="even" AutoGenerateColumns="false"
		OnRowEditing="_gvOrderLines_RowEditing">
		<Columns>
			<asp:TemplateField HeaderText="Выбрать деталь">
				<ItemTemplate><asp:CheckBox id="chkStatus" runat="server"/></ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField HeaderText="ID заказа" DataField="OrderID" />
			<asp:BoundField HeaderText="ID строки" DataField="OrderLineID"/>
			<asp:TemplateField HeaderText="Статус строки">
				<ItemTemplate>
					<%# OrderLineStatusUtil.DisplayName( ((OrderLine)Container.DataItem).CurrentStatus ) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField HeaderText="Дата статуса" DataField="CurrentStatusDate" />
			<asp:BoundField HeaderText="Номер детали" DataField="PartNumber" />
			<asp:BoundField HeaderText="Производитель" DataField="Manufacturer" />
			<asp:BoundField HeaderText="ID поставщика" DataField="SupplierID" />
			<asp:BoundField HeaderText="Наименование детали" DataField="PartName" />
			<asp:BoundField HeaderText="Описание детали" DataField="PartDescription" />
			<asp:BoundField HeaderText="Количество" DataField="Qty" />
		</Columns>
	</asp:GridView>
	
</asp:Content>
