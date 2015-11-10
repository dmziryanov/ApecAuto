<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FinancialReport.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.FinancialReport" %>

<div class="orders">
    <div class="tab-text">
        <div class="t-hold">
            <asp:GridView ID="ReportGrid" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" CssClass="table-big" 
                GridLines="None" onpageindexchanging="ClientGridView_PageIndexChanging" 
                onrowcreated="ClientGridView_RowCreated" onsorting="ClientGridView_Sorting" PagerStyle-CssClass="pager">
                <Columns>
                   <%-- <asp:BoundField DataField="dt" DataFormatString="{0:dd/MM/yyyy}"  HeaderText="Дата" SortExpression="dt"><HeaderStyle CssClass="GrayTextStyle" />
                    </asp:BoundField>
                    --%>
        
                     <asp:TemplateField HeaderText="Manager" SortExpression="Manager" >
                        <ItemTemplate>
                            <%# Eval("Manager")%>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                     <asp:TemplateField HeaderText="Client" SortExpression="ClientName"  >
                        <ItemTemplate>
                            <%# Eval("ClientNameDecor")%>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                      <asp:TemplateField HeaderText='Opening balance' ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("bClientSaldo") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Cash payment" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("CashPayments") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Non-cash payment" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("NonCashPayments") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Cash refund" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("CashReturn") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Non-cash refund" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("NonCashReturn")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Goods return" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <%# Eval("GoodsReturn") %>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Dispatch" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("Supply")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                     <asp:TemplateField HeaderText='Closing balance' ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("eClientSaldo") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="bottom">
            <asp:DropDownList runat="server" 
		        ID="_pageSizeBox" AutoPostBack="True" 
		        OnSelectedIndexChanged="_pageSizeBox_SelectedIndexChanged">
		        <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
		        <asp:ListItem Text="40" Value="40"></asp:ListItem>
		        <asp:ListItem Text="100" Value="100"></asp:ListItem>
		        <asp:ListItem Text="400" Value="400"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
</div>
