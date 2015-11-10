<%@ Control   Language="C#" AutoEventWireup="true" CodeBehind="RMSPagerControl.ascx.cs" Inherits="Controls.RMSPagerControl" %>


<style type="text/css">

#pager .arrow_page_left {
    margin-right: 3px;
    padding-top: 2px;
}

#pager .arrow_page_right {
    margin-left: 3px;
    padding-top: 2px;
}

#pager .current {
    background:#4478a3; 
    color: white;
    padding-left:  6px;
    padding-right:  6px;
    margin-left:  2px;
    margin-right:  2px;
}


#pager .PagerItem {
    color: #6c6c6c;
    padding:5px;
    text-decoration: none;
    width: 12px;
    color: #6c6c6c;
}

</style>


<script type="text/javascript"  language="javascript">
   
    $(document).ready(function() {
        $("a[id*='<%=ClientID %>'][class='PagerItem']").click(function() {
        $("#<%=ClientID %>_hfCurrentPageIndex").val(this.innerHTML);
        });

        $("input[id*='<%=ClientID %>'][class='arrow_page_left']").click(function() {
        $("#<%=ClientID %>_hfCurrentPageIndex").val((parseInt($("#<%=ClientID %>_hfCurrentPageIndex").val()) - 1).toString());
        });

        $("input[id*='<%=ClientID %>'][class='arrow_page_right']").click(function() {

        $("#<%=ClientID %>_hfCurrentPageIndex").val((parseInt($("#<%=ClientID %>_hfCurrentPageIndex").val()) + 1).toString());
        });
    });
</script>
<asp:PlaceHolder ID="ControlPlaceHolder" runat="server">
<asp:HiddenField ID="hfCurrentPageIndex"  runat="server" Value="1" />
<div id="pager">
<asp:PlaceHolder ID="MainItemsPlaceHolder" runat="server"></asp:PlaceHolder>
</div>
</asp:PlaceHolder>











