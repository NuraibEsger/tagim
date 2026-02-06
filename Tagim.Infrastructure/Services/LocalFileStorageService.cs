using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Tagim.Application.Interfaces;

namespace Tagim.Infrastructure.Services;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    public async Task<string> SaveFileAsync(IFormFile file, string folderName)
    {
        var uploadPath = Path.Combine(env.WebRootPath, "uploads", folderName);
        
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);
        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        return $"/uploads/{folderName}/{fileName}";
    }

    public void DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        var fullPath = Path.Combine(env.WebRootPath, filePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}