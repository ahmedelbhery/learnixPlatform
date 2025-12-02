namespace Learnix.Services.Interfaces
{
    public interface IVideoService
    {
        Task<string> UploadVideoAsync(IFormFile videoFile, string folderName);
        Task<bool> DeleteVideoAsync(string filePath);
        Task<FileStream> GetVideoStreamAsync(string filePath);
        Task<bool> VideoExistsAsync(string filePath);
    }
}


