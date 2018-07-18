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
        private static bool ChekFormcollection = false;

        public ActionResult ToHome()
        {
            return RedirectToAction("FromAuthorManager", "Home");
        }


        // GET: AuthorManager
        [HttpGet]
        public ActionResult Index()
        {
            return View(BookRepository.Instance.ListBooks);
        }


        [HttpGet]
        public ActionResult Create()
        {
            Book newBook1 = new Book();
            newBook1.Name = "Book`s name";
            newBook1.Publisher = new Publisher();
            newBook1.Authors = null;
            newBook1.PublishDate = new DateTime();
            newBook1.PageCount = 0;
            newBook1.ISBN = "Book`s ISBN";

            List<SelectListItem> tempSList = new List<SelectListItem>();
            List<string> tempNamePublishers = new List<string>();
            for (int i = 0; i < PublisherRepository.Instance.ListPublishers.Count; i++)
            {
                tempSList.Add(new SelectListItem
                {
                    Text = PublisherRepository.Instance.ListPublishers[i].Name,
                    Value = PublisherRepository.Instance.ListPublishers[i].Name
                });
                tempNamePublishers.Add(PublisherRepository.Instance.ListPublishers[i].Name);
            }
            ViewBag.Publisher = tempSList;


            List<SelectListItem> tempMList = new List<SelectListItem>();
            List<string> tempNameAuthors = new List<string>();
            for (int i = 0; i < AuthorRepository.Instance.ListAuthors.Count; i++)
            {
                tempMList.Add(new SelectListItem
                {
                    Text = AuthorRepository.Instance.ListAuthors[i].Name,
                    Value = AuthorRepository.Instance.ListAuthors[i].Name
                });
                tempNameAuthors.Add(AuthorRepository.Instance.ListAuthors[i].Name);
            }
            ViewBag.Authors = tempMList;

            return View(newBook1);
        }



        [HttpPost]
        public ActionResult Create(FormCollection formcollection)
        {
            if (CheckFormcollection(formcollection))
            {
                ChekFormcollection = false;
                return RedirectToAction("Index");
            }
            Publisher tempPublisher = new Publisher();
            List<Author> TempListAuthors = new List<Author>();
            string MasAuthors = formcollection["Authors"] as string;
            string[] separators = { "," };
            string[] ResMasAuthors = MasAuthors.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            BookCreate.Name = formcollection["Name"];
            string selectedPublisher = formcollection["Publisher"];

            for (int i = 0; i < PublisherRepository.Instance.ListPublishers.Count; i++)
            {
                if (PublisherRepository.Instance.ListPublishers[i].Name == selectedPublisher)
                    tempPublisher = PublisherRepository.Instance.ListPublishers[i];
            }
            BookCreate.Publisher = tempPublisher;


            for (int i = 0; i < AuthorRepository.Instance.ListAuthors.Count; i++)
            {
                for (int j = 0; j < ResMasAuthors.Length; j++)
                {
                    if (AuthorRepository.Instance.ListAuthors[i].Name == ResMasAuthors[j])
                        TempListAuthors.Add(AuthorRepository.Instance.ListAuthors[i]);
                }
            }
            BookCreate.Authors = TempListAuthors.ToList();

            DateTime res;
            bool isDateTime = DateTime.TryParse(formcollection["PublishDate"], out res);
            if (isDateTime)
                BookCreate.PublishDate = Convert.ToDateTime((formcollection["PublishDate"]) as string);
            int resPage;
            bool isInt = Int32.TryParse(formcollection["PageCount"], out resPage);
            if (isInt)
                BookCreate.PageCount = Convert.ToInt32((formcollection["PageCount"]) as string);
            BookCreate.ISBN = formcollection["ISBN"];

            BookRepository.Instance.Create(BookCreate);
            BookCreate = null;
            return RedirectToAction("Index", new { controller = "BookManager" });
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
                    bool isInt = Int32.TryParse(formcollection["Book"], out tempId);
                    if (isInt)
                        Id = tempId;
                }
                else
                    return RedirectToAction("Index", new { controller = "BookManager" });

                return RedirectToAction("Edite", new { controller = "BookManager" });
            }

            if (formcollection["DeleteBook"] != null)
            {
                if (Id == -1)
                {
                    int tempId;
                    bool isInt = Int32.TryParse(formcollection["Book"], out tempId);
                    if (isInt)
                        Id = tempId;
                }
                else
                    return RedirectToAction("Index", new { controller = "BookManager" });

                return RedirectToAction("Delete", new { controller = "BookManager" });
            }
            return View();
        }



        private void CheckTempDataPublisherRepository(Book existingBook)
        {
            bool isSelected = false;
            List<SelectListItem> tempSList = new List<SelectListItem>();
            for (int i = 0; i < PublisherRepository.Instance.ListPublishers.Count; i++)
            {
                if (existingBook.Publisher.Name == PublisherRepository.Instance.ListPublishers[i].Name)
                    isSelected = true;
                if (!isSelected)
                {
                    tempSList.Add(new SelectListItem
                    {
                        Text = PublisherRepository.Instance.ListPublishers[i].Name,
                        Value = PublisherRepository.Instance.ListPublishers[i].Name,
                    });
                }
                if (isSelected)
                {
                    tempSList.Add(new SelectListItem
                    {
                        Text = PublisherRepository.Instance.ListPublishers[i].Name,
                        Value = PublisherRepository.Instance.ListPublishers[i].Name,
                        Selected = true
                    });
                    isSelected = false;
                }
                ViewBag.Publisher = tempSList;
            }
        }


        private void CheckTempDataAuthorRepository(Book existingBook)
        {
            bool isSelected = false;
            List<string> selecList = new List<string>();
            List<SelectListItem> tempMList = new List<SelectListItem>();
            for (int i = 0; i < AuthorRepository.Instance.ListAuthors.Count; i++)
            {
                if ((existingBook.Authors as List<Author>).Count != 0)
                {
                    for (int j = 0; j < (existingBook.Authors as List<Author>).Count; j++)
                    {
                        if ((existingBook.Authors as List<Author>)[j].Name == AuthorRepository.Instance.ListAuthors[i].Name)
                            isSelected = true;
                        if (!isSelected)
                        {
                            if (j == (existingBook.Authors as List<Author>).Count - 1)
                            {
                                tempMList.Add(new SelectListItem
                                {
                                    Text = AuthorRepository.Instance.ListAuthors[i].Name,
                                    Value = AuthorRepository.Instance.ListAuthors[i].Name
                                });
                            }
                        }
                        if (isSelected)
                        {
                            selecList.Add(i.ToString());
                            tempMList.Add(new SelectListItem
                            {
                                Text = AuthorRepository.Instance.ListAuthors[i].Name,
                                Value = AuthorRepository.Instance.ListAuthors[i].Name,
                                Selected = true
                            });
                            isSelected = false;
                            break;
                        }
                    }
                }
                string[] defaultSelected = selecList.ToArray();
                //ViewBag.Authors = new MultiSelectList(tempMList, "Value", "Text", defaultSelected) ;
                ViewBag.Authors = tempMList;
                ViewBag.Selected = defaultSelected;
            }
        }



        [HttpGet]
        public ActionResult Edite()
        {
            var existingBook = BookRepository.Instance.Get(Id);
            if (existingBook == null)
            {
                Id = -1;
                return RedirectToAction("Index");
            }
            for (int i = 0; i < ListBooks.Count; i++)
            {
                if (ListBooks[i].Id == existingBook.Id)
                    index = i;
            }
            Id = -1;

            CheckTempDataPublisherRepository(existingBook);
            CheckTempDataAuthorRepository(existingBook);
            return View(existingBook);
        }



        private bool CheckFormcollection(FormCollection formcollection)
        {
            if (formcollection["Authors"] == null)
                ChekFormcollection = true;
            //if (formcollection["Id"] == null)
            //    ChekFormcollection = true;
            if (formcollection["ISBN"] == null)
                ChekFormcollection = true;
            if (formcollection["PageCount"] == null)
                ChekFormcollection = true;
            if (formcollection["Name"] == null)
                ChekFormcollection = true;
            if (formcollection["PublishDate"] == null)
                ChekFormcollection = true;
            if (formcollection["Publisher"] == null)
                ChekFormcollection = true;

            return ChekFormcollection;
        }



        [HttpPost]
        public ActionResult Edite(FormCollection formcollection)
        {
            if (CheckFormcollection(formcollection))
            {
                ChekFormcollection = false;
                return RedirectToAction("Index");
            }

            if (newBook == null)
            {
                if (formcollection != null)
                {
                    newBook = new Book();
                    Publisher tempPublisher = new Publisher();
                    List<Author> TempListAuthors = new List<Author>();
                    string MasAuthors = formcollection["Authors"] as string;
                    string[] separators = { "," };
                    string[] ResMasAuthors = MasAuthors.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    newBook.Name = formcollection["Name"];
                    string selectedPublisher = formcollection["Publisher"];

                    for (int i = 0; i < PublisherRepository.Instance.ListPublishers.Count; i++)
                    {
                        if (PublisherRepository.Instance.ListPublishers[i].Name == selectedPublisher)
                            tempPublisher = PublisherRepository.Instance.ListPublishers[i];
                    }
                    newBook.Publisher = tempPublisher;
                    for (int i = 0; i < AuthorRepository.Instance.ListAuthors.Count; i++)
                    {
                        for (int j = 0; j < ResMasAuthors.Length; j++)
                        {
                            if (AuthorRepository.Instance.ListAuthors[i].Name == ResMasAuthors[j])
                                TempListAuthors.Add(AuthorRepository.Instance.ListAuthors[i]);
                        }
                    }
                    newBook.Authors = TempListAuthors.ToList();
                    DateTime res;
                    bool isDateTime = DateTime.TryParse(formcollection["PublishDate"], out res);
                    if (isDateTime)
                        newBook.PublishDate = Convert.ToDateTime((formcollection["PublishDate"]) as string);
                    int resPage = 0;
                    bool isInt = Int32.TryParse(formcollection["PageCount"], out resPage);
                    if (isInt)
                        newBook.PageCount = resPage;
                    newBook.ISBN = formcollection["ISBN"];
                    int resId = 0;
                    bool isIntID = Int32.TryParse(formcollection["Id"], out resId);
                    if (isIntID)
                        newBook.Id = resId;

                    BookRepository.Instance.Edite(newBook, index);
                    index = -1;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }




        public ActionResult Delete()
        {
            if (Id != -1)
            {
                bool res = BookRepository.Instance.Delete(Id);
                TempData["ResForDel"] = res;
                Id = -1;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { controller = "BookManager" });
        }


    }
}