using MVMedia.Api.DTOs;
using MVMedia.Api.Models;

namespace MVMedia.Api.Services.Interfaces;

public interface IMediaSerivce
{
    Task<Media> AddMedia(MediaAddDTO mediaDTO);
    Task<MediaUpdateDTO> UpdateMedia(MediaUpdateDTO mediaUpdateDTO);
    Task<IEnumerable<MediaGetDTO>> GetAllMedia();
    Task<MediaGetDTO> GetMediaById(int id);
    Task<ClientWithMediaDTO> GetMediaByClientId(int clientId);
    Task DeleteMediaById(int id);

}
