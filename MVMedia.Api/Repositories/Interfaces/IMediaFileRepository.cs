using MVMedia.Api.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IMediaFileRepository
{
    Task<MediaFile> AddMediaFile(MediaFile mediaFile);
    Task<MediaFile> UpdateMediaFile(MediaFile mediaFile, string oldFileName);
    Task<MediaFile> GetMediaFileById(Guid id);
    Task<bool> DeleteMediaFile(Guid id);
    Task<ICollection<MediaFile>> GetAllMediaFiles();
    Task<ICollection<MediaFile>> GetMediaFilesByClientId(int clientId);
    Task DeactivateMediaFileByClientId(int clientId);
    Task ActivateMediaFileByClientId(int clientId);
}
