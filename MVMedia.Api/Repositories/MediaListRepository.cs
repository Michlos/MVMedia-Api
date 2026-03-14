using Microsoft.EntityFrameworkCore;
using MVMedia.Api.Context;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Videos;

namespace MVMedia.Api.Repositories;

public class MediaListRepository :IMediaListRepository
{
    private readonly ApiDbContext _context;
    private readonly VideoSettings _videoSettings;

    public MediaListRepository(ApiDbContext context, VideoSettings videoSettings)
    {
        _context = context;
        _videoSettings = videoSettings;
    }

    public async Task<MediaList> AddMediaList(MediaList mediaList)
    {
        mediaList.CreateDate = DateTime.UtcNow;
        await _context.MediaLists.AddAsync(mediaList);
        await _context.SaveChangesAsync();
        return mediaList;
    }

    public Task<MediaList> DesableMediaList(int id)
    {
        var mediaList= _context.MediaLists.FirstOrDefault(ml => ml.Id == id);
        if (mediaList != null)
        {
            mediaList.IsActive = false;
            _context.MediaLists.Update(mediaList);
            _context.SaveChanges();
            return Task.FromResult(mediaList);
        }
        return Task.FromResult<MediaList>(null);
    }

    public async Task<MediaList> GetMediaList()
    {
        return await _context.MediaLists.FirstOrDefaultAsync(ml => ml.IsActive);
    }
}
