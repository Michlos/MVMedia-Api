using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Services;

public class MediaListService : IMediaListService
{

    private readonly IMediaListRepository _mediaListRepository;

    public MediaListService(IMediaListRepository mediaListRepository)
    {
        _mediaListRepository = mediaListRepository;
    }

    public Task<MediaList> AddMediaList(MediaList mediaList)
    {
        var mediaListAdded = _mediaListRepository.AddMediaList(mediaList);
        return mediaListAdded;
    }

    public async Task<MediaList> DesableMediaList(int id)
    {
        return await _mediaListRepository.DesableMediaList(id);
    }

    public async Task<MediaList> GetMediaList()
    {
        return await _mediaListRepository.GetMediaList();
        
    }
}
