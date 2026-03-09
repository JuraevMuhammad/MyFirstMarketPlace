using Application.DTOs.ItemProduct;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.SaveFile;

public interface IFileStorage
{
    Task<string> SaveFileAsync(IFormFile file, string relativeFolder); 
    Task<List<string>> SaveFilesAsync(List<IFormFile> files, string relativeFolder);
    Task DeleteFileAsync(string relativePath);
}