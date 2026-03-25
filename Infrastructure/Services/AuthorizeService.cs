using System.Net;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Hashing;
using Infrastructure.Jwt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorizeService : IAuthorize
{
    private readonly ApplicationDbContext _context;
    private readonly IHashPassword _hash;
    private readonly IJwtProvider _jwt;
    private readonly ISendMail _mail;

    public AuthorizeService(ApplicationDbContext context,  IHashPassword hash,
        IJwtProvider jwt, ISendMail mail)
    {
        _context = context;
        _hash = hash;
        _jwt = jwt;
        _mail = mail;
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
        
        var result = await _context.SaveChangesAsync();

        if (result > 0)
            await _mail.SendMailLoginAsync(created, dto.Password);
        
        return result > 0 
            ? new Response<string>(HttpStatusCode.Created, "created new user")
            : new  Response<string>(HttpStatusCode.BadRequest, "not created");
    }

    public async Task<Response<string>> LoginUser(LoginUser dto)
    {
        var res = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        if (res == null)
            return new Response<string>(HttpStatusCode.NotFound, "invalid your password or username");

        var password = _hash.Verify(dto.Password, res.HashPassword);
        if (!password)
            return new Response<string>(HttpStatusCode.BadRequest, "invalid your password or username");

        var token = _jwt.GenerateToken(res);
        return new Response<string>(HttpStatusCode.OK, token);
    }
}