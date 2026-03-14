using MVMedia.Api.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task<User> Update(User user);
    Task<User> GetUser(int id);
    Task<IEnumerable<User>> GetAllUsers();
}
