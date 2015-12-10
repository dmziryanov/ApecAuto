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
			        .replace('{pn}', encodeURIComponent(pn))
			        .replace('{sc}', 1); // document.getElementById('=cbSearchCrosses.ClientID %>').checked ? '1' : '0' );
            
				return true;
			}
			$('<%=txtPartNumber.ClientID %>').insertAfter('<%= global::Resources.Texts.EnterPartNumber %>');
    		
    	}
        catch (ex)
        {
            alert(ex.message);
        }
        return false;
    }
	
</script>
<div class="block search" onkeypress="if(event.keyCode==13) { doSearch(); return false; }">
	<div class="lefttitle"><%= global::Resources.Texts.SparePartsSearch %> </div>
	<!--end .title -->
	<div class="in">
        <input style="border: none; margin-top: 4px;"  id="txtPartNumber" runat="server" placeholder="<%$ Resources:Texts, ByPartNo %>" type="text"/>&nbsp
       <span onclick="doSearch()" class="glyphicon glyphicon-search" aria-hidden="true"></span>
	</div>
</div>
<input type="checkbox" id="cbSearchCrosses" Visible="false" runat="server"/>&nbsp
<div id="partNumberError" class="error" Visible="false" style="display:none" runat="server"></div>



