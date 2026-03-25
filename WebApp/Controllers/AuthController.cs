using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthorize service) : ControllerBase
{
    [HttpPost ("register")]

    public async Task<IActionResult> Register([FromBody] CreatedUser dto)
    {
        var res = await service.CreatedUser(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser dto)
    {
        var res = await service.LoginUser(dto);
        return StatusCode(res.StatusCode, res);
    }
}