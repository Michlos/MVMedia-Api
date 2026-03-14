using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.DTOs;

public class MediaAddDTO
{
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [MaxLength(500)]
    public string MediaUrl { get; set; } 
    public string? Notes { get; set; }
    public int ClientId { get; set; }

}
