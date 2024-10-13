using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.IService.IFile
{
    public interface IFileRepository
    {
        Task<Stream?> GetAsync(string fileName);
        Task<string> UploadAsync([FromForm] IFormFile file);
        Task<string> GetFileUrlAsync(string fileName, Guid? tenantId = null);
    }
}

