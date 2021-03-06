﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.InvisibleManufacturers.List"
    MasterPageFile="~/Site.master" %>

<%@ Import Namespace="RmsAuto.Store.Adm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" runat="Server">

    <script src="../scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <h2>TecDocs - видимость производителей и моделей (отмеченные производители/модели будут скрыты)</h2>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>

        <script type="text/javascript">
            $(document).ready(
                function() {
                    $("input[id$='CheckBox']").live("click", function() {  __doPostBack('<%= UpdatePanel1.ClientID %>'); })
                }
         );
         
    </script>
            <asp:TreeView ID="TreeView1" runat="server" AutoGenerateDataBindings="False" OnTreeNodePopulate="TreeView1_TreeNodePopulate"
                ShowCheckBoxes="All" ExpandDepth="0" ShowLines="False" 
                OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged">
            </asp:TreeView>
            <asp:UpdateProgress
                ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
		        <div id="overlay"></div><div id="loader"></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="TreeNodePopulate" />
        </Triggers>
        
    </asp:UpdatePanel>
    
</asp:Content>
