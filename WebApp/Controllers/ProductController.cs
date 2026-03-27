using Application.DTOs.Product;
using Application.Filter;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService service) : ControllerBase
{
    [Authorize(Roles = nameof(Roles.User) + "," + nameof(Roles.Admin))]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromQuery]CreatedProduct dto)
    {
        var res = await service.CreateProduct(dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpGet("my-product")]
    public async Task<IActionResult> GetMyProduct()
    {
        var res = await service.GetProductsMe();
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetProducts([FromQuery]ProductFilter filter)
    {
        var res = await service.GetFilterProduct(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var res = await service.GetProductById(id);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User) + "," + nameof(Roles.Admin))]
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(int id, [FromQuery] UpdatedProduct dto)
    {
        var res = await service.UpdateProduct(id, dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User) + "," + nameof(Roles.Admin))]
    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var res = await service.DeleteProduct(id);
        return StatusCode(res.StatusCode, res);
    }
}