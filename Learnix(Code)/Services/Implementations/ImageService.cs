using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string Upload(IFormFile imageFile, string folderName)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            // Create upload folder if not exists
            string uploadFolder = Path.Combine(_env.WebRootPath, "Images", folderName);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // Generate unique name
            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // Return relative path (e.g. "uploads/teams/file.png")
            return Path.Combine("Images", folderName, uniqueFileName).Replace("\\", "/");
        }

        public void Delete(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            string fullPath = Path.Combine(_env.WebRootPath, imagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool Exists(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            string fullPath = Path.Combine(_env.WebRootPath, imagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            return File.Exists(fullPath);
        }
    }
}
