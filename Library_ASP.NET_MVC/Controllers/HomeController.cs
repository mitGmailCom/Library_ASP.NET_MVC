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
        private BookReposirory bookRepository = BookReposirory.Instance;


       
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
                authorRepository.ListAuthors.Add(new Author { Name = "James Chambers", DateOfBirth = new DateTime(1971, 08, 18), DateOfDeath = null });
            }

            if (bookRepository.ListBooks.Count == 0)
            {
                bookRepository.ListBooks.Add(new Book {
                    Id = 0,
                    Authors = authorRepository.ListAuthors[0] as IEnumerable<Author>,
                    ISBN = "978-1-118-79475-3",
                    PageCount = 620,
                    PublishDate = new DateTime(2014),
                    Publisher = publisherRepository.ListPublishers[0],
                    Name = "Professional ASP.NET MVC 5"
                });
                bookRepository.ListBooks.Add(new Book
                {
                    Id = 1,
                    Authors = authorRepository.ListAuthors[1] as IEnumerable<Author>,
                    ISBN = "978-5-496-03071-7",
                    PageCount = 464,
                    PublishDate = new DateTime(2018),
                    Publisher = publisherRepository.ListPublishers[1],
                    Name = "ASP.NET Core. Development: Building an application in four sprints."
                });
            }
            ViewBag.AllAuthors = authorRepository.GetAll();
            return View(authorRepository.GetAll());
        }
    }
}