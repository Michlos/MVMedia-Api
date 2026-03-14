using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.DTOs;

public class MediaGetDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    [Required]
    [MaxLength(500)]
    public string MediaUrl { get; set; } 
    public string? Notes { get; set; }
    public int ClientId { get; set; } 

}
