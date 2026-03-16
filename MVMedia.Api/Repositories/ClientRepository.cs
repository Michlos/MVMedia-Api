using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Context;
using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;

namespace MVMedia.Api.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApiDbContext _context;

    public ClientRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<Client> AddClient(Client client)
    {
        client.CreatedAt = DateTime.UtcNow; // Set the creation timestamp
        _context.Clients.Add(client); // Adiciona o cliente ao contexto
        await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
        return client; // Retorna o cliente adicionado
    }
    public async Task<Client> UpdateClient(ClientUpdateDTO clientDTO)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientDTO.Id);
        if (existingClient == null)
            throw new ArgumentException($"Client with id {clientDTO.Id} not found.");

        // Atualize as propriedades diretamente
        if (clientDTO.Name is not null && clientDTO.Name != existingClient.Name)
            existingClient.Name = clientDTO.Name;
        if (clientDTO.CPF is not null && clientDTO.CPF != existingClient.CPF)
            existingClient.CPF = clientDTO.CPF;
        if (clientDTO.CNPJ is not null && clientDTO.CNPJ != existingClient.CNPJ)
            existingClient.CNPJ = clientDTO.CNPJ;
        if (clientDTO.Email is not null && clientDTO.Email != existingClient.Email)
            existingClient.Email = clientDTO.Email;
        if (clientDTO.Phone is not null && clientDTO.Phone != existingClient.Phone)
            existingClient.Phone = clientDTO.Phone;
        if (clientDTO.Address is not null && clientDTO.Address != existingClient.Address)
            existingClient.Address = clientDTO.Address;
        if (clientDTO.City is not null && clientDTO.City != existingClient.City)
            existingClient.City = clientDTO.City;
        if (clientDTO.State is not null && clientDTO.State != existingClient.State)
            existingClient.State = clientDTO.State;
        if (clientDTO.ZipCode is not null && clientDTO.ZipCode != existingClient.ZipCode)
            existingClient.ZipCode = clientDTO.ZipCode;
        if (clientDTO.Country is not null && clientDTO.Country != existingClient.Country)
            existingClient.Country = clientDTO.Country;
        if (clientDTO.IsActive != existingClient.IsActive)
            existingClient.IsActive = clientDTO.IsActive;

        existingClient.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        //BUSCA DADOS DIRETO DO BANCO
        var clientReGet = new Client();
        clientReGet = await _context.Clients.Where(cliId => cliId.Id == existingClient.Id).FirstOrDefaultAsync();
        return clientReGet;
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client> GetClientById(int id)
    {
        return await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();

    }

}
