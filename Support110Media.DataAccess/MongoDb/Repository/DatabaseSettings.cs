using System;
using System.Collections.Generic;
using System.Text;

namespace Support110Media.DataAccess.MongoDb.Repository
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }
    }
}
