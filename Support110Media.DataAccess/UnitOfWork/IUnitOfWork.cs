using Support110Media.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Support110Media.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();
    }
}
