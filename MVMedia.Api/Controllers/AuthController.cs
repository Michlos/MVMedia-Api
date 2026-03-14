using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;

    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Valide o usuário (exemplo simplificado)
        if (model.Username == "admin" && model.Password == "senha")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var isadmin = new JwtSecurityTokenHandler()
                .ReadJwtToken(new JwtSecurityTokenHandler()
                .WriteToken(token)).Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value == "IsAdmin";

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                isadmin = new JwtSecurityTokenHandler().ReadJwtToken(new JwtSecurityTokenHandler().WriteToken(token)).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value == "IsAdmin"
            });
        }

        return Unauthorized();
    }


}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}