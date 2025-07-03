namespace CareHomeInfoTracker.Services.ImageFiles
{
    public interface IFileUploadService
    {
        Task<string?> SaveImageAsync(IFormFile file, string targetFolder, int maxSizeMB);
    }
}
