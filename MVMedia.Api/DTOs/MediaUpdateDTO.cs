using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.DTOs;

public class MediaUpdateDTO
{

    public int Id { get; set; }
    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string? MediaUrl { get; set; }
    public string? Notes { get; set; }

}
