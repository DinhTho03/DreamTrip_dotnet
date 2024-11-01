using System.Linq.Expressions;
using brandportal_dotnet.Configuration;
using brandportal_dotnet.Data.Utils;
using brandportal_dotnet.IService.IPageBanner;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace brandportal_dotnet.Service.PageBanner
{
    public class PageBannerRepository<T> : IPageBannerRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public PageBannerRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var client = new MongoClient((databaseSettings.Value.ConnectionString));
            var database = client.GetDatabase((databaseSettings.Value.DatabaseName));
            var attribute =
                (BsonConllectionAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(BsonConllectionAttribute));
            string collectionName = attribute.CollectionName;
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public async Task<T> GetById(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Insert(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task Update(string id, T item)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.ReplaceOneAsync(filter, item);
        }

        public async Task Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<T> FindByProperties(Dictionary<string, object> conditions)
        {
            var filters = new List<FilterDefinition<T>>();

            foreach (var condition in conditions)
            {
                var filter = Builders<T>.Filter.Eq(condition.Key, condition.Value);
                filters.Add(filter);
            }

            var combinedFilter = Builders<T>.Filter.And(filters);

            return await _collection.Find(combinedFilter).FirstOrDefaultAsync();
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            // Get the count of documents that match the filter
            long count = await _collection.CountDocumentsAsync(filter);
    
            // Cast the long count to int and return it
            return (int)count; // Note: This may throw an exception if count exceeds int.MaxValue
        }

        public async Task DeleteMany(string pageId, IEnumerable<string> bannerIds)
        {
            var filter = Builders<T>.Filter.And(
                Builders<T>.Filter.Eq("PageId", pageId),
                Builders<T>.Filter.In("_id", bannerIds.Select(id => ObjectId.Parse(id)))
            );

            await _collection.DeleteManyAsync(filter);
        }
        
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }
    }
}