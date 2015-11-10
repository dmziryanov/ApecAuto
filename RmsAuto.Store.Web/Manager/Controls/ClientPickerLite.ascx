<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientPickerLite.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientPickerLite" EnableViewState="true"%>
<h1 runat="server" ID="CaptionHeader">Поиск клиентов в учетной системе</h1>

<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#filter").accordion({ collapsible: true });
        $('.date').datepicker();
    });
</script>
<div id="filter">
    <h3><a href="#"><%= global::Resources.Texts.Filter2 %></a></h3>
<div onkeypress="if(event.keyCode==13) { <%=Page.GetPostBackClientEvent(_btnSearchClient,string.Empty)%>; return false; }">
        <table style="width:auto;">
            <tr>
                <td><%= global::Resources.Texts.ClientsName %>:</td>
                <td><asp:TextBox id="_txtClientName" runat="server" Width="200"></asp:TextBox></td>
                <td>&nbsp;&nbsp;&nbsp;<%= global::Resources.Texts.MainPhone %>:</td>
                <td><asp:TextBox ID="_txtPhone" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <asp:PlaceHolder runat="server" id="AddParams">
                <tr>
                    <td><%= global::Resources.Texts.IsChecked %>:</td>
                    <td>
                        <asp:DropDownList ID="isChecked" runat="server" Width="208">
	                        <asp:ListItem Value="1" Text="<%$ Resources:Texts, Yes %>"></asp:ListItem>
	                        <asp:ListItem Value="0" Text="<%$ Resources:Texts, No %>"></asp:ListItem>
	                        <asp:ListItem Value="-1" Text="<%$ Resources:Texts, DontMatter %>" Selected="True"></asp:ListItem>
	                    </asp:DropDownList>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;<%= global::Resources.Texts.Manager %>:</td>
                    <td><asp:DropDownList runat="server" ID="_managerList" AutoPostBack="False" DataTextField="FullName" DataValueField="EmployeeID" Width="208"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <%= global::Resources.Texts.RegistrationDateFrom %>: <asp:TextBox class="date" ID="RegDateMin" runat="server" Width="80"></asp:TextBox>&nbsp;&nbsp;&nbsp;<%= global::Resources.Texts.DateTill %>: <asp:TextBox class="date" ID="RegDateMax" runat="server" Width="80"></asp:TextBox>
                    </td>
                </tr>
            </asp:PlaceHolder>
        </table>
        <asp:Button ID="_btnSearchClient" runat="server" Text="<%$ Resources:Texts, Find %>" OnClick="_btnSearchClient_Click" CssClass="button" />
		<asp:HyperLink ID="ClientBootstrap" CssClass='button' runat="server" Text="<%$ Resources:Texts, Download %>" Visible="false" />	   
<%--	Type of a client:
        <asp:DropDownList ID="_clientType" runat="server">
	    <asp:ListItem Value='2'>Wholesale</asp:ListItem>
	    <asp:ListItem Value='1'>Retail</asp:ListItem>
	    <asp:ListItem Value='0' Selected="True" >Any</asp:ListItem>
	                </asp:DropDownList>
	    <div >
        &nbsp
	    Автозаказ: <asp:DropDownList ID="isAutoOrder" runat="server">
	    <asp:ListItem Value='2'>Есть</asp:ListItem>
	    <asp:ListItem Value='1'>Нет</asp:ListItem>
	    <asp:ListItem Value='0' Selected="True" >Неважно</asp:ListItem>
	       </asp:DropDownList> --%>
	    
	    <%--value подставляется в массив знаков--%>
	        
	    <%--</div>--%>
	</div>
</div>
<div class="tab-text">
    <div class="t-hold">
        <div class="br"></div>
        <div class="bl"></div>
        <asp:GridView ID="ClientGridView" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" 
    onsorting="ClientGridView_Sorting" GridLines="None" 
    onrowcreated="ClientGridView_RowCreated" 
    onrowcommand="ClientGridView_RowCommand" 
    onrowdatabound="ClientGridView_RowDataBound" 
            onpageindexchanging="ClientGridView_PageIndexChanging" PagerStyle-CssClass="pager" >
    <Columns>
        <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:Texts, CustomersName %>" SortExpression="ClientName">
            <HeaderStyle CssClass="GrayTextStyle" />
        </asp:BoundField>
        <asp:BoundField DataField="MainPhone" HeaderText="<%$ Resources:Texts, ContactPhone %>"/>
        <asp:BoundField DataField="CreationTime" HeaderText="<%$ Resources:Texts, CreationDate %>" SortExpression="CreationTime" DataFormatString="<%$ Resources:Format, Date %>" />
        <asp:TemplateField HeaderText="<%$ Resources:Texts, Type %>" SortExpression="TradingVolume">
            <ItemTemplate>
                <%# (byte)Eval("TradingVolume") == 1 ? Resources.Texts.Wholesale : Resources.Texts.Retail %>
            </ItemTemplate>
        </asp:TemplateField>
           
         <asp:TemplateField HeaderText="<%$ Resources:Texts, IsChecked %>" SortExpression="IsChecked">
            <ItemTemplate>
                <%# (bool)Eval("IsChecked") ? Resources.Texts.Yes : Resources.Texts.No %>
            </ItemTemplate>
        </asp:TemplateField>

    <%--    <asp:TemplateField HeaderText="Автозаказ" SortExpression="IsChecked">
            <ItemTemplate>
                <%# (bool)Eval("IsAutoOrder") ? "Есть" : "Нет"%>
            </ItemTemplate>
        </asp:TemplateField>--%>
        
        <asp:TemplateField HeaderText="<%$ Resources:Texts, Manager %>" SortExpression="Manager">
            <ItemTemplate>
                <%# Eval("Manager") %>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="<%$ Resources:Texts, Options %>">
            <ItemTemplate>
                <asp:LinkButton ID="_lbAddToHandySet" runat="server" CommandArgument='<%# Eval("ClientID") %>' CommandName="AddToHandySet" Text="<%$ Resources:Texts, WorkWithClients %>"></asp:LinkButton>
                <asp:Label ID="_lblWarning" runat="server" CssClass="error" />
                <asp:Label ID="_lblNotActivated" Visible = "false" CssClass="SimpleGrayTextStyle" runat = "server" meta:resourcekey="NotAсtivatedAlert" Text="Client has not been activated"  />
            </ItemTemplate>
        </asp:TemplateField>
        
    </Columns>
</asp:GridView>
<% if (NoClientsFound) { %>
    No client is found upon your request<br />
    Try to change search criterion.
<% } %>
<% if (TooManyClientsFound) { %>
    Search criterions are too indecipherable<br />
    Please specificate search terms.

<% } %>
    </div>
</div>