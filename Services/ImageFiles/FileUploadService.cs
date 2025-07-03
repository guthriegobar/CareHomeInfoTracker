namespace CareHomeInfoTracker.Services.ImageFiles
{
    public class FileUploadService: IFileUploadService
    {
        public async Task<string?> SaveImageAsync(IFormFile file, string targetFolder, int maxSizeMB = 2)
        {
            if(file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] {".jpg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Check Extension of the file provided
            if(!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Unsupported File type.");
            if (file.Length > maxSizeMB * 1024 * 1024)
                throw new InvalidOperationException("File size exceeds limit.");

            var fileName = Guid.NewGuid() + extension;
            var imagePath = Path.Combine(targetFolder, fileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return $"{fileName}";
        }
    }
}
