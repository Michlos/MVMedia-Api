using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.Models;

public class MediaFile
{
    [Key]
    public Guid Id { get; set; } //UUID PRIMARY KEY

    //NÃO PRECISA MANTER POIS O FILEPATH JÁ ESTÁ NO APPSETTINGS.JSON
    //[Required]
    //[MaxLength(500)]
    //public string FilePath { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }


    [Required]
    [MaxLength(100)]
    public string FileName { get; set; }

    public long FileSize { get; set; } // Size in bytes

    public DateTime UploadedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }

    public string? ThumbFileName { get; set; }
    public int ClientId { get; set; }
    public virtual Client Client { get; set; }
    public int CompanyId { get; set; }
    //public virtual Company Company { get; set; }


}
