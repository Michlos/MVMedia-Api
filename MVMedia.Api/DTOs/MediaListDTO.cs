namespace MVMedia.Api.DTOs;

public class MediaListDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsActive { get; set; }
    public string URI { get; set; }
}
