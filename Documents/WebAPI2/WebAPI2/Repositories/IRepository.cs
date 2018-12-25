using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI2.Models;

namespace WebAPI2.Repositories
{
    public interface IRepository<T> where T:class,IEntity
    {
        List<T> List { get; }
        T Find(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
