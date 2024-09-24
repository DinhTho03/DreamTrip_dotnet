using MongoDB.Bson;

namespace brandportal_dotnet.IService;

public interface IRepository<T>
{
    Task<List<T>> GetAll();
    Task<T> GetById(string id);
    Task<T> FindByProperties(Dictionary<string, object> conditions);
    Task Insert(T item);
    Task Update(string id, T item);
    Task Delete(string id);
}