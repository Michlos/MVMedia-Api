using AutoMapper;

using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;
using MVMedia.Api.Models;

namespace MVMedia.Api.Services;

public class MediaService : IMediaSerivce
{
    private readonly IMediaRepository _mediaRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public MediaService(IMediaRepository mediaRepository,  IMapper mapper, IClientRepository clientRepository)
    {
        _mediaRepository = mediaRepository;
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<Media> AddMedia(MediaAddDTO mediaDTO)
    {
        var media = _mapper.Map<Media>(mediaDTO);
        var mediaAdded = await _mediaRepository.AddMedia(media);
        return _mapper.Map<Media>(mediaAdded);
    }

    public async Task<IEnumerable<MediaGetDTO>> GetAllMedia()
    {
        var medias = await _mediaRepository.GetAllMedia();
        return _mapper.Map<IEnumerable<MediaGetDTO>>(medias);
    }

    public async Task<ClientWithMediaDTO> GetMediaByClientId(int clientId)
    {
        
        var client = await _clientRepository.GetClientById(clientId);
        var medias = await _mediaRepository.GetMediaByClientId(clientId);

        var dto = new ClientWithMediaDTO
        {
            Client = _mapper.Map<ClientSummaryDTO>(client),
            Medias = _mapper.Map<List<MediaListItemDTO>>(medias)
        };
        return dto;
    }

    public async Task<MediaGetDTO> GetMediaById(int id)
    {
        var media = await _mediaRepository.GetMediaById(id);
        return _mapper.Map<MediaGetDTO>(media);
    }

    public async Task<MediaUpdateDTO> UpdateMedia(MediaUpdateDTO mediaUpdateDTO)
    {
        var media = _mapper.Map<Media>(mediaUpdateDTO);
        var mediaUpdated = await _mediaRepository.UpdateMedia(mediaUpdateDTO);
        return _mapper.Map<MediaUpdateDTO>(mediaUpdated);
    }

    public async Task DeleteMediaById(int id)
    {
        await _mediaRepository.DeleteMediaById(id);
    }
}
