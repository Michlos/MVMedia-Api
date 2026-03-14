using AutoMapper;
using Microsoft.IdentityModel.Abstractions;
using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Services;

public class MediaFileService : IMediaFileService
{
    private readonly IMediaFileRepository _mediaFileRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public MediaFileService(IMediaFileRepository mediaFileRepository, IMapper mapper, IClientRepository clientRepository)
    {
        _mediaFileRepository = mediaFileRepository;
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<MediaFile> AddMediaFile(MediaFile mediaFile)
    {
        var mediaFileAdded = await _mediaFileRepository.AddMediaFile(mediaFile);
        return mediaFileAdded;
    }

    public async Task<bool> DeleteMediaFile(Guid id)
    {
        return await _mediaFileRepository.DeleteMediaFile(id);
    }

    public async Task<ClientWithMediaFileDTO> GetAllMediaByClientId(int clientId)
    {
        var client = await _clientRepository.GetClientById(clientId);
        var mediaFiles = await _mediaFileRepository.GetMediaFilesByClientId(clientId);

        var dto = new ClientWithMediaFileDTO
        {
            Client = _mapper.Map<ClientSummaryDTO>(client),
            MediaFiles = _mapper.Map<List<MediaFileListItemDTO>>(mediaFiles)
        };

        return dto;
    }

    public async Task<ICollection<MediaFile>> GetAllMediaFiles()
    {
        return await _mediaFileRepository.GetAllMediaFiles();
    }

    public async Task<MediaFile> GetMediaFileById(Guid id)
    {
        return await _mediaFileRepository.GetMediaFileById(id);
    }

    public async Task<MediaFile> UpdateMediaFile(MediaFile mediaFile, string oldFileName)
    {
        var updatedMediaFile = await _mediaFileRepository.UpdateMediaFile(mediaFile, oldFileName);
        return updatedMediaFile;
    }


}
