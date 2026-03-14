namespace MVMedia.Api.DTOs;

public class MediaListItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string MediaUrl { get; set; } 
    public bool IsActive { get; set; } = true;

}
