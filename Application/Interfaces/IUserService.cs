using Application.DTOs.User;
using Application.Filter;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<Response<GetUser>> GetUserByIdAsync(int userId);
    Task<Response<GetUser>> GetMe();
    Task<PaginationResponse<List<GetUser>>> GetAllUsersAsync(UserFilter filter);
    Task<Response<string>> UpdateUserAsync(int id, UpdatedUser user);
}