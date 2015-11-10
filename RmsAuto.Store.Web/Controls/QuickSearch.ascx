<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickSearch.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.QuickSearch" %>

<%@ Register assembly="RmsAuto.Store.Web" namespace="RmsAuto.Store.Web.Manager.Store" tagprefix="cc1" %>

<script type="text/javascript">

    function doSearch()
    {
    	try
    	{
			var pn = $.trim( document.getElementById('<%=txtPartNumber.ClientID %>').value );
			if (pn.length > 0)
			{
				var urlPattern = '<%=GetSearchManufacturersUrlPattern()%>';
			
				document.location.href = urlPattern
					.replace( '{pn}', encodeURIComponent(pn) )
					.replace( '{sc}', document.getElementById('<%=cbSearchCrosses.ClientID %>').checked ? '1' : '0' );
            
				return true;
			}
			document.getElementById('<%=partNumberError.ClientID %>').innerHTML = '<%= global::Resources.Texts.EnterPartNumber %>';
    		document.getElementById('<%=partNumberError.ClientID %>').style.display = 'block';
    	}
        catch (ex)
        {
            alert(ex.message);
        }
        return false;
    }
	
</script>
<div class="block search" onkeypress="if(event.keyCode==13) { doSearch(); return false; }">
	<div class="title"> <span class="icon"><img src="/images/search.png" width="16" height="16" alt="/"></span> <%= global::Resources.Texts.SparePartsSearch %> </div>
	<!--end .title -->
	<div class="in">
		<div class="form">
		<div class="input">
			<input id="txtPartNumber" runat="server" placeholder="<%$ Resources:Texts, ByPartNo %>" type="text">
		</div>
		<div id="partNumberError" class="error" style="display:none" runat="server"></div>
		<label>
			<input type="checkbox" id="cbSearchCrosses" runat="server">
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Texts, SearchAnalogs %>" />
		</label>
		<input class="button" value="<%= global::Resources.Texts.Search %>" type="button" onclick="doSearch()"/>
		</div>
	</div>
	<!--end .in --> 
<%--	<cc1:FloatingSearchFormLink ID="_floatingSearchFormLink" runat="server"><asp:Literal ID="lFloatingSearch" runat="server" Text="Открепить" /></cc1:FloatingSearchFormLink>--%>
</div>



