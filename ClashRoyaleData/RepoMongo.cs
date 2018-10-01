using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ClashRoyaleData
{
    public class RepoMongo<T, TKey> where T : class, new()
    {
        internal MongoClient MongoClient { get; set; }
        internal IMongoDatabase DataBase { get; set; }
        public Expression<Func<T, TKey>> _idSelector;
        private string _keyPropertyName;
        public Func<T, TKey> _funcIdSelector;
        //private Func<T, T, bool> _equalFunc;
        //private Func<T,string, bool> _equalMemberFunc;

        internal Expression<Func<T, TKey>> IdSelector
        {
            get { return _idSelector; }
            set
            {
                _idSelector = value;
                _keyPropertyName = ((MemberExpression)value.Body).Member.Name;
   
                _funcIdSelector = _idSelector.Compile();
                //_equalFunc = LambdaExtensions.CreateEqual(_idSelector);
                //_equalMemberFunc = LambdaExtensions.CreateEqualMember(_idSelector);
            }
        }

        private IMongoCollection<T> _collection;
        protected IMongoCollection<T> Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = DataBase.GetCollection<T>(typeof(T).Name);
                }
                return _collection;
            }
        }

        public void DeleteAll()
        {
            Collection.DeleteMany(prop => true);
        }
        public void Insert(T item)
        {
            Collection.InsertOne(item);
        }
        public void UpdateInsert(T item)
        {
            var key = _funcIdSelector(item);
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(_keyPropertyName, key);
            Collection.ReplaceOne(filter, item, new UpdateOptions { IsUpsert = true });
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> filter)
        {
            return Collection.Find(filter).ToEnumerable();
        }

        public IEnumerable<T> Get(TKey key)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(_keyPropertyName, key);
            return Collection.Find(filter).ToEnumerable();
        }
    }
}