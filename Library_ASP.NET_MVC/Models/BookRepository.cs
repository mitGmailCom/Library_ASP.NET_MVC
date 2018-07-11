using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_ASP.NET_MVC.Models
{
    public class BookRepository : IPublisherRepository<Book>
    {
        private static List<Book> Books = new List<Book>();

        private static BookRepository _instance;
        public static BookRepository Instance => _instance ?? (_instance = new BookRepository());

        public List<Book> ListBooks
        {
            get { return Books; }
            set { Books = value; }
        }

        public void Create(Book item)
        {
            if (item != null)
            {
                if (Books.Count > 0)
                {
                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].ISBN == item.ISBN)
                            throw new ArgumentException("Book exists");
                    }
                    item.Id = (Books.LastOrDefault()?.Id ?? 0) + 1;
                    Books.Add(item);
                }
            }
        }


        public bool Delete(object param)
        {
            if (param != null)
            {
                if (param is int)
                {
                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].Id == (int)param)
                        {
                            Books.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public void Edite(Book item, int index)
        {
            if (item != null)
                Books[item.Id] = item;
        }


        public Book Get(object param)
        {
            if (param != null)
            {
                if (param is int)
                {
                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].Id == (int)param)
                            return Books[i];
                    }
                }
            }
            return null;
        }


        public IEnumerable<Book> GetAll() => Books;
    }
}