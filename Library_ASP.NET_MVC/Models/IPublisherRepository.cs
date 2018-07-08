using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_ASP.NET_MVC.Models
{
    interface IPublisherRepository<T>
    {
        T Get(object param);
        IEnumerable<T> GetAll();
        void Create(T item);
        bool Delete(object param);
        void Edite(T item, int index);
    }
}
