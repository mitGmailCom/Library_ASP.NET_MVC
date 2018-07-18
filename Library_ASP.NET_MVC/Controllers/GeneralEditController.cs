using Library_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_ASP.NET_MVC.Controllers
{
    public class GeneralEditController : Controller
    {
        private static List<string> ListItemToManage = new List<string>();

        // GET: GeneralEdit
        public ActionResult Index()
        {
            if (ListItemToManage.Count == 0)
            {
                ListItemToManage.AddRange(new string[] { "Authors", "Publishers", "Books" });
            }
            //SelectList selectList = new SelectList(ListItemToManage);
            ViewBag.ListItemToManage = ListItemToManage;
            return View();
        }


        [HttpPost]
        public ActionResult Index(FormCollection formcollection)
        {
            string toController = null;
            if (formcollection != null)
            {
                if (formcollection["radioBtnName"] as string == ListItemToManage[0])
                    toController = "AuthorManager";
                if (formcollection["radioBtnName"] as string == ListItemToManage[1])
                    toController = "PublisherManager";
                if (formcollection["radioBtnName"] as string == ListItemToManage[2])
                    toController = "BookManager";
            }
            return RedirectToAction("Index", toController);
        }
    }
}