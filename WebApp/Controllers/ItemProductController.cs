using Application.DTOs.ItemProduct;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemProductController(IItemProductService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateItemProduct([FromQuery]CreatedItemProduct dto)
    {
        var res = await service.CreateItemProduct(dto);
        return StatusCode(res.StatusCode, res);
    }
}