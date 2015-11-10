<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BannerControl.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Banners.BannerControl" %>

<script type="text/javascript" language="javascript" >
    var <%=ClientID %>_IntervalID = 0;
    $(document).ready(function() {
        $("a[id*='<%=ClientID %>'][class='banner']").hide();
        var bannerlength = $("a[id*='<%=ClientID %>'][class='banner']").length;
       

        var f = function() {
            var cur = parseInt($("input[id*='<%=ClientID %>'][class='hfCurrentBannerIndex']").val());
            $("a[id*='<%=ClientID %>'][class='banner']").eq(cur).hide();
            if (bannerlength - 1 > cur) {
                cur = cur + 1;
            }
            else { cur = 0; }
            $("input[id*='<%=ClientID %>'][class='hfCurrentBannerIndex']").val(cur);
            $("a[id*='<%=ClientID %>'][class='banner']").eq(cur).show();
            //$(".banner").eq(cur).show("slide", { direction: 'left', distance: 300 }, 1000);
        }
        
        //Проблема в том, что в случае нескольких контролов надо почистить таймеры от других
        //clearInterval(IntervalID);
        //Читаем число милисекунд со страницы, а она из конфига
        <%=ClientID %>_IntervalID = setInterval(f, <%=NumOfmSec %>);


        $("a[id*='<%=ClientID %>'][class='banner']").eq(0).show();
        $(".ControlArrows").hide();


        $("div[id*='<%=ClientID %>'][class='bDiv']").hover(function() {
            $(".ControlArrows").show();
        });
        
        $(".ControlArrows").hover(function() {
            $(".ControlArrows").show();
        });

        $(".bDiv").mouseleave(function() {
                $(".ControlArrows").hide();
        });
        
        $(".ControlArrows").mouseleave(function() {
                $(".ControlArrows").hide();
        });

        function forwardclick(event) {
            clearInterval(<%=ClientID %>_IntervalID);
            var cur = parseInt($("input[id*='<%=ClientID %>'][class='hfCurrentBannerIndex']").val());
            $("a[id*='<%=ClientID %>'][class='banner']").eq(cur).hide();
            if (bannerlength - 1 > cur) {
                cur = cur + 1;
            }
            else { cur = 0; }
            $("input[id*='<%=ClientID %>'][class='hfCurrentBannerIndex']").val(cur);
            $("a[id*='<%=ClientID %>'][class='banner']").eq(cur).show();
           // $(".banner").eq(cur).show("slide", { direction: 'left', distance: 300 }, 1000); можно прикрутить анимацию
            event.preventDefault();
            return false;
        }

        function backclick(event) {
            clearInterval(<%=ClientID %>_IntervalID);
            var cur = parseInt($("input[id*='<%=ClientID %>'][class='hfCurrentBannerIndex']").val());
            $("a[id*='<%=ClientID %>'][class='banner']").eq(cur).hide();
            if (cur > 0) {
                cur = cur - 1;
            }
            else { cur = bannerlength - 1; }
            $("input[id*='<%=ClientID %>'][class='hfCurrentBannerIndex']").val(cur);
            $("a[id*='<%=ClientID %>'][class='banner']").eq(cur).show();
            event.preventDefault();
            return false;
        }

        $("#<%=ClientID %>_ArrowForward").mouseup(forwardclick);
        $("#<%=ClientID %>_ArrowBackward").mouseup(backclick);

    });

</script>

<asp:PlaceHolder runat="server" ID="_imagePlaceHolder">
   <input id = "hfCurrentBannerIndex" class="hfCurrentBannerIndex" type="hidden"  value="0" runat="server"   />
   <div id="bPlace" style="z-index:50;text-align:center; position: relative;">
   <asp:Repeater ID="Repeater1" runat="server">
       <ItemTemplate>
        <div class="bDiv" id="bd" runat="server" style="text-align:center" >
            <a ID="_url" class="banner" runat="server" href='<%# Eval("URL") %>'>
                <asp:Image runat="server" style="left: 0; top:0px" ID="_image" width=185 ImageUrl='<%# UrlManager.GetFileUrl(Convert.ToInt32(Eval("FileId"))) %>'/>
            </a> 
        </div>
        </ItemTemplate>
    </asp:Repeater> 
           
           <img class="ControlArrows ControlArrows-Left"  style="position:absolute; z-index:40; left:5%; left: 0; top:130px"  id="ArrowBackward" src="../../images/backward.png" runat="server" />
           <img class="ControlArrows ControlArrows-Right" style="position:absolute; z-index:40; left: 82%; top:130px" id="ArrowForward" src="../../images/forward.png" runat="server"  />  
      </div>
 </asp:PlaceHolder>
 
<asp:PlaceHolder runat="server" ID="_bodyPlaceHolder">


    <div>
        <asp:Literal runat="server" ID="_bodyLabel" />
    </div>
</asp:PlaceHolder>
    
<asp:PlaceHolder runat="server" ID="_fileUrlPlaceHolder">
    <a ID="_fileUrl" runat="server">
        <asp:Literal runat="server" ID="_fileLinkBodyLabel" />
    </a>
</asp:PlaceHolder>


