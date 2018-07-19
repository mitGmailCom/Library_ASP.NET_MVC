using Library_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_ASP.NET_MVC.Controllers
{
    public class AuthorManagerController : Controller
    {
        private static Author newAuth = new Author();
        private static string Name { get; set; } = null;
        private static int index = -1;



        [HttpGet]
        public ActionResult Index()
        {
            return View(AuthorRepository.Instance.ListAuthors);
        }


        [HttpGet]
        public ActionResult Create()
        {
            Author newAuthor1 = new Author();
            newAuthor1.Name = "Authors name";
            newAuthor1.DateOfBirth = new DateTime();
            newAuthor1.DateOfDeath = null;
            return View(newAuthor1);
        }


        [HttpPost]
        public ActionResult Create(Author _auth)
        {
            newAuth = _auth;
            if (newAuth != null)
            {
                AuthorRepository.Instance.Create(newAuth);
                newAuth = null;
                return RedirectToAction("Index", new { controller = "AuthorManager" });
            }
            return RedirectToAction("Index", new { controller = "AuthorManager" });
        }


        [HttpPost]
        public ActionResult ManagerGeneral(FormCollection formcollection)
        {
            newAuth = null;

            if (formcollection["EditeAuthor"] != null)
            {
                if (Name == null)
                    Name = formcollection["Author"] as string;
                return RedirectToAction("Edite");
            }
            if (formcollection["DeleteAuthor"] != null)
            {
                if (Name == null)
                    Name = formcollection["Author"] as string;
                return RedirectToAction("Delete");
            }
            return View();
        }


        [HttpGet]
        public ActionResult Edite()
        {
            var existingUser = AuthorRepository.Instance.Get(Name);
            if (existingUser == null)
            {
                Name = null;
                return RedirectToAction("Index");
            }
            for (int i = 0; i < AuthorRepository.Instance.ListAuthors.Count; i++)
            {
                if (AuthorRepository.Instance.ListAuthors[i].Name == existingUser.Name)
                    index = i;
            }
            return View(existingUser);
        }




        [HttpPost]
        public ActionResult Edite(FormCollection formcollection)
        {
            if (newAuth == null)
            {
                if (formcollection["Name"] != null)
                {
                    newAuth = new Author();
                    newAuth.Name = formcollection["Name"];
                    newAuth.DateOfBirth = Convert.ToDateTime((formcollection["DateOfBirth"]) as string);
                    DateTime res;
                    bool isDateTime = DateTime.TryParse(formcollection["DateOfDeath"], out res);
                    if (isDateTime)
                        newAuth.DateOfDeath = Convert.ToDateTime((formcollection["DateOfDeath"]) as string);

                    AuthorRepository.Instance.Edite(newAuth, index);
                    for (int i = 0; i < BookRepository.Instance.ListBooks.Count; i++)
                    {
                        for (int j = 0; j < (BookRepository.Instance.ListBooks[i].Authors as List<Author>).Count; j++)
                        {
                            if ((BookRepository.Instance.ListBooks[i].Authors as List<Author>).Count > 0)
                            {
                                if ((BookRepository.Instance.ListBooks[i].Authors as List<Author>)[j].Name == Name)
                                    (BookRepository.Instance.ListBooks[i].Authors as List<Author>)[j].Name = newAuth.Name;
                            }
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
                bool res = AuthorRepository.Instance.Delete(Name);
                for (int i = 0; i < BookRepository.Instance.ListBooks.Count; i++)
                {
                    for (int j = 0; j < (BookRepository.Instance.ListBooks[i].Authors as List<Author>).Count; j++)
                    {
                        if ((BookRepository.Instance.ListBooks[i].Authors as List<Author>)[j].Name == Name)
                            (BookRepository.Instance.ListBooks[i].Authors as List<Author>).RemoveAt(j);
                    }
                }
                TempData["ResForDel"] = res;
                Name = null;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { controller = "AuthorManager" });
        }
    }
}