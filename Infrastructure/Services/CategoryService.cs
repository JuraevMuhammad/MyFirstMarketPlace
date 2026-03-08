using System.Net;
using Application.DTOs.Category;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    #region CreateCategory

    public async Task<Response<string>> CreateCategory(CreatedCategory category)
    {
        var create = new Category()
        {
            Name = category.Name,
        };
        _logger.LogInformation($"Adding in progress for category {category.Name}...");
        var result = await _repository.CreateCategoryAsync(create);
        _logger.LogInformation($"Category {category.Name} created successfully");
        return new Response<string>(HttpStatusCode.Created, "create category");
    }

    #endregion

    #region UpdateCategory

    public async Task<Response<string>> UpdateCategory(int id, UpdatedCategory category)
    {
        var res = await _repository.GetCategoryAsync(id);
        if (res == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        
        res.Name = category.Name ?? res.Name;
        res.UpdatedAt = DateTime.UtcNow;
        
        var result = await _repository.UpdateSaveAsync(res);
        
        if (result <= 0) 
            return new Response<string>(HttpStatusCode.BadRequest, "updated failed");
        
        _logger.LogInformation($"Category {res.Name} saved successfully");
        return new Response<string>(HttpStatusCode.OK, $"update category name -> {res.Name}");
    }

    #endregion

    public async Task<Response<List<GetCategory>>> GetAllCategories()
    {
        var categories = await _repository.GetCategoriesAsync();
        if (categories == null)
            return new Response<List<GetCategory>>(HttpStatusCode.NotFound, "not found");
        var getCategory = categories.Select(x => new GetCategory()
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();
        _logger.LogInformation($"type: <Category> -> <GetCategories>");
        return new Response<List<GetCategory>>(getCategory);
    }

    public async Task<Response<string>> DeleteCategory(int id)
    {
        var category = await _repository.DeleteSaveAsync(id);
        if (category <= 0)
            return new Response<string>(HttpStatusCode.NotFound, "not found");
        _logger.LogInformation($"IsDeleted: false -> true");
        return new Response<string>(HttpStatusCode.OK, "is deleted success");
    }
}