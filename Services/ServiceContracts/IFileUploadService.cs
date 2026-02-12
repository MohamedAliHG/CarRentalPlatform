namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IFileUploadService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "voitures");
        Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> files, string folder = "voitures");
    }
}