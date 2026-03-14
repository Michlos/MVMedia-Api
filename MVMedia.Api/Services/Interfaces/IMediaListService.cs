using MVMedia.Api.Models;

namespace MVMedia.Api.Services.Interfaces;

public interface IMediaListService
{
    Task<MediaList> AddMediaList(MediaList mediaList);
    Task<MediaList> DesableMediaList(int id);
    Task<MediaList> GetMediaList();
}
