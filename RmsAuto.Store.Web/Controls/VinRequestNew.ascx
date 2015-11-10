<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VinRequestNew.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.VinRequestNew" %>
<%@ Register src="~/Controls/GarageCarEdit.ascx" tagname="GarageCarEdit" tagprefix="uc3" %>
<%@ Register Namespace="RmsAuto.Common.Web.UI" Assembly="RmsAuto.Common" TagPrefix="rmsauto" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<div style="margin: 14px 0 0 0"
      onkeypress="javascript: if(event.keyCode==13) { <%=Page.GetPostBackClientEvent(btnAddRequest, String.Empty)%>; return false; }">
    <h2><%=global::Resources.Texts.VinRequests_NewRequest %></h2>
    
    <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td nowrap>
                <b><%=global::Resources.Texts.VinRequests_OrSelectFromGarage%></b>
            </td>
            <td width="100%" style="padding:0px 0px 0px 10px;">
                <asp:DropDownList ID="ddCarFromGarage" runat="server" style="width:100%"
                AutoPostBack="true" OnSelectedIndexChanged="ddCarFromGarageChanged" />
            </td>
        </tr>
    </table>
    <br />
    <span class="right_block"><a href="<%=UrlManager.GetGarageUrl() %>"><%=global::Resources.Texts.VinRequests_Garage %></a></span><br />

    <uc3:GarageCarEdit runat="server" ID="_garageCarEdit" />

    <script>
    
    function scrollToGrid()
    {
        document.getElementById('gridPlace').scrollIntoView(true);
    }
    
    function scrollToButtons()
    {
        document.getElementById('buttonsPlace').scrollIntoView(true);
    }

    </script>
    
    <a id="gridPlace"></a>
    <asp:GridView
            runat="server"
            ID="_gvLineItems"
            AutoGenerateColumns="false"
            OnRowEditing="gvLineItemsRowEditing"
            OnRowUpdating="gvLineItemsRowUpdating"
            OnRowDeleting="gvLineItemsRowDeleting"
            OnRowCancelingEdit="gvLineItemsRowEditCanceling" CssClass="list">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%=global::Resources.Texts.Name %>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("Name") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="_txtName" Text='<%# Eval("Name") %>' runat="server" ValidationGroup="GridViewGroup" MaxLength="100" /><br />
                    <asp:RequiredFieldValidator
                            ForeColor="Red"
                            runat="server"
                            ControlToValidate="_txtName"
                            ErrorMessage="<%$ Resources:Exceptions, EnterName %>"
                            ValidationGroup="GridViewGroup" Display="Dynamic" />
                            
                    <%--asp:RegularExpressionValidator
                            id="_revTxtName"
                            ForeColor="Red"
                            runat="server"
                            ControlToValidate="_txtName"                            
                            Type="String"
                            ValidationExpression=".{0,100}"
                            ErrorMessage="<%$ Resources:Exceptions, LengthIs300 %>"
                            Display="Dynamic" /--%>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%=global::Resources.Texts.Qty %>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("Qty")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="_txtQty" Text='<%# Eval("Qty") %>' runat="server" ValidationGroup="GridViewGroup" /><br/>
                    <asp:RequiredFieldValidator
                            ForeColor="Red"
                            runat="server"
                            ControlToValidate="_txtQty"
                            ErrorMessage="<%$ Resources:Exceptions, EnterQty %>"
                            ValidationGroup="GridViewGroup" Display="Dynamic" />
                    <asp:RangeValidator
                            runat="server"
                            ControlToValidate="_txtQty"
                            Type="Integer"
                            MinimumValue="1"
                            MaximumValue="65535"
                            ErrorMessage="<%$ Resources:Exceptions, BadQty %>"
                            ValidationGroup="GridViewGroup" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <%=global::Resources.Texts.Comment %>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("Description")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox TextMode="MultiLine"
                                    ID="_txtDescription"
                                    Text='<%# Eval("Description") %>'
                                    runat="server"
                                    MaxLength="300"
                                    Width="200px"
                                    Rows="3" />
                    
                    <%--asp:RegularExpressionValidator
                            id="_revTxtDescription"
                            ForeColor="Red"
                            runat="server"
                            ControlToValidate="_txtDescription"                            
                            Type="String"
                            ValidationExpression=".{0,300}"
                            ErrorMessage="<%$ Resources:Exceptions, LengthIs300 %>"
                            Display="Dynamic" /--%>
                </EditItemTemplate>
            </asp:TemplateField>
            <rmsauto:ExtendedCommandField
                    ButtonType="Link"
                    HeaderText="<%$ Resources:Texts, Actions %>"
                    UpdateText="<%$ Resources:Texts, Save %>"
                    DeleteText="<%$ Resources:Texts, Delete %>"
                    CancelText="<%$ Resources:Texts, Cancel %>"
                    EditText="<%$ Resources:Texts, Edit %>"
                    ShowCancelButton="true"
                    ShowDeleteButton="true"
                    ShowEditButton="true"
                    ShowInsertButton="false"
                    ShowSelectButton="false"
                    ValidationGroup="GridViewGroup"
                    DeleteConfirmationText="<%$ Resources:Texts, DeleteConfirmation %>" />
        </Columns>
    </asp:GridView>
    
    <asp:Label ForeColor="Red" runat="server" EnableViewState="false" ID="_lblNoItems" Visible="false" Text="<%$ Resources:Exceptions, VinRequest_NoItems %>" />
    <asp:Label ForeColor="Red" runat="server" EnableViewState="false" ID="_lblEditingItems" Visible="false" Text="<%$ Resources:Exceptions, VinRequest_ItemsEditing %>" />
    
</div>
<br />
<a id="buttonsPlace"></a>
<asp:ImageButton ID="_btnAddItem" runat="server" OnClick="btnAddItemClick" ImageUrl="<%$ Resources:ImagesURL, add_btn %>" CausesValidation="false" />
<asp:ImageButton ID="btnAddRequest" runat="server" OnClick="btnAddRequestClick" ImageUrl="<%$ Resources:ImagesURL, send_btn %>" />
