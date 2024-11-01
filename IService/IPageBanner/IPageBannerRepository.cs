using System.Linq.Expressions;

namespace brandportal_dotnet.IService.IPageBanner
{
    public interface IPageBannerRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);
        Task<T> FindByProperties(Dictionary<string, object> conditions);
        Task Insert(T item);
        Task Update(string id, T item);
        Task Delete(string id);
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task DeleteMany(string pageId, IEnumerable<string> bannerIds);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter);
    }
}
