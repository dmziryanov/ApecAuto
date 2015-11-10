<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineOptions.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLineOptions" %>

<td><asp:CheckBox ID="_chkStrictlyThisNumber" runat="server" Checked="false" /></td>
<td runat="server" id="_vinRow">
<asp:CheckBox ID="_chkShowCars" runat="server" Checked="false" Text="<%$ Resources:Texts, Check %>" /><br />
<asp:Label runat="server" ID="_labelSelectedCar" />
<asp:DropDownList ID="_ddCarFromGarage" runat="server" style="width:130px" />

<script type="text/javascript">
$( function(){
	var chb = $('#<%=_chkShowCars.ClientID %>');
	var box = $('#<%=_ddCarFromGarage.ClientID %>');
	//var cc = <asp:Literal ID="_litCarsNum" runat="server">0</asp:Literal>;

	chb.click(function()
	            { 
		            if( this.checked ) { box.show(); }
		            else { box.hide(); }
		        });
		
	if( !chb.get(0).checked )
	{
		box.hide();
	}
	
});
</script>

</td>

