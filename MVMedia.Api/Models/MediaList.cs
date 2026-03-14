namespace MVMedia.Api.Models;

public class MediaList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; }

}
