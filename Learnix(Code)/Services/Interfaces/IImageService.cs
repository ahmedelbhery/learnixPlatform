namespace Learnix.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Uploads an image and returns its relative path.
        /// </summary>
        string Upload(IFormFile imageFile, string folderName);

        /// <summary>
        /// Deletes an image from the server.
        /// </summary>
        void Delete(string imagePath);

        /// <summary>
        /// Checks if an image exists on disk.
        /// </summary>
        bool Exists(string imagePath);
    }
}
