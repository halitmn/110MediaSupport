using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Support110Media.DataAccess.Repository
{
    public interface IRepository<T> where T :class
    {
        IQueryable<T> GetAll();

        T Get(Expression<Func<T, bool>> predicate);

        T GetById(int id);

        void Add(T model);

        void Update(T model);

        void Delete(int id);
    }
}
