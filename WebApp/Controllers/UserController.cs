using Application.DTOs.User;
using Application.Filter;
using Application.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res = await service.GetAllUsersAsync(filter);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var  res = await service.GetUserByIdAsync(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("/profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var res = await service.GetMe();
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromQuery]UpdatedUser user)
    {
        var res = await service.UpdateUserAsync(user);
        return StatusCode(res.StatusCode, res);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("/create-user")]
    public async Task<IActionResult> CreateUser([FromBody]CreatedUser dto)
    {
        var res = await service.CreateUser(dto);
        return StatusCode(res.StatusCode, res);
    }
}
