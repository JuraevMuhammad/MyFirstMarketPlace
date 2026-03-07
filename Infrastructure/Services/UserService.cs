using System.Net;
using System.Security.Claims;
using Application.DTOs.User;
using Application.Filter;
using Application.Interfaces;
using Application.Responses;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IUserRepository _repository;

    public UserService(IHttpContextAccessor accessor,
        IUserRepository repository)
    {
        _accessor = accessor;
        _repository = repository;
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

    public async Task<Response<string>> UpdateUserAsync(int id, UpdatedUser user)
    {
        var dbUser = await _repository.GetUserByIdAsync(id);

        if (dbUser == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        dbUser.Username = user.Username ?? dbUser.Username;
        dbUser.Email = user.Email ?? dbUser.Email;

        var result = await _repository.SaveAsync(user.Username ?? "");
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "filed update user")
            : new Response<string>(HttpStatusCode.OK, "update user");
    }

    #endregion
}