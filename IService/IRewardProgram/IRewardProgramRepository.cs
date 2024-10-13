namespace brandportal_dotnet.IService.IRewardProgram
{
    public interface IRewardProgramRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);
        Task<T> FindByProperties(Dictionary<string, object> conditions);
        Task Insert(T item);
        Task<T> Insert(T item, bool autoSave);
        Task Update(string id, T item);
        Task Delete(string id);
    }
}