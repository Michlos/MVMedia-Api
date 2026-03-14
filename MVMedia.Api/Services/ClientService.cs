using AutoMapper;

using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;
using MVMedia.Api.Models;

namespace MVMedia.Api.Services;

public class ClientService : IClientService
{
    public readonly IClientRepository _clientRepository;
    public readonly IMediaFileRepository _mediaFileRepository;
    public readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMediaFileRepository mediaFileRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mediaFileRepository = mediaFileRepository;
        _mapper = mapper;
    }

    public async Task<ClientAddDTO> AddClient(ClientAddDTO clientAddDTO)
    {
        var client = _mapper.Map<Client>(clientAddDTO);
        var clientAdded = await _clientRepository.AddClient(client);
        return _mapper.Map<ClientAddDTO>(clientAdded);
    }

    public async Task<IEnumerable<ClientGetDTO>> GetAllClients()
    {
        var clients = await _clientRepository.GetAllClients();
        return _mapper.Map<IEnumerable<ClientGetDTO>>(clients);
    }

    public async Task<ClientGetDTO> GetClientById(int id)
    {
        var client = await _clientRepository.GetClientById(id);
        return _mapper.Map<ClientGetDTO>(client);
    }

    public async Task<ClientUpdateDTO> UpdateClient(ClientUpdateDTO clientUpdateDTO)
    {
        var existingClient = await _clientRepository.GetClientById(clientUpdateDTO.Id);

        bool isDeactivating = existingClient.IsActive && !clientUpdateDTO.IsActive;

        var clientUpdated = await _clientRepository.UpdateClient(clientUpdateDTO);
        if (isDeactivating)
        {
            await _mediaFileRepository.DeactivateMediaFileByClientId(clientUpdated.Id);
        }
        else
        {
            await _mediaFileRepository.ActivateMediaFileByClientId(clientUpdated.Id);
        }
            return _mapper.Map<ClientUpdateDTO>(clientUpdated);

    }

}
