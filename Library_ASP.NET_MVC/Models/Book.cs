using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_ASP.NET_MVC.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Publisher Publisher { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public DateTime PublishDate { get; set; }
        public int PageCount { get; set; }
        public string ISBN { get; set; }
        public string[] SelectedItems { get; set; }
    }
}