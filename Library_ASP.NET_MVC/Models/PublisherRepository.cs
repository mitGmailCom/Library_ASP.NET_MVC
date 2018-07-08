using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_ASP.NET_MVC.Models
{
    public class PublisherRepository : IPublisherRepository<Publisher>
    {
        private static List<Publisher> Publishers = new List<Publisher>();

        private static PublisherRepository _instance;
        public static PublisherRepository Instance => _instance ?? (_instance = new PublisherRepository());

        public List<Publisher> ListPublishers
        {
            get { return Publishers; }
            set { Publishers = value; }
        }

        public Publisher Get(object param)
        {
            if (param is string)
            {
                for (int i = 0; i < Publishers.Count; i++)
                {
                    if (Publishers[i].Name == (string)param)
                        return Publishers[i];
                }
            }
            return null;
        }


        public IEnumerable<Publisher> GetAll() => Publishers;


        public void Create(Publisher item)
        {
            if (item != null)
            {
                if (Publishers.Count > 0)
                {
                    for (int i = 0; i < Publishers.Count; i++)
                    {
                        if (Publishers[i].Name != null)
                        {
                            if (Publishers[i].Name == item.Name)
                                throw new ArgumentException("Publisher exists");
                        }
                    }
                }
                Publishers.Add(item);
            }
        }


        public bool Delete(object param)
        {
            if (param != null)
            {
                if (param is string)
                {
                    for (int i = 0; i < Publishers.Count; i++)
                    {
                        if (Publishers[i].Name == (string)param)
                        {
                            Publishers.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public void Edite(Publisher item, int indx)
        {
           if (item != null)
            {
                if (indx >= 0 && indx < Publishers.Count)
                {
                    Publishers[indx] = item;
                }
            }
        }
    }
}