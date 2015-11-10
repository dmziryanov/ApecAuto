<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="DetailBalance.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.DetailBalance" %>

<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
    <uc3:TextItemControl runat="server" ID="TextBlock" ShowHeader="true" TextItemID="AlertConfigHint" />
    <script type="text/javascript">
        $(function() {
            $('.date').datepicker();
        });
	</script>
    
		<asp:MultiView ID="mvRequests" runat="server" ActiveViewIndex="0">
			<asp:View ID="vBalanceDetails" runat="server">
				<div class="tab_menu">
					<div class="on"><asp:LinkButton ID="LinkButton1" runat="server" Text="<%$ Resources:Requests, BalanceRequest %>" CommandName="SwitchViewByID" CommandArgument="vBalanceDetails" /></div>
					<div><asp:LinkButton ID="LinkButton2" runat="server" Text="<%$ Resources:Requests, StatusRequest %>" CommandName="SwitchViewByID" CommandArgument="vNonDeliveryDetails" /></div>
				</div>
				<h1><%= global::Resources.Requests.BalanceRequest %></h1>
				<asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
					<asp:View ID="vMainForm" runat="server">
						<p><img style="float:left; margin-right: 6px;" src="../images/info.png" alt="" /></p><p><strong><%= global::Resources.Requests.BalanceRequestInfo %></strong></p>
						<p>
							<asp:Literal ID="lNeedPeriod" runat="server" Text="<%$ Resources:Requests, BalancePeriodInfo %>" />
						</p>
						<p>
							<%=global::Resources.Texts.AC_From %>:
							<asp:TextBox runat="server" ID="dpStart" CssClass="date" Columns="10" 
								meta:resourcekey="dpStartResource1" />
							<asp:RequiredFieldValidator ID="rfvDpStart" runat="server" ErrorMessage="*" Display="Dynamic"
								ToolTip="Поле не заполнено" ValidationGroup="DateTime" 
								ControlToValidate="dpStart" meta:resourcekey="rfvDpStartResource1" />
							<%=global::Resources.Texts.AC_To %>:
							<asp:TextBox runat="server" ID="dpEnd" CssClass="date" Columns="10" 
								meta:resourcekey="dpEndResource1" />
                            <span class="info" title="<%$ Resources:ValidatorsMessages, InvalidDateFormat %>" runat="server"></span>
							&nbsp;
							<asp:RequiredFieldValidator ID="rfvDpEnd" runat="server" ErrorMessage="*" Display="Dynamic"
								ToolTip="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" ValidationGroup="DateTime" 
								ControlToValidate="dpEnd" />
							<br />
							<asp:RegularExpressionValidator ID="revDpStart" runat="server" Display="Dynamic"
								ValidationExpression="^((0[1-9])|([12]\d)|(3[01])).((0[1-9])|(1[0-2])).20\d{2}$" ValidationGroup="DateTime"
								ControlToValidate="dpStart" 
								ErrorMessage="<%$ Resources:ValidatorsMessages, IncorrectStartPeriod %>" />
							<asp:RegularExpressionValidator ID="revDpEnd" runat="server" Display="Dynamic" ValidationGroup="DateTime"
								ValidationExpression="^((0[1-9])|([12]\d)|(3[01])).((0[1-9])|(1[0-2])).20\d{2}$" ControlToValidate="dpEnd"
								ErrorMessage="<%$ Resources:ValidatorsMessages, IncorrectFinishPeriod %>" />
							<asp:Label ID="_errorMessage" runat="server" ForeColor="Red"></asp:Label>
						</p>
							
						<asp:Button runat="server" ID="btnSend" Text="Send" CssClass="button"
							OnClick="btnSend_Click" ValidationGroup="DateTime" 
							meta:resourcekey="btnSendResource1" />
						<br />
						<h3><%= global::Resources.Requests.EmailProcessingResults%></h3>
					</asp:View>
					<asp:View ID="vOK" runat="server">
						<%= global::Resources.Requests.RequestProceeded %><br />
						<h3><%= global::Resources.Requests.EmailResults%></h3>
					</asp:View>
					<asp:View ID="vError" runat="server">
						<%= global::Resources.Requests.RequestProcessingError %><br /><%= global::Resources.Requests.RequestTryAgain %>
					</asp:View>
				</asp:MultiView>
			</asp:View>
			
			<asp:View ID="vNonDeliveryDetails" runat="server">
				<div class="tab_menu">
					<div><asp:LinkButton ID="LinkButton3" runat="server" Text="<%$ Resources:Requests, BalanceRequest %>" CommandName="SwitchViewByID" CommandArgument="vBalanceDetails" /></div>
					<div class="on"><asp:LinkButton ID="LinkButton4" runat="server" Text="<%$ Resources:Requests, StatusRequest %>" CommandName="SwitchViewByID" CommandArgument="vNonDeliveryDetails" /></div>
				</div>
                <h1><%= global::Resources.Requests.StatusRequestFull %></h1>
					
				<asp:MultiView ID="mvNonDeliveryRequest" runat="server" ActiveViewIndex="0">
					<asp:View ID="vNonDeliveryMain" runat="server">
						<p><asp:Label ID="_errorNonDelivery" runat="server" ForeColor="Red" /></p>
						<p><img style="float:left; margin-right: 6px;" src="../images/info.png" alt="" /></p><p><strong><%= global::Resources.Requests.StatusRequestInfo %></strong></p>
							
                        <div class="feedback">
                            <ul>
                                <li>
                                    <div class="title"><%= global::Resources.Requests.EnterTheOrderNo %>:</div>
                                    <asp:TextBox ID="txtOrderNum" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="rfvOrderNum" runat="server" Display="Dynamic"
										ErrorMessage="<%$ Resources:ValidatorsMessages, FieldOrderNumEmpty %>" ValidationGroup="vgroup" 
										ControlToValidate="txtOrderNum" />
									<asp:RegularExpressionValidator ID="revOrderNum" runat="server" Display="Dynamic"
										ValidationExpression="^\d+$" ValidationGroup="vgroup"
										ControlToValidate="txtOrderNum"
										ErrorMessage="<%$ Resources:ValidatorsMessages, OrderNumInvalid %>" />
                                </li>
                                <li>
                                    <div class="title"><%= global::Resources.Requests.EnterThePartNo %>:</div>
                                    <asp:TextBox ID="txtPartNumber" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="rfvPartNumber" runat="server" Display="Dynamic"
										ErrorMessage="<%$ Resources:ValidatorsMessages, FieldArticulEmpty %>" ValidationGroup="vgroup"
										ControlToValidate="txtPartNumber" />
                                </li>
                                <li>
                                    <asp:Button runat="server" ID="ibSendNonDeliveryRequest" Text="Send" OnClick="ibSendNonDeliveryRequest_Click" CssClass="button" ValidationGroup="vgroup" />
                                </li>
                            </ul>
                        </div>						
						<h3><%= global::Resources.Requests.YourRequestWillProcessed24 %><br /><%= global::Resources.Requests.EmailResults %></h3>
					</asp:View>
					<asp:View ID="vNonDeliverySuccess" runat="server">
						<%= global::Resources.Requests.RequestProceeded %><br />
						<h3><%= global::Resources.Requests.EmailResults%></h3>
						<p>
							<asp:LinkButton ID="lnkReturn" runat="server" OnClick="lnkReturn_Click">Оформить следующий запрос...</asp:LinkButton>
						</p>
					</asp:View>
					<asp:View ID="vNonDeliveryError" runat="server">
						<%= global::Resources.Requests.RequestProcessingError %><br /><%= global::Resources.Requests.RequestTryAgain %>
					</asp:View>
				</asp:MultiView>
			</asp:View>
		</asp:MultiView>
		
</asp:Content>
