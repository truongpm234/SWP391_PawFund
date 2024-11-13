using MyWebApp1.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public ImageUploadService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file was uploaded.");
        }

        // Validate file type (e.g., .jpg, .png)
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, .png, and .gif files are allowed.");
        }

        // Validate file size (e.g., limit to 5 MB)
        if (file.Length > 10 * 1024 * 1024) // 5 MB
        {
            throw new ArgumentException("File size exceeds the 5 MB limit.");
        }

        var fileName = Guid.NewGuid().ToString() + fileExtension;
        var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");

        // Create directory if it does not exist
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        var filePath = Path.Combine(uploadDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var baseUrl = _configuration["AppSettings:BaseUrl"];
        var imageUrl = $"{baseUrl}/images/{fileName}";
        return imageUrl;
    }
}
