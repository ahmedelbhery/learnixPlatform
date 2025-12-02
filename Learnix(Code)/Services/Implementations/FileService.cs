using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, IEnumerable<string> allowedExtensions = null)
        {
            if (file == null || file.Length == 0)
                return null;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (allowedExtensions != null && !allowedExtensions.Contains(ext))
                return null;

            // Build path like wwwroot/Files/<folderName>
            var rootPath = Path.Combine(_env.WebRootPath, "Files", folderName);

            Directory.CreateDirectory(rootPath);

            var storedName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(rootPath, storedName);

            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return $"/Files/{folderName}/{storedName}";
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task<byte[]> ReadFileBytesAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found.");

            return await File.ReadAllBytesAsync(fullPath);
        }

        public Task<bool> FileExistsAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            return Task.FromResult(File.Exists(fullPath));
        }
    }
}
