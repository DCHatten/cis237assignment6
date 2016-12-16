using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cis237assignment6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "The history of Willie's Wine Shop:";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Questions? Comments? Complaints?  Contact us!";

            return View();
        }
    }
}