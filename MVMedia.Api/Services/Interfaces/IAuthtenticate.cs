using MVMedia.Api.Models;

namespace MVMedia.Api.Services.Interfaces;

public interface IAuthtenticate
{
    Task<bool> Autheniticate(string username, string password);
    Task<bool> UserExists(string username, string email);
    Task<bool> UserExists(string username);
    public Task<User> GetUserByUserName(string username);
    public string GenerateToken(int id, string username);
    (byte[] hash, byte[] salt) HashPassword(string password);
    Task<bool> UserIsActive(string loginUser);

}
