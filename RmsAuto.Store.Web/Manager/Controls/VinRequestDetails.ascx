<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VinRequestDetails.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.VinRequestDetails" %>
<%@ Register src="../../Controls/GarageCarDetails.ascx" tagname="GarageCarDetails" tagprefix="uc1" %>
<%@ Register Assembly="RmsAuto.Common" Namespace="RmsAuto.Common.Web.UI" TagPrefix="rmsauto" %>

<table class="info" cellpadding="0" cellspacing="0" width="100%">
<tr>
<th>Дата запроса</th>
<td><asp:Literal runat="server" ID="_requestDateLabel" /></td>
</tr>
</table>
<br />

<asp:Panel runat="server" ID="_replyPanel">
<h3>Ответ на запрос по VIN</h3>
	<asp:Label runat="server" ID="_errorLabel" class="error" EnableViewState="false" />
	<rmsauto:ExtendedListView
	            DataKeyNames="Id"
	            runat="server"
	            ID="_listView"
	            OnItemEditing="_listViewItemEditing"
	            OnItemCanceling="_listViewItemCanceling"
	            OnItemUpdating="_listViewItemUpdating">
		<LayoutTemplate>
			<table id="itemPlaceholderContainer" runat="server" cellspacing="0" cellpadding="0" class="list">
				<tr runat="server">
                    <th>
                        <%=global::Resources.Texts.Name %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.Qty %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.ClientCommentary %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.Manufacturer %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.Article %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.OriginalPartNumber %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.DeliveryPeriod %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.PricePerUnit %>
                    </th>
                    <th>
                        <%=global::Resources.Texts.ManagerComment %>
                    </th>
				    <th id="th1" runat="server" visible="<%# CanEditAnswer %>" class="empty">
				        <%=global::Resources.Texts.Actions %>
				    </th>
				</tr>
				<tr runat="server" id="itemPlaceholder">
				</tr>
			</table>
		</LayoutTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<asp:Literal runat="server" ID="_litName" Text='<%# Eval("Name") %>' />
				</td>
				<td>
					<asp:Literal runat="server" ID="_litQty" Text='<%# Eval("Quantity")%>' />
				</td>
				<td>
					<%# Eval("Description") %>
				</td>
                <td>
                    <asp:Literal runat="server" ID="_litMfr" Text='<%# Eval("Manufacturer") %>' />
					<asp:Label ID="_litOnSendMfrError"
					            EnableViewState="false"
					            runat="server"
					            Visible='<%# CheckError(Container.DisplayIndex, "Mfr") %>'
					            Text="*"
					            ForeColor="Red" />
				</td>
				<td>
                    <asp:Literal runat="server" ID="_litPN" Text='<%# Eval("PartNumber") %>' />
					<asp:Label EnableViewState="false"
					            ID="_litOnSendPNError"
					            runat="server"
					            Visible='<%# CheckError(Container.DisplayIndex, "PartNumber") %>'
					            Text="*"
					            ForeColor="Red" />
                </td>
                <td>
                    <asp:Literal runat="server" ID="_litPNOriginal" Text='<%# Eval("PartNumberOriginal") %>' />
					<asp:Label EnableViewState="false"
					            ID="_litOnSendPNOriginalError"
					            runat="server"
					            Visible='<%# CheckError(Container.DisplayIndex, "PartNumberOriginal") %>'
					            Text="*"
					            ForeColor="Red" />
                </td>
                <td>
                    <asp:Literal runat="server" ID="_litDeliveryDays" Text='<%# Eval("DeliveryDays")%>' />
					<asp:Label EnableViewState="false"
					            ID="_litOnSendDeliveryDaysError"
					            runat="server"
					            Visible='<%# CheckError(Container.DisplayIndex, "DeliveryDays") %>'
					            Text="*"
					            ForeColor="Red" />
                </td>
                <td>
                    <asp:Literal runat="server" ID="_litPricePerUnit" Text='<%# Eval("PricePerUnit")%>' />
					<asp:Label EnableViewState="false"
					            ID="_litOnSendNoPriceError"
					            runat="server"
					            Visible='<%# CheckError(Container.DisplayIndex, "PricePerUnit") %>'
					            Text="*"
					            ForeColor="Red" />
                </td>
				<td>
					<%# Eval("ManagerComment") %>
				</td>
				<td id="td1" runat="server" visible='<%#CanEditAnswer%>'>
					<asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="Изменить" />
				</td>
			</tr>
		</ItemTemplate>
		<EditItemTemplate>
			<tr style="background-color:white">
				<td>
				    <asp:TextBox ID="_txtName" runat="server" Text='<%# Eval("Name") %>' MaxLength="100" />
                    <asp:RequiredFieldValidator
                            ForeColor="Red"
                            runat="server"
                            Display="Dynamic"
                            ControlToValidate="_txtName"
                            ErrorMessage="<%$ Resources:Exceptions, EnterText %>"
                            ValidationGroup="ListViewGroup" />
				</td>
				<td>
					<asp:TextBox ID="_txtQuantity" runat="server" Text='<%# Eval("Quantity")%>' ValidationGroup="ListViewGroup" Width="25px" />
                    <asp:RequiredFieldValidator
                            ForeColor="Red"
                            runat="server"
                            Display="Dynamic"
                            ControlToValidate="_txtQuantity"
                            ErrorMessage="<%$ Resources:Exceptions, EnterText %>"
                            ValidationGroup="ListViewGroup" />
                    <asp:RangeValidator
					        runat="server"
					        ControlToValidate="_txtQuantity"
					        Type="Integer"
					        MinimumValue="1"
					        MaximumValue="65535"
					        ErrorMessage="<%$ Resources:Exceptions, BadQty %>"
					        ValidationGroup="ListViewGroup" />
				</td>
				<td>
					<%# Eval("Description") %>
				</td>
                <td>
                    <asp:TextBox ID="_txtManufacturer" runat="server" Text='<%# Eval("Manufacturer")%>'  Width="60px"/>
				</td>
				<td>
                    <asp:TextBox ID="_txtPartNumber" runat="server" Text='<%# Eval("PartNumber")%>'  Width="50px"/>
                </td>
                <td>
                    <asp:TextBox ID="_txtPartNumberOriginal" runat="server" Text='<%# Eval("PartNumberOriginal")%>'  Width="50px"/>
                </td>
                <td>
                    <asp:TextBox ID="_txtDeliveryDays" ValidationGroup="ListViewGroup" runat="server" Text='<%# Eval("DeliveryDays")%>' Width="50px" />
                </td>
                <td>
                    <asp:TextBox ID="_txtPricePerUnit" ValidationGroup="ListViewGroup" runat="server" Text='<%# Eval("PricePerUnit")%>' Width="50px" />
					<asp:RegularExpressionValidator
					        runat="server"
					        ControlToValidate="_txtPricePerUnit"
					        ValidationExpression="^\d+(\,\d\d)?$"
					        ErrorMessage="<%$ Resources:Exceptions, BadPrice %>"
					        ValidationGroup="ListViewGroup" />
			    </td>
				<td>
					<asp:TextBox ValidationGroup="ListViewGroup" ID="_txtManagerComment" runat="server" Text='<%# Eval("ManagerComment") %>' TextMode="MultiLine" Width="150px" Rows="3" />
                </td>
				<td>
					<asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="Сохранить" ValidationGroup="ListViewGroup" />
					<asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Отмена" />
				</td>
			</tr>
		</EditItemTemplate>
	</rmsauto:ExtendedListView>
	
	<asp:CustomValidator ID="_listValidator" runat="server" OnServerValidate="_listOnValidate" ErrorMessage="Заполните все необходимые поля перед отправлением запроса клиенту" ValidationGroup="OnSendGroup" />

	<span id="_sSendBLock" runat="server" class="link_block"><asp:LinkButton runat="server" ID="_sendAnswerButton" 
		onclick="_sendAnswerButton_Click" OnClientClick="return confirm('Отправить ответ?');">Отправить ответ</asp:LinkButton></span> 
	<span class="link_block"><asp:HyperLink runat="server" ID="_backLink">Вернуться к списку запросов</asp:HyperLink></span><br /><br />
</asp:Panel>

<br />
<h3>Параметры машины</h3>
<uc1:GarageCarDetails ID="_garageCarDetails" runat="server" CarViewType="OnlyPresentFields" />

