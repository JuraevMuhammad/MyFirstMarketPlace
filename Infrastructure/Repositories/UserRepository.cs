using System.Data;
using Application.Filter;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisCache _cache;

    public UserRepository(ApplicationDbContext context, IRedisCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        const string key = "users:all";
        
        _context.Users.Add(user);
        var result = await _context.SaveChangesAsync();
        
        await _cache.RemoveDataAsync(key);
        
        return user.Id;
    }

    public async Task<List<User>?> GetFilterUser(UserFilter filter)
    {
        var users = _context.Users
            .Where(x => !x.IsDeleted).AsQueryable();

        if (!string.IsNullOrEmpty(filter.Username))
            users = users.Where(x => x.Username.Contains(filter.Username));
        
        var result = await users
            .Where(x => !x.IsDeleted)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToListAsync();
        
        return result;
    }
    
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var cacheKey = $"user:{userId}";

        try
        {
            var cacheUser = await _cache.GetDataAsync<User>(cacheKey);
            if (cacheUser != null)
            {
                Console.WriteLine("=======Get In Cache=======");
                return cacheUser;
            }
        }
        catch
        {
            throw new Exception("error redis");
        }
        
        
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
        Console.WriteLine("=======Get In DataBase=======");
        await _cache.SetDataAsync(cacheKey, user);
        return user ?? null;
    }

    public async Task<List<User>?> GetAllUsersAsync()
    {
        const string key = "users:all";
        
        var cacheUser =  await _cache.GetDataAsync<List<User>>(key);
        if (cacheUser != null)
        {
            Console.WriteLine("=======Get In Cache=======");
            return cacheUser;
        }
        
        var users = await _context.Users.Where(x => !x.IsDeleted).ToListAsync();
        Console.WriteLine("=======Get In DataBase=======");
        await _cache.SetDataAsync(key, users);
        return users;
    }

    public async Task<int> DeleteUserAsync(int userId)
    {
        const string key = "users:all";
        var res = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
        if (res == null)
            return 0;
        _context.Users.Remove(res);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(key);
        return result;
    }

    public async Task<int> UpdateAsync(User update)
    {
        string key = $"user:{update.Id}";
        _context.Users.Update(update);
        var res = await _context.SaveChangesAsync(); 
        if (res > 0) 
            await _cache.RemoveDataAsync(key);
        return res;
    }
}