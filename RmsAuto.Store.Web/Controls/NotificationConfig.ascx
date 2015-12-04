<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotificationConfig.ascx.cs"
    Inherits="RmsAuto.Store.Web.Controls.NotificationConfig" %>

<script type="text/javascript">

	function ToggleAllStatuses() {
		var checkAllStatuses = $('input[name=check_all_statuses]');
		var statusCheckBoxes = $('input[name*=chkStatus]'); //$('input[name=status_id]');
		var checked = checkAllStatuses.get(0).checked;

		$.each(statusCheckBoxes, function() { this.checked = checked; });
	}

	function RefreshCheckBoxes() {
		var checkAllStatuses = $('input[name=check_all_statuses]');
		var statusCheckBoxes = $('input[name*=chkStatus]'); //$('input[name=status_id]');

		var isAllChecked = true;
		$.each(statusCheckBoxes, function() { if (!this.checked) isAllChecked = false; });

		if (checkAllStatuses.length != 0) checkAllStatuses.get(0).checked = isAllChecked;
	}

	$(function() {
		RefreshCheckBoxes();
		RefreshPeriod();
	});

	function RefreshPeriod() {
		var divPeriod = $('div[id=div_period]');
		var notificationFrequencyValue = $('#<%=ddlNotificationFrequency.ClientID%> option:selected').val();
		if (notificationFrequencyValue == "0") {
			divPeriod.attr({ style: "float:left; display:none;" });
		}
		else {
			divPeriod.attr({ style: "float:left; display:block;" });
		}
	}

	function GatherStatuses() {
		var hf = document.getElementById('<%=hfSelectedStatuses.ClientID%>');
		hf.value = ''; //на всякий случай
		var statusCheckBoxes = $('input[name*=chkStatus]');
		$.each(statusCheckBoxes, function() {
			if (this.checked) { hf.value += this.value + ';'; }
		});
		if (hf.value.length > 0) { hf.value = hf.value.substring(0, hf.value.length - 1); }
	}

</script>

<div>
    <div style="float: left; margin-right: 10px;">
        <asp:Literal ID="lFrSend" runat="server" Text="Периодичность рассылки:" 
			meta:resourcekey="lFrSendResource1" />
        <asp:DropDownList runat="server" ID="ddlNotificationFrequency" 
			onchange="RefreshPeriod()">
            <asp:ListItem Text="Немедленно" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
            <asp:ListItem Text="Раз в сутки" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div id="div_period" style="float: left; display: none;">
        <asp:Literal ID="lPeriod" runat="server" Text="Период:" 
			meta:resourcekey="lPeriodResource1" />
        <asp:DropDownList runat="server" ID="ddlPeriod">
            <asp:ListItem Text="09:00 - 10:00" Value="9"></asp:ListItem>
            <asp:ListItem Text="10:00 - 11:00" Value="10"></asp:ListItem>
            <asp:ListItem Text="11:00 - 12:00" Value="11"></asp:ListItem>
            <asp:ListItem Text="12:00 - 13:00" Value="12"></asp:ListItem>
            <asp:ListItem Text="13:00 - 14:00" Value="13"></asp:ListItem>
            <asp:ListItem Text="14:00 - 15:00" Value="14"></asp:ListItem>
            <asp:ListItem Text="15:00 - 16:00" Value="15"></asp:ListItem>
            <asp:ListItem Text="16:00 - 17:00" Value="16"></asp:ListItem>
            <asp:ListItem Text="17:00 - 18:00" Value="17"></asp:ListItem>
            <asp:ListItem Text="18:00 - 19:00" Value="18"></asp:ListItem>
            <asp:ListItem Text="19:00 - 20:00" Value="19"></asp:ListItem>
            <asp:ListItem Text="20:00 - 21:00" Value="20"></asp:ListItem>
            <asp:ListItem Text="03:00 - 04:00" Value="3"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="clearfloat"></div>
    <br />
    <asp:ListView ID="_listView" runat="server">
        <LayoutTemplate>
            <div class="tab-text">
                <div class="t-hold">
                    <table>
                        <tr>
                            <th style="width: 20px">
                                <input type="checkbox" name="check_all_statuses" onclick="ToggleAllStatuses()" />
                            </th>
                            <th>
                                <asp:Literal ID="lSendStatuses" runat="server" Text="Статусы к рассылке:" 
							        meta:resourcekey="lSendStatusesResource1" />
                            </th>
                        </tr>
                        <tr runat="server" id="itemPlaceHolder" />
                    </table>
                </div>
            </div>
            
        </LayoutTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td style="width: 20px" runat="server">
                    <input id="chkStatus" runat="server" type="checkbox" checked="checked"
                        value='<%# Eval("Key") %>' onclick="RefreshCheckBoxes()" />
                </td>
                <td runat="server">
                    <%#Eval("Value")%>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Literal ID="lErrorLoadStatusesList" runat="server" 
							Text="ошибка загрузки списка статусов" 
							meta:resourcekey="lErrorLoadStatusesListResource1" />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
    <br />
    <div>
        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click"
            OnClientClick="GatherStatuses()" CssClass="button" Text="<%$ Resources:Texts, Save %>" />
    </div>
    <asp:HiddenField ID="hfSelectedStatuses" runat="server" />
</div>
