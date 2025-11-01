namespace Selfcare_meets_Beautify.Services
{
	public interface IUploadService
	{
		Task<string?> FileSave(IFormFile? file);
	}

	public class UploadService : IUploadService
	{
		string webPath;
		public UploadService(IWebHostEnvironment env)
		{
			webPath = env.WebRootPath;
		}
        public async Task<string?> FileSave(IFormFile? file)
        {
            if (file != null)
            {
                // Generate a unique file name (e.g., using GUID)
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string relativePath = $"/images/{uniqueFileName}";

                string uploadPath = Path.Combine(webPath, relativePath.TrimStart('/')); // Use Path.Combine for safety

                // Ensure the directory exists (best practice)
                Directory.CreateDirectory(Path.GetDirectoryName(uploadPath)!);

                using var stream = System.IO.File.Create(uploadPath);
                await file.CopyToAsync(stream);

                return relativePath; // Return the path relative to wwwroot
            }
            return null;
        }
    }
}
