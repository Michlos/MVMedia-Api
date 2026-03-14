using MVMedia.Api.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IMediaListRepository
{
    Task<MediaList> AddMediaList(MediaList mediaList);
    Task<MediaList> DesableMediaList(int id);
    Task<MediaList> GetMediaList();
}
