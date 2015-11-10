<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="NewCityList.aspx.cs"
Inherits="RmsAuto.Store.Web.NewCityList"
MasterPageFile="~/PageTwoColumns.Master" %>
<%@ Register src="News/NewsList.ascx" tagname="NewsList" tagprefix="uc1" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <style>
        ._CityFirstLetterStyle
        {
	        color:#295d89;
	        font-family: Tahoma;
	        font-size: 12px;
	        text-decoration:none;
            font-weight:bold;
        }
        ._hrStyle
        { 
            height:1px;
            color:#c0c0c0;
            background-color:#c0c0c0;
            margin:0px;
            display: block;
            -webkit-margin-before: 0.0em;
            -webkit-margin-after: 0.0em;
            -webkit-margin-start: auto;
            -webkit-margin-end: auto;
            border-style:solid;
            border-width: 0px;
        }        
        .ItemIsSelected
        {
            background-color:Red;
        }

        ._TableRegionCities
        {
           width:100%;
        }
        
        .ListCellStyle
        {
            border-style:none;
            vertical-align:top;
        }

        .TableCellStyle
        {
            border-style:none;
            vertical-align:top;
 
            border-left-style:solid;
            border-left-width:1px;
            border-left-color:#c0c0c0;
        }

        .ListStyle
        {
            width:90%;
            height:100%;
            border-style:none;
            vertical-align:top;

            font-family: Arial, Helvetica, sans-serif;
            color: #295d89;
            font-weight:normal;
            font-size:12px;
            margin-top:15px;
            margin-left:10px;
            background:transparent;
        }

        .TableStyle
        {
            width:90%;
            height:100%;
            border-style:none;
            vertical-align:top;
            
            font-family: Arial, Helvetica, sans-serif;
            color: #295d89;
            font-weight:normal;
            font-size:12px;
            margin-top:15px;
            margin-left:10px;
            background:transparent;
        }

        .ListElement
        {
           width:100%;
           height:100%;
           font-family: Arial, Helvetica, sans-serif;
           color: #295d89;
           font-weight:normal;
           font-size:12px;
           margin-top:15px;
           margin-bottom:15px;
           border-style:solid;
           border-color: #295d89;
           border-width:1px;
        }
       
        ._ContentMargin
        {
           margin-left:15px;
           margin-top:15px;
           margin-right:15px;
           margin-bottom:15px;
        }
        
        .RegionHeader
        {
            font-family: Arial, Helvetica, sans-serif;
            color: #295d89;
            font-weight:bolder;
            font-size:20px;
            margin-bottom:5px;
            margin-left:10px;
        }
       
       .ListElement option
       {
           font-family: Arial, Helvetica, sans-serif;
           color: #295d89;
           font-weight:normal;
           font-size:12px;
           margin-bottom:5px;
           border: single, 1px, #295d89;
           background:#EEEEEE;
           overflow:auto;
       }
       
       .ListElement option:hover
       {
            text-decoration:underline;
            background:White;
            cursor:pointer !important;
       }
       
        div a:hover {
	        text-decoration:underline;
        }
        
    </style>
    
    
     <asp:Table ID="Table1" runat="server" Width="100%">
     
         <asp:TableRow ID="TableRow1" runat="server">
         
            <asp:TableCell ID="TableCell1" runat="server"
            
                CssClass="ListCellStyle"
                style="width:20%;"
           
            >
                <div class="RegionHeader">Регионы:</div>
                <hr class="_hrStyle"/>
                
                <asp:Panel ID="PanelRegionList" runat="server" ScrollBars="Auto" style="margin-left:10px;margin-top:20px;">
                
                </asp:Panel> 
                
            </asp:TableCell>
             
            <asp:TableCell ID="TableCell2" runat="server"
            
               CssClass="TableCellStyle"
            >
            
                <div class="RegionHeader">Города:</div>
                <hr style="margin-left:3px;" class="_hrStyle"/>
                <asp:Table ID="TableCities" runat="server"
                
                CssClass="TableStyle"
                >
                    <asp:TableRow ID="TableRow2" runat="server" style="vertical-align:top;"
                    
                    >
                        <asp:TableCell ID="TableCell3" runat="server"/>
                        <asp:TableCell ID="TableCell4" runat="server"/>
                        <asp:TableCell ID="TableCell5" runat="server"/>
                        <asp:TableCell ID="TableCell6" runat="server"/>
                    </asp:TableRow>
                    
                </asp:Table>
                
            </asp:TableCell>
         </asp:TableRow>
     </asp:Table>
   
</asp:Content>