using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Support110Media.DataAccess.MongoDb.Repository
{
    public class MongoRepository<T> : IDisposable, IMongoRepository<T> where T : class
    {
        //All mongodb databases and collections generated from mongoclient and mongoclient itself is threadsafe. because of that caching is logical choice.
        private static readonly ReaderWriterLockSlim _databaseLocker = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim _collecionLocker = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim _clientLocker = new ReaderWriterLockSlim();

        private static Dictionary<string, IMongoDatabase> Databases { get; set; } = new Dictionary<string, IMongoDatabase>();
        private static Dictionary<string, object> Collections { get; set; } = new Dictionary<string, object>();
        private static Dictionary<string, MongoClient> MongoClients { get; set; } = new Dictionary<string, MongoClient>();

        private IMongoCollection<T> GetCollection(IDatabaseSettings dbSettings = null)
        {
            if (dbSettings == null)
            {
                dbSettings = new DatabaseSettings()
                {
                    CollectionName = typeof(T).Name,
                    ConnectionString = Environment.GetEnvironmentVariable("MONGO_URI"),
                    DatabaseName = Environment.GetEnvironmentVariable("MONGO_DB"),
                };
            }

            string key = string.Format("{0}/{1}.{2}", dbSettings.ConnectionString.TrimEnd('/'), dbSettings.DatabaseName, dbSettings.CollectionName);

            object collection = null;
            _collecionLocker.EnterReadLock();
            try
            {
                if (Collections.ContainsKey(key))
                {
                    collection = Collections[key];
                }
                else
                {
                    _collecionLocker.ExitReadLock();
                    _collecionLocker.EnterWriteLock();
                    try
                    {
                        if (!Collections.ContainsKey(key))
                        {
                            IMongoDatabase database = GetDatabase(dbSettings);
                            if (database.GetCollection<T>(typeof(T).Name) == null)
                            {
                                database.CreateCollection(typeof(T).Name);
                            }
                            collection = database.GetCollection<T>(typeof(T).Name);
                            Collections.Add(key, collection);
                        }
                        collection = Collections[key];
                    }
                    finally
                    {
                        _collecionLocker.ExitWriteLock();
                    }
                }
            }
            finally
            {
                if (_collecionLocker.IsReadLockHeld)
                    _collecionLocker.ExitReadLock();
            }
            return collection as IMongoCollection<T>;
        }

        private IMongoDatabase GetDatabase(IDatabaseSettings dbSettings)
        {
            IMongoDatabase database = null;
            _databaseLocker.EnterReadLock();
            try
            {
                if (Databases.ContainsKey(dbSettings.DatabaseName))
                {
                    database = Databases[dbSettings.DatabaseName];
                }
                else
                {
                    _databaseLocker.ExitReadLock();
                    _databaseLocker.EnterWriteLock();
                    try
                    {
                        if (!MongoClients.ContainsKey(dbSettings.ConnectionString))
                        {
                            database = GetClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);
                            Databases.Add(dbSettings.ConnectionString, database);
                        }
                        database = Databases[dbSettings.ConnectionString];
                    }
                    finally
                    {
                        _databaseLocker.ExitWriteLock();
                    }
                }
            }
            finally
            {
                if (_databaseLocker.IsReadLockHeld)
                    _databaseLocker.ExitReadLock();
            }
            return database;
        }

        private MongoClient GetClient(string connectionString)
        {
            MongoClient client = null;
            _clientLocker.EnterReadLock();
            try
            {
                if (MongoClients.ContainsKey(connectionString))
                {
                    client = MongoClients[connectionString];
                }
                else
                {
                    _clientLocker.ExitReadLock();
                    _clientLocker.EnterWriteLock();
                    try
                    {
                        if (!MongoClients.ContainsKey(connectionString))
                        {
                            client = new MongoClient(connectionString);
                            MongoClients.Add(connectionString, client);
                        }
                        client = MongoClients[connectionString];
                    }
                    finally
                    {
                        _clientLocker.ExitWriteLock();
                    }
                }
            }
            finally
            {
                if (_clientLocker.IsReadLockHeld)
                    _clientLocker.ExitReadLock();
            }
            return client;
        }

        public IMongoCollection<T> CurrentCollection { get; set; }

        public MongoRepository(IDatabaseSettings dbSettings = null)
        {
            CurrentCollection = GetCollection(dbSettings);
        }

        public void Add(T entity)
        {
            CurrentCollection.InsertOne(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate, bool forceDelete = false)
        {
            CurrentCollection.DeleteOne(predicate);
        }

        public void Update(Expression<Func<T, bool>> predicate, T entity)
        {
            CurrentCollection.ReplaceOneAsync(predicate, entity);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return (int)CurrentCollection.Find(predicate).CountDocuments();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return CurrentCollection.Find(predicate).ToEnumerable().AsQueryable();
        }

        /// <summary>
        /// Şarta göre tek veri getirir
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return CurrentCollection.Find(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Aynı kayıt eklememek için objeyi kontrol ederek true veya false dönderir.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return CurrentCollection.Find(predicate).FirstOrDefault() != null;
        }

        IQueryable<T> IMongoRepository<T>.GetAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public IQueryable<dynamic> SelectList(Expression<Func<T, bool>> where, Expression<Func<T, dynamic>> select)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetDataPart(Expression<Func<T, bool>> where, Expression<Func<T, dynamic>> sort, SortTypeEnum sortType, int skipCount, int takeCount)
        {
            throw new NotImplementedException();
        }

        public List<T> SendSql(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity, bool forceDelete = false)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
