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
        private static Publisher newPublish = new Publisher();
        private static string Name { get; set; } = null;
        private static int index = -1;



        public ActionResult Index()
        {
            return View(PublisherRepository.Instance.ListPublishers);
        }


        [HttpGet]
        public ActionResult Create()
        {
            Publisher newPublisher1 = new Publisher();
            newPublisher1.Name = "Publisher`s name";
            return View(newPublisher1);
        }


        [HttpPost]
        public ActionResult Create(Publisher _publish)
        {
            newPublish = _publish;
            if (newPublish != null)
            {
                PublisherRepository.Instance.Create(newPublish);
                newPublish = null;
                return RedirectToAction("Index", new { controller = "PublisherManager" });
            }
            return RedirectToAction("Index", new { controller = "PublisherManager" });
        }



        [HttpPost]
        public ActionResult ManagerGeneral(FormCollection formcollection)
        {
            newPublish = null;

            if (formcollection["EditePublisher"] != null)
            {
                if (Name == null)
                    Name = formcollection["Publisher"] as string;
                return RedirectToAction("Edite");
            }

            if (formcollection["DeletePublisher"] != null)
            {
                if (Name == null)
                    Name = formcollection["Publisher"] as string;
                return RedirectToAction("Delete");
            }
            return View();
        }



        [HttpGet]
        public ActionResult Edite()
        {
                var existingUser = PublisherRepository.Instance.Get(Name);
                if (existingUser == null)
                {
                    Name = null;
                    return RedirectToAction("Index");
                }
                for (int i = 0; i < PublisherRepository.Instance.ListPublishers.Count; i++)
                {
                    if (PublisherRepository.Instance.ListPublishers[i].Name == existingUser.Name)
                        index = i;
                }
                return View(existingUser);
           
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

                    PublisherRepository.Instance.Edite(newPublish, index);
                    for (int i = 0; i < BookRepository.Instance.ListBooks.Count; i++)
                    {
                        if ((BookRepository.Instance.ListBooks[i].Publisher as Publisher) != null)
                        {
                            if ((BookRepository.Instance.ListBooks[i].Publisher as Publisher).Name == Name)
                                BookRepository.Instance.ListBooks[i].Publisher.Name = newPublish.Name;
                        }
                    }
                    index = -1;
                    Name = null;

                }
            }
            return RedirectToAction("Index");
        }



        public ActionResult Delete()
        {
            if (Name != null)
            {
                bool res = PublisherRepository.Instance.Delete(Name);
                for (int i = 0; i < BookRepository.Instance.ListBooks.Count; i++)
                {
                    if ((BookRepository.Instance.ListBooks[i].Publisher as Publisher).Name == Name)
                        BookRepository.Instance.ListBooks[i].Publisher = null;
                }
                TempData["ResForDel"] = res;
                Name = null;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { controller = "PublisherManager" });
        }
    }
}