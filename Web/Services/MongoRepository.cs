using System.Threading.Tasks;
using MongoDB.Driver;
using Web.Data;

namespace Web.Services
{
    public class MongoRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(string connectionString, string database, string collection)
        {
            _client = new MongoClient(connectionString);
            
            var db = _client.GetDatabase(database);
            _collection = db.GetCollection<T>(collection);
        }

        public async Task<bool> Contains(string id)
        {
            var count = await _collection.CountAsync(t => t.Id == id);
            return count > 0;
        }

        public async Task<T> Get(string id)
        {
            var result = await _collection.FindAsync(t => t.Id == id);
            return result.Single();
        }

        public async Task Set(T item, string id)
        {
            await _collection.InsertOneAsync(item);
        }
    }
}