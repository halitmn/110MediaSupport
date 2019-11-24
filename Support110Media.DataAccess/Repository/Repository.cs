using Microsoft.EntityFrameworkCore;
using Support110Media.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Support110Media.DataAccess.Repository
{
    class Repository<T> : IRepository<T> where T : class
    {
        public Repository(MasterContext context)
        {
            if (context != null)
            {
                masterContext = context;
                dbSet = masterContext.Set<T>();
            }

        }

        private readonly MasterContext masterContext;
        private readonly DbSet<T> dbSet;

        public void Add(T model)
        {
            if (model != null)
            {
                dbSet.Add(model);
            }
        }

        public void Delete(int id)
        {
            if (id != 0)
            {
                var model = dbSet.Find(id);
                dbSet.Remove(model);
            }
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> iQueryable = dbSet.Where(predicate);
            return iQueryable.ToList().FirstOrDefault();
        }


        public T GetById(int id)
        {
            if (id != 0)
                return dbSet.Find(id);
            else
                return null;
        }

        public void Update(T model)
        {
            if (model != null)
                masterContext.Entry(model).State = EntityState.Modified;
        }


    }
}
