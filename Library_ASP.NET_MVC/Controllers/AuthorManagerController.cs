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
        private static List<Author> ListAuthors = new List<Author>();
        private static Author newAuth = new Author();
        private static string Name { get; set; } = null;
        private static int index = -1;

        public ActionResult ToHome()
        {
            return RedirectToAction("FromAuthorManager", "Home");
        }


        // GET: AuthorManager
        [HttpGet]
        public ActionResult Index()
        {
            ListAuthors.Clear();
            ListAuthors = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors.ToList();
            return View(ListAuthors);
        }


        [HttpGet]
        public ActionResult Create()
        {
            if (newAuth != null)
            {
                if (TempData["AuthorRepository"] != null)
                {
                    (TempData["AuthorRepository"] as AuthorRepository).Create(newAuth);
                    newAuth = null;
                    return RedirectToAction("Index", new { controller = "AuthorManager" });
                }
            }

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
            TempData["Flag"] = "Create";
            TempData["From"] = "AuthorManager";
            return RedirectToAction("FromAuthorManager", new { controller = "Home" });
        }



        [HttpPost]
        public ActionResult ManagerGeneral(FormCollection formcollection)
        {
            newAuth = null;

            if (formcollection["EditeAuthor"] != null)
            {
                if (Name == null)
                {
                    Name = formcollection["Author"] as string;
                }
                TempData["Flag"] = "Edite";
                TempData["From"] = "AuthorManager";

                if (TempData["AuthorRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
             }


            if (formcollection["DeleteAuthor"] != null)
            {
                if (Name == null)
                {
                    Name = formcollection["Author"] as string;
                }
                TempData["Flag"] = "Delete";
                TempData["From"] = "AuthorManager";

                if (TempData["AuthorRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }

            return View();
        }




        [HttpGet]
        public ActionResult Edite()
        {
            if (newAuth != null)
            {
                (TempData["AuthorRepository"] as AuthorRepository).Edite(newAuth, index);
                index = -1;
                return RedirectToAction("Index");
            }


            if (TempData["AuthorRepository"] != null)
            {
                var existingUser = (TempData["AuthorRepository"] as AuthorRepository).Get(Name);

                if (existingUser == null)
                {
                    Name = null;
                    return RedirectToAction("Index");
                }

                for (int i = 0; i < ListAuthors.Count; i++)
                {
                    if (ListAuthors[i].Name == existingUser.Name)
                        index = i;
                }

                Name = null;
                return View(existingUser);
            }
            return RedirectToAction("Index", new { controller = "AuthorManager" });
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

                    TempData["Flag"] = "Edite";
                    TempData["From"] = "AuthorManager";
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }
            return RedirectToAction("Index");
        }




        public ActionResult Delete()
        {
            if (Name != null)
            {
                bool res = (TempData["AuthorRepository"] as AuthorRepository).Delete(Name);
                TempData["ResForDel"] = res;
                Name = null;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { controller = "AuthorManager" });
        }



    }
}