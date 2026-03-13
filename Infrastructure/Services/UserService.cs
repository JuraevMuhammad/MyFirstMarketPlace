using System.Net;
using System.Security.Claims;
using Application.DTOs.User;
using Application.Filter;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Hashing;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IUserRepository _repository;
    private readonly IHashPassword _hash;

    public UserService(IHttpContextAccessor accessor,
        IUserRepository repository,
        IHashPassword hash)
    {
        _accessor = accessor;
        _repository = repository;
        _hash = hash;
    }

    #region GetUserById

    public async Task<Response<GetUser>> GetUserByIdAsync(int userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);
        if (user == null)
            return new Response<GetUser>(HttpStatusCode.NotFound, "not found");

        var getUser = new GetUser()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
        };
        return new Response<GetUser>(getUser);
    }

    #endregion

    #region GetMe

    public async Task<Response<GetUser>> GetMe()
    {
        var userId = _accessor.HttpContext!.User.FindFirstValue("userId");
        
        if (!int.TryParse(userId, out var id))
            return new Response<GetUser>(HttpStatusCode.Unauthorized, "invalid token");

        var user = await _repository.GetUserByIdAsync(id);
        if (user == null)
            return new Response<GetUser>(HttpStatusCode.NotFound, "not found");
        var getUser = new GetUser()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
        };
        return new Response<GetUser>(getUser);
    }

    #endregion

    #region GetAllUsers

    public async Task<PaginationResponse<List<GetUser>>> GetAllUsersAsync(UserFilter filter)
    {
        var users = await _repository.GetFilterUser(filter);
        
        if(users == null)
            return new PaginationResponse<List<GetUser>>(HttpStatusCode.NotFound, "not found");

        var totalRecords = users.Count;

        var getUsers = users.Select(x => new GetUser()
        {
            Id = x.Id,
            Email = x.Email,
            Username = x.Username,
        }).ToList();
        
        return new PaginationResponse<List<GetUser>>(filter.PageNumber, filter.PageSize, totalRecords, getUsers);
    }

    #endregion

    #region UpdateUser

    public async Task<Response<string>> UpdateUserAsync(UpdatedUser user)
    {
        var userId = _accessor.HttpContext?.User.FindFirstValue("userId");
        
        if(!int.TryParse(userId, out var id))
            return new Response<string>(HttpStatusCode.Unauthorized, "invalid token");
        
        var dbUser = await _repository.GetUserByIdAsync(id);

        if (dbUser == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        dbUser.Username = user.Username ?? dbUser.Username;
        dbUser.Email = user.Email ?? dbUser.Email;
        dbUser.UpdatedAt =  DateTime.UtcNow;

        var result = await _repository.UpdateAsync(dbUser);
        return result >= 0
            ? new Response<string>(HttpStatusCode.OK, "update user")
            : new Response<string>(HttpStatusCode.BadRequest, "filed update user");
    }


    #endregion

    #region CreateUser

    public async Task<Response<string>> CreateUser(CreatedUser dto)
    {
        var users = await _repository.GetAllUsersAsync();
        var user = users!.FirstOrDefault(x => x.Username == dto.Username && !x.IsDeleted);
        if (user != null)
            return new Response<string>(HttpStatusCode.BadRequest, "rename your username");

        var createdUser = new User()
        {
            Username = dto.Username,
            Email = dto.Email,
            HashPassword = _hash.Generate(dto.Password),
            Role = Roles.User
        };

        var result = await _repository.CreateUserAsync(createdUser);
        return new Response<string>(HttpStatusCode.Created, $"Created User Id:{result} \nuser password: {dto.Password}");
    }

    #endregion
}