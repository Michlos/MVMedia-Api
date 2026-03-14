using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.DTOs;

public class MediaFileUploadDTO
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }

    [Required]
    public IFormFile File { get; set; }
    public bool IsPublic { get; set; }
    public int ClientId { get; set; }
    public int CompanyId { get; set; }
}
