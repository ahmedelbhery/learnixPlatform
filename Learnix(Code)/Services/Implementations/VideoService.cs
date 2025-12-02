using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class VideoService : IVideoService
    {
        private readonly IWebHostEnvironment _env;

        private static readonly HashSet<string> AllowedVideoExtensions = new()
        {
        ".mp4", ".mov", ".mkv", ".webm"
        };

        public VideoService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile, string folderName)
        {
            if (videoFile == null || videoFile.Length == 0)
                return null;

            var ext = Path.GetExtension(videoFile.FileName).ToLowerInvariant();
            if (!AllowedVideoExtensions.Contains(ext))
                return null;

            // Build path like: wwwroot/Videos/<folderName>
            var rootPath = Path.Combine(_env.WebRootPath, "Videos", folderName);

            Directory.CreateDirectory(rootPath);

            var storedName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(rootPath, storedName);

            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                await videoFile.CopyToAsync(fs);
            }

            // return relative path for storing in DB or using in <video> tag
            return $"/Videos/{folderName}/{storedName}";
        }

        public Task<bool> DeleteVideoAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> VideoExistsAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            return Task.FromResult(File.Exists(fullPath));
        }

        public Task<FileStream> GetVideoStreamAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Video not found.");

            return Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
        }

    }
}
