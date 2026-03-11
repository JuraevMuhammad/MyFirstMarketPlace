using Application.DTOs.ItemProduct;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemProductController(IItemProductService service) : ControllerBase
{
    [Authorize(Roles = nameof(Roles.User) + "," + nameof(Roles.Admin))]
    [HttpPost]
    public async Task<IActionResult> CreateItemProduct([FromQuery]CreatedItemProduct dto)
    {
        var res = await service.CreateItemProduct(dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User) + "," + nameof(Roles.Admin))]
    [HttpPut]
    public async Task<IActionResult> UpdateItemProduct(int id, [FromQuery] UpdateItemProduct dto)
    {
        var res = await service.UpdateItemProduct(id, dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User) + "," + nameof(Roles.Admin))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItemProduct(int id)
    {
        var res = await service.DeleteItemProduct(id);
        return StatusCode(res.StatusCode, res);
    }
}