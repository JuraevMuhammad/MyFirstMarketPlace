using System.Net;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Hashing;
using Infrastructure.Jwt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorizeService : IAuthorize
{
    private readonly ApplicationDbContext _context;
    private readonly IHashPassword _hash;
    private readonly IJwtProvider _jwt;

    public AuthorizeService(ApplicationDbContext context,  IHashPassword hash,
        IJwtProvider jwt)
    {
        _context = context;
        _hash = hash;
        _jwt = jwt;
    }
    
    public async Task<Response<string>> CreatedUser(CreatedUser dto)
    {
        var res = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        if(res != null)
            return new Response<string>(HttpStatusCode.BadRequest, "Username already exists");

        var created = new User()
        {
            Username = dto.Username,
            HashPassword = _hash.Generate(dto.Password),
            Email = dto.Email
        };
        
        
        await _context.Users.AddAsync(created);
        
        var orders = await _context.Orders
            .Where(x => x.UserId == created.Id).ToListAsync();

        var finance = new Finance()
        {
            UserId = created.Id,
            CompletedOrders = 0,
            TotalOrders = 0,
            CancelledOrders = 0,
            NewOrders = 0,
            TotalRevenue = 0,
        };
        await _context.Finances.AddAsync(finance);
        var result = await _context.SaveChangesAsync();
        return result > 0 
            ? new Response<string>(HttpStatusCode.Created, "created new user")
            : new  Response<string>(HttpStatusCode.BadRequest, "not created");
    }

    public async Task<Response<string>> LoginUser(LoginUser dto)
    {
        var res = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        if (res == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        var password = _hash.Verify(dto.Password, res.HashPassword);
        if (!password)
            return new Response<string>(HttpStatusCode.BadRequest, "invalid password");

        var token = _jwt.GenerateToken(res);
        return new Response<string>(HttpStatusCode.OK, token);
    }
}