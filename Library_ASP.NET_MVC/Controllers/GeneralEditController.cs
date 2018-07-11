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
        private static List<object> obj;

        // GET: GeneralEdit
        public ActionResult Index()
        {
            if (TempData["AuthorRepository"] != null && TempData["PublisherRepository"] != null && TempData["BookRepository"] != null)
                obj = new List<object>() { TempData["AuthorRepository"], TempData["PublisherRepository"], TempData["BookRepository"] };

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
                if (formcollection["Authors"] as string == ListItemToManage[0])
                    toController = "AuthorManager";
                if (formcollection["Publishers"] as string == ListItemToManage[1])
                    toController = "PublisherManager";
                if (formcollection["Books"] as string == ListItemToManage[2])
                    toController = "BookManager";
            }

            TempData["AuthorRepository"] = obj[0];
            TempData["PublisherRepository"] = obj[1];
            TempData["BookRepository"] = obj[2];

            return RedirectToAction("Index", toController);
        }
    }
}