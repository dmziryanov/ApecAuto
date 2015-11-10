<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaleReportTable.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.SaleReportTable" %>
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
        
                     <asp:TemplateField HeaderText="Date" SortExpression="StatusChangeTime" >
                        <ItemTemplate>
                            <%# Eval("StatusChangeTime")%>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                     <asp:TemplateField HeaderText="Customer" SortExpression="ClientName" >
                        <ItemTemplate>
                            <%# Eval("ClientNameDecor")%>
                        </ItemTemplate>
                    </asp:TemplateField>
        
        
                    <asp:TemplateField HeaderText="Order No">
                        <ItemTemplate>
                            <%# Eval("OrderId") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Brand">
                        <ItemTemplate>
                            <%# Eval("Manufacturer") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Part No">
                        <ItemTemplate>
                            <%# Eval("PartNumber") %>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <%# Eval("PartName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <%# Eval("Qty") %>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Supplier">
                        <ItemTemplate>
                            <%# Eval("SupplierName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Purchase price" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("SupplyPrice")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Sale price" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("UnitPrice")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Total amount, USD" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("Total")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="Profit (usd)" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <span class="blue"><%# Eval("ProfitSumm")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
        
                    <asp:TemplateField HeaderText="commercial viability (%)" ItemStyle-CssClass="price">
                        <ItemTemplate>
                            <%# Eval("ProfitPercent")%>
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

