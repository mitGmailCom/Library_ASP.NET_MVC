using Library_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_ASP.NET_MVC.Controllers
{
    public class BookManagerController : Controller
    {
        private static List<Book> ListBooks = new List<Book>();
        private static Book newBook = new Book();
        private static Book BookCreate = new Book();
        private static int Id { get; set; } = -1;
        private static int index = -1;
        private static FormCollection Copyformcollection = new FormCollection();

        public ActionResult ToHome()
        {
            return RedirectToAction("FromAuthorManager", "Home");
        }


        // GET: AuthorManager
        [HttpGet]
        public ActionResult Index()
        {
            ListBooks.Clear();
            ListBooks = (TempData["BookRepository"] as BookRepository).ListBooks.ToList();
            return View(ListBooks);
        }


        [HttpGet]
        public ActionResult Create()
        {
            if (BookCreate != null)
            {
                if (TempData["BookRepository"] != null)
                {
                    (TempData["BookRepository"] as BookRepository).Create(BookCreate);
                    BookCreate = null;
                    return RedirectToAction("Index", new { controller = "BookManager" });
                }
            }

            Book newBook1 = new Book();
            newBook1.Name = "Book`s name";
            newBook1.Publisher = new Publisher();
            newBook1.Authors = null;
            newBook1.PublishDate = new DateTime();
            newBook1.PageCount = 0;
            newBook1.ISBN = "Book`s ISBN";
            if (TempData["PublisherRepository"] != null)
            {
                List<SelectListItem> tempSList = new List<SelectListItem>();
                List<string> tempNamePublishers = new List<string>();
                for (int i = 0; i < (TempData["PublisherRepository"] as PublisherRepository).ListPublishers.Count; i++)
                {
                    tempSList.Add(new SelectListItem
                    {
                        Text = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name,
                        Value = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name
                    });
                    tempNamePublishers.Add((TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name);
                }
                //SelectList tempSList = new SelectList(tempNamePublishers, $"{tempNamePublishers}", $"{tempNamePublishers}");
                ViewBag.Publisher = tempSList;
                //SelectList tempSList = new SelectList((TempData["PublisherRepository"] as PublisherRepository).ListPublishers);
                //ViewBag.Publisher = tempSList;
            }
            
            if (TempData["AuthorRepository"] != null)
            {
                List<SelectListItem> tempMList = new List<SelectListItem>();
                List<string> tempNameAuthors = new List<string>();
                for (int i = 0; i < (TempData["AuthorRepository"] as AuthorRepository).ListAuthors.Count; i++)
                {
                    tempMList.Add(new SelectListItem
                    {
                        Text = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name,
                        Value = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name
                    });
                    tempNameAuthors.Add((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name);
                }
                //MultiSelectList tempMList = new MultiSelectList(tempNameAuthors, $"{tempNameAuthors}", $"{tempNameAuthors}");
                ViewBag.Authors = tempMList;
                //MultiSelectList tempMList = new MultiSelectList((TempData["AuthorRepository"] as AuthorRepository).ListAuthors);
                //ViewBag.Authors = tempMList;
            }
            return View(newBook1);
        }


        [HttpGet]
        public ActionResult CreateFormcollection()
        {
            Publisher tempPublisher = new Publisher();
            List<Author> TempListAuthors = new List<Author>();
            string MasAuthors = Copyformcollection["Authors"] as string;
            string[] separators = { "," };
            string[] ResMasAuthors = MasAuthors.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            BookCreate.Name = Copyformcollection["Name"];
            string selectedPublisher = Copyformcollection["Publisher"];
            if (TempData["PublisherRepository"] != null)
            {
                for (int i = 0; i < (TempData["PublisherRepository"] as PublisherRepository).ListPublishers.Count; i++)
                {
                    if ((TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name == selectedPublisher)
                        tempPublisher = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i];
                }
                BookCreate.Publisher = tempPublisher;
            }
            if (TempData["AuthorRepository"] != null)
            {
                for (int i = 0; i < (TempData["AuthorRepository"] as AuthorRepository).ListAuthors.Count; i++)
                {
                    for (int j = 0; j < ResMasAuthors.Length; j++)
                    {
                        if ((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name == ResMasAuthors[j])
                            TempListAuthors.Add((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i]);
                    }
                }
                BookCreate.Authors = TempListAuthors.ToList();
            }
            DateTime res;
            bool isDateTime = DateTime.TryParse(Copyformcollection["PublishDate"], out res);
            if (isDateTime)
                BookCreate.PublishDate = Convert.ToDateTime((Copyformcollection["PublishDate"]) as string);
            int resPage;
            bool isInt = Int32.TryParse(Copyformcollection["PageCount"], out resPage);
            if (isInt)
                BookCreate.PageCount = Convert.ToInt32((Copyformcollection["PageCount"]) as string);
            BookCreate.ISBN = Copyformcollection["ISBN"];

            TempData["Flag"] = "Create";
            TempData["From"] = "BookManager";
            return RedirectToAction("FromAuthorManager", new { controller = "Home" });
        }


        [HttpPost]
        public ActionResult Create(FormCollection formcollection)
        {
            if (TempData["PublisherRepository"] == null)
            {
                Copyformcollection = formcollection;
                TempData["Flag"] = "CreateFormcollection";
                TempData["From"] = "BookManager";
                return RedirectToAction("FromAuthorManager", new { controller = "Home" });
            }


            //Publisher tempPublisher = new Publisher();
            //List<Author> TempListAuthors = new List<Author>();
            //string MasAuthors = formcollection["Authors"] as string;
            //string[] separators = { "," };
            //string[]ResMasAuthors = MasAuthors.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //BookCreate.Name = formcollection["Name"];
            //string selectedPublisher = formcollection["Publisher"];
            //if (TempData["PublisherRepository"] != null)
            //{
            //    for (int i = 0; i < (TempData["PublisherRepository"] as PublisherRepository).ListPublishers.Count; i++)
            //    {
            //        if ((TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name == selectedPublisher)
            //            tempPublisher = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i];
            //    }
            //}

            //if (TempData["AuthorRepository"] != null)
            //{
            //    for (int i = 0; i < (TempData["AuthorRepository"] as AuthorRepository).ListAuthors.Count; i++)
            //    {
            //        for (int j = 0; j < ResMasAuthors.Length; j++)
            //        {
            //            if ((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name == ResMasAuthors[j])
            //                TempListAuthors.Add((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i]);
            //        }
            //    }
            //}
            //DateTime res;
            //bool isDateTime = DateTime.TryParse(formcollection["PublishDate"], out res);
            //if (isDateTime)
            //    BookCreate.PublishDate = Convert.ToDateTime((formcollection["PublishDate"]) as string);

            //int resPage;
            //bool isInt = Int32.TryParse(formcollection["PageCount"], out resPage);
            //if (isInt)
            //    BookCreate.PageCount = Convert.ToInt32((formcollection["PageCount"]) as string);

            //BookCreate.ISBN = formcollection["ISBN"];


            ////newBook = _book;
            //TempData["Flag"] = "Create";
            //TempData["From"] = "BookManager";
            return RedirectToAction("FromAuthorManager", new { controller = "Home" });
        }



        [HttpPost]
        public ActionResult ManagerGeneral(FormCollection formcollection)
        {
            newBook = null;

            if (formcollection["EditeBook"] != null)
            {
                if (Id == -1)
                {
                    int tempId;
                    bool isInt = Int32.TryParse(formcollection["PageCount"], out tempId);
                    if (isInt)
                        BookCreate.PageCount = Convert.ToInt32((formcollection["PageCount"]) as string);
                    Id = tempId;
                }
                TempData["Flag"] = "Edite";

                if (TempData["BookRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }

            if (formcollection["DeleteBook"] != null)
            {
                if (Id == -1)
                {
                    int tempId;
                    bool isInt = Int32.TryParse(formcollection["PageCount"], out tempId);
                    if (isInt)
                        BookCreate.PageCount = Convert.ToInt32((formcollection["PageCount"]) as string);
                    Id = tempId;
                }
                TempData["Flag"] = "Delete";

                if (TempData["BookRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }
            return View();
        }




        [HttpGet]
        public ActionResult Edite()
        {
            if (newBook != null)
            {
                (TempData["BookRepository"] as BookRepository).Edite(newBook, index);
                index = -1;
                return RedirectToAction("Index");
            }


            if (TempData["BookRepository"] != null)
            {
                var existingUser = (TempData["BookRepository"] as BookRepository).Get(Id);
                for (int i = 0; i < ListBooks.Count; i++)
                {
                    if (ListBooks[i].Id == existingUser.Id)
                        index = i;
                }

                if (existingUser == null)
                {
                    Id = -1;
                    return RedirectToAction("Index");
                }

                Id = -1;
                return View(existingUser);
            }
            return RedirectToAction("Index", new { controller = "BookManager" });
        }



        [HttpPost]
        public ActionResult Edite(FormCollection formcollection, string[] list2)
        {
            if (newBook == null)
            {
                if (formcollection != null)
                {
                    Publisher tempPublisher = new Publisher();
                    List<Author> TempListAuthors = new List<Author>();

                    newBook.Name = formcollection["Name"];
                    string selectedPublisher = formcollection["drDwnListPublisher"];
                    if (TempData["PublisherRepository"] != null)
                    {
                        for (int i = 0; i < (TempData["PublisherRepository"] as PublisherRepository).ListPublishers.Count; i++)
                        {
                            if ((TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name == selectedPublisher)
                                tempPublisher = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i];
                        }
                        newBook.Publisher = tempPublisher;
                    }

                    if (TempData["AuthorRepository"] != null)
                    {
                        for (int i = 0; i < (TempData["AuthorRepository"] as AuthorRepository).ListAuthors.Count; i++)
                        {
                            for (int j = 0; j < list2.Length; j++)
                            {
                                if ((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name == list2[j])
                                    TempListAuthors.Add((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i]);
                            }
                        }
                        newBook.Authors = TempListAuthors.ToList();
                    }
                    DateTime res;
                    bool isDateTime = DateTime.TryParse(formcollection["PublishDate"], out res);
                    if (isDateTime)
                        newBook.PublishDate = Convert.ToDateTime((formcollection["PublishDate"]) as string);

                    int resPage;
                    bool isInt = Int32.TryParse(formcollection["PageCount"], out resPage);
                    if (isInt)
                        newBook.PageCount = Convert.ToInt32((formcollection["PageCount"]) as string);

                    newBook.ISBN = formcollection["ISBN"];

                    int resId;
                    bool isID = Int32.TryParse(formcollection["Id"], out resId);
                    if (isID)
                        newBook.Id = Convert.ToInt32((formcollection["Id"]) as string);


                    TempData["Flag"] = "Edite";
                    TempData["From"] = "BookManager";
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }
            return RedirectToAction("Index");
        }



        public ActionResult Delete()
        {
            if (Id != -1)
            {
                bool res = (TempData["BookRepository"] as BookRepository).Delete(Id);
                TempData["ResForDel"] = res;
                Id = -1;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { controller = "BookManager" });
        }


    }
}