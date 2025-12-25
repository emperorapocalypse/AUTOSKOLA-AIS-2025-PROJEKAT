namespace Autoskola.MVC.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Nedozvoljen format slike. Dozvoljeni su: JPG, JPEG, PNG, GIF");

            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("Slika je prevelika. Maksimalna veličina je 5MB.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/{folder}/{fileName}";
        }

        public async Task<bool> DeleteImageAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                var path = filePath.TrimStart('/');
                var fullPath = Path.Combine(_environment.WebRootPath, path);

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public string GetDefaultImage()
        {
            return "/images/kandidati/default-avatar.png";
        }
    }
}