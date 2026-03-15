using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.Models;

public class Media
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [MaxLength(500)]
    public string MediaUrl { get; set; } // URL to access the media file
    public string? Notes { get; set; } // Additional notes or comments about the media

    //vincular ao cliente onde o video pertence a um cliente
    public int ClientId { get; set; }
    public virtual Client Client { get; set; } // Navigation property to the Client entity

}
