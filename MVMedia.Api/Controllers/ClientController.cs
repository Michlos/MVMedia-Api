using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Identity;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly IUserService _userService;


    public ClientController(IClientService clientService, IUserService userService)
    {
        _clientService = clientService;
        _userService = userService;

    }

    [HttpGet("GetAllClients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
    {

        ////CÓDIGO NOVO
        //1. verificaçãod e autorização simplificada
        var userId = User.GetUserId();
        var isAdmin = await _userService.IsAdmin(userId);
        var companyId = await _userService.GetCompanyId(userId);
        if (userId == 0)
            return Unauthorized("You are not authenticated to access this resource");

        //2. Busca de cados
        var clients = await _clientService.GetAllClients();
        if (!clients.Any())
        {
            var listClients = new List<Client>();
            return Ok(listClients);
        }

        //3. Filtra os clientes pela empresa do usuário
        if (!isAdmin)
        {
            var filteredClients = clients.Where(c => c.CompanyId == companyId).ToList();
            return Ok(filteredClients);
        }
        else
        {
            return Ok(clients);
        }
    }


    [HttpGet("GetClient/{id}")]
    public async Task<ActionResult<Client>> GetClientById(int id)
    {
        ////CÓDIGO NOVO
        ///
        //1. verificação de autorização simplificada
        
        //var userId = User.GetUserId();
        var user = await _userService.GetUser(User.GetUserId());
        
        if (user.Id == 0)
            return Unauthorized("You are not autenticated to access this resource");
        //var user = await _userService.IsAdmin(userId);
        //var companyId = await _userService.GetCompanyId(userId);
        //2. busca de dados
        var client = await _clientService.GetClientById(id);
        if (client == null)
            return BadRequest("Client not found");
        if (!user.IsAdmin)
        {
            if (client.CompanyId == user.CompanyId)
            {
                return Ok(client);
            }
            else
            {
                return BadRequest("This client is not a your client, please try a other clienteId");
            }

        }
        else
        {
            return Ok(client);
        }
        

    }


    [HttpPost]
    public async Task<ActionResult<Client>> AddClient(ClientAddDTO clientDTO)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource");


        var clientAdded = await _clientService.AddClient(clientDTO);
        if (clientAdded == null)
        {
            return BadRequest("Failed to add client");
        }
        return Ok(clientAdded);
    }

    [HttpPut]
    public async Task<ActionResult<Client>> UpdateClient([FromBody] ClientUpdateDTO clientDTO)
    {

        //GET USER DATA
        


        #region // CHERCK CLIENT SEND DATA

        //BADREQUEST FOR INVALID ID
        if (clientDTO.Id == 0)
            return BadRequest("Is not possible to update a client without an ID");

        //BADREQUEST FOR INAVLID DATA
        if (clientDTO == null)
            return BadRequest("Invalid client data");

        #endregion

        //SEARCH CLIENT
        var existingClient = await _clientService.GetClientById(clientDTO.Id);
        
        //CLIENT NOT FOUND
        if (existingClient == null)
            return NotFound($"Client with id {clientDTO.Id} not found!");

        
        var userId = User.GetUserId();
        var companyIdUser = await _userService.GetCompanyId(userId);
        var isAdmin = await _userService.IsAdmin(userId);

        //VALIDATION FOR CHECK FOR ISADMIN
        if (isAdmin == true)
        {
            //IF IS ADMIN - ALTER ANY CLIENTS
            var clientUpdated = await _clientService.UpdateClient(clientDTO);
            
            return Ok(clientUpdated);

        }

        //VALIDATION IF THE CLIENT BELONGS TO THE PORTFOLIO (SAME COMPANY)
        if (companyIdUser != existingClient.CompanyId)
            return BadRequest($"This company with id {existingClient.Id} is not in your Portfolio");
        

        await _clientService.UpdateClient(clientDTO);
        var returnedClient = _clientService.GetClientById(clientDTO.Id);

        return Ok(returnedClient);

    }



}
