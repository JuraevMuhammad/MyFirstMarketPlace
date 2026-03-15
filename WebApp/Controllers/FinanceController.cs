using Application.DTOs.ItemFinance;
using Application.Filter;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/finance")]
public class FinanceController(IFinanceService service) : ControllerBase
{
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.User))]
    [HttpGet ("id")]
    public async Task<IActionResult> GetById()
    {
        var res = await service.GetFinance();
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    [HttpGet]
    public async Task<IActionResult> GetFinances()
    {
        var res = await  service.GetAllFinances();
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpPost]
    public async Task<IActionResult> CreateItemFinance([FromQuery]CreateItemFinance dto)
    {
        var res = await service.CreateItemFinance(dto);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpGet("filter")]
    public async Task<IActionResult> GetItemFinance([FromQuery] FinanceFilter filter)
    {
        var res = await service.GetItemFinanceFilter(filter);
        return StatusCode(res.StatusCode, res);
    }
}