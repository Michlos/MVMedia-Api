namespace MVMedia.Api.DTOs;

public class MediaFileListItemDTO
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string FileName { get; set; }
    public int ClientId { get; set; }
}
