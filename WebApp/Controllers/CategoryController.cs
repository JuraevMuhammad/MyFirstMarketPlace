using Application.DTOs.Category;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromQuery] CreatedCategory category)
    {
        var res = await service.CreateCategory(category);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.User) + "," + nameof(Roles.Customer))]
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var res = await service.GetAllCategories();
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    [HttpPut]
    public async Task<IActionResult> UpdateCategory(int id, [FromQuery] UpdatedCategory category)
    {
        var res = await service.UpdateCategory(id, category);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var res = await service.DeleteCategory(id);
        return StatusCode(res.StatusCode, res);
    }
}