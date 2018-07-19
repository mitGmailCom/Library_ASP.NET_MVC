using Library_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Library_ASP.NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private AuthorRepository authorRepository = AuthorRepository.Instance;
        private PublisherRepository publisherRepository = PublisherRepository.Instance;
        private BookRepository bookRepository = BookRepository.Instance;


        List<Author> tempAuthors = new List<Author>();
        List<string> tempBooksName = new List<string>();
        Book tempBook = new Book();



        public ActionResult Index()
        {
            if (publisherRepository.ListPublishers.Count == 0)
            {
                publisherRepository.ListPublishers.Add(new Publisher { Name = "John Wiley & Sons" });
                publisherRepository.ListPublishers.Add(new Publisher { Name = "Питер" });
            }
            if (authorRepository.ListAuthors.Count == 0)
            {
                authorRepository.ListAuthors.Add(new Author { Name = "Jon Galloway", DateOfBirth = new DateTime(1972, 05, 12), DateOfDeath = null });
                authorRepository.ListAuthors.Add(new Author { Name = "Brad Wilson", DateOfBirth = new DateTime(1971, 08, 18), DateOfDeath = null });
                authorRepository.ListAuthors.Add(new Author { Name = "K. Scott Allen", DateOfBirth = new DateTime(1972, 05, 12), DateOfDeath = null });
                authorRepository.ListAuthors.Add(new Author { Name = "David Matson", DateOfBirth = new DateTime(1972, 05, 12), DateOfDeath = null });
                authorRepository.ListAuthors.Add(new Author { Name = "James Chambers", DateOfBirth = new DateTime(1971, 08, 18), DateOfDeath = null });
                authorRepository.ListAuthors.Add(new Author { Name = "David Paquette", DateOfBirth = new DateTime(1972, 05, 12), DateOfDeath = null });
                authorRepository.ListAuthors.Add(new Author { Name = "Simmon Timms", DateOfBirth = new DateTime(1972, 05, 12), DateOfDeath = null });
            }
            if (bookRepository.ListBooks.Count == 0)
            {
                List<Author> temp = new List<Author>();
                temp.AddRange(authorRepository.ListAuthors.GetRange(0, 4));
                bookRepository.ListBooks.Add(new Book
                {
                    Id = 0,
                    Authors = temp.ToList(),
                    ISBN = "978-1-118-79475-3",
                    PageCount = 620,
                    PublishDate = new DateTime(2014, 01, 01),
                    Publisher = publisherRepository.ListPublishers[0],
                    Name = "Professional ASP.NET MVC 5",
                    SelectedItems = new string[] { AuthorRepository.Instance.ListAuthors[0].Name, AuthorRepository.Instance.ListAuthors[1].Name, AuthorRepository.Instance.ListAuthors[2].Name, AuthorRepository.Instance.ListAuthors[3].Name }
                });
                temp.Clear();
                temp.AddRange(authorRepository.ListAuthors.GetRange(4, 3));
                bookRepository.ListBooks.Add(new Book
                {
                    Id = 1,
                    Authors = temp.ToList(),
                    ISBN = "978-5-496-03071-7",
                    PageCount = 464,
                    PublishDate = new DateTime(2018, 01, 01),
                    Publisher = publisherRepository.ListPublishers[1],
                    Name = "ASP.NET Core. Development: Building an application in four sprints.",
                    SelectedItems = new string[] { AuthorRepository.Instance.ListAuthors[4].Name, AuthorRepository.Instance.ListAuthors[5].Name, AuthorRepository.Instance.ListAuthors[6].Name }
                });
                temp.Clear();
                temp.Add(authorRepository.ListAuthors[0]);
                bookRepository.ListBooks.Add(new Book
                {
                    Id = 2,
                    Authors = temp.ToList(),
                    ISBN = "953-1-118-79475-6",
                    PageCount = 720,
                    PublishDate = new DateTime(2017, 01, 01),
                    Publisher = publisherRepository.ListPublishers[0],
                    Name = "ASP.NET MVC 5 Core",
                    SelectedItems = new string[] { AuthorRepository.Instance.ListAuthors[0].Name }
                });
            }
            ViewBag.AllAuthors = authorRepository.GetAll();
            TempData["Book"] = bookRepository.ListBooks[0];
            return View(authorRepository.GetAll());
        }


        [HttpPost]
        public ActionResult Index(FormCollection formcollection)
        {
            string selectetItemDrDownList = null;
            string selectetItemListBox = null;
            tempAuthors.Clear();
            tempBooksName.Clear();
            if (formcollection != null)
            {
                if (formcollection["Books"] != null)
                {
                    if (formcollection["drdowmlist"] != null)
                    {
                        selectetItemDrDownList = formcollection["drdowmlist"] as string;
                        for (int i = 0; i < bookRepository.ListBooks.Count; i++)
                        {
                            tempAuthors = (bookRepository.ListBooks[i].Authors as IEnumerable<Author>).ToList();
                            for (int j = 0; j < tempAuthors.Count; j++)
                            {
                                if (tempAuthors[j].Name == selectetItemDrDownList)
                                    tempBooksName.Add(bookRepository.ListBooks[i].Name);
                            }
                        }
                        SelectList selectList = new SelectList(tempBooksName);
                        TempData["listBooks"] = selectList;
                    }
                }
                if (formcollection["BookInfo"] != null)
                {
                    if (formcollection["listBoxBooks"] != null)
                    {
                        selectetItemListBox = formcollection["listBoxBooks"].ToString();
                        for (int i = 0; i < bookRepository.ListBooks.Count; i++)
                        {
                            tempBook = bookRepository.ListBooks[i];
                            if (bookRepository.ListBooks[i].Name == selectetItemListBox)
                            {
                                TempData["Book"] = bookRepository.ListBooks[i];
                            }
                        }
                    }
                }
                if (formcollection["Manager"] != null)
                {
                    return RedirectToAction("Index", new { controller = "GeneralEdit" });
                }
            }
            return View(authorRepository.GetAll());
        }
    }
}