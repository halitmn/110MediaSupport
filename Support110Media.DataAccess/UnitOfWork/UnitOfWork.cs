using Support110Media.Data.Context;
using Support110Media.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Support110Media.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly MasterContext masterContext;

        public UnitOfWork(MasterContext context)
        {
            if (context != null)
                masterContext = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(masterContext);
        }

        public int SaveChanges()
        {
            return masterContext.SaveChanges();
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
