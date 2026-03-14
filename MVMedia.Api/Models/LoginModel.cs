using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Please set the password")]
    [DataType(DataType.Password)]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}
