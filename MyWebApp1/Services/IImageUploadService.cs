namespace MyWebApp1.Services
{
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
