using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 10 * 1024 * 1024;

        public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folder = "voitures")
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    _logger.LogWarning("Extension de fichier non autorisée: {Extension}", extension);
                    return null;
                }

                if (file.Length > MaxFileSize)
                {
                    _logger.LogWarning("Fichier trop volumineux: {Size} bytes. Maximum autorisé: {MaxSize} bytes", file.Length, MaxFileSize);
                    return null;
                }

 
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"/uploads/{folder}/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'upload du fichier");
                return null;
            }
        }

        public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> files, string folder = "voitures")
        {
            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                var url = await UploadImageAsync(file, folder);
                if (url != null)
                {
                    uploadedUrls.Add(url);
                }
            }

            return uploadedUrls;
        }
    }
}