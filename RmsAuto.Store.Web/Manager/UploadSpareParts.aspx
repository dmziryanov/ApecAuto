<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="UploadSpareParts.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.UploadSpareParts" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
        <p>
        <%= global::Resources.Texts.UploadSparepartsInfo.Replace(";","<br>") %>
            </p>
     <%= global::Resources.Texts.RegionInfo %>: <%= SiteContext.Current.InternalFranchName %>
   
    <h3>Загрузка номенклатуры</h3>
	<p>
	<a href='<%= this.GetTemplateUrl() %>' Class="btn btn-primary btn-sm">File upload example</a>
	<br/>
	<br/>
	<asp:FileUpload ID="fuMain" runat="server" />&nbsp; 
	<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" Class="btn btn-primary btn-sm"/></p>
	<p><span style="color:Green"><asp:Literal ID="lPreloadInfo" runat="server"></asp:Literal>&nbsp;</span></p>
    <div class="tab-text">
        <div class="t-hold">
         
        </div>
    </div>
	
	<p runat="server" id="pButtons">
	</p>
	<p><asp:Literal ID="lSummaryInfo" runat="server"></asp:Literal></p>
</asp:Content>
