using System;
using System.Collections.Generic;
using System.Text;

namespace Support110Media.DataAccess.MongoDb.Repository
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string CollectionName { get; set; }
    }
}
