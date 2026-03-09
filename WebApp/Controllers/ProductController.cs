using Application.DTOs.Product;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm]CreatedProduct dto)
    {
        var res = await service.CreateProduct(dto);
        return StatusCode(res.StatusCode, res);
    }
}