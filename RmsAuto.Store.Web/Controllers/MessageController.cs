using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Messages;

namespace RmsAuto.Store.Web.Controllers
{
    public class MessageController : Controller
    {
        private IMessageRepository _repository;
        public MessageController()
        {

        }

        public MessageController(IMessageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet, ActionName("SellerMessage")]
        public ViewResult Index()
        {
            return View();
        }

        [ActionName("Send")]
        public ActionResult Send(string InternalFranchName, string Title, string message)
        {
            var id =
                AcctgRefCatalog.RmsEmployees.Items.Where(x => x.InternalFranchName == InternalFranchName)
                    .First()
                    .EmployeeId;
                 _repository.InsertMessage(0, Convert.ToInt32(id), message);
            RedirectToAction("Sent");
            return View("CloseMessage");
        }

        [ActionName("Sent")]
        public ViewResult Sent()
        {
            return View("CloseMessage");
        }
    }
}