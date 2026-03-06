using Application.DTOs.User;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<Response<GetUser>> GetUserByIdAsync(int userId);
    Task<Response<GetUser>> GetMe();
    Task<Response<List<GetUser>>> GetAllUsersAsync();
    Task<Response<string>> UpdateUserAsync(int id, UpdatedUser user);
}