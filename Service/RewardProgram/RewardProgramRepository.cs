using brandportal_dotnet.Data.Utils;
using brandportal_dotnet.IService.IRewardProgram;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using brandportal_dotnet.Configuration;

namespace brandportal_dotnet.Service.RewardProgram
{
    public class RewardProgramRepository<T> : IRewardProgramRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public RewardProgramRepository(IOptions<DatabaseSettings> databaseSettings)
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

        public async Task<T> Insert(T item,bool autoSave)
        {
            await _collection.InsertOneAsync(item); 
            return item; 
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
    }
}
