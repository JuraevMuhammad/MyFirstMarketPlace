using Application.Filter;
using Application.Responses;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<List<User>?> GetAllUsersAsync();
    Task<List<User>?> GetFilterUser(UserFilter filter);
    Task<int> DeleteUserAsync(int userId);
    Task<int> SaveAsync(string? username);
    Task<int> CreateUserAsync(User user);
}