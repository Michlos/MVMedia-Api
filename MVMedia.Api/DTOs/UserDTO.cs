using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MVMedia.Api.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    public int CompanyId { get; set; }


    [Required(ErrorMessage = "Name is required")]
    [MaxLength(250, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; }
   
    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [MaxLength(250, ErrorMessage = "Email cannot exceed 250 characters")]
    public string Email { get; set; }
   
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [NotMapped] 
    public string Password { get; set; }
    public bool IsActive { get; set; }
    
    [JsonIgnore]
    public bool IsAdmin { get; set; }

}
