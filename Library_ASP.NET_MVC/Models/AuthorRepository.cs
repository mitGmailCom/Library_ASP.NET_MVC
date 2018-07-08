using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_ASP.NET_MVC.Models
{
    public class AuthorRepository : IPublisherRepository<Author>
    {
        public static List<Author> Authors = new List<Author>();

        private static AuthorRepository _instance;
        public static AuthorRepository Instance => _instance ?? (_instance = new AuthorRepository());

        public List<Author> ListAuthors
        {
            get { return Authors; }
            set { Authors = value; }
        }

        public void Create(Author item)
        {
            if (item != null)
            {
                if (Authors.Count > 0)
                {
                    for (int i = 0; i < Authors.Count; i++)
                    {
                        if (Authors[i].Name == item.Name)
                            throw new ArgumentException("Author exists");
                    }
                    //Authors.Add(item);
                }
                //if (Authors.Count == 0)
                    Authors.Add(item);
            }
        }


        public bool Delete(object param)
        {
           if (param != null)
            {
                if (param is string)
                {
                    for (int i = 0; i < Authors.Count; i++)
                    {
                        if (Authors[i].Name == (string)param)
                        {
                            Authors.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public void Edite(Author item, int index)
        {
            if (item != null)
            {
                if (index >= 0 && index < Authors.Count)
                {
                    Authors[index] = item;
                }
            }
        }


        public Author Get(object param)
        {
            if (param != null)
            {
                if (param is string)
                {
                    for (int i = 0; i < Authors.Count; i++)
                    {
                        if (Authors[i].Name == (string)param)
                            return Authors[i];
                    }
                }
            }
            return null;
        }


        public IEnumerable<Author> GetAll() => Authors;
        
    }
}