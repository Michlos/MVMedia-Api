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

        ////CÓDIGO ANTERIOR
        //if (!await _userService.IsAdmin(User.GetUserId()))
        //    return Unauthorized("You are not authorized to access this resource");

        //var clients = await _clientService.GetAllClients();
        //if (clients.Any())
        //{
        //    var users = await _userService.GetAllUsers();
        //    if (users.Any())
        //    {
        //        if (!User.Identity?.IsAuthenticated ?? false)
        //            return Unauthorized("You are not authenticated to access this resource");

        //        var userId = User.GetUserId();
        //        var user = await _userService.GetUser(userId);
        //        if (user == null)
        //            return Unauthorized("User not found");

        //        var filteredClients = clients.Where(c => c.CompanyId == user.CompanyId).ToList();
        //        if (filteredClients.Any())
        //            return Ok(filteredClients);
        //    }
        //}
        //return Ok(clients);

    }
    [HttpGet("GetClient/{id}")]
    public async Task<ActionResult<Client>> GetClientById(int id)
    {
        //if (!await _userService.IsAdmin(User.GetUserId()))
        //    return Unauthorized("You are not authorized to access this resource");

        

        var clientUpdated = await _clientService.GetClientById(id);
        if (clientUpdated == null)
        {
            return NotFound($"Client with id {id} not found!");
        }
        return Ok(clientUpdated);
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

        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource");

        if (clientDTO.Id == 0)
            return BadRequest("Is not possible to update a client without an ID");

        var existingClient = await _clientService.GetClientById(clientDTO.Id);
        if (existingClient == null)
            return NotFound($"Client with id {clientDTO.Id} not found!");

        if (clientDTO == null)
            return BadRequest("Invalid client data");

        await _clientService.UpdateClient(clientDTO);

        return Ok(clientDTO);

    }



}
