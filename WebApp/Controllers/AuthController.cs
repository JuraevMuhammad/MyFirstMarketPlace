using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthorize service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromQuery] CreatedUser dto)
    {
        var res = await service.CreatedUser(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] LoginUser dto)
    {
        var res = await service.LoginUser(dto);
        return StatusCode(res.StatusCode, res);
    }
}