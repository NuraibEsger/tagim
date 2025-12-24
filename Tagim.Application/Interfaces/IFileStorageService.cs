using Microsoft.AspNetCore.Http;
namespace Tagim.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string folderName);
    
    void DeleteFile(string filePath);
}