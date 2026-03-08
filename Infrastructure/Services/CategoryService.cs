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

    public Task<Response<string>> UpdateCategory(int id, UpdatedCategory category)
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<GetCategory>>> GetAllCategories()
    {
        throw new NotImplementedException();
    }

    public Response<string> DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }
}