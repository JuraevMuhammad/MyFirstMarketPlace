using Application.DTOs.Category;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<Response<string>>  CreateCategory(CreatedCategory category);
    Task<Response<string>>  UpdateCategory(int id, UpdatedCategory category);
    Task<Response<List<GetCategory>>> GetAllCategories();
    Task<Response<string>> DeleteCategory(int id);
}