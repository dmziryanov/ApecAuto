<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecDocPartsTree.ascx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecDocPartsTree" EnableViewState="false" %>

<script type="text/javascript">
        $(document).ready(function() {
            var treeitemid = getCookie('treeitem');
            if (treeitemid && document.getElementById(treeitemid)) {
                var treeitem = $('#' + treeitemid);
                treeitem.parents('li').children('a').click();
                treeitem.children('a').click();
            }

        });
        
        function collapse(id) {
            $('#' + id + " > ul li").toggleClass("invisibleNode");
            $('#' + id + " > a").toggleClass("collapsedNode").toggleClass("expandedNode");
            setCookie('treeitem', id);
        }

        function setCookie(name, value) {
            var expires = new Date();
            expires.setTime(expires.getTime() + 31536000000);
            document.cookie = name + "=" + value + "; expires=" + expires.toGMTString();
        }

        function getCookie(name) {
            var prefix = name + "=";
            var start = document.cookie.indexOf(prefix);
            if (start == -1)
                return "";
            var end = document.cookie.indexOf(";", start + prefix.length);
            if (end == -1)
                end = document.cookie.length;
            var value = document.cookie.substring(start + prefix.length, end);
            return unescape(value);
        }
</script>

<div class="tree" style="width:50%;margin-top:10px">
	<asp:Repeater runat="server" ID="_repeater" 
		onitemdatabound="_repeater_ItemDataBound" >
	<HeaderTemplate>
		<ul>
	</HeaderTemplate>
	<ItemTemplate>
		 <li id="<%#Eval("SearchTreeNodeID")%>" class="<%#Eval("ParentSearchTreeNodeID")!=null?"invisibleNode":""%>">
			<a href="<%# (int)Eval("Children.Count")==0 ? GetPartsUrl((int)Eval("SearchTreeNodeID")) : "#" %>" class="<%#(int)Eval("Children.Count")==0 ? "emptyCell" : "collapsedNode" %>" onclick="<%# (int)Eval("Children.Count")==0 ? "" : "collapse("+Eval("SearchTreeNodeID")+"); return false;" %>"><%#Server.HtmlEncode((string)Eval("Text"))%></a>
			<asp:PlaceHolder runat="server" ID="_childsPlaceHolder"></asp:PlaceHolder>
		 </li>
	</ItemTemplate>
	<FooterTemplate>
		</ul>
	</FooterTemplate>
	</asp:Repeater>
</div>