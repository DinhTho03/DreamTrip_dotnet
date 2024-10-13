using brandportal_dotnet.IService;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using brandportal_dotnet.Configuration;
using brandportal_dotnet.Data.Utils;

namespace StreamAPI.Service;

public class MongoRepository<T> : IRepository<T>
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var client = new MongoClient((databaseSettings.Value.ConnectionString));
        var database = client.GetDatabase((databaseSettings.Value.DatabaseName));
        var attribute = (BsonConllectionAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(BsonConllectionAttribute));
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

    public async Task<List<T>> GetListById(string id)
    {
        var filter =Builders<T>.Filter.Eq("_id", ObjectId.Parse(id)); 
        return _collection.Find(filter).ToList();
    }

    public async Task<List<T>> FindListByProperties(Dictionary<string, object> conditions)
    {
        var filters = new List<FilterDefinition<T>>();

        foreach (var condition in conditions)
        {
            var filter = Builders<T>.Filter.Eq(condition.Key, condition.Value);
            filters.Add(filter);
        }

        var combinedFilter = Builders<T>.Filter.And(filters);

        return _collection.Find(combinedFilter).ToList();
    }

    public async Task Insert(T item)
    {
        await _collection.InsertOneAsync(item);
    }

    public async Task Update(string id, T item)
    {
        var filter = Builders<T>.Filter.Eq("_id",ObjectId.Parse(id));
        await _collection.ReplaceOneAsync(filter, item);
    }

    public async Task Delete(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        await _collection.DeleteOneAsync(filter);
    }
    
    public async Task DeleteAll(List<string> ids)
    {
        var filter = Builders<T>.Filter.In("_id", ids.Select(id => ObjectId.Parse(id)));
        await _collection.DeleteManyAsync(filter); 
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