using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Context;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;

namespace MVMedia.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApiDbContext _context;

    public UserRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<User> Add(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;

    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        //if (users == null || !users.Any())
        //{
        //    throw new ArgumentException("No users found.");
        //}
        return users;
    }

    public Task<User> GetUser(int id)
    {
        var user = _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new ArgumentException($"User with id {id} not found.");
        }
        return user;
    }

    public async Task<User> Update(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with id {user.Id} not found.");
        }
        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
        return existingUser;
    }
}
