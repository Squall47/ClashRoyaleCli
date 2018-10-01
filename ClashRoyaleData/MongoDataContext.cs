using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ClashRoyaleData
{
    public class MongoDataContext
    {
        public MongoClient MongoClient { get; private set; }
        public IMongoDatabase DataBase { get; private set; }
        protected readonly ConcurrentDictionary<Type, dynamic> Repositories = new ConcurrentDictionary<Type, dynamic>();

        public MongoDataContext(string mongoConnectionString, string database)
        {
            MongoClient = new MongoClient(mongoConnectionString);
            DataBase = MongoClient.GetDatabase(database);
        }

        public TRepo Repository<TRepo, TEntity, TKey>(Expression<Func<TEntity, TKey>> idSelector) 
            where TRepo : RepoMongo<TEntity, TKey>, new()
            where TEntity : class, new()
        {
            var type = typeof(TEntity);

            if (!Repositories.ContainsKey(type))
            {
                var repositoryInstance = new TRepo() { MongoClient = MongoClient, DataBase = DataBase, IdSelector = idSelector };
                Repositories.AddOrUpdate(type, repositoryInstance, (keyType, oldRepo) => repositoryInstance);
            }
            return (TRepo)Repositories[type];
        }
    }
}
