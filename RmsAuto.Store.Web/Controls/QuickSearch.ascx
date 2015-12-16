<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickSearch.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.QuickSearch" %>

<%@ Register Assembly="RmsAuto.Store.Web" Namespace="RmsAuto.Store.Web.Manager.Store" TagPrefix="cc1" %>

<script type="text/javascript">

    function doSearch() {
        try {
            var pn = $.trim(document.getElementById('<%=txtPartNumber.ClientID %>').value);
            if (pn.length > 0) {
                var urlPattern = '<%=GetSearchManufacturersUrlPattern()%>';

                document.location.href = urlPattern
			        .replace('{pn}', encodeURIComponent(pn))
			        .replace('{sc}', 1); // document.getElementById('=cbSearchCrosses.ClientID %>').checked ? '1' : '0' );

                return true;
            }
            $('<%=txtPartNumber.ClientID %>').insertAfter('<%= global::Resources.Texts.EnterPartNumber %>');

        }
        catch (ex) {
            alert(ex.message);
        }
        return false;
    }

</script>

<div class="form-group">
    <div style="float: left">
        <%= global::Resources.Texts.SparePartsSearch %>&nbsp;
    </div>
    <input style="display: inline-block; width: 233px; float: left" onkeypress="if(event.keyCode==13) { doSearch(); return false; }" id="txtPartNumber" runat="server" placeholder="<%$ Resources:Texts, ByPartNo %>" type="text" name="search" class="form-control input-sm" />
    <div class="input-group-btn" style="float: left">
        <span onclick="doSearch()" class="btn btn-sm btn-primary" type="submit">
            <%= global::Resources.Texts.Search %>
        </span>
    </div>
</div>
<input type="checkbox" id="cbSearchCrosses" visible="false" runat="server" />&nbsp
<div id="partNumberError" class="error" visible="false" style="display: none" runat="server"></div>



