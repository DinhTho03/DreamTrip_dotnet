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
    }
}
