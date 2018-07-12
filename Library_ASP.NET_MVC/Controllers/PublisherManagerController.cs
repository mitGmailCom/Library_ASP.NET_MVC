using Library_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_ASP.NET_MVC.Controllers
{
    public class PublisherManagerController : Controller
    {
        private static List<Publisher> ListPublishers = new List<Publisher>();
        private static Publisher newPublish = new Publisher();
        private static string Name { get; set; } = null;
        private static int index = -1;

        // GET: Publisher
        public ActionResult Index()
        {
            ListPublishers.Clear();
            ListPublishers = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers.ToList();
            return View(ListPublishers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (newPublish != null)
            {
                if (TempData["PublisherRepository"] != null)
                {
                    (TempData["PublisherRepository"] as PublisherRepository).Create(newPublish);
                    newPublish = null;
                    return RedirectToAction("Index", new { controller = "PublisherManager" });
                }
            }

            Publisher newPublisher1 = new Publisher();
            newPublisher1.Name = "Publisher`s name";
            return View(newPublisher1);
        }

        [HttpPost]
        public ActionResult Create(Publisher _publish)
        {
            newPublish = _publish;
            TempData["Flag"] = "Create";
            TempData["From"] = "PublisherManager";
            return RedirectToAction("FromAuthorManager", new { controller = "Home" });
        }


        [HttpPost]
        public ActionResult ManagerGeneral(FormCollection formcollection)
        {
            newPublish = null;

            if (formcollection["EditePublisher"] != null)
            {
                if (Name == null)
                {
                    Name = formcollection["Publisher"] as string;
                }
                TempData["Flag"] = "Edite";
                TempData["From"] = "PublisherManager";

                if (TempData["PublisherRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }

            if (formcollection["DeletePublisher"] != null)
            {
                if (Name == null)
                {
                    Name = formcollection["Publisher"] as string;
                }

                TempData["Flag"] = "Delete";
                TempData["From"] = "PublisherManager";

                if (TempData["PublisherRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }
            return View();
        }



        [HttpGet]
        public ActionResult Edite()
        {
            if (newPublish != null)
            {
                (TempData["PublisherRepository"] as PublisherRepository).Edite(newPublish, index);
                for (int i = 0; i < (TempData["BookRepository"] as BookRepository).ListBooks.Count; i++)
                {
                    if (((TempData["BookRepository"] as BookRepository).ListBooks[i].Publisher as Publisher) != null)
                    {
                        if (((TempData["BookRepository"] as BookRepository).ListBooks[i].Publisher as Publisher).Name == Name)
                            (TempData["BookRepository"] as BookRepository).ListBooks[i].Publisher.Name = newPublish.Name;
                    }

                }
                index = -1;
                Name = null;
                return RedirectToAction("Index");
            }

            if (TempData["PublisherRepository"] != null)
            {

                var existingUser = (TempData["PublisherRepository"] as PublisherRepository).Get(Name);

                if (existingUser == null)
                {
                    Name = null;
                    return RedirectToAction("Index");
                }

                for (int i = 0; i < ListPublishers.Count; i++)
                {
                    if (ListPublishers[i].Name == existingUser.Name)
                        index = i;
                }
                return View(existingUser);
            }
            return RedirectToAction("Index", new { controller = "PublisherManager" });
        }


        [HttpPost]
        public ActionResult Edite(FormCollection formcollection)
        {
            if (newPublish == null)
            {
                if (formcollection["Name"] != null)
                {
                    newPublish = new Publisher();
                    newPublish.Name = formcollection["Name"];

                    TempData["Flag"] = "Edite";
                    TempData["From"] = "PublisherManager";
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete()
        {
            if (Name != null)
            {
                bool res = (TempData["PublisherRepository"] as PublisherRepository).Delete(Name);
                for (int i = 0; i < (TempData["BookRepository"] as BookRepository).ListBooks.Count; i++)
                {
                    if (((TempData["BookRepository"] as BookRepository).ListBooks[i].Publisher as Publisher).Name == Name)
                        (TempData["BookRepository"] as BookRepository).ListBooks[i].Publisher = null;
                }
                TempData["ResForDel"] = res;
                Name = null;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { controller = "PublisherManager" });
        }
    }
}