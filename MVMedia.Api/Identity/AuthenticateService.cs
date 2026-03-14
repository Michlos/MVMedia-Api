using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using MVMedia.Api.Context;
using MVMedia.Api.Models;
using MVMedia.Api.Services;
using MVMedia.Api.Services.Interfaces;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MVMedia.Api.Identity;
public class AuthenticateService : IAuthtenticate
{
    private readonly ApiDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthenticateService(ApiDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<bool> Autheniticate(string username, string password)
    {
        var user = await _context.Users.Where(x => x.Login == username).FirstOrDefaultAsync();
        if (user == null)
            return false;

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return false;
        }
        return true;
    }

    public (byte[] hash, byte[] salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordSalt = hmac.Key;
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (passwordHash, passwordSalt);

    }

    public string GenerateToken(int id, string username)
    {
        var claims = new[]
        {
            //new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            //new Claim(ClaimTypes.Name, username)
            new Claim("id", id.ToString()),
            new Claim("username", username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(2);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> UserExists(string username, string email)
    {
        var user = await _context.Users.Where(x => x.Login == username || x.Email == email).FirstOrDefaultAsync();
        if (user == null)
            return false;

        return true;
    }

    public async Task<bool> UserExists(string username)
    {
        var user = await _context.Users.Where(x => x.Login == username).FirstOrDefaultAsync();
        if (user == null) return false;
        
        return true;
    }

    public async Task<bool> UserIsActive(string userName)
    {
        var user = await _context.Users.Where(x => x.Login == userName).FirstOrDefaultAsync();
        if (user == null) return false;
        return user.IsActive;
    }

    public async Task<User> GetUserByUserName(string username)
    {
        return await _context.Users
            .Where(x => x.Login == username)
            .FirstOrDefaultAsync();
    }

}

