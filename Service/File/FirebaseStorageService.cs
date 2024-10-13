using Firebase.Storage;
namespace brandportal_dotnet.Service.File
{
    public class FirebaseStorageService
    {
        private readonly string _bucket;

        public FirebaseStorageService(string bucket)
        {
            _bucket = bucket;
        }

        public async Task<string> UploadFileAsync(string folderName, string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var storagePath = $"{folderName}/{fileName}";

            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                try
                {
                    // Await the upload task to get the download URL
                    var downloadUrl = await new FirebaseStorage(_bucket)
                        .Child(storagePath)
                        .PutAsync(fileStream);

                    return downloadUrl; // Return the download URL
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during upload
                    Console.WriteLine($"Error uploading file: {ex.Message}");
                    throw; // Rethrow the exception if needed
                }
            }
        }

        

    }
}
