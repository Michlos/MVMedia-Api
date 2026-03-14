using MVMedia.Api.DTOs;
using MVMedia.Api.Models;

namespace MVMedia.Api.Services.Interfaces;

public interface IClientService
{
    Task<ClientAddDTO> AddClient(ClientAddDTO clientAddDTO);
    Task<ClientUpdateDTO> UpdateClient(ClientUpdateDTO cliendUpdateDTO);
    Task<IEnumerable<ClientGetDTO>> GetAllClients();
    Task<ClientGetDTO> GetClientById(int id);

}
