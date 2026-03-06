using System.Net;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IUserRepository _repository;

    public UserService(HttpContextAccessor accessor,
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

    public Task<Response<GetUser>> GetMe()
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<GetUser>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Response<string>> UpdateUserAsync(int id, UpdatedUser user)
    {
        throw new NotImplementedException();
    }
}