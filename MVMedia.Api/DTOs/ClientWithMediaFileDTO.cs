namespace MVMedia.Api.DTOs;

public class ClientWithMediaFileDTO
{
    public ClientSummaryDTO Client { get; set; }
    public List<MediaFileListItemDTO> MediaFiles { get; set; }
}
