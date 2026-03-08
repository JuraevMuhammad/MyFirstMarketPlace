using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.SaveFile;

public class FileStorage : IFileStorage
{
    private string _rootPath;

    public FileStorage(IWebHostEnvironment env)
    {
        _rootPath = env.ContentRootPath;
    }
    
    public async Task<string> SaveFileAsync(IFormFile file, string relativeFolder)
    {
        var folder = Path.Combine(_rootPath, "wwwroot", relativeFolder);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(folder, fileName);// wwwroot/image/126hhsjd-dsfhjs211-sdfbjh12-12.png

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Path.Combine(relativeFolder, fileName).Replace("\\", "/");
    }
    
    public async Task<List<string>> SaveFilesAsync(List<IFormFile> files, string relativeFolder)
    {
        var paths = new List<string>();

        foreach (var file in files)
        {
            var path = await SaveFileAsync(file, relativeFolder);
            paths.Add(path);
        }

        return paths;
    }

    public Task DeleteFileAsync(string relativePath)
    {
        var full = Path.Combine(_rootPath, "wwwroot", relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (File.Exists(full)) File.Delete(full);
        return Task.CompletedTask;
    }
}