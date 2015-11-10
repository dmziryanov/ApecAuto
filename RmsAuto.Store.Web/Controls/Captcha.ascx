<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Captcha.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.Captcha" %>

    <script>
    
    function rnd()
    {
        return String((new Date()).getTime()).replace(/\D/gi,'');
    }
    
    function changeCapture()
    {
        $('#captureImageImg').attr('src', '<%=ResolveUrl("~/capture.ashx") %>?' + rnd());
    }
    
    </script>

    <img src="<%=ResolveUrl("~/capture.ashx") %>" width="231" height="50" id="captureImageImg" /><br />
    <asp:TextBox ID="_txtCapture" runat="server" Width="228px"></asp:TextBox>
    <span class="info" title="<%$ Resources:Texts, CaptchaTitle %>" runat="server"></span>
    <br />
    <a href="javascript:changeCapture();"><%=global::Resources.Texts.ChangeCapture %></a>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="_txtCapture" 
                                ErrorMessage="<%$ Resources:Exceptions, CaptchaEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator>
    <asp:CustomValidator ID="_captureCustomValidator" runat="server"
                         ControlToValidate="_txtCapture"
                         ErrorMessage="<%$ Resources:Exceptions, CaptchaBad %>"
                         onservervalidate="_captureCustomValidator_ServerValidate">
    </asp:CustomValidator>