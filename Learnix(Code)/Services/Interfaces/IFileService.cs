namespace Learnix.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, IEnumerable<string> allowedExtensions = null);
        Task<bool> DeleteFileAsync(string filePath);
        Task<byte[]> ReadFileBytesAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);
    }
}
