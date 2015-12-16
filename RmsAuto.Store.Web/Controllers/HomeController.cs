using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RmsAuto.Store.Web.Controllers
{
    public class HomeController : Controller
    {
   

        public HomeController()
        {

        }

   
        [HttpGet, ActionName("Index")]
        public ViewResult Index()
        {
            return View();
        }
    }
}