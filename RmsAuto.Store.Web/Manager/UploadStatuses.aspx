<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="UploadStatuses.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.UploadStatuses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h3>Status upload</h3>
	<p>
	<a href='<%= this.GetTemplateUrl() %>' Class="btn btn-primary btn-sm">File upload example</a>
	<br/>
	<br/>
	<asp:FileUpload ID="fuMain" runat="server" />&nbsp; 
	<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" Class="btn btn-primary btn-sm"/></p>
	<p><span style="color:Green"><asp:Literal ID="lPreloadInfo" runat="server"></asp:Literal>&nbsp;</span></p>
    <div class="tab-text">
        <div class="t-hold">
            <asp:Repeater ID="rptMain" runat="server">
		        <HeaderTemplate>
			        <table>
				        <tr>
					        <th>Supplier ID</th>
					        <th>Brand</th>
					        <th>Partnumber</th>
					        <th>Q-ty</th>
					        <th>OrderLine ID</th>
					        <th>Order ID</th>
					        <th>Current Status</th>
					        <th>New status</th>
				        </tr>
		        </HeaderTemplate>
		        <ItemTemplate>
			        <tr>
				        <td><%# Eval("SupplierID")%></td>
				        <td><%# Eval("Manufacturer")%></td>
				        <td><%# Eval("PartNumber")%></td>
				        <td><%# Eval("Qty")%></td>
				        <td><%# Eval("AcctgOrderLineID")%></td>
				        <td><%# Eval("OrderID")%></td>
				        <td><%# GetCurrentStatusName( Eval("CurrentStatus") )%></td>
				        <td><%# GetNewStatusName( (string)Eval("NewStatus") )%></td>
			        </tr>
		        </ItemTemplate>
		        <FooterTemplate>
			        </table>
		        </FooterTemplate>
	        </asp:Repeater>
        </div>
    </div>
	
	<p runat="server" id="pButtons">
		<span class="link_block"><asp:LinkButton ID="lbClear" runat="server" Class="btn btn-primary btn-sm" onclick="lbClear_Click">Repeat upload</asp:LinkButton></span>
		<span class="link_block"><asp:LinkButton ID="lbUpdate" runat="server" Class="btn btn-primary btn-sm" onclick="lbUpdate_Click">Refresh statuses</asp:LinkButton></span>
	</p>
	<p><asp:Literal ID="lSummaryInfo" runat="server"></asp:Literal></p>
</asp:Content>
