using Application.Responses;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<List<User>?> GetAllUsersAsync();
    Task<int> DeleteUserAsync(int userId);
    Task<int> SaveAsync();
}