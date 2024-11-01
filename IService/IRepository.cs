using MongoDB.Bson;
using MongoDB.Driver;

namespace brandportal_dotnet.IService;

public interface IRepository<T>
{
    Task<List<T>> GetAll();
    Task<T> GetById(string id);
    Task<List<T>> GetListById(string id);
    Task<T> FindByProperties(Dictionary<string, object> conditions);
    Task<List<T>> FindListByProperties(Dictionary<string, object> conditions);
    Task Insert(T item);
    Task<T> Insert(T item, bool autoSave);
    Task Update(string id, T item);
    Task Delete(string id);
    Task DeleteAll(List<string> ids);
}