using Application.DTOs.Order;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService service) : ControllerBase
{
    [Authorize(Roles = nameof(Roles.Customer))]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromQuery]CreatedOrder dto)
    {
        var res = await service.CreateOrder(dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpPut]
    public async Task<IActionResult> UpdateOrder(int id, [FromQuery] UpdateOrder dto)
    {
        var res = await service.UpdateOrder(id, dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    [HttpGet("list")]
    public async Task<IActionResult> GetOrders()
    {
        var res = await service.GetOrders();
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var res = await service.GetOrder(id);
        return StatusCode(res.StatusCode, res);
    }
}