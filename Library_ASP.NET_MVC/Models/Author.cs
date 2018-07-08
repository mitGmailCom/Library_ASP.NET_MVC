using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_ASP.NET_MVC.Models
{
    public class Author
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
    }
}