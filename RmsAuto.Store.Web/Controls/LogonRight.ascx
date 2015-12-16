<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogonRight.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.LogonRight" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
  
        <% if (!Page.User.Identity.IsAuthenticated) { %>

		<div class="panel panel-primary" onkeypress="javascript: if(event.keyCode==13) { <%=Page.GetPostBackClientEvent(btnLogin, String.Empty)%>; return false; }">
		    <div class="panel-heading"> <span class="icon"><img src="/images/lock.png" width="12" height="16" alt="/"></span> <%=global::Resources.Texts.MembersLogin %> </div>
			<!--end .title -->
			<div class="panel-body">
				<div class="input">
					<input class="form-control" id="txtLogin" runat="server" placeholder="<%$ Resources:Texts, EnterYourName %>" type="text" maxlength="20"/>
				    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*"
                            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyLogin %>" ControlToValidate="txtLogin" ValidationGroup="LogonRightGroup"></asp:RequiredFieldValidator>
				</div>
				<!--end .input -->
				<div class="input">
				    <input class="form-control" id="txtPassword" runat="server" placeholder="<%$ Resources:Texts, EnterYourPassword %>" type="password" maxlength="10"/>
				    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*"
                            ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyPassword %>" ControlToValidate="txtPassword" ValidationGroup="LogonRightGroup"></asp:RequiredFieldValidator>
				</div>
				<!--end .input -->
				<asp:Label ID="errMessage" runat="server" EnableViewState="false" CssClass="error" />
				<a href="<%=UrlManager.GetPasswordRecoveryUrl()%>"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Texts, ForgotPassword %>"></asp:Literal></a>
				<asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Class="btn btn-primary btn-sm" Text="<%$ Resources:Texts, Login %>" ValidationGroup="LogonRightGroup" />
			</div>
			<!--end .in -->
        </div>

    
   		   		
		<!--end .block -->

   		<% } else { %>
   	    <%--<asp:HyperLink ID="_btnViewCabinetLink" runat="server"><img id="Img2" src="<%$ Resources:ImagesURL, reg_img %>" alt="Личный кабинет" runat="server" width="217" height="49" border="0"></asp:HyperLink><div class="reg" style="min-height:50px;padding-bottom:10px;"><span class="right_block" style="margin:13px 10px 0px 0px;">
   	    <asp:LinkButton Text="Выйти" runat="server" OnClick="OnLogOff" CausesValidation="false" />
   	    </span>
   			<div style="margin:0px 0px 0px 10px;width:132px;">
   				<center>
   					<br /><%=Server.HtmlEncode( CurrentClientName ) %>
   					<br /><%=Server.HtmlEncode( CurrentClientID ) %>
   				</center>
   			</div>
		</div>--%>
   		<% } %>