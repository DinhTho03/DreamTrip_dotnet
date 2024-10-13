using brandportal_dotnet.Contracts.File;
using brandportal_dotnet.Service.File;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
namespace brandportal_dotnet.Files
{
    [ApiController]
    [Route("[controller]")]
    public class FileAppService : ControllerBase
    {
        private readonly FirebaseStorageService _storageService;
        private string bucket = "dreamtrip-415eb.appspot.com";
        public FileAppService()
        {
            // Initialize FirebaseStorageService with your bucket name
            _storageService = new FirebaseStorageService("dreamtrip-415eb.appspot.com");
        }


        // POST: api/FirebaseStorage/upload
        [HttpPost("~/api/files")]
        public async Task<string> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "No file uploaded.";
            }

            var key = Guid.NewGuid().ToString();

            var filePath = Path.Combine(Path.GetTempPath(), file.FileName + "_" + key);

            // Save the uploaded file temporarily
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                // Use the default folder name "files" for uploads
                string downloadUrl = await _storageService.UploadFileAsync("files", filePath);
                return file.FileName + "_" + key;
            }
            catch (Exception ex)
            {
                return "Error uploading file: {ex.Message}";
            }
         
        }

        // GET: api/FirebaseStorage/retrieve
        [HttpGet("/api/files/url/{fileName}")]
        public async Task<FileDto> GetFileUrls(string fileName)
        {
            // Logic for retrieving file URLs can be implemented here
            return new FileDto { fileName = $"https://firebasestorage.googleapis.com/v0/b/{bucket}/o/files%2F{fileName}?alt=media" };
        }
    }
}