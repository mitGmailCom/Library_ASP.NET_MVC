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

            return RedirectToAction("FromAuthorManager", new { controller = "Home" });
        }




        [HttpPost]
        public ActionResult ManagerGeneral(FormCollection formcollection)
        {
            newBook = null;
            if (formcollection["EditeBook"] != null)
            {
                //newBook = new Book();
                if (Id == -1)
                {
                    int tempId;
                    bool isInt = Int32.TryParse(formcollection["Book"], out tempId);
                    if (isInt)
                        //newBook.Id = tempId;
                    Id = tempId;
                }
                else
                    return RedirectToAction("Index", new { controller = "BookManager" });

                TempData["Flag"] = "Edite";
                TempData["From"] = "BookManager";

                if (TempData["BookRepository"] == null)
                {
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }

            if (formcollection["DeleteBook"] != null)
            {
                //newBook = new Book();
                if (Id == -1)
                {
                    int tempId;
                    bool isInt = Int32.TryParse(formcollection["Book"], out tempId);
                    if (isInt)
                        //newBook.Id = tempId;
                    Id = tempId;
                }
                TempData["Flag"] = "Delete";
                TempData["From"] = "BookManager";

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
                if (existingUser == null)
                {
                    Id = -1;
                    return RedirectToAction("Index");
                }

                for (int i = 0; i < ListBooks.Count; i++)
                {
                    if (ListBooks[i].Id == existingUser.Id)
                        index = i;
                }
                Id = -1;

                if (TempData["PublisherRepository"] != null)
                {
                    bool isSelected = false;
                    List<SelectListItem> tempSList = new List<SelectListItem>();
                    for (int i = 0; i < (TempData["PublisherRepository"] as PublisherRepository).ListPublishers.Count; i++)
                    {
                        if (existingUser.Publisher.Name == (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name)
                            isSelected = true;
                        if (!isSelected)
                        {
                            tempSList.Add(new SelectListItem
                            {
                                Text = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name,
                                Value = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name,
                            });
                        }
                        if (isSelected)
                        {
                            tempSList.Add(new SelectListItem
                            {
                                Text = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name,
                                Value = (TempData["PublisherRepository"] as PublisherRepository).ListPublishers[i].Name,
                                Selected = true
                            });
                            isSelected = false;
                        }
                    }
                    ViewBag.Publisher = tempSList;
                }

                if (TempData["AuthorRepository"] != null)
                {
                    bool isSelected = false;
                    List<string> selecList = new List<string>();
                    List<SelectListItem> tempMList = new List<SelectListItem>();
                    for (int i = 0; i < (TempData["AuthorRepository"] as AuthorRepository).ListAuthors.Count; i++)
                    {
                        if ((existingUser.Authors as List<Author>).Count != 0)
                        {
                            for (int j = 0; j < (existingUser.Authors as List<Author>).Count; j++)
                            {
                                if ((existingUser.Authors as List<Author>)[j].Name == (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name)
                                    isSelected = true;
                                if (!isSelected)
                                {
                                    if (j == (existingUser.Authors as List<Author>).Count - 1)
                                    {
                                        tempMList.Add(new SelectListItem
                                        {
                                            Text = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name,
                                            Value = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name
                                        });
                                    }
                                }
                                if (isSelected)
                                {
                                    selecList.Add(i.ToString());
                                    tempMList.Add(new SelectListItem
                                    {
                                        Text = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name,
                                        Value = (TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name,
                                        Selected = true
                                    });
                                    isSelected = false;
                                    break;
                                }
                            }
                        }
                    }
                    string[] defaultSelected = selecList.ToArray();
                    //ViewBag.Authors = new MultiSelectList(tempMList, "Value", "Text", defaultSelected) ;
                    ViewBag.Authors = tempMList;
                    ViewBag.Selected = defaultSelected;
                }
                return View(existingUser);
            }
            return RedirectToAction("Index", new { controller = "BookManager" });
        }





        [HttpGet]
        public ActionResult EditeFormcollection(FormCollection formcollection)
        {
            if (newBook == null)
            {
                if (formcollection != null)
                {
                    newBook = new Book();
                    Publisher tempPublisher = new Publisher();
                    List<Author> TempListAuthors = new List<Author>();
                    string MasAuthors = Copyformcollection["Authors"] as string;
                    string[] separators = { "," };
                    string[] ResMasAuthors = MasAuthors.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    newBook.Name = Copyformcollection["Name"];
                    string selectedPublisher = Copyformcollection["Publisher"];
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
                            for (int j = 0; j < ResMasAuthors.Length; j++)
                            {
                                if ((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i].Name == ResMasAuthors[j])
                                    TempListAuthors.Add((TempData["AuthorRepository"] as AuthorRepository).ListAuthors[i]);
                            }
                        }
                        newBook.Authors = TempListAuthors.ToList();
                    }
                    DateTime res;
                    bool isDateTime = DateTime.TryParse(Copyformcollection["PublishDate"], out res);
                    if (isDateTime)
                        newBook.PublishDate = Convert.ToDateTime((Copyformcollection["PublishDate"]) as string);
                    int resPage;
                    bool isInt = Int32.TryParse(Copyformcollection["PageCount"], out resPage);
                    if (isInt)
                        newBook.PageCount = resPage;
                    newBook.ISBN = Copyformcollection["ISBN"];
                    int resId;
                    bool isIntID = Int32.TryParse(Copyformcollection["Id"], out resId);
                    if (isIntID)
                        newBook.Id = resId;

                    TempData["Flag"] = "Edite";
                    TempData["From"] = "BookManager";
                    return RedirectToAction("FromAuthorManager", new { controller = "Home" });
                }
            }
            return RedirectToAction("Index");
        }





        [HttpPost]
        public ActionResult Edite(FormCollection formcollection)
        {
            if (TempData["PublisherRepository"] == null)
            {
                Copyformcollection.Clear();
                Copyformcollection = formcollection;
                TempData["Flag"] = "EditeFormcollection";
                TempData["From"] = "BookManager";
                return RedirectToAction("FromAuthorManager", new { controller = "Home" });
            }
            return RedirectToAction("FromAuthorManager", new { controller = "Home" });
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